namespace ParrotInc.SquawkService.Domain.Interfaces;

public interface IDomainEvent
{
    DateTimeOffset OccurredOn { get; }
}
