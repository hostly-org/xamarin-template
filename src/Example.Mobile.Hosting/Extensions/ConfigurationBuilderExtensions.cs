using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Example.Mobile.Hosting.AppSettings;
using Microsoft.Extensions.Configuration;

namespace Example.Mobile.Hosting.Extensions
{
    internal static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddEmbeddedJsonFile(this IConfigurationBuilder cb,
            Assembly assembly, string name, bool optional = false)
        {
            // reload on change is not supported, always pass in false
            return cb.AddJsonFile(new EmbeddedFileProvider(assembly), name, optional, false);
        }
    }
}
