using Example.Mobile.EntityFrameworkCore.Extensions;
using Example.Mobile.Hosting;

namespace Example.Mobile.Extensions
{
    public static class XamarinHostExtensions
    {
        public static void RunExampleMobile(this IXamarinHost host)
        {
            host.SeedDatabase()
                .StartAsync().Wait();
        }
    }
}
