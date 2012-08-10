/* Copyright (C) 2012 Modality Systems - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the Microsoft Public License, a copy of which 
 * can be seen at: http://www.microsoft.com/en-us/openness/licenses.aspx
 * 
 * http://www.LyncAutoAnswer.com
*/

using System;
using LyncUISupressionWrapper;

namespace SuperSimpleLyncKiosk.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        LyncUISupressionWrapper.ILyncModel _model = null;

        private string _currentVisualState;

        public string CurrentVisualState
        {
            get { return _currentVisualState; }
            private set
            {
                _currentVisualState = value;
                NotifyPropertyChanged("CurrentVisualState");
            }
        }

        public MainViewModel()
        {
            _model = LyncModel.Instance;
            _model.StateChanged += new EventHandler<StateChangedEventArgs>(_model_StateChanged);

            _model.SignIn(Properties.Settings.Default.LyncAccountEmail, Properties.Settings.Default.LyncAccountDomainUser, Properties.Settings.Default.LyncAccountPassword);
        }

        public void ShutDownLync()
        {
            _model.Shutdown();
        }

        private void _model_StateChanged(object sender, EventArgs e)
        {
            CurrentVisualState = _model.State.ToString();
        }

    }

}
