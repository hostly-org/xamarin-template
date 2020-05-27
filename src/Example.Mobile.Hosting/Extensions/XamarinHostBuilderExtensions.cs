using System;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xamarin.Forms;

namespace Example.Mobile.Hosting.Extensions
{
    public static class XamarinHostBuilderExtensions
    {

        public static IXamarinHostBuilder UseApplication<TApp>(this IXamarinHostBuilder builder) where TApp : Application
        {
            return builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<Application, TApp>();
            });
        }

        public static IXamarinHostBuilder UsePlatform<TPlatform>(this IXamarinHostBuilder builder, TPlatform platform) where TPlatform : IXamarinHostingPlatform
        {
            return builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IXamarinHostingPlatform>(platform);
            });
        }

        public static IXamarinHostBuilder UseStartup(this IXamarinHostBuilder hostBuilder, Type startupType)
        {
            var startupAssemblyName = startupType.GetTypeInfo().Assembly.GetName().Name;

            return hostBuilder
                .ConfigureServices((context, services) =>
                {
                    if (typeof(IXamarinStartup).GetTypeInfo().IsAssignableFrom(startupType.GetTypeInfo()))
                    {
                        services.AddSingleton(typeof(IXamarinStartup), startupType);
                    }
                    else
                    {
                        services.AddSingleton(typeof(IXamarinStartup), sp =>
                        {
                            var hostingEnvironment = sp.GetRequiredService<IHostEnvironment>();
                            return new XamarinStartup(StartupLoader.LoadMethods(sp, startupType, hostingEnvironment.EnvironmentName));
                        });
                    }
                });
        }

        /// <summary>
        /// Specify the startup type to be used by the web host.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IWebHostBuilder"/> to configure.</param>
        /// <typeparam name ="TStartup">The type containing the startup methods for the application.</typeparam>
        /// <returns>The <see cref="IWebHostBuilder"/>.</returns>
        public static IXamarinHostBuilder UseStartup<TStartup>(this IXamarinHostBuilder hostBuilder) where TStartup : class
        {
            return hostBuilder.UseStartup(typeof(TStartup));
        }

        /// <summary>
        /// Configures the default service provider
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IWebHostBuilder"/> to configure.</param>
        /// <param name="configure">A callback used to configure the <see cref="ServiceProviderOptions"/> for the default <see cref="IServiceProvider"/>.</param>
        /// <returns>The <see cref="IWebHostBuilder"/>.</returns>
        public static IXamarinHostBuilder UseDefaultServiceProvider(this IXamarinHostBuilder hostBuilder, Action<ServiceProviderOptions> configure)
        {
            return hostBuilder.UseDefaultServiceProvider((context, options) => configure(options));
        }

        /// <summary>
        /// Configures the default service provider
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IWebHostBuilder"/> to configure.</param>
        /// <param name="configure">A callback used to configure the <see cref="ServiceProviderOptions"/> for the default <see cref="IServiceProvider"/>.</param>
        /// <returns>The <see cref="IWebHostBuilder"/>.</returns>
        public static IXamarinHostBuilder UseDefaultServiceProvider(this IXamarinHostBuilder hostBuilder, Action<XamarinHostBuilderContext, ServiceProviderOptions> configure)
        {
            return hostBuilder.ConfigureServices((context, services) =>
            {
                var options = new ServiceProviderOptions();
                configure(context, options);
                services.Replace(ServiceDescriptor.Singleton<IServiceProviderFactory<IServiceCollection>>(new DefaultServiceProviderFactory(options)));
            });
        }

        /// <summary>
        /// Adds a delegate for configuring the <see cref="IConfigurationBuilder"/> that will construct an <see cref="IConfiguration"/>.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IWebHostBuilder"/> to configure.</param>
        /// <param name="configureDelegate">The delegate for configuring the <see cref="IConfigurationBuilder" /> that will be used to construct an <see cref="IConfiguration" />.</param>
        /// <returns>The <see cref="IWebHostBuilder"/>.</returns>
        /// <remarks>
        /// The <see cref="IConfiguration"/> and <see cref="ILoggerFactory"/> on the <see cref="WebHostBuilderContext"/> are uninitialized at this stage.
        /// The <see cref="IConfigurationBuilder"/> is pre-populated with the settings of the <see cref="IWebHostBuilder"/>.
        /// </remarks>
        public static IXamarinHostBuilder ConfigureAppConfiguration(this IXamarinHostBuilder hostBuilder, Action<IConfigurationBuilder> configureDelegate)
        {
            return hostBuilder.ConfigureAppConfiguration((context, builder) => configureDelegate(builder));
        }

        /// <summary>
        /// Adds a delegate for configuring the provided <see cref="ILoggingBuilder"/>. This may be called multiple times.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IWebHostBuilder" /> to configure.</param>
        /// <param name="configureLogging">The delegate that configures the <see cref="ILoggingBuilder"/>.</param>
        /// <returns>The <see cref="IWebHostBuilder"/>.</returns>
        public static IXamarinHostBuilder ConfigureLogging(this IXamarinHostBuilder hostBuilder, Action<ILoggingBuilder> configureLogging)
        {
            return hostBuilder.ConfigureServices((context, collection) => collection.AddLogging(configureLogging));
        }

        /// <summary>
        /// Adds a delegate for configuring the provided <see cref="LoggerFactory"/>. This may be called multiple times.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IWebHostBuilder" /> to configure.</param>
        /// <param name="configureLogging">The delegate that configures the <see cref="LoggerFactory"/>.</param>
        /// <returns>The <see cref="IWebHostBuilder"/>.</returns>
        public static IXamarinHostBuilder ConfigureLogging(this IXamarinHostBuilder hostBuilder, Action<XamarinHostBuilderContext, ILoggingBuilder> configureLogging)
        {
            return hostBuilder.ConfigureServices((context, collection) => collection.AddLogging(builder => configureLogging(context, builder)));
        }
    }
}
