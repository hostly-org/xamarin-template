using System;
using System.Collections.Generic;

namespace Example.Mobile.Infrastructure.Events
{
    public class Subscription : IDisposable
    {
        private readonly IEventConsumer _consumer;
        private readonly List<IEventConsumer> _consumers;

        public Subscription(List<IEventConsumer> consumers, IEventConsumer consumer)
        {
            _consumers = consumers;
            _consumer = consumer;
        }

        public void Dispose()
        {
            _consumers.RemoveAll(c => c.ConsumerId.Equals(_consumer.ConsumerId));
        }
    }
}
