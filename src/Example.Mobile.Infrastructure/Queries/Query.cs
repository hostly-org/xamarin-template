using System;

namespace Example.Mobile.Infrastructure.Queries
{
    public abstract class Query<TQueryResult> : IQuery<TQueryResult>
    {
        public Guid Id { get; }
        public DateTimeOffset Timestamp { get; }
        public Guid CorrelationId { get; }
        public string UserId { get; }

        public Query(Guid correlationId, string userId)
        {
            Id = Guid.NewGuid();
            Timestamp = DateTimeOffset.UtcNow;
            CorrelationId = correlationId;
            UserId = userId;
        }
    }
}
