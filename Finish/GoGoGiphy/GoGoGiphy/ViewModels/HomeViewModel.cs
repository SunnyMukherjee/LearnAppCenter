using FreshMvvm;
using GoGoGiphy.Core.Models;
using GoGoGiphy.Core.Services;
using Microsoft.AppCenter.Analytics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GoGoGiphy.Core.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        #region Members

        private GiphyService _giphyServiceObj { get; set; }

        private SearchCacheService _searchCacheServiceObj { get; set; }

        internal Giphy Giphy { get; set; }

        internal bool AreGifsPreLoaded
        {
            get
            {
                string savedValue = SecureStorage.GetAsync(nameof(AreGifsPreLoaded)).Result;
                return (savedValue != null) ? (savedValue.Equals("1")) : false;
            }
            set
            {
                bool areGifsPreLoaded = Convert.ToBoolean(value);
                string savedValue = (areGifsPreLoaded) ? "1" : "0";
                SecureStorage.SetAsync(nameof(AreGifsPreLoaded), savedValue);
            }
        }
              
        private int _cancelSearchButtonWidth;

        public int CancelSearchButtonWidth
        {
            get { return _cancelSearchButtonWidth; }
            set { _cancelSearchButtonWidth = value; RaisePropertyChanged(); }
        }

        private bool _isCancelButtonVisible;

        public bool IsCancelButtonVisible
        {
            get { return _isCancelButtonVisible; }
            set { _isCancelButtonVisible = value; RaisePropertyChanged(); }
        }

        public bool IsGettingMoreGifsDone { get; set; }

        private bool _isRefreshing;

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { _isRefreshing = value; RaisePropertyChanged(); }
        }

        private bool _isTrendingImagesPanelVisible;

        public bool IsTrendingImagesPanelVisible
        {
            get { return _isTrendingImagesPanelVisible; }
            set { _isTrendingImagesPanelVisible = value; RaisePropertyChanged(); }
        }

        private string _searchText;

        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                _searchText = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<GifDataItem> TrendingImages { get; set; }

        public ObservableCollection<SearchCacheItem> SearchCache { get; set; }

        #endregion


        #region Commands

        private ICommand _cancelSearchCommand;

        public ICommand CancelSearchCommand
        {
            get
            {
                return _cancelSearchCommand ??
                    (
                        _cancelSearchCommand = new Command<object>
                        (
                            (obj) =>
                            {
                                IsTrendingImagesPanelVisible = true;
                                IsCancelButtonVisible = false;
                            }
                        )
                    );
            }
        }

        private ICommand _clearSearchCacheCommand;

        public ICommand ClearSearchCacheCommand
        {
            get
            {
                return _clearSearchCacheCommand ??
                    (
                        _clearSearchCacheCommand = new Command
                        (
                            (obj) =>
                            {
                                MainThread.BeginInvokeOnMainThread
                                (
                                    async () =>
                                    {
                                        bool response = await CoreMethods.DisplayAlert("Question", "Are you sure you want to clear all searches?", "Yes", "No");

                                        if (response && _searchCacheServiceObj != null)
                                        {
                                            _searchCacheServiceObj.DeleteSearchCache();
                                            SearchCache.Clear();
                                        }
                                    }
                                );
                            }
                            , (obj) => { return IsConnected(); }
                        )
                    );
            }
        }

        private ICommand _refreshCommand;

        public ICommand RefreshCommand
        {
            get
            {
                return _refreshCommand ??
                    (
                        _refreshCommand = new Command
                        (
                            (obj) =>
                            {
                                if (IsConnected())
                                {
                                    MainThread.BeginInvokeOnMainThread
                                    (
                                        async () =>
                                        {
                                            await RefreshTrendingGifs();
                                            SearchText = "";
                                            MessagingCenter.Send<HomeViewModel>(this, "RefreshFinished");
                                        }
                                    ); 
                                }
                                else
                                {
                                    MainThread.BeginInvokeOnMainThread
                                    (
                                        async () =>
                                        {
                                            await CoreMethods.DisplayAlert("Uh Oh.  No Connection", "Please check your connection.", "Ok");
                                        }
                                    );
                                }
                            }
                        )
                    );
            }
        }

        private ICommand _searchCommand;

        public ICommand SearchCommand => _searchCommand ??
        (
            _searchCommand = new Command<object>
                (
                    (obj) =>
                    {
                        if (IsConnected())
                        {
                            // Handle manually typed search.
                            if (obj is string)
                            {
                                string text = obj as string;

                                //Crashes.GenerateTestCrash();
                                SearchText = text;
                                Search(text);
                            }
                            // Handle user selection from the SearchCacheList.
                            else if (obj is SearchCacheItem)
                            {
                                SearchCacheItem searchCache = obj as SearchCacheItem;
                                SearchText = searchCache.SearchString;
                                Search(searchCache.SearchString);
                            }
                            // Handle user selection from the SearchCacheList.
                            else if (obj is SelectedItemChangedEventArgs)
                            {
                                SelectedItemChangedEventArgs selectedItemChangedEventArgs = obj as SelectedItemChangedEventArgs;
                                SearchCacheItem searchCacheItem = selectedItemChangedEventArgs.SelectedItem as SearchCacheItem;

                                if (searchCacheItem != null)
                                {
                                    SearchText = searchCacheItem.SearchString;
                                    MessagingCenter.Send<HomeViewModel>(this, "SearchBarUnfocused");
                                    Search(searchCacheItem.SearchString);
                                }
                            } 
                        }
                        else
                        {
                            MainThread.BeginInvokeOnMainThread
                            (
                                async () =>
                                {
                                    await CoreMethods.DisplayAlert("Uh Oh.  No Connection", "Please check your connection.", "Ok");
                                }
                            );
                        }
                    }
                )
        );

        private ICommand _tappedCommand;

        public ICommand TappedCommand
        {
            get
            {
                return _tappedCommand ??
                    (
                        _tappedCommand = new Command<object>
                        (
                            (obj) =>
                            {
                                GifDataItem gifDataItem = obj as GifDataItem;
                                CoreMethods.PushPageModel<GifViewModel>(gifDataItem, true);
                            }
                        )
                    );
            }
        }

        private ICommand _testCommand;

        public ICommand TestCommand
        {
            get
            {
                return _testCommand ??
                    (
                        _testCommand = new Command<string>
                        (
                            (obj) =>
                            {
                                CoreMethods.PushPageModel<ModalActivityViewModel>("Testing", true);
                            }
                        )
                    );
            }
        }

        #endregion


        #region Constructors

        public HomeViewModel()
        {
            Initialize();
        }

        #endregion


        #region Functions

        internal List<GifDataItem> CreateGifDataItemList(List<Datum> datumList)
        {
            List<GifDataItem> gifDataItems = datumList.Select<Datum, GifDataItem>
                (
                    datumObj =>
                    {
                        GifDataItem gifDataItem = new GifDataItem();

                        gifDataItem.Id = datumObj.Id;
                        gifDataItem.CachedImageAutomationId = datumObj.Id;
                        gifDataItem.GifCollectionId = 0;
                        gifDataItem.ContentUrl = datumObj.ContentUrl;
                        gifDataItem.EmbedUrl = datumObj.EmbedUrl;
                        gifDataItem.ImageDownsizedUrl = datumObj.Images.Downsized.Url;
                        gifDataItem.ImagePreviewGifUrl = datumObj.Images.PreviewGif.Url;
                        gifDataItem.Rating = datumObj.Rating;
                        gifDataItem.Source = datumObj.Source;
                        gifDataItem.Title = datumObj.Title;
                        gifDataItem.TrendingDatetime = datumObj.TrendingDatetime;
                        gifDataItem.Type = datumObj.Type;
                        gifDataItem.Url = datumObj.Url;

                        return gifDataItem;
                    }
                ).ToList<GifDataItem>();

            return gifDataItems;
        }

        private async void Initialize()
        {
            try
            {
                if (IsConnected())
                {
                    _searchCacheServiceObj = FreshIOC.Container.Resolve<SearchCacheService>();
                    _giphyServiceObj = FreshIOC.Container.Resolve<GiphyService>();

                    TrendingImages = new ObservableCollection<GifDataItem>();
                    SearchCache = new ObservableCollection<SearchCacheItem>();

                    await RefreshTrendingGifs();
                    await RefreshSearchCache();

                    IsTrendingImagesPanelVisible = true;
                    IsGettingMoreGifsDone = true;
                }
                else
                {
                    // Handle in ViewIsAppearing(..) handler.
                }
            }
            catch (Exception exception)
            {
                MainThread.BeginInvokeOnMainThread
                   (
                       async () =>
                       {
                           await CoreMethods.DisplayAlert("Uh Oh", "Something bad happened: \n" + exception.Message, "Ok");
                       }
                   );
            }
        }

        internal async void GetMoreGifs()
        {
            try
            {
                _giphyServiceObj = FreshIOC.Container.Resolve<GiphyService>();

                Giphy = String.IsNullOrEmpty(SearchText)
                        ? await _giphyServiceObj.GetTrendingGifs()
                        : await _giphyServiceObj.SearchGifs(SearchText);

                List<GifDataItem> gifDataItems = CreateGifDataItemList(Giphy.Data.ToList());
                gifDataItems.ForEach((gifDataItem) => { TrendingImages.Add(gifDataItem); });
                IsGettingMoreGifsDone = true;
            }
            catch (Exception exception)
            {
                MainThread.BeginInvokeOnMainThread
                   (
                       () =>
                       {
                           CoreMethods.DisplayAlert("Uh Oh", "Something bad happened: \n" + exception.Message, "Ok");
                       }
                   );
            }
        }

        internal async Task<bool> RefreshSearchCache()
        {
            if (SearchCache != null)
            {
                SearchCache.Clear();

                List<SearchCacheItem> searchCache = await _searchCacheServiceObj.GetSearchCache();
                List<SearchCacheItem> orderedSearchCache = searchCache.OrderByDescending(x => x.TimeSearched).ToList<SearchCacheItem>();

                foreach (SearchCacheItem searchItem in orderedSearchCache)
                {
                    SearchCache.Add(searchItem);
                }

                return true;
            }

            return false;
        }

        internal async Task<bool> RefreshTrendingGifs()
        {
            if (TrendingImages != null)
            {
                TrendingImages.Clear();

                Giphy = await _giphyServiceObj.GetTrendingGifs();

                List<GifDataItem> gifDataItems = CreateGifDataItemList(Giphy.Data.ToList());
                gifDataItems.ForEach((gifDataItem) => { TrendingImages.Add(gifDataItem); });

                return true;
            }

            return false;
        }

        private async void Search(string searchText)
        {
            Analytics.TrackEvent("Search: " + searchText);

            try
            {
                _giphyServiceObj = FreshIOC.Container.Resolve<GiphyService>();

                if (TrendingImages != null)
                {
                    TrendingImages.Clear();
                    Settings.Offset = 0;
                }

                SearchCacheItem existingSearchCacheItem = SearchCache.FirstOrDefault((obj) => obj.SearchString == searchText);

                if (existingSearchCacheItem == null)
                {
                    _searchCacheServiceObj.InsertSearchItem(new SearchCacheItem(searchText));
                }
                else
                {
                    // Update the search time so it appears first in the list.
                    existingSearchCacheItem.TimeSearched = DateTime.Now;
                    _searchCacheServiceObj.UpdateSearchItem(existingSearchCacheItem);
                }

                Giphy = await _giphyServiceObj.SearchGifs(searchText);

                if (Giphy != null)
                {
                    List<GifDataItem> gifDataItems = CreateGifDataItemList(Giphy.Data.ToList());
                    gifDataItems.ForEach((gifDataItem) => { TrendingImages.Add(gifDataItem); });

                    //foreach (Datum datum in Giphy.Data)
                    //{
                    //    TrendingImages.Add(datum);
                    //}
                }

                IsCancelButtonVisible = false;
                IsTrendingImagesPanelVisible = true;
            }
            catch (Exception exception)
            {
                MainThread.BeginInvokeOnMainThread
                   (
                       () =>
                       {
                           CoreMethods.DisplayAlert("Uh Oh", "Something bad happened: \n" + exception.Message, "Ok");
                       }
                   );
            }
        }

        #endregion


        #region Event Handlers

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            if (!IsConnected())
            {
                MainThread.BeginInvokeOnMainThread
                    (
                        () =>
                        {
                            //if (CoreMethods != null)
                            CoreMethods.DisplayAlert("No Connection", "Uh oh.  It looks like you are no longer connected.", "Ok");
                            //else
                            //MessagingCenter.Send<string>("Uh Oh.  No Internet.  Please check your connection.", "DisplayAlert");
                        }
                    );
            }
        }

        protected override void ViewIsDisappearing(object sender, EventArgs e)
        {
            base.ViewIsDisappearing(sender, e);
        }

        #endregion
    }
}
