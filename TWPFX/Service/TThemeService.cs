using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace TWPFX.Service
{
    public static class TThemeService
    {
        private static ResourceDictionary _resources;

        [DllImport("dwmapi.dll", PreserveSig = false)]
        private static extern void DwmGetColorizationColor(out uint color, out bool opaque);

        public static void Initialize(Application app)
        {
            _resources = app.Resources;

            // 设置系统主题色
            var systemAccentColor = GetSystemAccentColor();
            _resources["TSystemAccentColor"] = new SolidColorBrush(systemAccentColor);
            _resources["TSystemAccentColorLight"] = new SolidColorBrush(ModifyColorBrightness(systemAccentColor, 0.3f));
            _resources["TSystemAccentColorDark"] = new SolidColorBrush(ModifyColorBrightness(systemAccentColor, -0.3f));
        }

        // 获取当前系统主题色
        public static Color GetSystemAccentColor()
        {
            try
            {
                // 方法1：DWM API（更准确）
                DwmGetColorizationColor(out uint color, out _);
                return ToColor(color);
            }
            catch
            {
                try
                {
                    // 方法2：注册表（兼容性更强）
                    return GetSystemAccentColorFromRegistry();
                }
                catch
                {
                    return Color.FromArgb(255, 0, 120, 215); // 默认蓝色
                }
            }
        }

        // 从注册表读取主题色（Windows 10/11）
        private static Color GetSystemAccentColorFromRegistry()
        {
            using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\DWM"))
            {
                if (key?.GetValue("ColorizationColor") is int colorValue)
                {
                    byte[] bytes = BitConverter.GetBytes(colorValue);
                    return Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
                }
            }
            return Color.FromArgb(255, 0, 120, 215); // 默认蓝色
        }

        private static Color ModifyColorBrightness(Color color, float correctionFactor)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (byte)red, (byte)green, (byte)blue);
        }

        private static Color ToColor(uint color)
        {
            return Color.FromArgb(
                (byte)((color >> 24) & 0xFF),
                (byte)((color >> 16) & 0xFF),
                (byte)((color >> 8) & 0xFF),
                (byte)(color & 0xFF));
        }
    }
}
