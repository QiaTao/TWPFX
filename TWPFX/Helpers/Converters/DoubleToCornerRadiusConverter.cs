using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TWPFX.Helpers
{
    public class DoubleToCornerRadiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double doubleValue)
            {
                return new CornerRadius(doubleValue);
            }
            return new CornerRadius(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CornerRadius cornerRadius)
            {
                return cornerRadius.TopLeft; // 返回左上角值作为double
            }
            return 0.0;
        }
    }
}