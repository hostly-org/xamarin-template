using System;
using Example.Mobile.EntityFrameworkCore.Sqlite;
using Example.Mobile.Hosting;
using Example.Mobile.Infrastructure;
using Example.Mobile.Views;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;

namespace Example.Mobile
{
    public class Startup : IXamarinStartup
    {
        public void ConfigureServices(XamarinHostBuilderContext ctx, IServiceCollection services)
        {
            services.AddExampleMobile()
                .AddInfrastructure()
                .AddEntityFrameworkCoreSqlite();

            services.AddSingleton<Func<INavigation>>(() => Application.Current?.MainPage?.Navigation);
            services.AddTransient<MainPage>();
            services.AddTransient<ExampleContentPage>();
        }
    }
}
