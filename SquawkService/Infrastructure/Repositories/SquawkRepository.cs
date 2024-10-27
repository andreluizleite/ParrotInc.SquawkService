using ParrotInc.SquawkService.Domain.Entities;
using ParrotInc.SquawkService.Domain.Interfaces;
using System.Collections.Concurrent;

namespace ParrotInc.SquawkService.Infrastructure.Repositories
{
    public class SquawkRepository : ISquawkRepository
    {
        private readonly ConcurrentDictionary<SquawkId, Squawk> _squawks = new();

        public Task<Squawk> GetByIdAsync(SquawkId id)
        {
            _squawks.TryGetValue(id, out var squawk);
            return Task.FromResult(squawk);
        }

        public Task<IEnumerable<Squawk>> GetAllAsync()
        {
            var allSquawks = _squawks.Values;
            return Task.FromResult<IEnumerable<Squawk>>(allSquawks);
        }

        public Task AddAsync(Squawk squawk)
        {
            _squawks[squawk.Id] = squawk;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Squawk squawk)
        {
            if (_squawks.ContainsKey(squawk.Id))
            {
                _squawks[squawk.Id] = squawk;
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(SquawkId id)
        {
            _squawks.TryRemove(id, out _);
            return Task.CompletedTask;
        }
    }
}
