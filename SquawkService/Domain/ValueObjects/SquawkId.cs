namespace ParrotInc.SquawkService.Domain.ValueObjects;

public readonly record struct SquawkId(Guid Value)
{
    public static SquawkId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();
}
