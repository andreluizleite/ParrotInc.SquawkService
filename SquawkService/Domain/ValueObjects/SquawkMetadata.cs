namespace ParrotInc.SquawkService.Domain.ValueObjects;

public sealed record SquawkMetadata(Guid UserId, DateTimeOffset CreatedAt);
