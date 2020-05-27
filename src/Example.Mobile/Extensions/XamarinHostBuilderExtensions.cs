using Example.Mobile.Hosting;
using Example.Mobile.Hosting.Extensions;

namespace Example.Mobile.Extensions
{
    public static class XamarinHostBuilderExtensions
    {
        public static IXamarinHostBuilder ConfigureExampleMobile(this IXamarinHostBuilder builder)
        {
            return builder.UseAppSettings(typeof(Startup).Assembly)
                .UseStartup<Startup>()
                .UseApplication<App>();
        }
    }
}
