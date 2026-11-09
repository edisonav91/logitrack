namespace LogiTrack.Domain;

public readonly record struct DeliveryId(Guid Value)
{
    public static DeliveryId Create() => new(Guid.NewGuid());

    public static DeliveryId Parse(string value) =>
        Guid.TryParse(value, out var id) && id != Guid.Empty
            ? new DeliveryId(id)
            : throw new ArgumentException("DeliveryId must be a non-empty GUID.", nameof(value));

    public override string ToString() => Value.ToString();
}

public readonly record struct OrderId(Guid Value)
{
    public override string ToString() => Value.ToString();
}

public readonly record struct RouteId(Guid Value)
{
    public override string ToString() => Value.ToString();
}

public readonly record struct CourierId(Guid Value)
{
    public static CourierId Parse(string value) =>
        Guid.TryParse(value, out var id) && id != Guid.Empty
            ? new CourierId(id)
            : throw new ArgumentException("CourierId must be a non-empty GUID.", nameof(value));

    public override string ToString() => Value.ToString();
}

public readonly record struct ActorId(string Value)
{
    public static ActorId Create(string value) =>
        !string.IsNullOrWhiteSpace(value)
            ? new ActorId(value.Trim())
            : throw new ArgumentException("ActorId is required.", nameof(value));

    public override string ToString() => Value;
}

