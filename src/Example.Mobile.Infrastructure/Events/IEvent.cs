using Example.Mobile.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Example.Mobile.Infrastructure.Events
{
    public interface IEvent
    {
        Guid Id { get; }
        string Subject { get; }
        Guid? CorrelationId { get; }
        Guid? CausationId { get; }
        DateTimeOffset Timestamp { get; }
        string UserId { get; }

        void UpdateFrom(ICommand command);
    }
}
