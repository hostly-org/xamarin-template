using System.Threading.Tasks;

namespace Example.Mobile.Infrastructure.Events
{
    public interface IEventBusPublisher
    {
        ValueTask<bool> PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}
