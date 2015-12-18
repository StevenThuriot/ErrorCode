using System;
using System.Windows;
using ErrorCode.ViewModels;
using System.Collections.ObjectModel;

namespace ErrorCode
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static readonly DependencyProperty CurrentViewModelProperty = DependencyProperty.Register("CurrentViewModel", typeof (object), typeof (MainWindow));
        
        public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register("IsLoading", typeof (bool), typeof (MainWindow), new PropertyMetadata(false, OnIsLoadingChanged));


        public static readonly DependencyPropertyKey IsLoadingVisibilityPropertyKey = DependencyProperty.RegisterReadOnly("IsLoadingVisibility", typeof (Visibility), typeof (MainWindow), new PropertyMetadata(Visibility.Collapsed));

        public static readonly DependencyProperty IsLoadingVisibilityProperty = IsLoadingVisibilityPropertyKey.DependencyProperty;

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

        public object CurrentViewModel
        {
            get { return GetValue(CurrentViewModelProperty); }
            set { SetValue(CurrentViewModelProperty, value); }
        }

        public bool IsLoading
        {
            get { return (bool) GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public Visibility IsLoadingVisibility
        {
            get { return (Visibility) GetValue(IsLoadingVisibilityProperty); }
            protected set { SetValue(IsLoadingVisibilityPropertyKey, value); }
        }


        private static void OnIsLoadingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MainWindow) d).IsLoadingVisibility =
                (bool) e.NewValue
                    ? Visibility.Visible
                    : Visibility.Collapsed;
        }
    }
}