using System.Threading.Tasks;

namespace Example.Mobile.Infrastructure.Store
{
    public interface IModule
    {
        Task<bool> DispatchAsync(IAction action);
        void Commit(IMutation mutation);
    }
}
