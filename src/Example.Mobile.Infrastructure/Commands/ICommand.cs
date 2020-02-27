using System;

namespace Example.Mobile.Infrastructure.Commands
{
    public interface ICommand
    {
        Guid Id { get; }
        string Subject { get; }
        Guid CorrelationId { get; }
        Guid? CausationId { get; }
        DateTimeOffset Timestamp { get; }
        string UserId { get; }
    }
}
