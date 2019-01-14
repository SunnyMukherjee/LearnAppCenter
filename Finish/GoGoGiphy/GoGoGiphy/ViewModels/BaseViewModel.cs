using FreshMvvm;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GoGoGiphy.Core.ViewModels
{
    public class BaseViewModel : FreshBasePageModel
    {
        private Command _closeModalCommand;

        public Command CloseModalCommand
        {
            get
            {
                return (_closeModalCommand) ??
                    (
                        _closeModalCommand = new Command
                        (
                            () =>
                            {
                                CoreMethods.PopPageModel(true);
                            }
                        )
                    );
            }
        }

        internal bool IsConnected()
        {
            NetworkAccess networkAccess = Connectivity.NetworkAccess;

            if (networkAccess == NetworkAccess.None)
            {
                return false;
            }

            return true;
        }       
    }
}
