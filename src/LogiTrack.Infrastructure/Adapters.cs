using System.Diagnostics;
using System.Diagnostics.Metrics;
using LogiTrack.Application;
using LogiTrack.Domain;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace LogiTrack.Infrastructure;

public sealed record AvailabilityOptions(
    IReadOnlySet<Guid> UnavailableCourierIds,
    bool SimulateFailure = false);

public sealed class ConfiguredCourierAvailability(AvailabilityOptions options) : ICourierAvailability
{
    public Task<CourierAvailability> CheckAsync(
        CourierId courierId,
        RouteId routeId,
        DateTimeOffset checkedAt,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (options.SimulateFailure)
        {
            return Task.FromResult(CourierAvailability.ServiceUnavailable);
        }

        var result = options.UnavailableCourierIds.Contains(courierId.Value)
            ? CourierAvailability.Unavailable
            : CourierAvailability.Available;
        return Task.FromResult(result);
    }
}

public sealed class RoleAuthorizationService : IAuthorizationService
{
    private static readonly HashSet<string> AllowedRoles =
        new(StringComparer.OrdinalIgnoreCase) { "operator", "admin" };

    public bool CanAssignCourier(ActorContext actor, DeliveryId deliveryId) =>
        AllowedRoles.Contains(actor.Role);
}

public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}

public sealed record EventPublishingOptions(bool SimulateFailure = false, int MaxAttempts = 3);

public sealed class SqliteOutboxPublisher(
    SqliteConnectionFactory connectionFactory,
    EventPublishingOptions options,
    ILogger<SqliteOutboxPublisher> logger) : IDomainEventPublisher
{
    public async Task<bool> PublishPendingAsync(
        DeliveryId deliveryId,
        CancellationToken cancellationToken)
    {
        await using var connection = connectionFactory.Create();
        await connection.OpenAsync(cancellationToken);
        var select = connection.CreateCommand();
        select.CommandText = """
            SELECT id, event_type, payload, attempts
            FROM outbox_events
            WHERE delivery_id = $deliveryId
              AND published_at IS NULL
              AND attempts < $maxAttempts
            ORDER BY occurred_at;
            """;
        select.Parameters.AddWithValue("$deliveryId", deliveryId.ToString());
        select.Parameters.AddWithValue("$maxAttempts", options.MaxAttempts);

        var events = new List<(string Id, string Type, string Payload, int Attempts)>();
        await using (var reader = await select.ExecuteReaderAsync(cancellationToken))
        {
            while (await reader.ReadAsync(cancellationToken))
            {
                events.Add((reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3)));
            }
        }

        foreach (var pendingEvent in events)
        {
            if (options.SimulateFailure)
            {
                await RecordFailureAsync(connection, pendingEvent.Id, "Simulated publisher failure.", cancellationToken);
                logger.LogWarning(
                    "Domain event {EventId} for delivery {DeliveryId} remains pending after publication failure",
                    pendingEvent.Id,
                    deliveryId);
                return false;
            }

            logger.LogInformation(
                "Published domain event {EventType} with id {EventId} for delivery {DeliveryId}",
                pendingEvent.Type,
                pendingEvent.Id,
                deliveryId);

            var markPublished = connection.CreateCommand();
            markPublished.CommandText = """
                UPDATE outbox_events
                SET published_at = $publishedAt,
                    attempts = attempts + 1,
                    last_error = NULL
                WHERE id = $id;
                """;
            markPublished.Parameters.AddWithValue("$publishedAt", DateTimeOffset.UtcNow.ToString("O"));
            markPublished.Parameters.AddWithValue("$id", pendingEvent.Id);
            await markPublished.ExecuteNonQueryAsync(cancellationToken);
        }

        return true;
    }

    private static async Task RecordFailureAsync(
        SqliteConnection connection,
        string eventId,
        string error,
        CancellationToken cancellationToken)
    {
        var command = connection.CreateCommand();
        command.CommandText = """
            UPDATE outbox_events
            SET attempts = attempts + 1,
                last_error = $error
            WHERE id = $id;
            """;
        command.Parameters.AddWithValue("$error", error);
        command.Parameters.AddWithValue("$id", eventId);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}

public sealed class AssignmentTelemetry(ILogger<AssignmentTelemetry> logger) : IAssignmentTelemetry
{
    public const string SourceName = "LogiTrack.Assignments";
    private static readonly ActivitySource ActivitySource = new(SourceName);
    private static readonly Meter Meter = new(SourceName);
    private static readonly Counter<long> SuccessCounter = Meter.CreateCounter<long>("assign_courier.success");
    private static readonly Counter<long> RejectionCounter = Meter.CreateCounter<long>("assign_courier.rejected");
    private static readonly Counter<long> PendingCounter = Meter.CreateCounter<long>("assign_courier.event_pending");

    public IDisposable Start(Guid requestId, DeliveryId deliveryId)
    {
        var activity = ActivitySource.StartActivity("AssignCourier");
        activity?.SetTag("request.id", requestId);
        activity?.SetTag("delivery.id", deliveryId.ToString());
        logger.LogInformation(
            "Starting courier assignment request {RequestId} for delivery {DeliveryId}",
            requestId,
            deliveryId);
        return (IDisposable?)activity ?? NullScope.Instance;
    }

    public void RecordSuccess()
    {
        SuccessCounter.Add(1);
        logger.LogInformation("Courier assignment completed");
    }

    public void RecordRejection(string code)
    {
        RejectionCounter.Add(1, new KeyValuePair<string, object?>("reason", code));
        logger.LogWarning("Courier assignment rejected with code {Code}", code);
    }

    public void RecordPublicationPending()
    {
        PendingCounter.Add(1);
        logger.LogWarning("Courier assignment event remains pending");
    }

    private sealed class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();
        public void Dispose() { }
    }
}

