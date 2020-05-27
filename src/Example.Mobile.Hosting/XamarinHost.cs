using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xamarin.Forms;

namespace Example.Mobile.Hosting
{
    internal sealed class XamarinHost : IXamarinHost
    {
        private readonly ILogger<XamarinHost> _logger;
        private readonly IHostLifetime _hostLifetime;
        private readonly ApplicationLifetime _applicationLifetime;
        private readonly HostOptions _options;
        private IEnumerable<IHostedService> _hostedServices;
        public IServiceProvider Services { get; }

        public XamarinHost(IServiceProvider services, IHostApplicationLifetime applicationLifetime, ILogger<XamarinHost> logger,
            IHostLifetime hostLifetime, IOptions<HostOptions> options)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            _applicationLifetime = (applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime))) as ApplicationLifetime;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _hostLifetime = hostLifetime ?? throw new ArgumentNullException(nameof(hostLifetime));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public void Run(object caller)
        {
            var callerType = caller.GetType();

            var methods = callerType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
            var loadApplicationMethod = methods.Where(method => method.Name.Equals("LoadApplication")).FirstOrDefault();

            loadApplicationMethod.Invoke(caller, new object[] { Services.GetRequiredService<Application>() });
        }

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            await _hostLifetime.WaitForStartAsync(cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            _hostedServices = Services.GetService<IEnumerable<IHostedService>>();

            foreach (var hostedService in _hostedServices)
            {
                await hostedService.StartAsync(cancellationToken).ConfigureAwait(false);
            }

            _applicationLifetime?.NotifyStarted();
        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            using (var cts = new CancellationTokenSource(_options.ShutdownTimeout))
            using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, cancellationToken))
            {
                var token = linkedCts.Token;
                // Trigger IApplicationLifetime.ApplicationStopping
                _applicationLifetime?.StopApplication();

                IList<Exception> exceptions = new List<Exception>();
                if (_hostedServices != null) // Started?
                {
                    foreach (var hostedService in _hostedServices.Reverse())
                    {
                        token.ThrowIfCancellationRequested();
                        try
                        {
                            await hostedService.StopAsync(token).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            exceptions.Add(ex);
                        }
                    }
                }

                token.ThrowIfCancellationRequested();
                await _hostLifetime.StopAsync(token);

                // Fire IApplicationLifetime.Stopped
                _applicationLifetime?.NotifyStopped();

                if (exceptions.Count > 0)
                {
                    var ex = new AggregateException("One or more hosted services failed to stop.", exceptions);
                    throw ex;
                }
            }
        }

        public void Dispose()
        {
            (Services as IDisposable)?.Dispose();
        }
    }
}
