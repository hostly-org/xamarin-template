using System;
using Example.Mobile.Extensions;
using Foundation;
using Hostly;
using Hostly.Extensions;
using Hostly.IOS.Extensions;
using UIKit;

namespace Example.Mobile.IOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            new XamarinHostBuilder().ConfigureExampleMobile()
                .UsePlatform(this)
                .Build()
                .RunExampleMobile();

            return base.FinishedLaunching(app, options);
        }
    }
}
