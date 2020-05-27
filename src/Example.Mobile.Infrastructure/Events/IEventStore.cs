using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Example.Mobile.Infrastructure.Events
{
    public interface IEventStore
    {
        Task SaveAsync(IEnumerable<IEvent> events);
        Task SaveAsync(IEvent @event);
    }
}
