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

        private void _model_StateChanged(object sender, EventArgs e)
        {
            CurrentVisualState = _model.State.ToString();
        }

    }

}
