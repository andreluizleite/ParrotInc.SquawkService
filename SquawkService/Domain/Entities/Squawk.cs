using ParrotInc.SquawkService.Domain.Events;
using ParrotInc.SquawkService.Domain.Interfaces;
using ParrotInc.SquawkService.Domain.ValueObjects;
using ParrotInc.SquawkService.Specifications;

namespace ParrotInc.SquawkService.Domain.Entities
{
    public class Squawk
    {
        public SquawkId Id { get; private set; }
        public string Content { get; private set; }
        public SquawkMetadata Metadata { get; private set; }

        private Squawk(SquawkId id, string content, SquawkMetadata metadata)
        {
            Id = id;
            Metadata = metadata;
            Content = content;
        }
        public static Squawk CreateSquawk(Guid userId, string content, IEnumerable<ISquawkSpecification> squawkSpecifications, IEventPublisher eventPublisher)
        {
            foreach (var specification in squawkSpecifications)
            {
                if (!specification.IsSatisfiedBy(content))
                    throw new ArgumentNullException(nameof(content), "Content cannot be empty.");
            }

            var squawkId = new SquawkId();
            var metadata = new SquawkMetadata(userId);

            // Publish the event
            var squawkCreatedEvent = new SquawkCreatedEvent(squawkId, content, userId);
            eventPublisher.Publish(squawkCreatedEvent);

            return new Squawk(squawkId, content, metadata); 
        }
    }
}
