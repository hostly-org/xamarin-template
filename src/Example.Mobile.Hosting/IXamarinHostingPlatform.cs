using System;
using Xamarin.Forms;

namespace Example.Mobile.Hosting
{
    public interface IXamarinHostingPlatform
    {
        event EventHandler OnStarted;
        event EventHandler OnStopped;
        void LoadApplication(Application application);
    }
}
