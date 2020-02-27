using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Example.Mobile.Infrastructure.Events
{
    public interface IEventStore
    {
        Task<Page> GetEventsAsync(long offset);
        Task<IEnumerable<IEvent>> GetEventsAsync(string subject);
        Task<IEnumerable<IEvent>> GetEventsAsync(string subject, long offset);
        Task SaveAsync(IEnumerable<IEvent> events);
        Task SaveAsync(IEvent @event);
        Task<long> CountAsync(string subject);
    }
}
