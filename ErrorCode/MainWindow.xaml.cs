using System;
using System.Windows;
using ErrorCode.ViewModels;
using System.Collections.ObjectModel;
using ErrorCode.Base;

namespace ErrorCode
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static readonly DependencyProperty CurrentViewModelProperty = DependencyProperty.Register("CurrentViewModel", typeof (IViewModel), typeof (MainWindow));

        public MainWindow()
        {
            UseLayoutRounding = true;
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            LeftWindowCommands = new MahApps.Metro.Controls.WindowCommands();
            LeftWindowCommands.ItemsSource = new ObservableCollection<UIElement>();

            RightWindowCommands = new MahApps.Metro.Controls.WindowCommands();
            RightWindowCommands.ItemsSource = new ObservableCollection<UIElement>();

            CurrentViewModel = new Overview();
        }

        public IViewModel CurrentViewModel
        {
            get { return (IViewModel)GetValue(CurrentViewModelProperty); }
            set { SetValue(CurrentViewModelProperty, value); }
        }
    }
}