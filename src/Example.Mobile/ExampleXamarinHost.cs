using Example.Mobile.EntityFrameworkCore.Extensions;
using Example.Mobile.Extensions;
using Example.Mobile.Hosting;
using Example.Mobile.Hosting.Extensions;
using Microsoft.Extensions.Configuration;
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

        public void Run()
        {
            Host = Builder.Build();
            Host.SeedDatabase();
            Host.StartAsync().Wait();
        }
    }
}
