using ParrotInc.SquawkService.Domain.Entities;

namespace ParrotInc.SquawkService.Domain.Interfaces
{
    public interface ISquawkRepository
    {
        Task<Squawk> GetByIdAsync(SquawkId id);
        Task<IEnumerable<Squawk>> GetAllAsync();
        Task AddAsync(Squawk squawk);
        Task UpdateAsync(Squawk squawk);
        Task DeleteAsync(SquawkId id);
    }
}
