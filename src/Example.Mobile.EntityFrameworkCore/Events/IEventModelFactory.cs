using Example.Mobile.Infrastructure.Events;

namespace Example.Mobile.EntityFrameworkCore.Events
{
    public interface IEventModelFactory
    {
        Entities.Event Create(IEvent @event);
    }
}
