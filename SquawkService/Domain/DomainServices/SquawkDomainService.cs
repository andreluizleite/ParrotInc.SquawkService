using ParrotInc.SquawkService.Domain.Entities;
using ParrotInc.SquawkService.Domain.Events;
using ParrotInc.SquawkService.Domain.Exceptions;
using ParrotInc.SquawkService.Domain.Interfaces;

namespace ParrotInc.SquawkService.Domain.Services;

public sealed class SquawkDomainService : ISquawkDomainService
{
    public static readonly TimeSpan PostingCooldown = TimeSpan.FromSeconds(20);
    public static readonly TimeSpan DuplicateWindow = TimeSpan.FromHours(24);

    private readonly ISquawkRepository _repository;
    private readonly ICacheService _cache;
    private readonly IEventPublisher _eventPublisher;
    private readonly TimeProvider _timeProvider;

    public SquawkDomainService(
        ISquawkRepository repository,
        ICacheService cache,
        IEventPublisher eventPublisher,
        TimeProvider timeProvider)
    {
        _repository = repository;
        _cache = cache;
        _eventPublisher = eventPublisher;
        _timeProvider = timeProvider;
    }

    public async Task<Squawk> CreateSquawkAsync(
        Guid userId,
        string? content,
        CancellationToken cancellationToken = default)
    {
        var now = _timeProvider.GetUtcNow();
        var squawk = Squawk.Create(userId, content, now);
        var duplicateKey = $"duplicate:{Squawk.GenerateContentHash(userId, squawk.Content.Value)}";
        var cooldownKey = $"cooldown:{userId:N}";

        if (!_cache.TryAdd(duplicateKey, "reserved", DuplicateWindow))
        {
            throw new SquawkRuleViolationException(
                "duplicate_squawk",
                "The same user cannot submit duplicate content within 24 hours.");
        }

        if (!_cache.TryAdd(cooldownKey, "reserved", PostingCooldown))
        {
            _cache.Delete(duplicateKey);

            throw new SquawkRuleViolationException(
                "posting_too_fast",
                "The same user must wait 20 seconds before posting again.");
        }

        try
        {
            await _repository.AddAsync(squawk, cancellationToken);

            await _eventPublisher.PublishAsync(
                new SquawkCreatedEvent(
                    squawk.Id,
                    squawk.Content.Value,
                    squawk.Metadata.UserId,
                    now),
                cancellationToken);

            return squawk;
        }
        catch
        {
            _cache.Delete(duplicateKey);
            _cache.Delete(cooldownKey);
            throw;
        }
    }
}
