using System;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Mobile.Hosting
{
    internal interface IServiceFactoryAdapter
    {
        object CreateBuilder(IServiceCollection services);
        IServiceProvider CreateServiceProvider(object containerBuilder);
    }
}
