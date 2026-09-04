using System.Collections.Concurrent;
using ParrotInc.SquawkService.Domain.Interfaces;

namespace ParrotInc.SquawkService.Infrastructure.EventPublishing;

public sealed class InMemoryEventPublisher : IEventPublisher
{
    private readonly ConcurrentQueue<IDomainEvent> _publishedEvents = new();

    public Task PublishAsync(
        IDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _publishedEvents.Enqueue(domainEvent);

        return Task.CompletedTask;
    }

    public IReadOnlyCollection<IDomainEvent> GetPublishedEvents()
    {
        return _publishedEvents.ToArray();
    }
}
