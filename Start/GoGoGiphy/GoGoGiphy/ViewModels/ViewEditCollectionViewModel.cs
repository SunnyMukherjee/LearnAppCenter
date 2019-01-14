using FreshMvvm;
using GoGoGiphy.Core.Models;
using GoGoGiphy.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GoGoGiphy.Core.ViewModels
{
    public class ViewEditCollectionViewModel : BaseViewModel
    {
        #region Members

        private GifDataService _gifDataServiceObj;

        internal GifCollectionItem GifCollectionItemObj { get; set; }

        public ObservableCollection<GifDataItem> Gifs { get; set; }

        private bool _isGifLayoutVisible;

        public bool IsGifLayoutVisible
        {
            get { return _isGifLayoutVisible; }
            set { _isGifLayoutVisible = value; RaisePropertyChanged(); }
        }

        private bool _isRefreshed;

        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; RaisePropertyChanged(); }
        }

        #endregion


        #region Commands

        private ICommand _deleteGifCommand;

        public ICommand DeleteGifCommand => (_deleteGifCommand) ??
                    (
                        _deleteGifCommand = new Command<object>
                        (
                            (obj) =>
                            {
                                if (obj is GifDataItem)
                                {
                                    MainThread.BeginInvokeOnMainThread
                                    (
                                        async () =>
                                        {
                                            bool response = await CoreMethods.DisplayAlert("Question", "Are you sure you want to delete?", "Yes", "No");

                                            if (response)
                                            {
                                                GifDataItem gifDataItem = obj as GifDataItem;
                                                DeleteGifDataItem(gifDataItem);
                                                RefreshGifDataItems();
                                            }
                                        }
                                    );
                                }
                            }
                            , (obj) => { return IsConnected(); }
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

        #endregion


        #region Constructors

        public ViewEditCollectionViewModel()
        {
            Initialize();
        }

        #endregion


        #region Functions

        private void DeleteGifDataItem(GifDataItem gifDataItem)
        {
            _gifDataServiceObj.DeleteGifDataItem(gifDataItem);
        }

        public override void Init(object initData)
        {
            if (initData is GifCollectionItem)
            {
                GifCollectionItemObj = initData as GifCollectionItem;
                Title = GifCollectionItemObj.Name + " Collection";
                RefreshGifDataItems();
            }
        }

        private void Initialize()
        {
            _gifDataServiceObj = FreshIOC.Container.Resolve<GifDataService>();
            Gifs = new ObservableCollection<GifDataItem>();
        }

        private async void RefreshGifDataItems()
        {
            List<GifDataItem> gifDataItems = await _gifDataServiceObj.GetGifDataItems(GifCollectionItemObj.Id);

            if (gifDataItems.Count > 0)
            {
                Gifs.Clear();
                gifDataItems.ForEach((gif) => { Gifs.Add(gif); });
                IsGifLayoutVisible = true;
            }
            else
            {
                IsGifLayoutVisible = false;
            }

            _isRefreshed = true;
        }

        #endregion


        #region Event Handlers

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            if (!_isRefreshed)
            {
                RefreshGifDataItems();
            }
        }

        protected override void ViewIsDisappearing(object sender, EventArgs e)
        {
            base.ViewIsDisappearing(sender, e);
            _isRefreshed = false;
        }

        #endregion
    }
}
