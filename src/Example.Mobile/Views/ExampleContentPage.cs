using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Example.Mobile.Infrastructure.Events;
using Example.Mobile.Modals;
using Xamarin.Forms;

namespace Example.Mobile.Views
{
    public class ExampleContentPage : ContentPage, IEventConsumer
    {
        private readonly Dictionary<Type, Func<IEvent, Task>> _handlers;
        private readonly IEventBusConsumer _eventBusConsumer;
        private IDisposable _subscription;
        private bool _loading;
        private EventHandler<bool> _loadingChanged;

        public Guid ConsumerId { get; }        

        protected bool Loading
        {
            get { return _loading; }
            set
            {
                var currentValue = _loading;
                _loading = value;

                if (!currentValue && _loading)
                    _loadingChanged.Invoke(this, true);
                else if (currentValue && !_loading)
                    _loadingChanged.Invoke(this, false);
            }
        }

        protected ExampleContentPage(IEventBusConsumer eventBusConsumer)
        {
            if (eventBusConsumer == null)
                throw new ArgumentNullException(nameof(eventBusConsumer));

            ConsumerId = Guid.NewGuid();
            _eventBusConsumer = eventBusConsumer;
            _handlers = new Dictionary<Type, Func<IEvent, Task>>();
            _loadingChanged += async (sender, args) => await ShowLoadingAsync(args);
        }

        protected override void OnAppearing()
        {
            _subscription?.Dispose();
            _subscription = _eventBusConsumer.Register(this);
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            _subscription?.Dispose();
            base.OnDisappearing();
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

        private async Task ShowLoadingAsync(bool show)
        {
            if (show)
                await Navigation.PushModalAsync(new LoadingModal(), false);
            else
                await Navigation.PopModalAsync(false);
        }
    }
}
