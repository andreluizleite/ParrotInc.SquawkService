using ParrotInc.SquawkService.Application.Interfaces;
using ParrotInc.SquawkService.Domain.Entities;

namespace ParrotInc.SquawkService.Application.Services
{
    public class UserPostPolicyService : IUserPostPolicyService
    {
        public async Task<bool> CanUserPostAsync(User user)
        {
            return user.CanPost();
        }
    }
}
