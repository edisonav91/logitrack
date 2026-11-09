using System.Globalization;
using System.Reflection;
using System.Text.Json;
using LogiTrack.Application;
using LogiTrack.Domain;
using Microsoft.Data.Sqlite;

namespace LogiTrack.Infrastructure;

public sealed class SqliteConnectionFactory(string connectionString)
{
    public SqliteConnection Create() => new(connectionString);
}

public sealed class DatabaseMigrator(SqliteConnectionFactory connectionFactory)
{
    public async Task MigrateAsync(CancellationToken cancellationToken = default)
    {
        await using var connection = connectionFactory.Create();
        await connection.OpenAsync(cancellationToken);

        var createHistory = connection.CreateCommand();
        createHistory.CommandText = """
            CREATE TABLE IF NOT EXISTS schema_migrations (
                version TEXT PRIMARY KEY,
                applied_at TEXT NOT NULL
            );
            """;
        await createHistory.ExecuteNonQueryAsync(cancellationToken);

        const string version = "001_initial";
        var check = connection.CreateCommand();
        check.CommandText = "SELECT COUNT(1) FROM schema_migrations WHERE version = $version;";
        check.Parameters.AddWithValue("$version", version);
        var alreadyApplied = Convert.ToInt32(await check.ExecuteScalarAsync(cancellationToken), CultureInfo.InvariantCulture) > 0;
        if (alreadyApplied)
        {
            return;
        }

        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = assembly.GetManifestResourceNames()
            .Single(name => name.EndsWith("001_initial.sql", StringComparison.Ordinal));
        await using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException("Embedded migration was not found.");
        using var reader = new StreamReader(stream);
        var sql = await reader.ReadToEndAsync(cancellationToken);

        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);
        var migrate = connection.CreateCommand();
        migrate.Transaction = (SqliteTransaction)transaction;
        migrate.CommandText = sql;
        await migrate.ExecuteNonQueryAsync(cancellationToken);

        var record = connection.CreateCommand();
        record.Transaction = (SqliteTransaction)transaction;
        record.CommandText = "INSERT INTO schema_migrations(version, applied_at) VALUES ($version, $at);";
        record.Parameters.AddWithValue("$version", version);
        record.Parameters.AddWithValue("$at", DateTimeOffset.UtcNow.ToString("O"));
        await record.ExecuteNonQueryAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }

    public async Task SeedDemoDeliveryAsync(
        DeliveryId deliveryId,
        OrderId orderId,
        RouteId routeId,
        CancellationToken cancellationToken = default)
    {
        await using var connection = connectionFactory.Create();
        await connection.OpenAsync(cancellationToken);
        var command = connection.CreateCommand();
        command.CommandText = """
            INSERT OR IGNORE INTO deliveries
                (id, order_id, route_id, courier_id, status, version, updated_at)
            VALUES
                ($id, $orderId, $routeId, NULL, $status, 1, $updatedAt);
            """;
        command.Parameters.AddWithValue("$id", deliveryId.ToString());
        command.Parameters.AddWithValue("$orderId", orderId.ToString());
        command.Parameters.AddWithValue("$routeId", routeId.ToString());
        command.Parameters.AddWithValue("$status", (int)DeliveryStatus.PendingAssignment);
        command.Parameters.AddWithValue("$updatedAt", DateTimeOffset.UtcNow.ToString("O"));
        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}

public sealed class SqliteDeliveryRepository(SqliteConnectionFactory connectionFactory)
    : IDeliveryRepository, IDeliveryReader
{
    public async Task<Delivery?> GetAsync(DeliveryId deliveryId, CancellationToken cancellationToken)
    {
        await using var connection = connectionFactory.Create();
        await connection.OpenAsync(cancellationToken);
        var command = connection.CreateCommand();
        command.CommandText = """
            SELECT id, order_id, route_id, courier_id, status, version
            FROM deliveries
            WHERE id = $id;
            """;
        command.Parameters.AddWithValue("$id", deliveryId.ToString());
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        return await reader.ReadAsync(cancellationToken) ? ReadDelivery(reader) : null;
    }

    async Task<DeliveryView?> IDeliveryReader.GetAsync(
        DeliveryId deliveryId,
        CancellationToken cancellationToken)
    {
        var delivery = await GetAsync(deliveryId, cancellationToken);
        return delivery is null
            ? null
            : new DeliveryView(
                delivery.Id,
                delivery.OrderId,
                delivery.RouteId,
                delivery.CourierId,
                delivery.Status,
                delivery.Version);
    }

    public async Task<SaveDeliveryResult> SaveAsync(
        Delivery delivery,
        int expectedVersion,
        CancellationToken cancellationToken)
    {
        await using var connection = connectionFactory.Create();
        await connection.OpenAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        var update = connection.CreateCommand();
        update.Transaction = (SqliteTransaction)transaction;
        update.CommandText = """
            UPDATE deliveries
            SET courier_id = $courierId,
                status = $status,
                version = $newVersion,
                updated_at = $updatedAt
            WHERE id = $id AND version = $expectedVersion;
            """;
        update.Parameters.AddWithValue("$courierId", delivery.CourierId?.ToString() ?? (object)DBNull.Value);
        update.Parameters.AddWithValue("$status", (int)delivery.Status);
        update.Parameters.AddWithValue("$newVersion", delivery.Version);
        update.Parameters.AddWithValue("$updatedAt", DateTimeOffset.UtcNow.ToString("O"));
        update.Parameters.AddWithValue("$id", delivery.Id.ToString());
        update.Parameters.AddWithValue("$expectedVersion", expectedVersion);

        if (await update.ExecuteNonQueryAsync(cancellationToken) != 1)
        {
            await transaction.RollbackAsync(cancellationToken);
            return SaveDeliveryResult.VersionConflict;
        }

        foreach (var domainEvent in delivery.DomainEvents)
        {
            var insertEvent = connection.CreateCommand();
            insertEvent.Transaction = (SqliteTransaction)transaction;
            insertEvent.CommandText = """
                INSERT INTO outbox_events
                    (id, delivery_id, event_type, payload, occurred_at, attempts)
                VALUES
                    ($id, $deliveryId, $eventType, $payload, $occurredAt, 0);
                """;
            insertEvent.Parameters.AddWithValue("$id", domainEvent.EventId.ToString());
            insertEvent.Parameters.AddWithValue("$deliveryId", delivery.Id.ToString());
            insertEvent.Parameters.AddWithValue("$eventType", domainEvent.EventType);
            insertEvent.Parameters.AddWithValue("$payload", JsonSerializer.Serialize(domainEvent, domainEvent.GetType()));
            insertEvent.Parameters.AddWithValue("$occurredAt", domainEvent.OccurredAt.ToString("O"));
            await insertEvent.ExecuteNonQueryAsync(cancellationToken);
        }

        await transaction.CommitAsync(cancellationToken);
        delivery.ClearDomainEvents();
        return SaveDeliveryResult.Saved;
    }

    private static Delivery ReadDelivery(SqliteDataReader reader)
    {
        var courierId = reader.IsDBNull(3)
            ? (CourierId?)null
            : new CourierId(Guid.Parse(reader.GetString(3)));
        return Delivery.Rehydrate(
            new DeliveryId(Guid.Parse(reader.GetString(0))),
            new OrderId(Guid.Parse(reader.GetString(1))),
            new RouteId(Guid.Parse(reader.GetString(2))),
            courierId,
            (DeliveryStatus)reader.GetInt32(4),
            reader.GetInt32(5));
    }
}

public sealed class SqliteIdempotencyStore(SqliteConnectionFactory connectionFactory) : IIdempotencyStore
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<AssignCourierResult?> GetAsync(Guid requestId, CancellationToken cancellationToken)
    {
        await using var connection = connectionFactory.Create();
        await connection.OpenAsync(cancellationToken);
        var command = connection.CreateCommand();
        command.CommandText = "SELECT result_json FROM idempotency_records WHERE request_id = $id;";
        command.Parameters.AddWithValue("$id", requestId.ToString());
        var json = await command.ExecuteScalarAsync(cancellationToken) as string;
        return json is null ? null : JsonSerializer.Deserialize<AssignCourierResult>(json, JsonOptions);
    }

    public async Task StoreAsync(Guid requestId, AssignCourierResult result, CancellationToken cancellationToken)
    {
        await using var connection = connectionFactory.Create();
        await connection.OpenAsync(cancellationToken);
        var command = connection.CreateCommand();
        command.CommandText = """
            INSERT OR IGNORE INTO idempotency_records(request_id, result_json, created_at)
            VALUES ($id, $result, $createdAt);
            """;
        command.Parameters.AddWithValue("$id", requestId.ToString());
        command.Parameters.AddWithValue("$result", JsonSerializer.Serialize(result, JsonOptions));
        command.Parameters.AddWithValue("$createdAt", DateTimeOffset.UtcNow.ToString("O"));
        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}

