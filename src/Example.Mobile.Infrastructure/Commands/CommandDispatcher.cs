using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Example.Mobile.Infrastructure.Commands
{
    internal sealed class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICommandStore _commandStore;        

        public CommandDispatcher(IServiceProvider serviceProvider, ICommandStore commandStore)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));
            if (commandStore == null)
                throw new ArgumentNullException(nameof(commandStore));

            _commandStore = commandStore;
            _serviceProvider = serviceProvider;
        }

        public async Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();

            if (handler == null)
                throw new InvalidOperationException($"No command handler for type '{typeof(TCommand)}' has been registered.");

            await handler.ExecuteAsync(command);

            await _commandStore.SaveAsync(command);
        }
    }
}
