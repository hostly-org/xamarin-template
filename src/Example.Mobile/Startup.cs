using System;
using Example.Mobile.EntityFrameworkCore;
using Example.Mobile.EntityFrameworkCore.Sqlite;
using Example.Mobile.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Example.Mobile
{
    public class Startup
    {
        public static App Init(Action<HostBuilderContext, IServiceCollection> nativeConfigureServices = null)
        {
            var host = new HostBuilder()
                            .ConfigureHostConfiguration(c =>
                            {
                                c.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" });
                                c.AddEmbeddedJsonFile(typeof(Startup).Assembly, "appsettings.json");      
                                c.AddEmbeddedJsonFile(typeof(Startup).Assembly, $"appsettings.{c.Build().GetValue<string>("environment")}.json");
                            })
                            .ConfigureServices((c, x) =>
                            {
                                ConfigureServices(c, x);
                                nativeConfigureServices?.Invoke(c, x);
                            })
                            .ConfigureLogging(l => l.AddConsole(o =>
                            {
                                o.DisableColors = true;
                            }))
                            .Build();

            App.ServiceProvider = host.Services;

            host.SeedDatabase();

            return App.ServiceProvider.GetService<App>();
        }

        static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            services.AddExampleMobile()
                .AddInfrastructure()
                .AddEntityFrameworkCoreSqlite();

            services.AddSingleton<App>();
            services.AddSingleton<Func<INavigation>>(() => Application.Current?.MainPage?.Navigation);
        }
    }
}
