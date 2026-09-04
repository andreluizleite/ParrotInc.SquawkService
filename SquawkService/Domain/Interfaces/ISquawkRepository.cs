using ParrotInc.SquawkService.Domain.Entities;
using ParrotInc.SquawkService.Domain.ValueObjects;

namespace ParrotInc.SquawkService.Domain.Interfaces;

public interface ISquawkRepository
{
    Task<Squawk?> GetByIdAsync(
        SquawkId id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Squawk>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Squawk squawk,
        CancellationToken cancellationToken = default);
}
