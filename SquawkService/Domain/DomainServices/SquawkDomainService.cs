using ParrotInc.SquawkService.Domain.Entities;
using ParrotInc.SquawkService.Domain.Interfaces;
using ParrotInc.SquawkService.Domain.Specifications;

namespace ParrotInc.SquawkService.Domain.Services
{
    public class SquawkDomainService : ISquawkDomainService
    {
        private readonly ISquawkRepository _squawkRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly CompositeSquawkSpecification _compositeSpecification;

        public SquawkDomainService(
            ISquawkRepository squawkRepository,
            IEventPublisher eventPublisher,
            CompositeSquawkSpecification compositeSpecification)
        {
            _squawkRepository = squawkRepository;
            _eventPublisher = eventPublisher;
            _compositeSpecification = compositeSpecification;
        }

        public async Task<Squawk> CreateSquawkAsync(Guid userId, string content)
        {

            // Validate content against the composite specification
            if (!_compositeSpecification.IsSatisfiedBy(content))
            {
                throw new ArgumentException("Content does not satisfy specifications.", nameof(content));
            }

            // Create the Squawk with the specified content and user ID
            var squawk = await Squawk.CreateSquawkAsync(userId, content, _eventPublisher);

            // Save the Squawk to the repository
            await _squawkRepository.AddAsync(squawk);

            return squawk;
        }
    }
}
