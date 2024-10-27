using ParrotInc.SquawkService.Domain.Entities;
public interface ISquawkAppService
{
    Task<Squawk> CreateSquawkAsync(Guid userId, string content);
}
