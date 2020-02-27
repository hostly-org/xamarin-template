using System;
using System.Threading.Tasks;

namespace Example.Mobile.Infrastructure.Queries
{
    internal sealed class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IQueryStore _queryStore;

        public QueryDispatcher(IServiceProvider serviceProvider,
            IQueryStore queryStore)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));
            if (queryStore == null)
                throw new ArgumentNullException(nameof(queryStore));
            _serviceProvider = serviceProvider;
            _queryStore = queryStore;
        }


        public async Task<TQueryResult> DispatchAsync<TQueryResult>(IQuery<TQueryResult> query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var type = query.GetType();

            var handler = _serviceProvider.GetService(typeof(IQueryHandler<,>).MakeGenericType(type, typeof(TQueryResult)));

            if (handler == null)
                throw new InvalidOperationException($"No query handler for type '{type}' has been registered.");
            try
            {
                var result = await (Task<TQueryResult>)handler.GetType().GetMethod("RetrieveAsync").Invoke(handler, new[] { query });
                await _queryStore.SaveAsync(query);

                return result;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
