using System.Threading.Tasks;

namespace ErrorCode.Base
{
    public abstract class LoadingAsyncCommand<T> : Command<T>
        where T : ViewModel<T>
    {
        public override sealed async void Execute(object parameter)
        {
            using (App.Load)
            {
                if (await OnExecute(parameter))
                {
                    OnExecuteCompleted(parameter);
                }
            }
        }


        protected abstract Task<bool> OnExecute(object parameter);

        protected virtual void OnExecuteCompleted(object parameter)
        {
        }
    }
}
