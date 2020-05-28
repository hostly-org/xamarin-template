using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Mobile.Infrastructure.Extensions
{
    public static class ServiceDescriptorExtensions
    {
        public static IEnumerable<ServiceDescriptor> Singleton<I1, I2, T>()
            where T : class, I1, I2
            where I1 : class
            where I2 : class
        {
            yield return ServiceDescriptor.Singleton<I1, T>();
            yield return ServiceDescriptor.Singleton<I2, T>(x => (T)x.GetService<I1>());
        }
    }
}
