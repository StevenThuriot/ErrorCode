using System.Windows;

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


        bool _isLoading = false;
        Visibility _isLoadingVisibility = Visibility.Collapsed;

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (SetValue(ref _isLoading, value))
                {
                    IsLoadingVisibility = value ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        public Visibility IsLoadingVisibility
        {
            get { return _isLoadingVisibility; }
            private set { SetValue(ref _isLoadingVisibility, value); }
        }
    }
}