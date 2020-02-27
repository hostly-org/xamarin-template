using Microsoft.Extensions.DependencyInjection;
using System;

namespace Example.Mobile.Infrastructure
{
    internal class ExampleMobileBuilder : IExampleMobileBuilder
    {
        public IServiceCollection Services { get; }

        public ExampleMobileBuilder(IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            Services = services;
        }
    }
}
