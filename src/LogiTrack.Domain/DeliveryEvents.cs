namespace LogiTrack.Domain;

public interface IDomainEvent
{
    Guid EventId { get; }
    string EventType { get; }
    DateTimeOffset OccurredAt { get; }
}

public sealed record CourierAssigned(
    Guid EventId,
    DeliveryId DeliveryId,
    OrderId OrderId,
    RouteId RouteId,
    CourierId CourierId,
    ActorId AssignedBy,
    DateTimeOffset OccurredAt,
    int DeliveryVersion) : IDomainEvent
{
    public string EventType => nameof(CourierAssigned);
}

