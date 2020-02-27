using Microsoft.Extensions.DependencyInjection;

namespace Example.Mobile.Infrastructure
{
    public interface IExampleMobileBuilder
    {
        IServiceCollection Services { get; }
    }
}
