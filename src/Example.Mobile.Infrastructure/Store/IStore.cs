using System.Threading.Tasks;

namespace Example.Mobile.Infrastructure.Store
{
    public interface IStore
    {
        Task DispatchAsync(IAction action);
    }
}
