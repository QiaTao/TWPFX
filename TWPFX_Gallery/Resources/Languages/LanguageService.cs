using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace TWPFX_Gallery.Resources.Languages
{
    public static class LanguageService
    {
        private static ResourceDictionary _currentDictionary;

        public static void ChangeLanguage(string languageCode)
        {
            // 构建资源路径 (根据实际程序集名调整)
            string path = $"pack://application:,,,/TWPFX_Gallery;component/Resources/Languages/Strings.{languageCode}.xaml";

            // 移除旧语言资源
            if (_currentDictionary != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(_currentDictionary);
            }

            // 加载新语言资源
            _currentDictionary = new ResourceDictionary { Source = new Uri(path) };
            Application.Current.Resources.MergedDictionaries.Add(_currentDictionary);
        }

        // 可选：自动检测系统语言
        public static void Initialize()
        {
            string sysLang = CultureInfo.CurrentCulture.Name;
            ChangeLanguage(sysLang == "zh-CN" ? "zh-CN" : "en-US");
        }
    }
}
