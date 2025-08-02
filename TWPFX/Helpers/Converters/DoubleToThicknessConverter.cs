using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TWPFX.Helpers
{
    public class DoubleToThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double doubleValue)
            {
                return new Thickness(doubleValue);
            }
            return new Thickness(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Thickness thickness)
            {
                return thickness.Left; // 返回左边界值作为double
            }
            return 0.0;
        }
    }
}