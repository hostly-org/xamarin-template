using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Example.Mobile.EntityFrameworkCore.DbContexts;
using Example.Mobile.EntityFrameworkCore.Sqlite;
using Example.Mobile.Infrastructure;

namespace Example.Mobile.Migrations
{
    class Program
    {
        public static void Main(string[] args)
        => CreateHostBuilder(args).Build().Run();

        // EF Core uses this method at design time to access the DbContext
        public static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                .ConfigureServices(services => services.AddTransient<IDesignTimeDbContextFactory<ExampleMobileDbContext>, ExampleMobileDbContextFactory>()
            );
    }
}
