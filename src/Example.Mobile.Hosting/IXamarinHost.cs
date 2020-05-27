using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Hosting;

namespace Example.Mobile.Hosting
{
    public interface IXamarinHost : IHost
    {
        void Run(object caller);
    }
}
