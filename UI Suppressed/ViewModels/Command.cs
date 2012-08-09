using System;
using System.Windows.Input;

namespace SuperSimpleLyncKiosk.ViewModels
{
    public class Command : ICommand
    {
        #region Fields

        private EventHandler _canExecuteChanged;

        #endregion

        #region Properties

        public Func<Object, Boolean> CanExecute { get; set; }

        public Action<Object> Execute { get; set; }

        #endregion

        #region Constructors

        public Command()
        {
        }

        public Command(Action<Object> execute)
            : this(execute, null)
        {
        }

        public Command(Action<Object> execute, Func<Object, Boolean> canExecute)
        {
            Execute = execute;
            CanExecute = canExecute;
        }

        #endregion

        #region Methods

        public void NotifyCanExecuteChanged()
        {
            if (_canExecuteChanged != null)
                _canExecuteChanged(this, new EventArgs());
        }

        #endregion

        #region ICommand Members

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute == null || CanExecute(parameter);
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add { _canExecuteChanged += value; }
            remove { _canExecuteChanged -= value; }
        }

        void ICommand.Execute(object parameter)
        {
            if (Execute != null && (CanExecute == null || CanExecute(parameter)))
            {
                Execute(parameter);
            }
        }

        #endregion
    }
}
