using ParrotInc.SquawkService.Domain.Interfaces;
using ParrotInc.SquawkService.Domain.ValueObjects;

namespace ParrotInc.SquawkService.Domain.Events;

public sealed record SquawkCreatedEvent(
    SquawkId SquawkId,
    string Content,
    Guid UserId,
    DateTimeOffset OccurredOn) : IDomainEvent;
