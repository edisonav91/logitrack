using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace LogiTrack.IntegrationTests;

public sealed class AssignCourierEndpointTests
{
    private static readonly Guid DeliveryId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid AvailableCourierId = Guid.Parse("44444444-4444-4444-4444-444444444444");
    private static readonly Guid UnavailableCourierId = Guid.Parse("99999999-9999-9999-9999-999999999999");

    [Fact]
    public async Task AssignCourier_TraversesApiDomainAndSqlitePersistence()
    {
        await using var factory = new LogiTrackApiFactory();
        using var client = factory.CreateClient();
        AddAuthorizedHeaders(client);

        var response = await client.PostAsJsonAsync(
            $"/deliveries/{DeliveryId}/assign-courier",
            new { requestId = Guid.NewGuid(), courierId = AvailableCourierId, expectedVersion = 1 });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("COURIER_ASSIGNED", body.GetProperty("code").GetString());
        Assert.Equal(2, body.GetProperty("version").GetInt32());

        var persisted = await client.GetAsync($"/deliveries/{DeliveryId}");
        Assert.Equal(HttpStatusCode.OK, persisted.StatusCode);
        var delivery = await persisted.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(AvailableCourierId, delivery.GetProperty("courierId").GetProperty("value").GetGuid());
        Assert.Equal(1, delivery.GetProperty("status").GetInt32());
    }

    [Fact]
    public async Task AssignCourier_WhenRequestIsRepeated_ReturnsPersistedIdempotentResult()
    {
        await using var factory = new LogiTrackApiFactory();
        using var client = factory.CreateClient();
        AddAuthorizedHeaders(client);
        var requestId = Guid.NewGuid();
        var payload = new
        {
            requestId,
            courierId = AvailableCourierId,
            expectedVersion = 1
        };

        var first = await client.PostAsJsonAsync(
            $"/deliveries/{DeliveryId}/assign-courier",
            payload);
        var repeated = await client.PostAsJsonAsync(
            $"/deliveries/{DeliveryId}/assign-courier",
            payload);

        Assert.Equal(HttpStatusCode.OK, first.StatusCode);
        Assert.Equal(HttpStatusCode.OK, repeated.StatusCode);
        var firstBody = await first.Content.ReadFromJsonAsync<JsonElement>();
        var repeatedBody = await repeated.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(firstBody.GetProperty("code").GetString(), repeatedBody.GetProperty("code").GetString());
        Assert.Equal(2, repeatedBody.GetProperty("version").GetInt32());

        var persisted = await client.GetFromJsonAsync<JsonElement>($"/deliveries/{DeliveryId}");
        Assert.Equal(2, persisted.GetProperty("version").GetInt32());
    }

    [Fact]
    public async Task AssignCourier_WhenDeliveryAlreadyAssigned_ReturnsConflict()
    {
        await using var factory = new LogiTrackApiFactory();
        using var client = factory.CreateClient();
        AddAuthorizedHeaders(client);
        var first = await client.PostAsJsonAsync(
            $"/deliveries/{DeliveryId}/assign-courier",
            new { requestId = Guid.NewGuid(), courierId = AvailableCourierId, expectedVersion = 1 });
        Assert.Equal(HttpStatusCode.OK, first.StatusCode);

        var second = await client.PostAsJsonAsync(
            $"/deliveries/{DeliveryId}/assign-courier",
            new { requestId = Guid.NewGuid(), courierId = Guid.NewGuid(), expectedVersion = 2 });

        Assert.Equal(HttpStatusCode.Conflict, second.StatusCode);
        var body = await second.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("DELIVERY_ALREADY_ASSIGNED", body.GetProperty("code").GetString());
    }

    [Fact]
    public async Task AssignCourier_WhenCourierUnavailable_ReturnsUnprocessableEntity()
    {
        await using var factory = new LogiTrackApiFactory();
        using var client = factory.CreateClient();
        AddAuthorizedHeaders(client);

        var response = await client.PostAsJsonAsync(
            $"/deliveries/{DeliveryId}/assign-courier",
            new { requestId = Guid.NewGuid(), courierId = UnavailableCourierId, expectedVersion = 1 });

        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("COURIER_NOT_AVAILABLE", body.GetProperty("code").GetString());
    }

    [Fact]
    public async Task AssignCourier_WithoutApiKey_ReturnsUnauthorized()
    {
        await using var factory = new LogiTrackApiFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            $"/deliveries/{DeliveryId}/assign-courier",
            new { requestId = Guid.NewGuid(), courierId = AvailableCourierId, expectedVersion = 1 });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AssignCourier_WhenAvailabilityIntegrationFails_ReturnsServiceUnavailable()
    {
        await using var factory = new LogiTrackApiFactory(simulateAvailabilityFailure: true);
        using var client = factory.CreateClient();
        AddAuthorizedHeaders(client);

        var response = await client.PostAsJsonAsync(
            $"/deliveries/{DeliveryId}/assign-courier",
            new { requestId = Guid.NewGuid(), courierId = AvailableCourierId, expectedVersion = 1 });

        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("AVAILABILITY_UNAVAILABLE", body.GetProperty("code").GetString());
    }

    [Fact]
    public async Task AssignCourier_WithMalformedJson_ReturnsBadRequestWithoutInternalDetails()
    {
        await using var factory = new LogiTrackApiFactory();
        using var client = factory.CreateClient();
        AddAuthorizedHeaders(client);
        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"/deliveries/{DeliveryId}/assign-courier")
        {
            Content = new StringContent("{not-json", Encoding.UTF8, "application/json")
        };

        var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("INVALID_REQUEST", body.GetProperty("code").GetString());
        Assert.DoesNotContain("JsonException", body.GetProperty("message").GetString());
    }

    private static void AddAuthorizedHeaders(HttpClient client)
    {
        client.DefaultRequestHeaders.Add("X-Api-Key", "integration-test-key");
        client.DefaultRequestHeaders.Add("X-Actor-Id", "integration-operator");
        client.DefaultRequestHeaders.Add("X-Actor-Role", "operator");
    }
}

public sealed class LogiTrackApiFactory(bool simulateAvailabilityFailure = false)
    : WebApplicationFactory<Program>
{
    private readonly string _directory = Path.Combine(
        Path.GetTempPath(),
        "logitrack-tests",
        Guid.NewGuid().ToString("N"));

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Directory.CreateDirectory(_directory);
        var databasePath = Path.Combine(_directory, "integration.db");
        builder.ConfigureAppConfiguration((_, configuration) =>
        {
            configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:LogiTrack"] = $"Data Source={databasePath}",
                ["Security:ApiKey"] = "integration-test-key",
                ["DemoData:Enabled"] = "true",
                ["CourierAvailability:UnavailableCourierIds:0"] =
                    "99999999-9999-9999-9999-999999999999",
                ["CourierAvailability:SimulateFailure"] = simulateAvailabilityFailure.ToString(),
                ["EventPublishing:SimulateFailure"] = "false"
            });
        });
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        SqliteConnection.ClearAllPools();
        for (var attempt = 1; attempt <= 5 && Directory.Exists(_directory); attempt++)
        {
            try
            {
                Directory.Delete(_directory, recursive: true);
            }
            catch (IOException) when (attempt < 5)
            {
                await Task.Delay(50 * attempt);
            }
        }
    }
}

