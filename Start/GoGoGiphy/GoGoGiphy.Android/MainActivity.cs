
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using FFImageLoading.Forms.Platform;
using FreshMvvm;
using GoGoGiphy.Core.Services;
using Microsoft.AppCenter.Push;
using Refractored.XamForms.PullToRefresh.Droid;
using System;

namespace GoGoGiphy.Droid
{
    [Activity(Label = "GoGoGiphy", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            CachedImageRenderer.Init(false);
            PullToRefreshLayoutRenderer.Init();

            InitializeEvents();
            InitializeAppCenter();

            LoadApplication(new App());
        }

        private void InitializeAppCenter()
        {
            // TODO: Add implementation here.
        }

        private void InitializeEvents()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {

            };
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            Push.CheckLaunchedFromNotification(this, intent);
        }
    }
}

