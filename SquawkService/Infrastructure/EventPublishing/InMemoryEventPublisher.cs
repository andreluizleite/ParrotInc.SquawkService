using ParrotInc.SquawkService.Domain.Interfaces;
using System.Collections.Concurrent;

namespace ParrotInc.SquawkService.Infrastructure.EventPublishing
{
    public class InMemoryEventPublisher : IEventPublisher
    {
        private readonly ConcurrentQueue<IDomainEvent> _eventQueue = new ConcurrentQueue<IDomainEvent>();
        private readonly List<Func<IDomainEvent, Task>> _eventHandlers = new List<Func<IDomainEvent, Task>>();
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public InMemoryEventPublisher()
        {
            Task.Run(() => ProcessEventsAsync(_cts.Token));
        }

        public Task Publish<TEvent>(IEnumerable<TEvent> events) where TEvent : IDomainEvent
        {
            foreach (var domainEvent in events)
            {
                _eventQueue.Enqueue(domainEvent);
            }

            return Task.CompletedTask;
        }

        public void RegisterEventHandler(Func<IDomainEvent, Task> handler)
        {
            _eventHandlers.Add(handler);
        }

        private async Task ProcessEventsAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                while (_eventQueue.TryDequeue(out var domainEvent))
                {
                    foreach (var handler in _eventHandlers)
                    {
                        try
                        {
                            await handler(domainEvent);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error processing event: {ex.Message}");
                        }
                    }
                }

                await Task.Delay(500, cancellationToken);
            }
        }
    }
}
