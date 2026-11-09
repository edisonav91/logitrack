using LogiTrack.Application;
using LogiTrack.Domain;

namespace LogiTrack.Application.Tests;

public sealed class AssignCourierUseCaseTests
{
    private static readonly DeliveryId DeliveryId = new(Guid.Parse("11111111-1111-1111-1111-111111111111"));
    private static readonly OrderId OrderId = new(Guid.Parse("22222222-2222-2222-2222-222222222222"));
    private static readonly RouteId RouteId = new(Guid.Parse("33333333-3333-3333-3333-333333333333"));
    private static readonly CourierId CourierId = new(Guid.Parse("44444444-4444-4444-4444-444444444444"));
    private static readonly DateTimeOffset Now = new(2026, 11, 8, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public async Task Execute_WhenValid_AssignsSavesAndPublishes()
    {
        var context = TestContext.Create();

        var result = await context.UseCase.ExecuteAsync(context.Command());

        Assert.True(result.IsSuccess);
        Assert.Equal("COURIER_ASSIGNED", result.Code);
        Assert.Equal(1, context.Repository.SaveCalls);
        Assert.Equal(1, context.Publisher.Calls);
        Assert.Equal(DeliveryStatus.Assigned, context.Repository.Delivery!.Status);
    }

    [Fact]
    public async Task Execute_WhenCourierUnavailable_DoesNotChangeOrSaveDelivery()
    {
        var context = TestContext.Create();
        context.Availability.Result = CourierAvailability.Unavailable;

        var result = await context.UseCase.ExecuteAsync(context.Command());

        Assert.False(result.IsSuccess);
        Assert.Equal("COURIER_NOT_AVAILABLE", result.Code);
        Assert.Equal(0, context.Repository.SaveCalls);
        Assert.Equal(DeliveryStatus.PendingAssignment, context.Repository.Delivery!.Status);
    }

    [Fact]
    public async Task Execute_WhenAvailabilityServiceFails_ReturnsControlledFailure()
    {
        var context = TestContext.Create();
        context.Availability.Result = CourierAvailability.ServiceUnavailable;

        var result = await context.UseCase.ExecuteAsync(context.Command());

        Assert.Equal("AVAILABILITY_UNAVAILABLE", result.Code);
        Assert.Equal(0, context.Repository.SaveCalls);
    }

    [Fact]
    public async Task Execute_WhenVersionIsStale_DoesNotCheckAvailability()
    {
        var context = TestContext.Create();

        var result = await context.UseCase.ExecuteAsync(context.Command(expectedVersion: 99));

        Assert.Equal("VERSION_CONFLICT", result.Code);
        Assert.Equal(0, context.Availability.Calls);
        Assert.Equal(0, context.Repository.SaveCalls);
    }

    [Fact]
    public async Task Execute_WhenActorIsNotAuthorized_DoesNotLoadDelivery()
    {
        var context = TestContext.Create();

        var result = await context.UseCase.ExecuteAsync(context.Command(role: "customer"));

        Assert.Equal("FORBIDDEN", result.Code);
        Assert.Equal(0, context.Repository.GetCalls);
    }

    [Fact]
    public async Task Execute_WhenRequestIsRepeated_ReturnsStoredResult()
    {
        var context = TestContext.Create();
        var command = context.Command();
        var first = await context.UseCase.ExecuteAsync(command);

        var second = await context.UseCase.ExecuteAsync(command);

        Assert.Equal(first, second);
        Assert.Equal(1, context.Repository.SaveCalls);
    }

    [Fact]
    public async Task Execute_WhenPublisherFails_KeepsAssignmentAndMarksEventPending()
    {
        var context = TestContext.Create();
        context.Publisher.Result = false;

        var result = await context.UseCase.ExecuteAsync(context.Command());

        Assert.True(result.IsSuccess);
        Assert.True(result.EventPublicationPending);
        Assert.Equal(1, context.Repository.SaveCalls);
    }

    private sealed class TestContext
    {
        private TestContext()
        {
            Repository = new FakeRepository
            {
                Delivery = Delivery.CreatePending(DeliveryId, OrderId, RouteId)
            };
            Availability = new FakeAvailability();
            Publisher = new FakePublisher();
            Idempotency = new FakeIdempotency();
            UseCase = new AssignCourierUseCase(
                Repository,
                Availability,
                new FakeAuthorization(),
                Publisher,
                Idempotency,
                new FakeClock(),
                new FakeTelemetry());
        }

        public FakeRepository Repository { get; }
        public FakeAvailability Availability { get; }
        public FakePublisher Publisher { get; }
        public FakeIdempotency Idempotency { get; }
        public AssignCourierUseCase UseCase { get; }

        public static TestContext Create() => new();

        public AssignCourierCommand Command(int expectedVersion = 1, string role = "operator") =>
            new(
                Guid.Parse("55555555-5555-5555-5555-555555555555"),
                DeliveryId,
                CourierId,
                expectedVersion,
                new ActorContext(ActorId.Create("actor-1"), role));
    }

    private sealed class FakeRepository : IDeliveryRepository
    {
        public Delivery? Delivery { get; set; }
        public int GetCalls { get; private set; }
        public int SaveCalls { get; private set; }

        public Task<Delivery?> GetAsync(DeliveryId deliveryId, CancellationToken cancellationToken)
        {
            GetCalls++;
            return Task.FromResult(Delivery);
        }

        public Task<SaveDeliveryResult> SaveAsync(
            Delivery delivery,
            int expectedVersion,
            CancellationToken cancellationToken)
        {
            SaveCalls++;
            Delivery = delivery;
            return Task.FromResult(SaveDeliveryResult.Saved);
        }
    }

    private sealed class FakeAvailability : ICourierAvailability
    {
        public CourierAvailability Result { get; set; } = CourierAvailability.Available;
        public int Calls { get; private set; }

        public Task<CourierAvailability> CheckAsync(
            CourierId courierId,
            RouteId routeId,
            DateTimeOffset checkedAt,
            CancellationToken cancellationToken)
        {
            Calls++;
            return Task.FromResult(Result);
        }
    }

    private sealed class FakeAuthorization : IAuthorizationService
    {
        public bool CanAssignCourier(ActorContext actor, DeliveryId deliveryId) =>
            actor.Role == "operator";
    }

    private sealed class FakePublisher : IDomainEventPublisher
    {
        public int Calls { get; private set; }
        public bool Result { get; set; } = true;
        public Task<bool> PublishPendingAsync(DeliveryId deliveryId, CancellationToken cancellationToken)
        {
            Calls++;
            return Task.FromResult(Result);
        }
    }

    private sealed class FakeIdempotency : IIdempotencyStore
    {
        private readonly Dictionary<Guid, AssignCourierResult> _results = [];
        public Task<AssignCourierResult?> GetAsync(Guid requestId, CancellationToken cancellationToken) =>
            Task.FromResult(_results.GetValueOrDefault(requestId));

        public Task StoreAsync(Guid requestId, AssignCourierResult result, CancellationToken cancellationToken)
        {
            _results.TryAdd(requestId, result);
            return Task.CompletedTask;
        }
    }

    private sealed class FakeClock : IClock
    {
        public DateTimeOffset UtcNow => Now;
    }

    private sealed class FakeTelemetry : IAssignmentTelemetry
    {
        public IDisposable Start(Guid requestId, DeliveryId deliveryId) => EmptyDisposable.Instance;
        public void RecordSuccess() { }
        public void RecordRejection(string code) { }
        public void RecordPublicationPending() { }

        private sealed class EmptyDisposable : IDisposable
        {
            public static readonly EmptyDisposable Instance = new();
            public void Dispose() { }
        }
    }
}

