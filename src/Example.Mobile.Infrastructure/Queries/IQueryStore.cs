using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Example.Mobile.Infrastructure.Queries
{
    public interface IQueryStore
    {
        Task SaveAsync<TQueryResult>(IQuery<TQueryResult> query);
    }
}
