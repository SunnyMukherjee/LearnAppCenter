using FreshMvvm;
using GoGoGiphy.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoGoGiphy.Core.ViewModels
{
    public class GifViewModel : BaseViewModel
    {
        #region Members

        private GifDataItem _gifDataItem;

        public GifDataItem GifDataItem
        {
            get { return _gifDataItem; }
            set { _gifDataItem = value; RaisePropertyChanged(); }
        }

        #endregion


        #region Commands

        private ICommand _addToCollectionCommand;

        public ICommand AddToCollectionCommand
        {
            get
            {
                return _addToCollectionCommand ??
                    (
                        _addToCollectionCommand = new Command
                        (
                            (obj) => 
                            {
                                CoreMethods.PushPageModel<AddToCollectionViewModel>(GifDataItem, true);
                            }
                            , (obj) => { return IsConnected(); }
                        )
                    );
            }
        }       

        #endregion


        #region Constructors 

        public GifViewModel()
        {
            Initialize();
        }

        #endregion


        #region Functions

        private void AddToCollection()
        {

        }

        private void Initialize()
        {
            
        }

        public override void Init(object initData)
        {
            if (initData != null && initData is GifDataItem)
            {
                GifDataItem = initData as GifDataItem;
            }
        }

        #endregion
    }
}
