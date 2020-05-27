using System;
using Microsoft.Extensions.DependencyInjection;
using Example.Mobile.EntityFrameworkCore.DbContexts;
using Example.Mobile.EntityFrameworkCore.Events;
using Example.Mobile.EntityFrameworkCore.Stores;
using Example.Mobile.Infrastructure;
using Example.Mobile.Infrastructure.Commands;
using Example.Mobile.Infrastructure.Events;
using Example.Mobile.Infrastructure.Queries;

namespace Example.Mobile.EntityFrameworkCore.Extensions
{
    public static class ExampleMobileBuilderExtensions
    {
        public static IExampleMobileBuilder AddEntityFrameworkCore(this IExampleMobileBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.Services.AddScoped<ICommandStore, EntityFrameworkCoreCommandStore>();
            builder.Services.AddScoped<IEventStore, EntityFrameworkCoreEventStore>();
            builder.Services.AddScoped<IQueryStore, EntityFrameworkCoreQueryStore>();
            builder.Services.AddScoped<IEventModelFactory, EntityFrameworkCoreEventModelFactory>();
            builder.Services.AddScoped<IDbContextFactory, ExampleMobileDbContextFactory>();

            return builder;
        }
    }
}
