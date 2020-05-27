using System;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Mobile.Hosting
{
    internal sealed class StartupMethods
    {
        public StartupMethods(object instance, Func<IServiceCollection, IServiceProvider> configureServices)
        {
            StartupInstance = instance;
            ConfigureServicesDelegate = configureServices;
        }

        public object StartupInstance { get; }
        public Func<IServiceCollection, IServiceProvider> ConfigureServicesDelegate { get; }
    }
}
