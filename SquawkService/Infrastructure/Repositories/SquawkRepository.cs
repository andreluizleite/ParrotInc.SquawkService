using System.Collections.Concurrent;
using ParrotInc.SquawkService.Domain.Entities;
using ParrotInc.SquawkService.Domain.Interfaces;
using ParrotInc.SquawkService.Domain.ValueObjects;

namespace ParrotInc.SquawkService.Infrastructure.Repositories;

public sealed class SquawkRepository : ISquawkRepository
{
    private readonly ConcurrentDictionary<SquawkId, Squawk> _squawks = new();

    public Task<Squawk?> GetByIdAsync(
        SquawkId id,
        CancellationToken cancellationToken = default)
    {
        _squawks.TryGetValue(id, out var squawk);
        return Task.FromResult(squawk);
    }

    public Task<IReadOnlyCollection<Squawk>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<Squawk> squawks = _squawks.Values
            .OrderByDescending(squawk => squawk.Metadata.CreatedAt)
            .ToArray();

        return Task.FromResult(squawks);
    }

    public Task AddAsync(
        Squawk squawk,
        CancellationToken cancellationToken = default)
    {
        if (!_squawks.TryAdd(squawk.Id, squawk))
        {
            throw new InvalidOperationException($"Squawk {squawk.Id} already exists.");
        }

        return Task.CompletedTask;
    }
}
