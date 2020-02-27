using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Example.Mobile.EntityFrameworkCore.DbContexts;
using Example.Mobile.Infrastructure.Queries;
using Example.Mobile.Infrastructure.Serialization;

namespace Example.Mobile.EntityFrameworkCore.Stores
{
    internal sealed class EntityFrameworkCoreQueryStore : IQueryStore
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IQuerySerializer _querySerializer;
        private readonly ILogger<EntityFrameworkCoreQueryStore> _logger;

        public EntityFrameworkCoreQueryStore(IDbContextFactory dbContextFactory,
            IQuerySerializer querySerializer,
            ILogger<EntityFrameworkCoreQueryStore> logger)
        {
            if (dbContextFactory == null)
                throw new ArgumentNullException(nameof(dbContextFactory));
            if (querySerializer == null)
                throw new ArgumentNullException(nameof(querySerializer));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _dbContextFactory = dbContextFactory;
            _querySerializer = querySerializer;
            _logger = logger;
        }

        public async Task SaveAsync<TQueryResult>(IQuery<TQueryResult> query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var type = query.GetType();
            var data = _querySerializer.Serialize(query);

            using (var context = _dbContextFactory.Create())
            {
                await context.Queries.AddAsync(new Entities.Query
                {
                    Name = type.Name,
                    Type = type.FullName,
                    Data = data,
                    Id = query.Id,
                    CorrelationId = query.CorrelationId,
                    Timestamp = query.Timestamp,
                    UserId = query.UserId,
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
