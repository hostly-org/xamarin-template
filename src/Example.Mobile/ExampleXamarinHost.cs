using System.Threading.Tasks;
using Example.Mobile.EntityFrameworkCore;
using Example.Mobile.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xamarin.Essentials;

namespace Example.Mobile
{
    public class ExampleXamarinHostBuilder
    {
        public IXamarinHostBuilder Builder { get; }
        public IXamarinHost Host { get; set; }

        public ExampleXamarinHostBuilder()
        {
            Builder = new XamarinHostBuilder();
        }

        public IXamarinHostBuilder Configure()
        {
            return Builder.ConfigureHostConfiguration(c =>
            {
                c.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" });
                c.AddEmbeddedJsonFile(typeof(Startup).Assembly, "appsettings.json");
                c.AddEmbeddedJsonFile(typeof(Startup).Assembly, $"appsettings.{c.Build().GetValue<string>("environment")}.json");
            })
            .UseStartup<Startup>()
            .UseApplication<App>();
        }

        public void Run(object caller)
        {
            Host = Builder.Build();
            Host.SeedDatabase();
            Host.StartAsync().Wait();
            Host.Run(caller);
        }
    }
}
