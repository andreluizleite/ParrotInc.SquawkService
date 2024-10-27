namespace ParrotInc.SquawkService.Domain.Interfaces
{
    public interface IEventPublisher
    {
        Task Publish<TEvent>(IEnumerable<TEvent> events) where TEvent : IDomainEvent;

        void RegisterEventHandler(Func<IDomainEvent, Task> handler);
    }
}
