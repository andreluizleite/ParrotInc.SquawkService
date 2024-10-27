using ParrotInc.SquawkService.Domain.Events;

namespace ParrotInc.SquawkService.Domain.Interfaces
{
    public interface IEventDatabaseService
    {
        Task SaveEventAsync(SquawkCreatedEvent squawkEvent);
    }
}
