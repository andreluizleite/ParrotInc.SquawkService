using ParrotInc.SquawkService.Domain.Exceptions;
using ParrotInc.SquawkService.Domain.Services;
using ParrotInc.SquawkService.Infrastructure.EventPublishing;
using ParrotInc.SquawkService.Infrastructure.Repositories;

namespace ParrotInc.SquawkService.Tests.Domain;

public sealed class SquawkDomainServiceTests
{
    private readonly ManualTimeProvider _timeProvider =
        new(new DateTimeOffset(2026, 9, 4, 12, 0, 0, TimeSpan.Zero));

    [Fact]
    public async Task CreateSquawkAsync_ShouldPersistAndPublishTheSquawk()
    {
        var (service, repository, publisher) = CreateService();

        var squawk = await service.CreateSquawkAsync(
            Guid.NewGuid(),
            "A deterministic domain rule example.");

        var persisted = await repository.GetByIdAsync(squawk.Id);

        Assert.NotNull(persisted);
        Assert.Single(publisher.GetPublishedEvents());
    }

    [Fact]
    public async Task CreateSquawkAsync_WithDifferentContentInsideCooldown_ShouldFail()
    {
        var (service, _, _) = CreateService();
        var userId = Guid.NewGuid();

        await service.CreateSquawkAsync(userId, "First note.");

        var exception = await Assert.ThrowsAsync<SquawkRuleViolationException>(
            () => service.CreateSquawkAsync(userId, "Second note."));

        Assert.Equal("posting_too_fast", exception.Code);
    }

    [Fact]
    public async Task CreateSquawkAsync_WithDuplicateContentInsideWindow_ShouldFail()
    {
        var (service, _, _) = CreateService();
        var userId = Guid.NewGuid();

        await service.CreateSquawkAsync(userId, "Original note.");
        _timeProvider.Advance(SquawkDomainService.PostingCooldown.Add(TimeSpan.FromSeconds(1)));

        var exception = await Assert.ThrowsAsync<SquawkRuleViolationException>(
            () => service.CreateSquawkAsync(userId, "Original note."));

        Assert.Equal("duplicate_squawk", exception.Code);
    }

    [Fact]
    public async Task CreateSquawkAsync_AfterCooldown_ShouldAllowDifferentContent()
    {
        var (service, repository, _) = CreateService();
        var userId = Guid.NewGuid();

        await service.CreateSquawkAsync(userId, "First note.");
        _timeProvider.Advance(SquawkDomainService.PostingCooldown.Add(TimeSpan.FromSeconds(1)));
        await service.CreateSquawkAsync(userId, "Second note.");

        var squawks = await repository.GetAllAsync();
        Assert.Equal(2, squawks.Count);
    }

    private (SquawkDomainService Service, SquawkRepository Repository, InMemoryEventPublisher Publisher)
        CreateService()
    {
        var repository = new SquawkRepository();
        var cache = new InMemoryCacheService(_timeProvider);
        var publisher = new InMemoryEventPublisher();
        var service = new SquawkDomainService(repository, cache, publisher, _timeProvider);

        return (service, repository, publisher);
    }

    private sealed class ManualTimeProvider : TimeProvider
    {
        private DateTimeOffset _utcNow;

        public ManualTimeProvider(DateTimeOffset utcNow)
        {
            _utcNow = utcNow;
        }

        public override DateTimeOffset GetUtcNow() => _utcNow;

        public void Advance(TimeSpan duration)
        {
            _utcNow = _utcNow.Add(duration);
        }
    }
}
