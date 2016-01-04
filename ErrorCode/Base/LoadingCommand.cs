namespace ErrorCode.Base
{
    public abstract class LoadingCommand<T> : Command<T>
        where T : ViewModel<T>
    {
        public sealed override void Execute(object parameter)
        {
            using (App.Load)
            {
                ExecuteWhileLoading(parameter);
            }
        }

        protected abstract void ExecuteWhileLoading(object parameter);
    }
}
