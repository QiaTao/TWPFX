using System;
using System.Globalization;
using System.Windows.Data;

namespace TWPFX.Helpers.Converters
{
    /// <summary>
    /// 将半径和线宽转换为直径（用于环形进度条的宽高绑定，支持MultiBinding）。
    /// 直径 = 2 * 半径 + 线宽
    /// </summary>
    public class RadiusAndThicknessToDiameterConverter : IMultiValueConverter
    {
        /// <summary>
        /// 将半径和线宽转换为直径。
        /// </summary>
        /// <param name="values">[0]=半径, [1]=线宽</param>
        /// <returns>直径</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double radius = System.Convert.ToDouble(values[0]);
            double thickness = System.Convert.ToDouble(values[1]);
            return 2 * radius + thickness;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
} 