using System;
using System.Windows.Input;

namespace ErrorCode.Base
{
    public class Command<T> : ICommand
        where T : ViewModel<T>
    {
        public virtual bool CanExecute(object parameter) => true;

        public virtual void Execute(object parameter)
        {
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public T ViewModel { get; private set; }

        public void Init(T viewModel)
        {
            ViewModel = viewModel;
        }
    }
}