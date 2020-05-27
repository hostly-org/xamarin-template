using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Example.Mobile.Hosting
{
    public class XamarinHostBuilderContext
    {
        public IHostEnvironment HostEnvironment { get; set; }
        public IConfiguration Configuration { get; set; }
        public string RuntimePlatform { get; set; }
    }
}
