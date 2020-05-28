using System;
using Example.Mobile.EntityFrameworkCore.Sqlite.Extensions;
using Example.Mobile.Infrastructure.Extensions;
using Example.Mobile.Serialization.UTF8Json.Extensions;
using Example.Mobile.Views;
using Hostly;
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
                .AddEntityFrameworkCoreSqlite()
                .AddUTF8JsonSerialization();

            services.AddSingleton<Func<INavigation>>(() => Application.Current?.MainPage?.Navigation);
            services.AddTransient<MainPage>();
        }
    }
}
