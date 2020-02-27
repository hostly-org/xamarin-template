using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Mobile.EntityFrameworkCore.DbContexts
{
    internal sealed class ExampleMobileDbContextFactory : IDbContextFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ExampleMobileDbContextFactory(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            _serviceProvider = serviceProvider;
        }

        public ExampleMobileDbContext Create()
        {
            var options = _serviceProvider.GetRequiredService<DbContextOptions<ExampleMobileDbContext>>();

            return new ExampleMobileDbContext(options);
        }
    }
}
