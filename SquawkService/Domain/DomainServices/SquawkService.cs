using ParrotInc.SquawkService.Domain.Entities;
using ParrotInc.SquawkService.Domain.Interfaces;
using ParrotInc.SquawkService.Specifications;

public class SquawkService
{
    private readonly ISquawkRepository _squawkRepository;
    private readonly IEventPublisher _eventPublisher;

    public SquawkService(ISquawkRepository squawkRepository, IEventPublisher eventPublisher)
    {
        _squawkRepository = squawkRepository;
        _eventPublisher = eventPublisher;
    }

    public Squawk CreateSquawk(Guid userId, string content)
    {
        var specifications = new List<ISquawkSpecification>
        {
            new SquawkContentSpecification(),
            new SquawkLengthSpecification(400) //put it on settings
        };

        return Squawk.CreateSquawk(userId, content, specifications, _eventPublisher);
    }
}
