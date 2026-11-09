using LogiTrack.Application;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LogiTrack.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddLogiTrackInfrastructure(
        this IServiceCollection services)
    {
        services.AddSingleton(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("LogiTrack")
                ?? "Data Source=./data/logitrack.db";
            EnsureDataDirectory(connectionString);
            return new SqliteConnectionFactory(connectionString);
        });
        services.AddSingleton(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var unavailableIds = configuration
                .GetSection("CourierAvailability:UnavailableCourierIds")
                .Get<string[]>()
                ?.Select(Guid.Parse)
                .ToHashSet() ?? [];
            return new AvailabilityOptions(
                unavailableIds,
                configuration.GetValue<bool>("CourierAvailability:SimulateFailure"));
        });
        services.AddSingleton(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            return new EventPublishingOptions(
                configuration.GetValue<bool>("EventPublishing:SimulateFailure"),
                configuration.GetValue<int?>("EventPublishing:MaxAttempts") ?? 3);
        });
        services.AddSingleton<DatabaseMigrator>();
        services.AddScoped<SqliteDeliveryRepository>();
        services.AddScoped<IDeliveryRepository>(provider => provider.GetRequiredService<SqliteDeliveryRepository>());
        services.AddScoped<IDeliveryReader>(provider => provider.GetRequiredService<SqliteDeliveryRepository>());
        services.AddScoped<IIdempotencyStore, SqliteIdempotencyStore>();
        services.AddScoped<ICourierAvailability, ConfiguredCourierAvailability>();
        services.AddScoped<IAuthorizationService, RoleAuthorizationService>();
        services.AddScoped<IDomainEventPublisher, SqliteOutboxPublisher>();
        services.AddSingleton<IClock, SystemClock>();
        services.AddSingleton<IAssignmentTelemetry, AssignmentTelemetry>();
        return services;
    }

    private static void EnsureDataDirectory(string connectionString)
    {
        var builder = new SqliteConnectionStringBuilder(connectionString);
        var dataSource = builder.DataSource;
        if (string.IsNullOrWhiteSpace(dataSource) || dataSource == ":memory:")
        {
            return;
        }

        var fullPath = Path.GetFullPath(dataSource);
        var directory = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
}

