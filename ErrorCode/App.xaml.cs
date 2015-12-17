using System;
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
                
                var action = new Action<MainWindow, bool>((w, b) => w.IsLoading = b);

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
                
                dispatcher.BeginInvoke(priority, action, Window, value);
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
    }
}