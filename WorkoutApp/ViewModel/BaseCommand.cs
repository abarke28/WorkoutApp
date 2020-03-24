using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PodcastApp.ViewModel.Commands
{
    public class BaseCommand : ICommand
    {
        private readonly Predicate<object> _canExecuteMethod = null;
        private readonly Action<object> _executeMethod = null;

        public BaseCommand(Predicate<object> canExecuteMethod, Action<object> executeMethod)
        {
            _canExecuteMethod = canExecuteMethod ?? throw new ArgumentNullException(nameof(canExecuteMethod));
            _executeMethod = executeMethod ?? throw new ArgumentNullException(nameof(executeMethod));
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecuteMethod.Invoke(parameter);
        }
        public void Execute(object parameter)
        {
            _executeMethod.Invoke(parameter);
        }
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}