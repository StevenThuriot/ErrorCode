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
            int padding;
            if (parameter == null || parameter == DependencyProperty.UnsetValue) 
            {
                padding = (int)parameter;
            }
            else if (parameter is int)
            {
                padding = -25;
            }
            else
            {
                padding = int.Parse((string)parameter);
            }

            var actual = (double)value;


            return actual + padding;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int padding;
            if (parameter == null || parameter == DependencyProperty.UnsetValue)
            {
                padding = (int)parameter;
            }
            else if (parameter is int)
            {
                padding = -25;
            }
            else
            {
                padding = int.Parse((string)parameter);
            }

            var actual = (double)value;


            return actual - padding;
        }
    }
}
