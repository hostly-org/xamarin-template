using Example.Mobile.EntityFrameworkCore.Extensions;
using Hostly;

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
