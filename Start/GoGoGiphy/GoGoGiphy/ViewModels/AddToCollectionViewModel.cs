using FreshMvvm;
using GoGoGiphy.Core.Models;
using GoGoGiphy.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GoGoGiphy.Core.ViewModels
{
    public class AddToCollectionViewModel : BaseViewModel
    {
        #region Members

        private GifCollectionService _gifCollectionServiceObj;
        private GifDataService _gifDataServiceObj;
        private GifDataItem _gifDataItem;

        public ObservableCollection<GifCollectionItem> GifCollections { get; set; }

        private bool _isCreateCollectionButtonEnabled;

        public bool IsCreateCollectionButtonEnabled
        {
            get { return _isCreateCollectionButtonEnabled; }
            set { _isCreateCollectionButtonEnabled = value; RaisePropertyChanged(); }
        }

        private string _newCollectionName;

        public string NewCollectionName
        {
            get { return _newCollectionName; }
            set
            {
                _newCollectionName = value;
                IsCreateCollectionButtonEnabled = !String.IsNullOrEmpty(_newCollectionName);
                RaisePropertyChanged();
            }
        }

        #endregion


        #region Commands

        private ICommand _createCollectionCommand;

        public ICommand CreateCollectionCommand
        {
            get
            {
                return _createCollectionCommand ??
                    (
                        _createCollectionCommand = new Command
                        (
                            (obj) =>
                            {
                                GifCollectionItem gifCollectionItem = CreateCollection();
                                AddCollectionItem(gifCollectionItem);
                                AddGifDataItemToCollection(gifCollectionItem);
                                CloseModalCommand.Execute(null);
                            }
                            , (obj) => { return IsConnected(); }
                        )
                    );
            }
        }

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
                                    GifCollectionItem gifCollectionItem = selectedItemChangedEventArgs.SelectedItem as GifCollectionItem;
                                    AddGifDataItemToCollection(gifCollectionItem);
                                }

                                CloseModalCommand.Execute(null);
                            }
                        )
                    );
            }
        }

        #endregion


        #region Constructors

        public AddToCollectionViewModel()
        {
            Initialize();
        }


        #endregion


        #region Functions

        private void AddCollectionItem(GifCollectionItem gifCollectionItem)
        {
            _gifCollectionServiceObj.InsertGifCollectionItem(gifCollectionItem);

            GifCollections.Add(gifCollectionItem);
            NewCollectionName = String.Empty;
        }

        private void AddGifDataItemToCollection(GifCollectionItem gifCollectionItem)
        {
            GifDataItem existingItem = _gifDataServiceObj.GetGifDataItem(_gifDataItem, gifCollectionItem);

            // If the gif does not previously exist in the collection
            if (existingItem == null)
            {
                _gifDataItem.GifCollectionId = gifCollectionItem.Id;
                _gifDataServiceObj.InsertGifDataItem(_gifDataItem);
            }
            else
            {
                GifCollectionItem priorCollectionItem = _gifCollectionServiceObj.GetGifCollectionItem(existingItem.GifCollectionId);

                MainThread.BeginInvokeOnMainThread
                    (
                        () =>
                        {
                            CoreMethods.DisplayAlert("Oops", $"Gif already exists in the { priorCollectionItem.Name } collection.", "Ok");
                        }
                    );
            }
        }

        private GifCollectionItem CreateCollection()
        {
            GifCollectionItem gifCollectionItem = new GifCollectionItem(NewCollectionName);
            gifCollectionItem.Id = _gifCollectionServiceObj.GetLastGifCollectionItemId() + 1;

            return gifCollectionItem;
        }

        private GifCollectionItem GetCollectionItem(string collectionName)
        {
            return _gifCollectionServiceObj.GetGifCollectionItem(collectionName);
        }

        public override void Init(object initData)
        {
            if (initData is GifDataItem)
            {
                _gifDataItem = initData as GifDataItem;
            }
        }

        private async void Initialize()
        {
            try
            {
                _gifCollectionServiceObj = FreshIOC.Container.Resolve<GifCollectionService>();
                _gifDataServiceObj = FreshIOC.Container.Resolve<GifDataService>();

                GifCollections = new ObservableCollection<GifCollectionItem>();
                await RefreshGifCollectionItems();
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

        private async Task<bool> RefreshGifCollectionItems()
        {
            if (GifCollections != null)
            {
                List<GifCollectionItem> gifCollectionItems = await _gifCollectionServiceObj.GetGifCollections();
                gifCollectionItems.ForEach((item) => { GifCollections.Add(item); });

                return true;
            }

            return false;
        }

        #endregion 
    }
}
