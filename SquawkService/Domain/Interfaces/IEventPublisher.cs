using System;

namespace ParrotInc.SquawkService.Domain.Interfaces
{
    public interface IEventPublisher
    {
        void Publish<T>(T domainEvent) where T : class;
    }
}
