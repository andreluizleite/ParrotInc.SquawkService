using System.Collections.Concurrent;
using ParrotInc.SquawkService.Domain.Interfaces;

namespace ParrotInc.SquawkService.Infrastructure.Repositories;

public sealed class InMemoryCacheService : ICacheService
{
    private readonly ConcurrentDictionary<string, CacheEntry> _entries = new();
    private readonly TimeProvider _timeProvider;

    public InMemoryCacheService(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public bool TryAdd(string key, string value, TimeSpan expiry)
    {
        var replacement = new CacheEntry(value, _timeProvider.GetUtcNow().Add(expiry));

        while (true)
        {
            if (_entries.TryAdd(key, replacement))
            {
                return true;
            }

            if (!_entries.TryGetValue(key, out var existing))
            {
                continue;
            }

            if (existing.ExpiresAt > _timeProvider.GetUtcNow())
            {
                return false;
            }

            if (_entries.TryUpdate(key, replacement, existing))
            {
                return true;
            }
        }
    }

    public void Delete(string key)
    {
        _entries.TryRemove(key, out _);
    }

    private sealed record CacheEntry(string Value, DateTimeOffset ExpiresAt);
}
