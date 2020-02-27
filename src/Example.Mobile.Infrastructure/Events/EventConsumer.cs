using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Example.Mobile.Infrastructure.Events
{
    public abstract class EventConsumer : IEventConsumer
    {
        private readonly Dictionary<Type, Func<IEvent, Task>> _handlers;

        public Guid ConsumerId { get; }

        protected EventConsumer()
        {
            ConsumerId = Guid.NewGuid();
            _handlers = new Dictionary<Type, Func<IEvent, Task>>();
        }

        protected void Handles<TEvent>(Func<TEvent, Task> handler)
            where TEvent : IEvent
        {
            _handlers.Add(typeof(TEvent), e => handler((TEvent)e));
        }

        public async Task HandleAsync(IEvent @event)
        {
            if (_handlers.TryGetValue(@event.GetType(), out var handler))
                await handler(@event);
        }
    }

    public class EventConsumer<TEvent> : EventConsumer
        where TEvent : IEvent
    {
        private readonly Action<TEvent> _callback;
        private readonly Func<TEvent, Task> _asyncCallback;

        public EventConsumer(Action<TEvent> callback)
        {
            _callback = callback;

            Handles<TEvent>(Consume);
        }

        public EventConsumer(Func<TEvent, Task> asyncCallback)
        {
            _asyncCallback = asyncCallback;

            Handles<TEvent>(ConsumeAsync);
        }

        private async Task ConsumeAsync(TEvent @event)
        {
            await _asyncCallback(@event);
        }

        private Task Consume(TEvent @event)
        {
            _callback(@event);
            return Task.CompletedTask;
        }
    }
}
