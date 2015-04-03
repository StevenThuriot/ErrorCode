namespace ErrorCode.Base
{
    public class ViewModel<T> : Notifyable
        where T : ViewModel<T>
    {
        public ViewModel()
        {
            Commands = new CommandsManager<T>(this);
        }

        public dynamic Commands { get; private set; }
    }
}