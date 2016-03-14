using System.Threading.Tasks;

namespace ErrorCode.Base
{
    public abstract class TypedLoadingAsyncCommand<T, TType> : LoadingAsyncCommand<T>
        where T : ViewModel<T>
    {
        public sealed override bool CanExecute(object parameter) => base.CanExecute(parameter) && parameter is TType && CanExecute((TType)parameter);

        protected sealed override Task<bool> OnExecute(object parameter) => OnExecute((TType)parameter);
        
        protected sealed override void OnExecuteCompleted(object parameter)
        {
            OnExecuteCompleted((TType)parameter);
        }

        protected abstract bool CanExecute(TType parameter);
        protected abstract Task<bool> OnExecute(TType parameter);
        protected virtual void OnExecuteCompleted(TType parameter)
        {
        }
    }
}
