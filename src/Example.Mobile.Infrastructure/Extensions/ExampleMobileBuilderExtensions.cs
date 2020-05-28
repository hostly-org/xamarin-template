using Example.Mobile.Infrastructure.Commands;
using Example.Mobile.Infrastructure.Events;
using Example.Mobile.Infrastructure.Queries;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;

namespace Example.Mobile.Infrastructure.Extensions
{
    public static class ExampleMobileBuilderExtensions
    {
        public static IExampleMobileBuilder AddInfrastructure(this IExampleMobileBuilder builder)
        {            
            builder.Services.TryAdd(GetServices());
            builder.Services.AddHostedService<EventBus>();

            return builder;
        }

        private static IEnumerable<ServiceDescriptor> GetServices()
        {
            yield return ServiceDescriptor.Transient<IQueryDispatcher, QueryDispatcher>();
            yield return ServiceDescriptor.Transient<ICommandDispatcher, CommandDispatcher>();

            foreach (var descriptor in ServiceDescriptorExtensions.Singleton<IEventBusPublisher, IEventBusConsumer, EventBus>())
                yield return descriptor;

            yield return ServiceDescriptor.Singleton<IConnectivityProvider, ConnectivityProvider>();
        }
    }
}
