using ParrotInc.SquawkService.Domain.Interfaces;

namespace ParrotInc.SquawkService.Application.EventPublishers
{
    public class InMemoryEventPublisher : IEventPublisher
    {
        private readonly List<IDomainEvent> _events = new();

        public Task Publish<TEvent>(IEnumerable<TEvent> events) where TEvent : IDomainEvent
        {
            // Add events to the in-memory list
            _events.AddRange(events as IEnumerable<IDomainEvent>);
            return Task.CompletedTask;
        }

        public IReadOnlyList<IDomainEvent> GetPublishedEvents() => _events.AsReadOnly();

        public void RegisterEventHandler(Func<IDomainEvent, Task> handler)
        {
        }
    }
}
