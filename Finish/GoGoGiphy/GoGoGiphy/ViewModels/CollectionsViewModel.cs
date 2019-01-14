using FreshMvvm;
using GoGoGiphy.Core.Models;
using GoGoGiphy.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoGoGiphy.Core.ViewModels
{
    public class CollectionsViewModel : BaseViewModel
    {
        #region Members

        private GifCollectionService _gifCollectionService;

        public ObservableCollection<GifCollectionItem> GifCollections { get; set; }

        // Field is needed to prevent the GifCollections getting reloaded on the first initialization of the viewmodel class.
        private bool _isRefreshed;

        private bool _isCollectionsVisible;

        public bool IsCollectionsVisible
        {
            get { return _isCollectionsVisible; }
            set { _isCollectionsVisible = value; RaisePropertyChanged(); }
        }

        #endregion


        #region Commands

        private ICommand _listViewItemSelectedCommand;

        public ICommand ListViewItemSelectedCommand
        {
            get
            {
                return _listViewItemSelectedCommand ??
                    (
                        _listViewItemSelectedCommand = new Command<object>
                        (
                            (obj) =>
                            {
                                if (obj is SelectedItemChangedEventArgs)
                                {
                                    SelectedItemChangedEventArgs selectedItemChangedEventArgs = obj as SelectedItemChangedEventArgs;
                                    CoreMethods.PushPageModel<ViewEditCollectionViewModel>(selectedItemChangedEventArgs.SelectedItem);
                                }
                            }
                            , (obj) => { return IsConnected(); }
                        )
                    );
            }
        }

        #endregion 


        #region Constructors

        public CollectionsViewModel()
        {
            Initialize();
        }

        #endregion


        #region Functions

        public override void ReverseInit(object returnedData)
        {
            base.ReverseInit(returnedData);
        }

        private void Initialize()
        {
            _gifCollectionService = FreshIOC.Container.Resolve<GifCollectionService>();
            GifCollections = new ObservableCollection<GifCollectionItem>();
            RefreshCollections();
        }

        internal async void RefreshCollections()
        {
            if (GifCollections != null)
            {
                GifCollections.Clear();
                List<GifCollectionItem> gifCollectionItems = await _gifCollectionService.GetGifCollections();
                gifCollectionItems.ForEach((item) => GifCollections.Add(item));
                IsCollectionsVisible = (GifCollections.Count > 0);
                _isRefreshed = true;
            }
        }

        #endregion


        #region Event Handlers

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            if (!_isRefreshed)
                RefreshCollections();
        }

        protected override void ViewIsDisappearing(object sender, EventArgs e)
        {
            base.ViewIsDisappearing(sender, e);

            // Set it to false to force a refresh of GifCollections when the user clicks on the Collections page.
            _isRefreshed = false; 
        }

        #endregion
    }
}
