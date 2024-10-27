using ParrotInc.SquawkService.Domain.Entities;
using ParrotInc.SquawkService.Domain.Interfaces;
using ParrotInc.SquawkService.Domain.Interfaces.ParrotInc.SquawkService.Domain.Services;
using ParrotInc.SquawkService.Domain.Specifications;

namespace ParrotInc.SquawkService.Domain.Services
{
    public class SquawkDomainService : ISquawkDomainService
    {
        private readonly ISquawkRepository _squawkRepository;
        private readonly ICacheService _cacheService;
        private readonly IEventPublisher _eventPublisher;
        private readonly CompositeSquawkSpecification _compositeSpecification;

        public SquawkDomainService(
            ISquawkRepository squawkRepository,
            ICacheService cacheService,
            IEventPublisher eventPublisher,
            CompositeSquawkSpecification compositeSpecification)
        {
            _squawkRepository = squawkRepository;
            _cacheService = cacheService;
            _eventPublisher = eventPublisher;
            _compositeSpecification = compositeSpecification;
        }

        public async Task<Squawk> CreateSquawkAsync(Guid userId, string content)
        {

            if (!_compositeSpecification.IsSatisfiedBy(content))
            {
                throw new ArgumentException("Content does not satisfy specifications.", nameof(content));
            }

            var expirationDate = DateTime.UtcNow.AddDays(1);
            var hashKey = Squawk.GenerateHash(userId, content);

            if (IsDuplicateSquawk(hashKey))
            {
                throw new ArgumentException("The Squawk is duplicated.", nameof(content));
            }
            CacheSquawk(hashKey, expirationDate);

            var squawk = await Squawk.CreateSquawkAsync(userId, content, _eventPublisher);

            // Save the Squawk to the repository
            //await _squawkRepository.AddAsync(squawk);

            return squawk;
        }
        private bool IsDuplicateSquawk(string hashKey)
        {
            return _cacheService.Get(hashKey) != null;
        }
        private void CacheSquawk(string hashKey, DateTime expirationDate)
        {
            _cacheService.Set(hashKey, expirationDate.ToLongTimeString());
        }
    }
}
