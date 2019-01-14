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
            // TODO: Add implementation here.

            if (!AppCenter.Configured)
            {
                Crashes.SendingErrorReport += (sender, e) =>
                {
                    string message = "Sending error report";
                    AppCenterLog.Info(Settings.AppName, message);

                    var args = e as SendingErrorReportEventArgs;
                    ErrorReport report = args.Report;

                    if (report.Exception != null)
                    {
                        AppCenterLog.Info(Settings.AppName, report.Exception.ToString());
                    }
                    else if (report.AndroidDetails != null)
                    {
                        AppCenterLog.Info(Settings.AppName, report.AndroidDetails.ThreadName);
                    }

                    // Show modal page displaying status of crash report.
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Tuple<string, bool> values = new Tuple<string, bool>(message, true);
                        FreshBasePageModel pageModel = GetCurrentPageModel();
                        pageModel.CoreMethods.PushPageModel<ModalActivityViewModel>(values, true);
                    });                       
                };

                Crashes.SentErrorReport += (sender, e) =>
                {
                    AppCenterLog.Info(Settings.AppName, "Sent error report");

                    var args = e as SentErrorReportEventArgs;
                    ErrorReport report = args.Report;

                    if (report.Exception != null)
                    {
                        AppCenterLog.Info(Settings.AppName, report.Exception.ToString());
                    }
                    else
                    {
                        AppCenterLog.Info(Settings.AppName, "No system exception was found");
                    }

                    if (report.AndroidDetails != null)
                    {
                        AppCenterLog.Info(Settings.AppName, report.AndroidDetails.ThreadName);
                    }
                };

                Crashes.FailedToSendErrorReport += (sender, e) =>
                {
                    string message = "Failed to send error report.  Please check your internet connection.";
                    AppCenterLog.Info(Settings.AppName, message);

                    var args = e as FailedToSendErrorReportEventArgs;
                    ErrorReport report = args.Report;

                    if (report.Exception != null)
                    {
                        AppCenterLog.Info(Settings.AppName, report.Exception.ToString());
                    }
                    else if (report.AndroidDetails != null)
                    {
                        AppCenterLog.Info(Settings.AppName, report.AndroidDetails.ThreadName);
                    }

                    if (e.Exception != null)
                    {
                        AppCenterLog.Info(Settings.AppName, "There is an exception associated with the failure");
                    }

                    // Show modal page displaying status of crash report.
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Tuple<string, bool> values = new Tuple<string, bool>(message, false);
                        FreshBasePageModel pageModel = GetCurrentPageModel();
                        pageModel.CoreMethods.PushPageModel<ModalActivityViewModel>(values, true);
                    });
                };

                Crashes.ShouldAwaitUserConfirmation = () =>
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Current.MainPage.DisplayActionSheet("Uh Oh.  Sorry I crashed.  Do you want to send a crash report?", "Cancel", null, "Send", "Always Send", "Don't Send")
                            .ContinueWith((arg) =>
                        {
                            var answer = arg.Result;
                            UserConfirmation userConfirmationSelection;

                            if (answer == "Send")
                            {
                                userConfirmationSelection = UserConfirmation.Send;
                            }
                            else if (answer == "Always Send")
                            {
                                userConfirmationSelection = UserConfirmation.AlwaysSend;
                            }
                            else
                            {
                                userConfirmationSelection = UserConfirmation.DontSend;
                            }

                            Crashes.NotifyUserConfirmation(userConfirmationSelection);
                        });

                    });

                    return true;
                };

                Crashes.HasCrashedInLastSessionAsync().ContinueWith((arg) =>
                {
                    //MainThread.BeginInvokeOnMainThread(() =>
                    //{
                    //    try
                    //    {
                    //        Task<bool> didAppCrash = Crashes.HasCrashedInLastSessionAsync();

                    //        if (didAppCrash.Result)
                    //        {
                    //            Current.MainPage.DisplayAlert("Oops", "We noticed the app crashed on you.  Sorry for the inconvenience.", "Ok");
                    //        }
                    //    }
                    //    catch (Exception exception)
                    //    {
                    //        Debug.WriteLine(exception.Message);
                    //    }
                    //});
                });

                Push.PushNotificationReceived += (sender, e) =>
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        // Summarize the notification title and message.
                        StringBuilder summary = new StringBuilder
                            ("Push Notification Received \n\tNotification Title: " + e.Title + "\n\tMessage: " + e.Message);

                        bool isContentAvailable = false;
                        string searchText = String.Empty;

                        // If receiving custom data, add to the summary.
                        if (e.CustomData != null)
                        {
                            summary.AppendLine("\nCustom Data:");

                            foreach(KeyValuePair<string, string> value in e.CustomData)
                            {
                                summary.AppendLine("\t" + value.Key + "\t" + value.Value);

                                switch(value.Key)
                                {
                                    case "content-available":

                                        isContentAvailable = Convert.ToBoolean(value.Value);
                                        break;

                                    case "SearchText":

                                        searchText = value.Value;
                                        break;
                                }
                            }

                            if (!String.IsNullOrEmpty(searchText))
                            {
                                FreshBasePageModel freshBasePageModel = GetCurrentPageModel();

                                if (freshBasePageModel is HomeViewModel)
                                {
                                    HomeViewModel homeViewModel = freshBasePageModel as HomeViewModel;
                                    homeViewModel.SearchCommand.Execute(searchText);
                                } 
                            }
                        }
                                                
                        //Current.MainPage.DisplayAlert("Push Notification Received", summary.ToString(), "Ok");
                    });
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
