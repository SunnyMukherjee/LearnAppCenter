using FFImageLoading.Forms.Platform;
using Foundation;
using Microsoft.AppCenter.Push;
using Refractored.XamForms.PullToRefresh.iOS;
using System;
using UIKit;

namespace GoGoGiphy.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private App _app;

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
#if ENABLE_TEST_CLOUD

            Xamarin.Calabash.Start();

#endif

            global::Xamarin.Forms.Forms.Init();
            CachedImageRenderer.Init();
            PullToRefreshLayoutRenderer.Init();

            _app = new App();
            LoadApplication(_app);

            return base.FinishedLaunching(app, options);
        }

        public override void OnActivated(UIApplication uiApplication)
        {
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;

            base.OnActivated(uiApplication);
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            try
            {
                _app.RefreshData();

                // TODO: Add implementation here.

                Push.DidReceiveRemoteNotification(userInfo);

                completionHandler(UIBackgroundFetchResult.NewData);
            }
            catch (Exception)
            {

            }
        }
    }
}
