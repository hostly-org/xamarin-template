using System;
using System.Threading.Tasks;

namespace Example.Mobile.Infrastructure.Events
{
    public interface IEventConsumer
    {
        Guid ConsumerId { get; }
        Task HandleAsync(IEvent @event);
    }
}
