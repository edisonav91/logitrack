using System.Security.Cryptography;
using System.Text;
using LogiTrack.Application;
using LogiTrack.Domain;
using LogiTrack.Infrastructure;
using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.TimestampFormat = "yyyy-MM-ddTHH:mm:ss.fffZ ";
    options.UseUtcTimestamp = true;
});
builder.Services.AddLogiTrackInfrastructure();
builder.Services.AddScoped<AssignCourierUseCase>();

var app = builder.Build();

app.Use(async (context, next) =>
{
    var correlationId = context.Request.Headers["X-Correlation-Id"].FirstOrDefault();
    context.TraceIdentifier = string.IsNullOrWhiteSpace(correlationId) || correlationId.Length > 100
        ? context.TraceIdentifier
        : correlationId;
    context.Response.Headers["X-Correlation-Id"] = context.TraceIdentifier;

    try
    {
        await next();
    }
    catch (BadHttpRequestException exception)
    {
        app.Logger.LogWarning(
            "Invalid HTTP request for correlation {CorrelationId}: {Reason}",
            context.TraceIdentifier,
            exception.Message);
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(new ApiError(
            "INVALID_REQUEST",
            "The request body or parameters are invalid.",
            context.TraceIdentifier));
    }
    catch (SqliteException exception)
    {
        app.Logger.LogError(exception, "Persistence failure for correlation {CorrelationId}", context.TraceIdentifier);
        context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
        await context.Response.WriteAsJsonAsync(new ApiError(
            "PERSISTENCE_UNAVAILABLE",
            "The operation cannot be completed now.",
            context.TraceIdentifier));
    }
    catch (Exception exception)
    {
        app.Logger.LogError(exception, "Unexpected failure for correlation {CorrelationId}", context.TraceIdentifier);
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new ApiError(
            "UNEXPECTED_ERROR",
            "An unexpected error occurred.",
            context.TraceIdentifier));
    }
});

app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

app.MapPost("/deliveries/{deliveryId:guid}/assign-courier", async (
    Guid deliveryId,
    AssignCourierRequest request,
    HttpContext httpContext,
    AssignCourierUseCase useCase,
    IConfiguration configuration,
    CancellationToken cancellationToken) =>
{
    if (!ApiSecurity.HasValidApiKey(httpContext, configuration))
    {
        return Results.Json(
            new ApiError("UNAUTHENTICATED", "A valid API key is required.", httpContext.TraceIdentifier),
            statusCode: StatusCodes.Status401Unauthorized);
    }

    if (request.RequestId == Guid.Empty || request.CourierId == Guid.Empty || request.ExpectedVersion < 1)
    {
        return Results.BadRequest(new ApiError(
            "INVALID_REQUEST",
            "RequestId, CourierId and a positive ExpectedVersion are required.",
            httpContext.TraceIdentifier));
    }

    var actorId = httpContext.Request.Headers["X-Actor-Id"].FirstOrDefault();
    var actorRole = httpContext.Request.Headers["X-Actor-Role"].FirstOrDefault();
    if (string.IsNullOrWhiteSpace(actorId) || string.IsNullOrWhiteSpace(actorRole))
    {
        return Results.BadRequest(new ApiError(
            "INVALID_ACTOR",
            "X-Actor-Id and X-Actor-Role headers are required.",
            httpContext.TraceIdentifier));
    }

    var command = new AssignCourierCommand(
        request.RequestId,
        new DeliveryId(deliveryId),
        new CourierId(request.CourierId),
        request.ExpectedVersion,
        new ActorContext(ActorId.Create(actorId), actorRole));
    var result = await useCase.ExecuteAsync(command, cancellationToken);
    return result.IsSuccess
        ? Results.Ok(result)
        : ApiResults.FromFailure(result, httpContext.TraceIdentifier);
});

app.MapGet("/deliveries/{deliveryId:guid}", async (
    Guid deliveryId,
    HttpContext httpContext,
    IDeliveryReader reader,
    IConfiguration configuration,
    CancellationToken cancellationToken) =>
{
    if (!ApiSecurity.HasValidApiKey(httpContext, configuration))
    {
        return Results.Json(
            new ApiError("UNAUTHENTICATED", "A valid API key is required.", httpContext.TraceIdentifier),
            statusCode: StatusCodes.Status401Unauthorized);
    }

    var delivery = await reader.GetAsync(new DeliveryId(deliveryId), cancellationToken);
    return delivery is null
        ? Results.NotFound(new ApiError("DELIVERY_NOT_FOUND", "Delivery was not found.", httpContext.TraceIdentifier))
        : Results.Ok(delivery);
});

await InitializeDatabaseAsync(app);
app.Run();

static async Task InitializeDatabaseAsync(WebApplication app)
{
    await using var scope = app.Services.CreateAsyncScope();
    var migrator = scope.ServiceProvider.GetRequiredService<DatabaseMigrator>();
    await migrator.MigrateAsync();

    if (app.Configuration.GetValue<bool>("DemoData:Enabled"))
    {
        await migrator.SeedDemoDeliveryAsync(
            new DeliveryId(Guid.Parse("11111111-1111-1111-1111-111111111111")),
            new OrderId(Guid.Parse("22222222-2222-2222-2222-222222222222")),
            new RouteId(Guid.Parse("33333333-3333-3333-3333-333333333333")));
    }
}

public sealed record AssignCourierRequest(Guid RequestId, Guid CourierId, int ExpectedVersion);
public sealed record ApiError(string Code, string Message, string CorrelationId);

public static class ApiResults
{
    public static IResult FromFailure(AssignCourierResult result, string correlationId)
    {
        var error = new ApiError(result.Code, result.Message, correlationId);
        return result.Code switch
        {
            "FORBIDDEN" => Results.Json(error, statusCode: StatusCodes.Status403Forbidden),
            "DELIVERY_NOT_FOUND" => Results.NotFound(error),
            "DELIVERY_ALREADY_ASSIGNED" or "INVALID_DELIVERY_STATE" or "VERSION_CONFLICT" =>
                Results.Conflict(error),
            "COURIER_NOT_AVAILABLE" => Results.Json(error, statusCode: StatusCodes.Status422UnprocessableEntity),
            "AVAILABILITY_UNAVAILABLE" => Results.Json(error, statusCode: StatusCodes.Status503ServiceUnavailable),
            _ => Results.BadRequest(error)
        };
    }
}

public static class ApiSecurity
{
    public static bool HasValidApiKey(HttpContext context, IConfiguration configuration)
    {
        var expected = configuration["Security:ApiKey"];
        var supplied = context.Request.Headers["X-Api-Key"].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(expected) || string.IsNullOrWhiteSpace(supplied))
        {
            return false;
        }

        var expectedBytes = Encoding.UTF8.GetBytes(expected);
        var suppliedBytes = Encoding.UTF8.GetBytes(supplied);
        return expectedBytes.Length == suppliedBytes.Length &&
               CryptographicOperations.FixedTimeEquals(expectedBytes, suppliedBytes);
    }
}

public partial class Program;

