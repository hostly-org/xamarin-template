using System;
using System.Collections.Generic;
using System.Text;

namespace Example.Mobile.Hosting
{
    internal interface IConfigureContainerAdapter
    {
        void ConfigureContainer(XamarinHostBuilderContext hostContext, object containerBuilder);
    }
}
