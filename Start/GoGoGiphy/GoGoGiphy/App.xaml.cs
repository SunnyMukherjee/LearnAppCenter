using FreshMvvm;
using GoGoGiphy.Core.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GoGoGiphy.Core;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;
using System.Text;
using System.Collections.Generic;
using GoGoGiphy.Core.Services;
using Xamarin.Essentials;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace GoGoGiphy
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            RegisterServices();
            InitializePages();
        }

        private FreshBasePageModel GetCurrentPageModel()
        {
            FreshTabbedNavigationContainer freshTabbedNavigationContainer = Current.MainPage as FreshTabbedNavigationContainer;
            NavigationPage navigationPage = freshTabbedNavigationContainer.CurrentPage as NavigationPage;
            Page page = navigationPage.RootPage as Page;
            FreshBasePageModel pageModel = page.BindingContext as FreshBasePageModel;
            return pageModel;
        }                

        private void InitializeAppCenter()
        {

            if (!AppCenter.Configured)
            {
                Crashes.SendingErrorReport += (sender, e) =>
                {
                    // TODO: Add implementation here.
                };

                Crashes.SentErrorReport += (sender, e) =>
                {
                    // TODO: Add implementation here.
                };

                Crashes.FailedToSendErrorReport += (sender, e) =>
                {
                    // TODO: Add implementation here.
                };

                Crashes.ShouldAwaitUserConfirmation = () =>
                {
                    // TODO: Add implementation here.

                    return true;
                };

                Crashes.HasCrashedInLastSessionAsync().ContinueWith((arg) =>
                {
                    // TODO: Add implementation here.
                });

                Push.PushNotificationReceived += (sender, e) =>
                {
                    // TODO: Add implementation here.
                };

                // Initialize AppCenter SDK
                AppCenter.Start
                    (
                        String.Format
                        (
                            "ios={0};android={1};"
                            , Settings.AppCenterSecretiOS
                            , Settings.AppCenterSecretAndroid
                        )
                        , typeof(Analytics)
                        , typeof(Crashes)                  
                        , typeof(Push)
                    );
            }
        }
        
        private void InitializePages()
        {
            var tabbedNavigation = new FreshTabbedNavigationContainer();

            tabbedNavigation.AutomationId = "MainTab";
            tabbedNavigation.AddTab<HomeViewModel>("Home", "Home.png", null);
            //tabbedNavigation.AddTab<NotificationViewModel>("Notification", "Notification.png", null);
            tabbedNavigation.AddTab<AccountViewModel>("Account", "Account.png", null);

            MainPage = tabbedNavigation;
        }
        
        public void RefreshData()
        {
            Analytics.TrackEvent("RefreshData");

            // TODO: Add implementation here.
        }

        /// <summary>
        /// Function registers the classes in the FreshMVVM IOC container.
        /// </summary>
        private void RegisterServices()
        {
            GiphyService giphyService = new GiphyService();
            SearchCacheService searchCacheService = new SearchCacheService();
            GifCollectionService gifCollectionService = new GifCollectionService();
            GifDataService gifDataService = new GifDataService();

            FreshIOC.Container.Register(giphyService);
            FreshIOC.Container.Register(searchCacheService);
            FreshIOC.Container.Register(gifCollectionService);
            FreshIOC.Container.Register(gifDataService);
        }

        protected override void OnStart()
        {
            InitializeAppCenter();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
