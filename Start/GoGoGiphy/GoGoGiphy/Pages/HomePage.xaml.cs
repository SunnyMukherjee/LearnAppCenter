using GoGoGiphy.Core.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoGoGiphy.Core.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private object LockObject = new object();

        public HomePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<HomeViewModel>(this, "RefreshFinished",
                (viewModel) =>
                {
                    MainThread.BeginInvokeOnMainThread(() => { this.PullToRefreshLayoutControl.IsRefreshing = false; });                    
                });

            MessagingCenter.Subscribe<HomeViewModel>
                (
                    this,
                    "SearchBarUnfocused",
                    (viewModel) =>
                    {
                        MainThread.BeginInvokeOnMainThread(() => { this.SearchBar.Unfocus(); });
                    }
                );

            MessagingCenter.Subscribe<string>
                (
                    this,
                    "DisplayAlert",
                    (message) =>
                    {
                        MainThread.BeginInvokeOnMainThread(async () => { await DisplayAlert(message, message, "Ok"); });
                    }
                );
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
        }

        private void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            ScrollView scrollView = sender as ScrollView;
            HomeViewModel homeViewModel = BindingContext as HomeViewModel;

            double scrollingSpace = scrollView.ContentSize.Height - scrollView.Height;

            if (scrollingSpace <= e.ScrollY && homeViewModel.IsGettingMoreGifsDone)
            {
                homeViewModel.IsGettingMoreGifsDone = false;

                lock (LockObject)
                {
                    homeViewModel.GetMoreGifs(); 
                }                
            }
        }

        private void SearchBar_Focused(object sender, FocusEventArgs e)
        {
            HomeViewModel homeViewModel = BindingContext as HomeViewModel;

            if (homeViewModel != null)
            {
                homeViewModel.RefreshSearchCache();
                homeViewModel.IsTrendingImagesPanelVisible = false;
                homeViewModel.IsCancelButtonVisible = true;
                homeViewModel.CancelSearchButtonWidth = 75;
            }
        }       
    }
}