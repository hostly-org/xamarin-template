using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Example.Mobile.Infrastructure.Events
{
    public class EventBus : IEventBusPublisher, IEventBusConsumer
    {
        private readonly List<IEventConsumer> _consumers;
        private readonly IEventStore _eventStore;
        private readonly Channel<IEvent> _channel;
        private readonly ChannelReader<IEvent> _channelReader;
        private readonly ChannelWriter<IEvent> _channelWriter;
        private readonly object _lock = new object();

        private Task _executingTask;        
        public EventBus(IEventStore eventStore)
        {
            if (eventStore == null)
                throw new ArgumentNullException(nameof(eventStore));

            _consumers = new List<IEventConsumer>();
            _channel = Channel.CreateUnbounded<IEvent>();
            _channelReader = _channel.Reader;
            _channelWriter = _channel.Writer;
            _eventStore = eventStore;
        }


        public ValueTask<bool> PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            lock(_lock)
            {
                if (_executingTask == null)
                    _executingTask = ExecuteAsync();
            }            

            async Task<bool> AsyncSlowPath(TEvent item)
            {
                while (await _channelWriter.WaitToWriteAsync())
                {
                    if (_channelWriter.TryWrite(item)) 
                        return true;
                }

                return false;
            }

            return _channelWriter.TryWrite(@event) ? new ValueTask<bool>(true) : new ValueTask<bool>(AsyncSlowPath(@event));
        }

        public IDisposable Register(IEventConsumer consumer)
        {
            if (consumer == null)
                throw new ArgumentNullException(nameof(consumer));

            var subscription = new Subscription(_consumers, consumer);

            _consumers.Add(consumer);

            return subscription;
        }

        private async Task ExecuteAsync()
        {
            while(await _channelReader.WaitToReadAsync())
            {
                if (_channelReader.TryRead(out var @event))
                {
                    await _eventStore.SaveAsync(@event);
                    await Task.WhenAll(_consumers.Select(c => c.HandleAsync(@event)));                    
                }
            }
        }
    }
}
