using Example.Mobile.Infrastructure.Commands;
using System;

namespace Example.Mobile.Infrastructure.Events
{
    public abstract class Event : IEvent
    {
        public Guid Id { get; protected set; }
        public string Subject { get; protected set; }
        public Guid? CorrelationId { get; protected set; }
        public Guid? CausationId { get; protected set; }
        public DateTimeOffset Timestamp { get; protected set; }
        public string UserId { get; protected set; }

        public Event(string subject)
        {
            Id = Guid.NewGuid();
            Subject = subject;
            Timestamp = DateTimeOffset.UtcNow;
        }

        public void UpdateFrom(ICommand command)
        {
            CorrelationId = command.CorrelationId;
            CausationId = command.Id;
            UserId = command.UserId;
        }
    }
}
