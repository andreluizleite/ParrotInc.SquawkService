using ParrotInc.SquawkService.Domain.Entities;

namespace ParrotInc.SquawkService.Domain.Interfaces;

public interface ISquawkDomainService
{
    Task<Squawk> CreateSquawkAsync(
        Guid userId,
        string? content,
        CancellationToken cancellationToken = default);
}
