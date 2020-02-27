using System.Threading.Tasks;

namespace Example.Mobile.Infrastructure.Queries
{
    public interface IQueryDispatcher
    {
        Task<TQueryResult> DispatchAsync<TQueryResult>(IQuery<TQueryResult> query);
    }
}
