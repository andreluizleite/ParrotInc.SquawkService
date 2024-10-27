using ParrotInc.SquawkService.Domain.Entities;
namespace ParrotInc.SquawkService.Application.Interfaces
{
    public interface IUserPostPolicyService
    {
        Task<bool> CanUserPostAsync(User user);
    }
}
