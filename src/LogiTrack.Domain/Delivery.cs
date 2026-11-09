namespace LogiTrack.Domain;

public enum DeliveryStatus
{
    PendingAssignment = 0,
    Assigned = 1,
    InRoute = 2,
    Delivered = 3,
    NotDelivered = 4,
    Cancelled = 5
}

public sealed class DomainRuleException(string code, string message) : Exception(message)
{
    public string Code { get; } = code;
}

public sealed class Delivery
{
    private readonly List<IDomainEvent> _domainEvents = [];

    private Delivery(
        DeliveryId id,
        OrderId orderId,
        RouteId routeId,
        CourierId? courierId,
        DeliveryStatus status,
        int version)
    {
        if (id.Value == Guid.Empty || orderId.Value == Guid.Empty || routeId.Value == Guid.Empty)
        {
            throw new DomainRuleException("INVALID_DELIVERY", "Delivery, order and route identifiers are required.");
        }

        if (version < 1)
        {
            throw new DomainRuleException("INVALID_VERSION", "Delivery version must be positive.");
        }

        Id = id;
        OrderId = orderId;
        RouteId = routeId;
        CourierId = courierId;
        Status = status;
        Version = version;
    }

    public DeliveryId Id { get; }
    public OrderId OrderId { get; }
    public RouteId RouteId { get; }
    public CourierId? CourierId { get; private set; }
    public DeliveryStatus Status { get; private set; }
    public int Version { get; private set; }
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public static Delivery CreatePending(DeliveryId id, OrderId orderId, RouteId routeId) =>
        new(id, orderId, routeId, null, DeliveryStatus.PendingAssignment, 1);

    public static Delivery Rehydrate(
        DeliveryId id,
        OrderId orderId,
        RouteId routeId,
        CourierId? courierId,
        DeliveryStatus status,
        int version) => new(id, orderId, routeId, courierId, status, version);

    public void AssignCourier(CourierId courierId, ActorId actorId, DateTimeOffset assignedAt)
    {
        if (courierId.Value == Guid.Empty)
        {
            throw new DomainRuleException("INVALID_COURIER", "Courier identifier is required.");
        }

        if (CourierId is not null)
        {
            throw new DomainRuleException("DELIVERY_ALREADY_ASSIGNED", "Delivery already has an active courier.");
        }

        if (Status != DeliveryStatus.PendingAssignment)
        {
            throw new DomainRuleException("INVALID_DELIVERY_STATE", "Only a pending delivery can receive a courier.");
        }

        CourierId = courierId;
        Status = DeliveryStatus.Assigned;
        Version++;

        _domainEvents.Add(new CourierAssigned(
            Guid.NewGuid(),
            Id,
            OrderId,
            RouteId,
            courierId,
            actorId,
            assignedAt,
            Version));
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}

