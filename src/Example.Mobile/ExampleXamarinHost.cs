using Example.Mobile.EntityFrameworkCore.Extensions;
using Example.Mobile.Hosting;
using Example.Mobile.Hosting.Extensions;

namespace Example.Mobile
{
    public class ExampleXamarinHostBuilder
    {
        private readonly IXamarinHostBuilder _builder;

        public ExampleXamarinHostBuilder()
        {
            _builder = new XamarinHostBuilder();
        }

        public IXamarinHostBuilder Configure()
        {
            return _builder
            .UseAppSettings(typeof(Startup).Assembly)
            .UseStartup<Startup>()
            .UseApplication<App>();
        }

        public void Run()
        {
            _builder.Build()
            .SeedDatabase()
            .StartAsync().Wait();
        }
    }
}
