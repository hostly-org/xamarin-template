using Hostly;
using Hostly.Extensions;
using Microsoft.Extensions.Configuration;
using Xamarin.Essentials;

namespace Example.Mobile.Extensions
{
    public static class XamarinHostBuilderExtensions
    {
        public static IXamarinHostBuilder ConfigureExampleMobile(this IXamarinHostBuilder builder)
        {
            return builder.UseAppSettings<Startup>()
                .UseStartup<Startup>()
                .UseApplication<App>()
                .ConfigureHostConfiguration(c => c.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" }));
        }
    }
}
