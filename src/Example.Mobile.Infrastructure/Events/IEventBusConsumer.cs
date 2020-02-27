using System;

namespace Example.Mobile.Infrastructure.Events
{
    public interface IEventBusConsumer
    {
        IDisposable Register(IEventConsumer consumer);
    }
}
