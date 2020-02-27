using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Example.Mobile.Domain.Tests")]
[assembly: InternalsVisibleTo("Example.Mobile.Infrastructure.Tests")]
namespace Example.Mobile.Infrastructure.Store
{
    public abstract class Module : IModule
    {
        internal readonly Dictionary<Type, Action<IMutation>> _mutations;
        internal readonly Dictionary<Type, Func<IAction, Task>> _actions;

        public Module()
        {
            _mutations = new Dictionary<Type, Action<IMutation>>();
            _actions = new Dictionary<Type, Func<IAction, Task>>();
        }

        protected void Mutates<TMutation>(Action<TMutation> mutation)
            where TMutation : IMutation
        {
            _mutations.Add(typeof(TMutation), e => mutation((TMutation)e));
        }

        protected void Executes<TAction>(Func<TAction, Task> handler)
            where TAction : IAction
        {
            _actions.Add(typeof(TAction), e => handler((TAction)e));
        }

        public void Commit(IMutation mutation)
        {
            if (_mutations.TryGetValue(mutation.GetType(), out var handler))
                handler(mutation);
            else
                throw new UnRegisteredMutationException($"{mutation.GetType().Name} is not registered on the {GetType().Name} Module");
        }

        public async Task<bool> DispatchAsync(IAction action)
        {
            if (_actions.TryGetValue(action.GetType(), out var handler))
            {
                await handler(action);
                return true;
            }

            return false;
        }
    }
}
