using LogiTrack.Domain;

namespace LogiTrack.Application;

public sealed class AssignCourierUseCase(
    IDeliveryRepository deliveryRepository,
    ICourierAvailability courierAvailability,
    IAuthorizationService authorizationService,
    IDomainEventPublisher eventPublisher,
    IIdempotencyStore idempotencyStore,
    IClock clock,
    IAssignmentTelemetry telemetry)
{
    public async Task<AssignCourierResult> ExecuteAsync(
        AssignCourierCommand command,
        CancellationToken cancellationToken = default)
    {
        using var activity = telemetry.Start(command.RequestId, command.DeliveryId);

        if (command.RequestId == Guid.Empty)
        {
            return Reject("INVALID_REQUEST", "RequestId is required.");
        }

        if (!authorizationService.CanAssignCourier(command.Actor, command.DeliveryId))
        {
            return Reject("FORBIDDEN", "The actor cannot assign couriers.");
        }

        var previousResult = await idempotencyStore.GetAsync(command.RequestId, cancellationToken);
        if (previousResult is not null)
        {
            return previousResult;
        }

        var delivery = await deliveryRepository.GetAsync(command.DeliveryId, cancellationToken);
        if (delivery is null)
        {
            return Reject("DELIVERY_NOT_FOUND", "Delivery was not found.");
        }

        if (delivery.Version != command.ExpectedVersion)
        {
            return Reject("VERSION_CONFLICT", "Delivery was changed by another operation.");
        }

        var now = clock.UtcNow;
        var availability = await courierAvailability.CheckAsync(
            command.CourierId,
            delivery.RouteId,
            now,
            cancellationToken);

        if (availability == CourierAvailability.ServiceUnavailable)
        {
            return Reject("AVAILABILITY_UNAVAILABLE", "Courier availability cannot be checked now.");
        }

        if (availability == CourierAvailability.Unavailable)
        {
            return Reject("COURIER_NOT_AVAILABLE", "Courier is not available for this route.");
        }

        try
        {
            delivery.AssignCourier(command.CourierId, command.Actor.ActorId, now);
        }
        catch (DomainRuleException exception)
        {
            return Reject(exception.Code, exception.Message);
        }

        var saveResult = await deliveryRepository.SaveAsync(delivery, command.ExpectedVersion, cancellationToken);
        if (saveResult == SaveDeliveryResult.VersionConflict)
        {
            return Reject("VERSION_CONFLICT", "Delivery was changed by another operation.");
        }

        var published = await eventPublisher.PublishPendingAsync(delivery.Id, cancellationToken);
        var result = AssignCourierResult.Success(delivery, now, !published);
        await idempotencyStore.StoreAsync(command.RequestId, result, cancellationToken);

        telemetry.RecordSuccess();
        if (!published)
        {
            telemetry.RecordPublicationPending();
        }

        return result;

        AssignCourierResult Reject(string code, string message)
        {
            telemetry.RecordRejection(code);
            return AssignCourierResult.Failure(code, message);
        }
    }
}

