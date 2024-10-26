using System;

namespace ParrotInc.SquawkService.Domain.ValueObjects
{
    public record SquawkMetadata
    {
        public Guid UserId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public SquawkMetadata(Guid userId)
        {
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
