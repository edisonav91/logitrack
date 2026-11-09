using LogiTrack.Domain;

namespace LogiTrack.Application;

public interface IDeliveryRepository
{
    Task<Delivery?> GetAsync(DeliveryId deliveryId, CancellationToken cancellationToken);
    Task<SaveDeliveryResult> SaveAsync(Delivery delivery, int expectedVersion, CancellationToken cancellationToken);
}

public interface IDeliveryReader
{
    Task<DeliveryView?> GetAsync(DeliveryId deliveryId, CancellationToken cancellationToken);
}

public interface ICourierAvailability
{
    Task<CourierAvailability> CheckAsync(
        CourierId courierId,
        RouteId routeId,
        DateTimeOffset checkedAt,
        CancellationToken cancellationToken);
}

public interface IAuthorizationService
{
    bool CanAssignCourier(ActorContext actor, DeliveryId deliveryId);
}

public interface IDomainEventPublisher
{
    Task<bool> PublishPendingAsync(DeliveryId deliveryId, CancellationToken cancellationToken);
}

public interface IIdempotencyStore
{
    Task<AssignCourierResult?> GetAsync(Guid requestId, CancellationToken cancellationToken);
    Task StoreAsync(Guid requestId, AssignCourierResult result, CancellationToken cancellationToken);
}

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}

public interface IAssignmentTelemetry
{
    IDisposable Start(Guid requestId, DeliveryId deliveryId);
    void RecordSuccess();
    void RecordRejection(string code);
    void RecordPublicationPending();
}

