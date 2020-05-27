﻿using System;
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
        private readonly IXamarinHostingPlatform _platform;
        private readonly Application _application;

        private IEnumerable<IHostedService> _hostedServices;
        public IServiceProvider Services { get; }

        public XamarinHost(IServiceProvider services, 
            IHostApplicationLifetime applicationLifetime, 
            ILogger<XamarinHost> logger,
            IHostLifetime hostLifetime, 
            IOptions<HostOptions> options,
            IXamarinHostingPlatform platform,
            Application application)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (applicationLifetime == null)
                throw new ArgumentNullException(nameof(applicationLifetime));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            if (hostLifetime == null)
                throw new ArgumentNullException(nameof(hostLifetime));
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            if (platform == null)
                throw new ArgumentNullException(nameof(platform));
            if (application == null)
                throw new ArgumentNullException(nameof(application));

            Services = services;
            _applicationLifetime = applicationLifetime as ApplicationLifetime;
            _logger = logger;
            _hostLifetime = hostLifetime;
            _options = options.Value;
            _platform = platform;
            _application = application;
        }

        public void Run(object caller)
        {
            //var callerType = caller.GetType();

            //var methods = callerType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
            //var loadApplicationMethod = methods.Where(method => method.Name.Equals("LoadApplication")).FirstOrDefault();

            //loadApplicationMethod.Invoke(caller, new object[] { Services.GetRequiredService<Application>() });
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

            _platform.LoadApplication(_application);
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