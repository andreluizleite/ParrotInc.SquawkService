using ParrotInc.SquawkService.Domain.Entities;
public interface ISquawkDomainService
{
    Task<Squawk> CreateSquawkAsync(Guid userId, string content);
}
