using System;
using Example.Mobile.Infrastructure.Events;
using Example.Mobile.Infrastructure.Serialization;

namespace Example.Mobile.EntityFrameworkCore.Events
{
    internal sealed class EntityFrameworkCoreEventModelFactory : IEventModelFactory
    {
        private readonly IEventSerializer _eventSerializer;

        public EntityFrameworkCoreEventModelFactory(IEventSerializer eventSerializer)
        {
            if (eventSerializer == null)
                throw new ArgumentNullException(nameof(eventSerializer));

            _eventSerializer = eventSerializer;
        }

        public Entities.Event Create(IEvent @event)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            var type = @event.GetType();

            return new Entities.Event
            {
                Subject = @event.Subject,
                CorrelationId = @event.CorrelationId,
                CausationId = @event.CausationId,
                Data = _eventSerializer.Serialize(@event),
                Id = @event.Id,
                Name = type.Name,
                Type = type.FullName,
                Timestamp = @event.Timestamp,
                UserId = @event.UserId,
            };
        }
    }
}
