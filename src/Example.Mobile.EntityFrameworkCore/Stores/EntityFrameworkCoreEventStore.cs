using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Example.Mobile.EntityFrameworkCore.DbContexts;
using Example.Mobile.EntityFrameworkCore.Events;
using Example.Mobile.Infrastructure.Events;
using Example.Mobile.Infrastructure.Serialization;
using Microsoft.Extensions.Logging;

namespace Example.Mobile.EntityFrameworkCore.Stores
{
    internal sealed class EntityFrameworkCoreEventStore : IEventStore
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IEventDeserializer _eventDeserializer;
        private readonly IEventModelFactory _eventModelFactory;
        private readonly ILogger<EntityFrameworkCoreEventStore> _logger;

        public EntityFrameworkCoreEventStore(IDbContextFactory dbContextFactory,
                                             IEventDeserializer eventDeserializer,
                                             IEventModelFactory eventModelFactory,
                                             ILogger<EntityFrameworkCoreEventStore> logger)
        {
            if (dbContextFactory == null)
                throw new ArgumentNullException(nameof(dbContextFactory));
            if (eventDeserializer == null)
                throw new ArgumentNullException(nameof(eventDeserializer));
            if (eventModelFactory == null)
                throw new ArgumentNullException(nameof(eventModelFactory));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _dbContextFactory = dbContextFactory;
            _eventDeserializer = eventDeserializer;
            _eventModelFactory = eventModelFactory;
            _logger = logger;
        }

        public async Task SaveAsync(IEnumerable<IEvent> events)
        {
            if (events == null)
                throw new ArgumentNullException(nameof(events));

            using (var context = _dbContextFactory.Create())
            {
                foreach (var @event in events)
                    await context.Events.AddAsync(_eventModelFactory.Create(@event));

                await context.SaveChangesAsync();
            }
        }

        public async Task SaveAsync(IEvent @event)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            using (var context = _dbContextFactory.Create())
            {
                await context.Events.AddAsync(_eventModelFactory.Create(@event));
                await context.SaveChangesAsync();
            }
        }
    }
}
