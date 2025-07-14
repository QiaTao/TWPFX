using System.ComponentModel;
using System.Windows.Media;

namespace TWPFX.Controls.Icon.SegoeIcon
{
    public static class TSegoeIconExtensions
    {
        /// <summary>
        /// 获取图标字符
        /// </summary>
        public static char ToChar(this TSegoeIconType icon) => (char)icon;

        /// <summary>
        /// 获取图标描述
        /// </summary>
        public static string GetDescription(this TSegoeIconType icon)
        {
            var field = icon.GetType().GetField(icon.ToString());
            var attribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false)
                              .FirstOrDefault() as DescriptionAttribute;
            return attribute?.Description ?? icon.ToString();
        }

        /// <summary>
        /// 获取带字体家族的TextBlock
        /// </summary>
        public static System.Windows.Controls.TextBlock ToTextBlock(this TSegoeIconType icon,
                                          double fontSize = 16,
                                          Brush foreground = null)
        {
            return new System.Windows.Controls.TextBlock
            {
                Text = icon.ToChar().ToString(),
                FontFamily = new FontFamily("Segoe Fluent Icons"),
                FontSize = fontSize,
                Foreground = foreground ?? Brushes.Black
            };
        }

        /// <summary>
        /// 将十六进制值转换为C# Unicode转义字符串
        /// </summary>
        /// <param name="hexValue">如 0xE700</param>
        /// <returns>返回如 "\xE700"</returns>
        public static string ToUnicodeEscapeString(this ushort hexValue)
        {
            return $"\\x{hexValue:X4}";
        }

        /// <summary>
        /// 将SegoeIconType枚举值转换为可渲染的Unicode字符
        /// </summary>
        public static string ToUnicodeChar(this TSegoeIconType icon)
        {
            return char.ConvertFromUtf32((ushort)icon);
        }

    }
}
