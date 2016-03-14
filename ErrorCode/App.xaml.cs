using ErrorCode.Base;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace ErrorCode
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static IDisposable Load => new LoadingHelper();
        public static Dispatcher CurrentDispatcher => Current?.Dispatcher;
        public static MainWindow Window => (MainWindow)Current?.MainWindow;
        public static IViewModel CurrentViewModel => Window?.CurrentViewModel;

        public static bool IsLoading
        {
            get { return CurrentViewModel?.IsLoading ?? false; }
            set
            {
                var dispatcher = CurrentDispatcher;
                var viewmodel = CurrentViewModel;
                if (dispatcher == null || viewmodel == null)
                    return;

                DispatcherPriority priority;

                if (value)
                {
                    if (dispatcher.CheckAccess())
                    {
                        viewmodel.IsLoading = value;
                        return;
                    }

                    priority = DispatcherPriority.Send;
                }
                else
                {
                    priority = DispatcherPriority.Loaded;
                }

                dispatcher.BeginInvoke(priority, new Action(() => viewmodel.IsLoading = value));
            }
        }

        class LoadingHelper : IDisposable
        {
            public LoadingHelper()
            {
                IsLoading = true;
            }

            public void Dispose()
            {
                IsLoading = false;
                GC.SuppressFinalize(this);
            }

            ~LoadingHelper()
            {
                IsLoading = false;
            }
        }
        

        public static ObservableCollection<UIElement> LeftWindowControls => (ObservableCollection<UIElement>)(Window.LeftWindowCommands.ItemsSource);
        public static ObservableCollection<UIElement> RightWindowControls => (ObservableCollection<UIElement>)(Window.RightWindowCommands.ItemsSource);
    }
}