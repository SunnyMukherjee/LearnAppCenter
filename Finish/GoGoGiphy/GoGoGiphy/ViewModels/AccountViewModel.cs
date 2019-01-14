using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoGoGiphy.Core.ViewModels
{
    public class AccountViewModel : BaseViewModel
    {
        #region Members

        public List<string> Ratings { get; set; }        

        public string SelectedRating
        {
            get
            {
                return Settings.Rating;
            }
            set
            {
                Settings.Rating = value; ;
                RaisePropertyChanged();
            }
        }

        #endregion


        #region Commands

        private ICommand _viewCollectionsCommand;

        public ICommand ViewCollectionsCommand
        {
            get
            {
                return _viewCollectionsCommand ??
                    (
                        _viewCollectionsCommand = new Command
                        (
                            (obj) =>
                            {
                                CoreMethods.PushPageModel<CollectionsViewModel>(false);
                            }
                        )
                    );
            }
        }

        #endregion


        #region Constructors

        public AccountViewModel()
        {
            Initialize();
        }

        #endregion 


        #region Functions        

        private void Initialize()
        {
            Ratings = new List<string>(new string[] { "Y", "G", "PG", "PG-13", "R" });
        }

        #endregion
    }
}
