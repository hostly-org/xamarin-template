using System.Threading.Tasks;

namespace Example.Mobile.Infrastructure.Commands
{
    public interface ICommandDispatcher
    {
        Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
