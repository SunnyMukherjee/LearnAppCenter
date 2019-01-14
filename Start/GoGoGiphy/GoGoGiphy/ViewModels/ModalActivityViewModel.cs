using FreshMvvm;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoGoGiphy.Core.ViewModels
{
    public class ModalActivityViewModel : BaseViewModel
    {
        #region Members

        private bool _isActivityRunning;

        public bool IsActivityRunning
        {
            get { return _isActivityRunning; }
            set
            {
                _isActivityRunning = value;
                RaisePropertyChanged(nameof(IsActivityRunning));
            }
        }

        private bool _isActivityVisible;

        public bool IsActivityVisible
        {
            get { return _isActivityVisible; }
            set
            {
                _isActivityVisible = value;
                RaisePropertyChanged(nameof(IsActivityVisible));
            }
        }

        private string _message;

        public string Message
        {
            get { return _message;  }
            set
            {
                _message = value;
                RaisePropertyChanged(nameof(Message));
            }
        }

        #endregion 


        #region Commands

        

        #endregion


        public ModalActivityViewModel()
        {
            InitializeEvents();
        }


        #region Functions

        public override void Init(object initData)
        {
            if (initData is Tuple<string, bool>)
            {
                Tuple<string, bool> values = initData as Tuple<string, bool>;

                Message = values.Item1;
                IsActivityRunning = values.Item2;
            }
        }

        private void InitializeEvents()
        {
            Crashes.SentErrorReport += (sender, e) =>
            {
                var args = e as SentErrorReportEventArgs;
                ErrorReport report = args.Report;

                Message = "Successfully sent error report.  You can close this dialog";
                IsActivityRunning = false;
                IsActivityVisible = false;
            };            
        }

        #endregion


        #region Event Handlers



        #endregion
    }
}
