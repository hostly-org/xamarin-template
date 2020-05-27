using System.Collections.Generic;
using Example.Mobile.Infrastructure;
using Example.Mobile.Infrastructure.Serialization;
using Example.Mobile.Serialization.UTF8Json.Command;
using Example.Mobile.Serialization.UTF8Json.Event;
using Example.Mobile.Serialization.UTF8Json.Query;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Example.Mobile.Serialization.UTF8Json.Extensions
{
    public static class ExampleMobileBuilderExtensions
    {
        public static IExampleMobileBuilder AddUTF8JsonSerialization(this IExampleMobileBuilder builder)
        {
            builder.Services.TryAdd(GetServices());

            return builder;
        }

        private static IEnumerable<ServiceDescriptor> GetServices()
        {
            yield return ServiceDescriptor.Transient<ICommandDeserializer, CommandDeserializer>();
            yield return ServiceDescriptor.Singleton<ICommandSerializer, CommandSerializer>();
            yield return ServiceDescriptor.Transient<IEventDeserializer, EventDeserializer>();
            yield return ServiceDescriptor.Transient<IEventSerializer, EventSerializer>();
            yield return ServiceDescriptor.Transient<IQueryDeserializer, QueryDeserializer>();
            yield return ServiceDescriptor.Transient<IQuerySerializer, QuerySerializer>();
        }
    }
}
