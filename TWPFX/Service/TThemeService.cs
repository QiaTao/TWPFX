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

            // 初始化基础颜色
            InitializeBaseColors();

            // 初始化系统颜色
            InitializeSystemColors();

            // 初始化状态颜色
            InitializeStatusColors();
        }

        /// <summary>
        /// 初始化基础颜色
        /// </summary>
        private static void InitializeBaseColors()
        {
            // 白色系
            _resources["TColorWhite400"] = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            _resources["TColorWhite300"] = new SolidColorBrush(Color.FromRgb(248, 248, 248));
            _resources["TColorWhite200"] = new SolidColorBrush(Color.FromRgb(245, 245, 245));

            // 黑色系
            _resources["TColorBlack100"] = new SolidColorBrush(Color.FromRgb(180, 180, 180));
            _resources["TColorBlack200"] = new SolidColorBrush(Color.FromRgb(150, 150, 150));
            _resources["TColorBlack300"] = new SolidColorBrush(Color.FromRgb(64, 64, 64));
            _resources["TColorBlack400"] = new SolidColorBrush(Color.FromRgb(0, 0, 0));

            // 灰色系
            _resources["TColorGrey300"] = new SolidColorBrush(Color.FromRgb(245, 245, 245));
            _resources["TColorGrey400"] = new SolidColorBrush(Color.FromRgb(229, 229, 229));

        }

        /// <summary>
        /// 初始化系统颜色
        /// </summary>
        private static void InitializeSystemColors()
        {
            // 设置系统主题色
            var systemAccentColor = GetSystemAccentColor();
            // ------------------------------
            // 系统色（跟随系统主题）- 7阶色值
            // ------------------------------
            _resources["TColorSystem100"] = new SolidColorBrush(ModifyColorBrightness(systemAccentColor, 0.8f));  // 最浅（背景色）
            _resources["TColorSystem200"] = new SolidColorBrush(ModifyColorBrightness(systemAccentColor, 0.6f));
            _resources["TColorSystem300"] = new SolidColorBrush(ModifyColorBrightness(systemAccentColor, 0.4f));  // 浅色调（次要元素）
            _resources["TColorSystem400"] = new SolidColorBrush(systemAccentColor);                               // 主色（系统默认accent色）
            _resources["TColorSystem500"] = new SolidColorBrush(ModifyColorBrightness(systemAccentColor, -0.2f)); // 深色调（强调元素）
            _resources["TColorSystem600"] = new SolidColorBrush(ModifyColorBrightness(systemAccentColor, -0.4f));
            _resources["TColorSystem700"] = new SolidColorBrush(ModifyColorBrightness(systemAccentColor, -0.6f)); // 最深（边框、重要标记）

        }

        /// <summary>
        /// 初始化状态颜色
        /// </summary>
        private static void InitializeStatusColors()
        {
            // ------------------------------
            // 主色（Primary）- 7阶色值
            // ------------------------------
            _resources["TColorPrimary100"] = new SolidColorBrush(Color.FromRgb(220, 236, 255)); // 最浅（背景、次要元素）
            _resources["TColorPrimary200"] = new SolidColorBrush(Color.FromRgb(180, 218, 255));
            _resources["TColorPrimary300"] = new SolidColorBrush(Color.FromRgb(139, 201, 255));
            _resources["TColorPrimary400"] = new SolidColorBrush(Color.FromRgb(64, 158, 255));  // 主色（核心交互元素）
            _resources["TColorPrimary500"] = new SolidColorBrush(Color.FromRgb(38, 143, 255));
            _resources["TColorPrimary600"] = new SolidColorBrush(Color.FromRgb(0, 122, 255));
            _resources["TColorPrimary700"] = new SolidColorBrush(Color.FromRgb(0, 92, 191));   // 最深（强调、重要文本）

            // ------------------------------
            // 信息色（Info）- 7阶色值
            // ------------------------------
            _resources["TColorInfo100"] = new SolidColorBrush(Color.FromRgb(242, 242, 245));  // 最浅（背景）
            _resources["TColorInfo200"] = new SolidColorBrush(Color.FromRgb(229, 229, 234));
            _resources["TColorInfo300"] = new SolidColorBrush(Color.FromRgb(209, 209, 214));
            _resources["TColorInfo400"] = new SolidColorBrush(Color.FromRgb(144, 147, 153));  // 主色（普通文本、说明）
            _resources["TColorInfo500"] = new SolidColorBrush(Color.FromRgb(116, 119, 124));
            _resources["TColorInfo600"] = new SolidColorBrush(Color.FromRgb(86, 89, 94));
            _resources["TColorInfo700"] = new SolidColorBrush(Color.FromRgb(59, 61, 65));    // 最深（次要标题）

            // ------------------------------
            // 成功色（Success）- 7阶色值
            // ------------------------------
            _resources["TColorSuccess100"] = new SolidColorBrush(Color.FromRgb(237, 247, 237)); // 最浅（背景）
            _resources["TColorSuccess200"] = new SolidColorBrush(Color.FromRgb(215, 236, 215));
            _resources["TColorSuccess300"] = new SolidColorBrush(Color.FromRgb(187, 227, 187));
            _resources["TColorSuccess400"] = new SolidColorBrush(Color.FromRgb(103, 194, 58));  // 主色（成功状态）
            _resources["TColorSuccess500"] = new SolidColorBrush(Color.FromRgb(82, 170, 47));
            _resources["TColorSuccess600"] = new SolidColorBrush(Color.FromRgb(66, 144, 38));
            _resources["TColorSuccess700"] = new SolidColorBrush(Color.FromRgb(50, 117, 29));   // 最深（成功强调）

            // ------------------------------
            // 警告色（Warning）- 7阶色值
            // ------------------------------
            _resources["TColorWarning100"] = new SolidColorBrush(Color.FromRgb(255, 248, 230));  // 最浅（背景）
            _resources["TColorWarning200"] = new SolidColorBrush(Color.FromRgb(255, 238, 200));
            _resources["TColorWarning300"] = new SolidColorBrush(Color.FromRgb(255, 227, 167));
            _resources["TColorWarning400"] = new SolidColorBrush(Color.FromRgb(230, 162, 60));  // 主色（警告状态）
            _resources["TColorWarning500"] = new SolidColorBrush(Color.FromRgb(217, 144, 37));
            _resources["TColorWarning600"] = new SolidColorBrush(Color.FromRgb(194, 125, 23));
            _resources["TColorWarning700"] = new SolidColorBrush(Color.FromRgb(166, 102, 11));   // 最深（警告强调）

            // ------------------------------
            // 危险色（Danger）- 7阶色值
            // ------------------------------
            _resources["TColorDanger100"] = new SolidColorBrush(Color.FromRgb(255, 240, 240));   // 最浅（背景）
            _resources["TColorDanger200"] = new SolidColorBrush(Color.FromRgb(255, 220, 220));
            _resources["TColorDanger300"] = new SolidColorBrush(Color.FromRgb(255, 197, 197));
            _resources["TColorDanger400"] = new SolidColorBrush(Color.FromRgb(245, 108, 108));  // 主色（错误/危险状态）
            _resources["TColorDanger500"] = new SolidColorBrush(Color.FromRgb(237, 73, 73));
            _resources["TColorDanger600"] = new SolidColorBrush(Color.FromRgb(220, 41, 41));
            _resources["TColorDanger700"] = new SolidColorBrush(Color.FromRgb(184, 25, 25));    // 最深（危险强调）
        }

        /// <summary>
        /// 从资源字典获取画刷
        /// </summary>
        /// <param name="key">资源键</param>
        /// <returns>画刷</returns>
        public static SolidColorBrush GetBrush(string key)
        {
            if (_resources == null)
                return new SolidColorBrush(Colors.Black);
                
            return _resources[key] as SolidColorBrush ?? new SolidColorBrush(Colors.Black);
        }

        /// <summary>
        /// 获取系统主题色
        /// </summary>
        /// <returns>系统主题色</returns>
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

        /// <summary>
        /// 从注册表读取主题色（Windows 10/11）
        /// </summary>
        /// <returns>主题色</returns>
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

        /// <summary>
        /// 修改颜色亮度
        /// </summary>
        /// <param name="color">原始颜色</param>
        /// <param name="correctionFactor">修正因子</param>
        /// <returns>修改后的颜色</returns>
        public static Color ModifyColorBrightness(Color color, float correctionFactor)
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

        /// <summary>
        /// 将uint转换为Color
        /// </summary>
        /// <param name="color">uint颜色值</param>
        /// <returns>Color对象</returns>
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
