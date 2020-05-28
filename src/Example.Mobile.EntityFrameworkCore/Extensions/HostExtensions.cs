using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Example.Mobile.EntityFrameworkCore.DbContexts;

namespace Example.Mobile.EntityFrameworkCore.Extensions
{
    public static class HostExtensions
    {
        public static IHost SeedDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory>();
                using (var context = contextFactory.Create())
                    context.Database.Migrate();

                return host;
            }
        }
    }
}
