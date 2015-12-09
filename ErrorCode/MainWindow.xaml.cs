using System;
using System.Windows;
using ErrorCode.ViewModels;

namespace ErrorCode
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static readonly DependencyProperty CurrentViewModelProperty = DependencyProperty.Register("CurrentViewModel", typeof (object), typeof (MainWindow), new PropertyMetadata(new Overview()));
        
        public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register("IsLoading", typeof (bool), typeof (MainWindow), new PropertyMetadata(false, OnIsLoadingChanged));


        public static readonly DependencyPropertyKey IsLoadingVisibilityPropertyKey = DependencyProperty.RegisterReadOnly("IsLoadingVisibility", typeof (Visibility), typeof (MainWindow), new PropertyMetadata(Visibility.Collapsed));

        public static readonly DependencyProperty IsLoadingVisibilityProperty = IsLoadingVisibilityPropertyKey.DependencyProperty;

        public MainWindow()
        {
            UseLayoutRounding = true;
            InitializeComponent();
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