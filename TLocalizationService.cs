using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TWPFX.Service
{
    // 存储外部资源的元数据（基础路径+当前语言）
    public class ExternalResourceInfo
    {
        public string AssemblyName { get; set; } // 外部程序集名称（简短标识）
        public string CurrentLanguage { get; set; }
    }

    public static class TLocalizationService
    {
        // 内置固定路径模板（约定外部资源的存放规则）
        private const string ExternalResourcePathTemplate =
            "pack://application:,,,/{0};component/Resources/Languages/Strings.{1}.xaml";
        // 格式说明：{0}=程序集名称，{1}=语言代码

        private static ResourceDictionary _defaultDictionary;
        private static readonly Dictionary<string, ExternalResourceInfo> _externalResources = [];
        private static string _currentLanguage;

        // 静态构造函数 - 自动初始化
        static TLocalizationService()
        {
            Initialize();
        }

        /// <summary>
        /// 初始化并加载默认语言（自动调用，无需手动触发）
        /// </summary>
        private static void Initialize()
        {
            string sysLang = CultureInfo.CurrentCulture.Name;
            _currentLanguage = sysLang == "zh-CN" ? "zh-CN" : "en-US";
            LoadDefaultResource(_currentLanguage);
        }

        /// <summary>
        /// 注册外部程序集资源（仅需传入程序集名称，自动拼接路径）
        /// </summary>
        /// <param name="assemblyName">外部程序集名称（如"TWPFX_Gallery"）</param>
        public static void ChangeLanguage(string assemblyName)
        {
            if (string.IsNullOrEmpty(assemblyName))
                throw new ArgumentNullException(nameof(assemblyName), "程序集名称不能为空");

            if (_externalResources.ContainsKey(assemblyName))
            {
                // 已注册过，直接刷新
                ReloadExternalResource(assemblyName);
                return;
            }

            // 存储程序集名称（后续用于生成路径）
            _externalResources[assemblyName] = new ExternalResourceInfo
            {
                AssemblyName = assemblyName,
                CurrentLanguage = null
            };

            ReloadExternalResource(assemblyName);
        }

        // 重新加载外部资源（使用内置模板生成路径）
        private static void ReloadExternalResource(string assemblyName)
        {
            if (!_externalResources.TryGetValue(assemblyName, out var info))
                return;

            try
            {
                // 1. 移除旧资源
                if (!string.IsNullOrEmpty(info.CurrentLanguage))
                {
                    string oldPath = string.Format(ExternalResourcePathTemplate, assemblyName, info.CurrentLanguage);
                    var oldDict = Application.Current.Resources.MergedDictionaries
                        .FirstOrDefault(d => d.Source?.OriginalString == oldPath);
                    if (oldDict != null)
                    {
                        Application.Current.Resources.MergedDictionaries.Remove(oldDict);
                    }
                }

                // 2. 生成新路径（使用内置模板 + 程序集名称 + 当前语言）
                string newPath = string.Format(ExternalResourcePathTemplate, assemblyName, _currentLanguage);
                var newDict = new ResourceDictionary { Source = new Uri(newPath) };
                Application.Current.Resources.MergedDictionaries.Add(newDict);

                // 3. 更新记录
                info.CurrentLanguage = _currentLanguage;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载资源 {assemblyName}（{_currentLanguage}）失败：{ex.Message}");
            }
        }

        // 加载默认资源（保持不变）
        private static void LoadDefaultResource(string languageCode)
        {
            if (_defaultDictionary != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(_defaultDictionary);
            }

            try
            {
                string path = $"pack://application:,,,/TWPFX_Gallery;component/Resources/Languages/Strings.{languageCode}.xaml";
                _defaultDictionary = new ResourceDictionary { Source = new Uri(path) };
                Application.Current.Resources.MergedDictionaries.Add(_defaultDictionary);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载默认资源失败：{ex.Message}");
            }
        }

        // 移除外部资源
        public static void UnregisterExternalResource(string assemblyName)
        {
            if (_externalResources.TryGetValue(assemblyName, out var info))
            {
                string path = string.Format(ExternalResourcePathTemplate, assemblyName, info.CurrentLanguage);
                var dict = Application.Current.Resources.MergedDictionaries
                    .FirstOrDefault(d => d.Source?.OriginalString == path);
                if (dict != null)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(dict);
                }
                _externalResources.Remove(assemblyName);
            }
        }
    }
}
