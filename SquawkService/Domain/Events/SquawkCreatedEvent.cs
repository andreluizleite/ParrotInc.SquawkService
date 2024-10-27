using ParrotInc.SquawkService.Domain.Entities;
using ParrotInc.SquawkService.Domain.Interfaces;

namespace ParrotInc.SquawkService.Domain.Events
{
    public class SquawkCreatedEvent : IDomainEvent
    {
        public SquawkId SquawkId { get; }
        public string Content { get; }
        public Guid UserId { get; }
        public DateTime CreatedAt { get; }
        public DateTime OccurredOn { get; }

        public SquawkCreatedEvent(SquawkId squawkId, string content, Guid userId)
        {
            SquawkId = squawkId;
            Content = content;
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
