using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Example.Mobile.Infrastructure.Commands
{
    public interface ICommandStore
    {
        Task SaveAsync(ICommand command);
    }
}
