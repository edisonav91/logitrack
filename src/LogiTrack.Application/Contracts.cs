using LogiTrack.Domain;

namespace LogiTrack.Application;

public sealed record ActorContext(ActorId ActorId, string Role);

public sealed record AssignCourierCommand(
    Guid RequestId,
    DeliveryId DeliveryId,
    CourierId CourierId,
    int ExpectedVersion,
    ActorContext Actor);

public sealed record AssignCourierResult(
    bool IsSuccess,
    string Code,
    string Message,
    DeliveryId? DeliveryId = null,
    CourierId? CourierId = null,
    DeliveryStatus? Status = null,
    int? Version = null,
    DateTimeOffset? AssignedAt = null,
    bool EventPublicationPending = false)
{
    public static AssignCourierResult Success(
        Delivery delivery,
        DateTimeOffset assignedAt,
        bool eventPublicationPending) =>
        new(
            true,
            "COURIER_ASSIGNED",
            "Courier assigned successfully.",
            delivery.Id,
            delivery.CourierId,
            delivery.Status,
            delivery.Version,
            assignedAt,
            eventPublicationPending);

    public static AssignCourierResult Failure(string code, string message) =>
        new(false, code, message);
}

public sealed record DeliveryView(
    DeliveryId DeliveryId,
    OrderId OrderId,
    RouteId RouteId,
    CourierId? CourierId,
    DeliveryStatus Status,
    int Version);

public enum CourierAvailability
{
    Available,
    Unavailable,
    ServiceUnavailable
}

public enum SaveDeliveryResult
{
    Saved,
    VersionConflict
}

