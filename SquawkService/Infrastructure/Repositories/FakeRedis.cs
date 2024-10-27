using ParrotInc.SquawkService.Domain.Interfaces.ParrotInc.SquawkService.Domain.Services;
using System.Collections.Concurrent;

namespace ParrotInc.SquawkService.Infrastructure.Repositories
{
    public class FakeRedis : ICacheService
    {
        private readonly ConcurrentDictionary<string, (string Value, DateTime Expiry)> _store;

        public FakeRedis()
        {
            _store = new ConcurrentDictionary<string, (string, DateTime)>();
        }

        public void Set(string key, string value, TimeSpan? expiry = null)
        {
            var expirationTime = expiry.HasValue ? DateTime.UtcNow.Add(expiry.Value) : DateTime.MaxValue;
            _store[key] = (value, expirationTime);
        }

        public string Get(string key)
        {
            if (_store.TryGetValue(key, out var entry) && entry.Expiry > DateTime.UtcNow)
            {
                return entry.Value;
            }

            // If expired or not found, remove the key
            _store.TryRemove(key, out _);
            return null;
        }

        public void Delete(string key)
        {
            _store.TryRemove(key, out _);
        }

        public void Clear()
        {
            _store.Clear();
        }
    }
}
