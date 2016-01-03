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


        public static bool IsLoading
        {
            get { return Window?.IsLoading ?? false; }
            set
            {
                var dispatcher = CurrentDispatcher;

                if (dispatcher == null)
                    return;
                
                DispatcherPriority priority;

                if (value)
                {
                    if (dispatcher.CheckAccess())
                    {
                        Window.IsLoading = value;
                        return;
                    }

                    priority = DispatcherPriority.Send;                    
                }
                else
                {
                    priority = DispatcherPriority.Loaded;
                }
                
                dispatcher.BeginInvoke(priority, new Action(() => Window.IsLoading = value));
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


        public static ObservableCollection<UIElement> LeftWindowControls => (Window.LeftWindowCommands.ItemsSource) as ObservableCollection<UIElement>;
        public static ObservableCollection<UIElement> RightWindowControls => (Window.RightWindowCommands.ItemsSource) as ObservableCollection<UIElement>;
    }
}