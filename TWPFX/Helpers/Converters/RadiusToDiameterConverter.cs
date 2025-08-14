using System;
using System.Globalization;
using System.Windows.Data;

namespace TWPFX.Helpers.Converters
{
    /// <summary>
    /// 将圆环的半径和线宽转换为直径，用于环形进度条等控件的宽高绑定。
    /// </summary>
    public class RadiusToDiameterConverter : IValueConverter
    {
        /// <summary>
        /// 将半径和线宽转换为直径。
        /// </summary>
        /// <param name="value">半径</param>
        /// <param name="parameter">线宽</param>
        /// <returns>直径</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double radius = System.Convert.ToDouble(value);
            double thickness = parameter != null ? System.Convert.ToDouble(parameter) : 0;
            return 2 * radius + 2 * thickness;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
} 