using ParrotInc.SquawkService.Domain.Entities;

namespace ParrotInc.SquawkService.Application.Services
{
    public class SquawkAppService : ISquawkAppService
    {
        private readonly ISquawkDomainService _squawkDomainService;

        public SquawkAppService(ISquawkDomainService squawkDomainService)
        {
            _squawkDomainService = squawkDomainService;
        }

        public async Task<Squawk> CreateSquawkAsync(Guid userId, string content)
        {
            // Use the domain service to create a new Squawk
            var squawk = await _squawkDomainService.CreateSquawkAsync(userId, content);

            return squawk;
        }
    }
}
