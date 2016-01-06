using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace ErrorCode
{
    class PaddingConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
        


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int padding = parameter == null || parameter == DependencyProperty.UnsetValue
                ? -25 
                : int.Parse((string)parameter);

            var actual = (double)value;


            return actual + padding;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
