
using ParrotInc.SquawkService.Domain.Entities;

namespace ParrotInc.SquawkService.Domain.Events
{
    public class SquawkCreatedEvent
    {
        public SquawkId SquawkId { get; }
        public string Content { get; }
        public Guid UserId { get; }
        public DateTime CreatedAt { get; }

        public SquawkCreatedEvent(SquawkId squawkId, string content, Guid userId)
        {
            SquawkId = squawkId;
            Content = content;
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
