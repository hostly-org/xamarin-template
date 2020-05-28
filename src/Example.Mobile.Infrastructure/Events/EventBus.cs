using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Example.Mobile.Infrastructure.Events
{
    public class EventBus : IEventBusPublisher, IEventBusConsumer, IHostedService
    {
        private readonly CancellationTokenSource _stoppingCts;
        private readonly List<IEventConsumer> _consumers;
        private readonly IEventStore _eventStore;
        private readonly Channel<IEvent> _channel;
        private readonly ChannelReader<IEvent> _channelReader;
        private readonly ChannelWriter<IEvent> _channelWriter;

        private Task _executingTask;        
        public EventBus(IEventStore eventStore)
        {
            if (eventStore == null)
                throw new ArgumentNullException(nameof(eventStore));

            _stoppingCts = new CancellationTokenSource();
            _consumers = new List<IEventConsumer>();
            _channel = Channel.CreateUnbounded<IEvent>();
            _channelReader = _channel.Reader;
            _channelWriter = _channel.Writer;
            _eventStore = eventStore;
        }


        public ValueTask<bool> PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
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

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            _executingTask = ExecuteAsync(_stoppingCts.Token);

            if (_executingTask.IsCompleted)
                return _executingTask;

            return Task.CompletedTask;
        }
        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            if (_executingTask == null)
                return;

            try
            {
                _stoppingCts.Cancel();
            }
            finally
            {
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }

        private async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            while(!cancellationToken.IsCancellationRequested)
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
