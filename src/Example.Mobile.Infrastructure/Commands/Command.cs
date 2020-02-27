using System;
using Example.Mobile.Infrastructure.Events;

namespace Example.Mobile.Infrastructure.Commands
{
    public abstract class Command : ICommand
    {
        public Guid Id { get; }
        public string Subject { get; }
        public Guid CorrelationId { get; }
        public Guid? CausationId { get; protected set; }
        public string UserId { get; }
        public DateTimeOffset Timestamp { get; }

        public Command(string subject, Guid correlationId, string userId)
        {
            Id = Guid.NewGuid();
            Subject = subject;
            CorrelationId = correlationId;
            UserId = userId;
            Timestamp = DateTimeOffset.UtcNow;
        }

        public void UpdateFrom(ICommand command)
        {
            CausationId = command.Id;
        }

        public void UpdateFrom(IEvent @event)
        {
            CausationId = @event.Id;
        }
    }
}
