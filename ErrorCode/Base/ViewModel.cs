namespace ErrorCode.Base
{
    public class ViewModel<T> : Notifyable, IViewModel
        where T : ViewModel<T>
    {
        public ViewModel()
        {
            Commands = new CommandsManager<T>(this);
        }

        public dynamic Commands { get; }
    }
}