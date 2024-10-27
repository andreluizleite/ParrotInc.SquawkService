using ParrotInc.SquawkService.Domain.Events;
using ParrotInc.SquawkService.Infrastructure.Entity;

public interface IEventDatabaseService
{
    Task SaveEventAsync(SquawkCreatedEvent squawkEvent);
}

public class EventDatabaseService : IEventDatabaseService
{
    private readonly MyDbContext _dbContext;

    public EventDatabaseService(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveEventAsync(SquawkCreatedEvent squawkEvent)
    {
        var eventEntity = new SquawkEventEntity
        {
            SquawkId = squawkEvent.SquawkId.ToString(),
            Content = squawkEvent.Content,
            UserId = squawkEvent.UserId,
            CreatedAt = squawkEvent.CreatedAt,
            OccurredOn = squawkEvent.OccurredOn
        };

        _dbContext.SquawkEvents.Add(eventEntity);
        await _dbContext.SaveChangesAsync();
    }
}
