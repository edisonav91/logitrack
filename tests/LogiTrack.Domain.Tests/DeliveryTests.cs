using LogiTrack.Domain;

namespace LogiTrack.Domain.Tests;

public sealed class DeliveryTests
{
    private static readonly DeliveryId DeliveryId = new(Guid.Parse("11111111-1111-1111-1111-111111111111"));
    private static readonly OrderId OrderId = new(Guid.Parse("22222222-2222-2222-2222-222222222222"));
    private static readonly RouteId RouteId = new(Guid.Parse("33333333-3333-3333-3333-333333333333"));
    private static readonly CourierId CourierId = new(Guid.Parse("44444444-4444-4444-4444-444444444444"));
    private static readonly ActorId ActorId = ActorId.Create("operator-1");
    private static readonly DateTimeOffset Now = new(2026, 11, 8, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public void AssignCourier_WhenPending_AssignsCourierAndRaisesEvent()
    {
        var delivery = Delivery.CreatePending(DeliveryId, OrderId, RouteId);

        delivery.AssignCourier(CourierId, ActorId, Now);

        Assert.Equal(CourierId, delivery.CourierId);
        Assert.Equal(DeliveryStatus.Assigned, delivery.Status);
        Assert.Equal(2, delivery.Version);
        var domainEvent = Assert.IsType<CourierAssigned>(Assert.Single(delivery.DomainEvents));
        Assert.Equal(CourierId, domainEvent.CourierId);
        Assert.Equal(ActorId, domainEvent.AssignedBy);
    }

    [Fact]
    public void AssignCourier_WhenAlreadyAssigned_RejectsSecondCourier()
    {
        var delivery = Delivery.CreatePending(DeliveryId, OrderId, RouteId);
        delivery.AssignCourier(CourierId, ActorId, Now);

        var exception = Assert.Throws<DomainRuleException>(() =>
            delivery.AssignCourier(new CourierId(Guid.NewGuid()), ActorId, Now));

        Assert.Equal("DELIVERY_ALREADY_ASSIGNED", exception.Code);
        Assert.Equal(CourierId, delivery.CourierId);
        Assert.Equal(2, delivery.Version);
    }

    [Fact]
    public void AssignCourier_WhenStateIsNotPending_RejectsAssignment()
    {
        var delivery = Delivery.Rehydrate(
            DeliveryId,
            OrderId,
            RouteId,
            null,
            DeliveryStatus.Cancelled,
            2);

        var exception = Assert.Throws<DomainRuleException>(() =>
            delivery.AssignCourier(CourierId, ActorId, Now));

        Assert.Equal("INVALID_DELIVERY_STATE", exception.Code);
    }

    [Fact]
    public void AssignCourier_WhenCourierIdIsEmpty_RejectsAssignment()
    {
        var delivery = Delivery.CreatePending(DeliveryId, OrderId, RouteId);

        var exception = Assert.Throws<DomainRuleException>(() =>
            delivery.AssignCourier(new CourierId(Guid.Empty), ActorId, Now));

        Assert.Equal("INVALID_COURIER", exception.Code);
        Assert.Equal(1, delivery.Version);
    }

    [Fact]
    public void CreatePending_WhenIdentityIsInvalid_RejectsDelivery()
    {
        var exception = Assert.Throws<DomainRuleException>(() =>
            Delivery.CreatePending(new DeliveryId(Guid.Empty), OrderId, RouteId));

        Assert.Equal("INVALID_DELIVERY", exception.Code);
    }
}

