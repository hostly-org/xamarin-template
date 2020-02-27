using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Example.Mobile.EntityFrameworkCore.DbContexts;

namespace Example.Mobile.EntityFrameworkCore
{
    public static class HostExtensions
    {
        public static IHost SeedDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<ExampleMobileDbContext>())
                    context.Database.Migrate();

                return host;
            }
        }
    }
}
