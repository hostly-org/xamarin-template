using System;
using System.Collections.Generic;
using System.Text;

namespace Example.Mobile.Infrastructure.Events
{
    public interface IEventTypeCache
    {
        bool TryGet(string name, out Type type);
    }
}
