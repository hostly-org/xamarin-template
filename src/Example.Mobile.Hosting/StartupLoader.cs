using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Mobile.Hosting
{
    internal sealed class StartupLoader
    {
        internal static StartupMethods LoadMethods(IServiceProvider hostingServiceProvider, Type startupType, string environmentName)
        {
            var servicesMethod = FindConfigureServicesDelegate(startupType, environmentName);
            var configureContainerMethod = FindConfigureContainerDelegate(startupType, environmentName);

            object instance = null;
            if (servicesMethod != null && !servicesMethod.MethodInfo.IsStatic)
            {
                instance = ActivatorUtilities.GetServiceOrCreateInstance(hostingServiceProvider, startupType);
            }

            var configureServicesCallback = servicesMethod.Build(instance);
            var configureContainerCallback = configureContainerMethod.Build(instance);

            Func<IServiceCollection, IServiceProvider> configureServices = services =>
            {
                // Call ConfigureServices, if that returned an IServiceProvider, we're done
                var applicationServiceProvider = configureServicesCallback.Invoke(services);

                if (applicationServiceProvider != null)
                {
                    return applicationServiceProvider;
                }

                // If there's a ConfigureContainer method
                if (configureContainerMethod.MethodInfo != null)
                {
                    // We have a ConfigureContainer method, get the IServiceProviderFactory<TContainerBuilder>
                    var serviceProviderFactoryType = typeof(IServiceProviderFactory<>).MakeGenericType(configureContainerMethod.GetContainerType());
                    var serviceProviderFactory = hostingServiceProvider.GetRequiredService(serviceProviderFactoryType);
                    // var builder = serviceProviderFactory.CreateBuilder(services);
                    var builder = serviceProviderFactoryType.GetMethod(nameof(DefaultServiceProviderFactory.CreateBuilder)).Invoke(serviceProviderFactory, new object[] { services });
                    configureContainerCallback.Invoke(builder);
                    // applicationServiceProvider = serviceProviderFactory.CreateServiceProvider(builder);
                    applicationServiceProvider = (IServiceProvider)serviceProviderFactoryType.GetMethod(nameof(DefaultServiceProviderFactory.CreateServiceProvider)).Invoke(serviceProviderFactory, new object[] { builder });
                }
                else
                {
                    // Get the default factory
                    var serviceProviderFactory = hostingServiceProvider.GetRequiredService<IServiceProviderFactory<IServiceCollection>>();

                    // Don't bother calling CreateBuilder since it just returns the default service collection
                    applicationServiceProvider = serviceProviderFactory.CreateServiceProvider(services);
                }

                return applicationServiceProvider ?? services.BuildServiceProvider();
            };

            return new StartupMethods(instance, configureServices);
        }

        private static Type FindStartupType(string startupAssemblyName, string environmentName)
        {
            if (string.IsNullOrEmpty(startupAssemblyName))
            {
                throw new ArgumentException( $"A startup method, startup type or startup assembly is required. If specifying an assembly, '{nameof(startupAssemblyName)}' cannot be null or empty.",  nameof(startupAssemblyName));
            }

            var assembly = Assembly.Load(new AssemblyName(startupAssemblyName));
            if (assembly == null)
            {
                throw new InvalidOperationException($"The assembly '{startupAssemblyName}' failed to load.");
            }

            var startupNameWithEnv = "Startup" + environmentName;
            var startupNameWithoutEnv = "Startup";

            // Check the most likely places first
            var type =
                assembly.GetType(startupNameWithEnv) ??
                assembly.GetType(startupAssemblyName + "." + startupNameWithEnv) ??
                assembly.GetType(startupNameWithoutEnv) ??
                assembly.GetType(startupAssemblyName + "." + startupNameWithoutEnv);

            if (type == null)
            {
                // Full scan
                var definedTypes = assembly.DefinedTypes.ToList();

                var startupType1 = definedTypes.Where(info => info.Name.Equals(startupNameWithEnv, StringComparison.Ordinal));
                var startupType2 = definedTypes.Where(info => info.Name.Equals(startupNameWithoutEnv, StringComparison.Ordinal));

                var typeInfo = startupType1.Concat(startupType2).FirstOrDefault();
                if (typeInfo != null)
                {
                    type = typeInfo.AsType();
                }
            }

            if (type == null)
            {
                throw new InvalidOperationException($"A type named '{startupNameWithEnv}' or '{startupNameWithoutEnv}' could not be found in assembly '{startupAssemblyName}'.");
            }

            return type;
        }

        private static ConfigureContainerBuilder FindConfigureContainerDelegate(Type startupType, string environmentName)
        {
            var configureMethod = FindMethod(startupType, "Configure{0}Container", environmentName, typeof(void), required: false);
            return new ConfigureContainerBuilder(configureMethod);
        }

        private static ConfigureServicesBuilder FindConfigureServicesDelegate(Type startupType, string environmentName)
        {
            var servicesMethod = FindMethod(startupType, "Configure{0}Services", environmentName, typeof(IServiceProvider), required: false)
                ?? FindMethod(startupType, "Configure{0}Services", environmentName, typeof(void), required: false);
            return new ConfigureServicesBuilder(servicesMethod);
        }

        private static MethodInfo FindMethod(Type startupType, string methodName, string environmentName, Type returnType = null, bool required = true)
        {
            var methodNameWithEnv = string.Format(CultureInfo.InvariantCulture, methodName, environmentName);
            var methodNameWithNoEnv = string.Format(CultureInfo.InvariantCulture, methodName, "");

            var methods = startupType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            var selectedMethods = methods.Where(method => method.Name.Equals(methodNameWithEnv)).ToList();
            if (selectedMethods.Count > 1)
            {
                throw new InvalidOperationException($"Having multiple overloads of method '{methodNameWithEnv}' is not supported.");
            }
            if (selectedMethods.Count == 0)
            {
                selectedMethods = methods.Where(method => method.Name.Equals(methodNameWithNoEnv)).ToList();
                if (selectedMethods.Count > 1)
                {
                    throw new InvalidOperationException($"Having multiple overloads of method '{methodNameWithNoEnv}' is not supported.");
                }
            }

            var methodInfo = selectedMethods.FirstOrDefault();
            if (methodInfo == null)
            {
                if (required)
                {
                    throw new InvalidOperationException($"A public method named '{methodNameWithEnv}' or '{methodNameWithNoEnv}' could not be found in the '{startupType.FullName}' type.");

                }
                return null;
            }
            if (returnType != null && methodInfo.ReturnType != returnType)
            {
                if (required)
                {
                    throw new InvalidOperationException($"The '{methodInfo.Name}' method in the type '{startupType.FullName}' must have a return type of '{returnType.Name}'.");
                }
                return null;
            }
            return methodInfo;
        }
    }
}
