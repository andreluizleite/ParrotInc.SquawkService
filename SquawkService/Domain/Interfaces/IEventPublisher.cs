namespace ParrotInc.SquawkService.Domain.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync(
        IDomainEvent domainEvent,
        CancellationToken cancellationToken = default);
}
