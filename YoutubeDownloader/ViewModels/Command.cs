using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DIYoutubeDownloader.ViewModels
{
    public class Command : ICommand
    {
        private Action<object> execute { get; set; }
        private Predicate<object> canExecute { get; set; }

        #region Ctor

        public Command(Action<object> execute)
            : this(execute, DefaultCanExecute)
        {

        }

        public Command(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("Execute method must be defined");
            if (canExecute == null)
                throw new ArgumentNullException("CanExecute predicate must be defined");

            this.execute = execute;
            this.canExecute = canExecute;
        }

        #endregion

        #region DefaultCanExecute

        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }

        #endregion

        #region ICommand implementation

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

        #endregion
    }
}
