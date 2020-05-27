using System;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Mobile.Hosting
{
    public interface IXamarinStartup
    {
        void ConfigureServices(XamarinHostBuilderContext ctx, IServiceCollection services);
    }
}
