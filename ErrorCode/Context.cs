using System.Windows;

namespace ErrorCode
{
    static class Context
    {
        public static Base.IViewModel GetViewModel(DependencyObject obj) => (Base.IViewModel)obj.GetValue(ViewModelProperty);

        public static void SetViewModel(DependencyObject obj, Base.IViewModel value)
        {
            obj.SetValue(ViewModelProperty, value);
        }
        
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.RegisterAttached("ViewModel", typeof(Base.IViewModel), typeof(Context), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));
    }
}
