namespace ErrorCode.Base
{
    public abstract class TypedLoadingCommand<T, TType> : LoadingCommand<T>
        where T : ViewModel<T>
    {
        public sealed override bool CanExecute(object parameter) => base.CanExecute(parameter) && parameter is TType && CanExecute((TType)parameter);

        protected sealed override void ExecuteWhileLoading(object parameter) => ExecuteWhileLoading((TType)parameter);


        protected abstract bool CanExecute(TType parameter);
        protected abstract void ExecuteWhileLoading(TType parameter);
    }
}
