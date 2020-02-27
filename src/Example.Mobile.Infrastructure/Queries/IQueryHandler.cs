using System.Threading.Tasks;

namespace Example.Mobile.Infrastructure.Queries
{
    public interface IQueryHandler<TRequest, TResult>
        where TRequest : IQuery<TResult>
        where TResult : IQueryResult
    {
        Task<TResult> RetrieveAsync(TRequest query);
    }
}
