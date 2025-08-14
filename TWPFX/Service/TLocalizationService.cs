using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace TWPFX.Service
{

    public static class TLocalizationService
    {
        private static readonly Dictionary<string, Dictionary<string, ResourceDictionary>> _loadedResources = [];   // 存储所有已加载程序集的语言资源字典

        private static string _currentLanguage = "en-US";  // 当前使用的语言代码
        private static readonly List<string> _loadedAssemblies = [];  // 已加载本地化资源的程序集列表

        // 记录每个程序集当前应用的语言资源字典
        private static readonly Dictionary<string, ResourceDictionary> _currentResourceDicts = [];

        // 支持的语言列表（可扩展）
        private static readonly HashSet<string> _supportedLanguages = new(StringComparer.OrdinalIgnoreCase) 
        {
            "en-US", "en-GB",
            "zh-CN", "zh-TW", "zh-HK",
            "ja-JP", "ko-KR",
            "fr-FR", "de-DE", "es-ES",
            "ar-SA", "ru-RU"
        };

        /// <summary>
        /// 添加支持的语言代码
        /// </summary>
        /// <param name="languageCode">要添加的BCP-47语言代码</param>
        public static void AddSupportedLanguage(string languageCode)
        {
            if (!IsValidLanguageCode(languageCode))
                throw new ArgumentException($"Invalid language code: {languageCode}");

            _supportedLanguages.Add(NormalizeLanguageCode(languageCode));
        }

        /// <summary>
        /// 加载指定程序集的本地化资源
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        public static void LoadLocalization(string assemblyName)
        {
            if (string.IsNullOrWhiteSpace(assemblyName))
                throw new ArgumentException("Assembly name cannot be null or empty.");

            if (_loadedAssemblies.Contains(assemblyName, StringComparer.OrdinalIgnoreCase))
                return;

            try
            {
                var languageDicts = new Dictionary<string, ResourceDictionary>(StringComparer.OrdinalIgnoreCase);

                foreach (var lang in _supportedLanguages)
                {
                    string uriString = $"pack://application:,,,/{assemblyName};component/Resources/Langs/Lang.{lang}.xaml";

                    try
                    {
                        var resourceDict = new ResourceDictionary
                        {
                            Source = new Uri(uriString, UriKind.Absolute)
                        };
                        languageDicts[lang] = resourceDict;
                        Debug.WriteLine($"Loaded {lang} resources for {assemblyName}");
                    }
                    catch (System.IO.IOException ex)
                    {
                        Debug.WriteLine($"Resource not found: {uriString}. Error: {ex.Message}");
                    }
                    catch (UriFormatException ex)
                    {
                        Debug.WriteLine($"Invalid URI format: {uriString}. Error: {ex.Message}");
                    }
                }

                _loadedResources[assemblyName] = languageDicts;
                _loadedAssemblies.Add(assemblyName);

                ApplyLanguageToAssembly(assemblyName, _currentLanguage);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading resources for {assemblyName}: {ex.Message}");
                throw new InvalidOperationException($"Failed to load localization for {assemblyName}", ex);
            }
        }

        /// <summary>
        /// 切换应用程序语言
        /// </summary>
        /// <param name="languageCode">BCP-47语言代码</param>
        public static void ChangeLanguage(string languageCode)
        {
            if (string.IsNullOrWhiteSpace(languageCode))
                throw new ArgumentException("Language code cannot be null or empty.");

            languageCode = NormalizeLanguageCode(languageCode);

            if (!IsValidLanguageCode(languageCode))
                throw new ArgumentException($"Invalid BCP-47 language code: {languageCode}");

            if (string.Equals(_currentLanguage, languageCode, StringComparison.OrdinalIgnoreCase))
                return;

            _currentLanguage = languageCode;

            foreach (var assembly in _loadedAssemblies.ToList())
            {
                ApplyLanguageToAssembly(assembly, languageCode);
            }
        }

        /// <summary>
        /// 获取当前使用的语言代码
        /// </summary>
        public static string GetCurrentLanguage() => _currentLanguage;

        /// <summary>
        /// 获取支持的语言列表
        /// </summary>
        public static IReadOnlyCollection<string> GetSupportedLanguages() => _supportedLanguages;

        /// <summary>
        /// 将指定语言应用到程序集
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="languageCode">语言代码</param>
        /// <remarks>
        /// 此方法执行以下操作：
        /// 1. 移除该程序集当前使用的语言资源
        /// 2. 尝试加载精确匹配的语言资源
        /// 3. 如果精确匹配失败，尝试主要语言回退（如 zh-CN → zh）
        /// 4. 如果回退失败，使用英语作为最终回退
        /// 5. 如果英语不可用，使用第一个可用的语言资源
        /// </remarks>
        private static void ApplyLanguageToAssembly(string assemblyName, string languageCode)
        {
            if (!_loadedResources.TryGetValue(assemblyName, out var languageDicts))
                return;

            var appDictionaries = Application.Current.Resources.MergedDictionaries;

            // 只移除该程序集的语言资源，不影响其他资源
            if (_currentResourceDicts.TryGetValue(assemblyName, out var currentDict))
            {
                appDictionaries.Remove(currentDict);
                _currentResourceDicts.Remove(assemblyName);
            }

            // 尝试加载精确匹配的语言
            if (languageDicts.TryGetValue(languageCode, out var newDict))
            {
                appDictionaries.Add(newDict);
                _currentResourceDicts[assemblyName] = newDict;
                Debug.WriteLine($"Applied language '{languageCode}' for {assemblyName}");
                return;
            }

            // 回退机制：尝试主要语言部分
            var mainLanguage = languageCode.Split('-')[0];
            var fallbackDict = languageDicts.FirstOrDefault(x =>
                x.Key.StartsWith(mainLanguage + "-", StringComparison.OrdinalIgnoreCase) ||
                x.Key.Equals(mainLanguage, StringComparison.OrdinalIgnoreCase)).Value;

            if (fallbackDict != null)
            {
                appDictionaries.Add(fallbackDict);
                _currentResourceDicts[assemblyName] = fallbackDict;
                Debug.WriteLine($"Fallback to '{fallbackDict.Source}' for {assemblyName}");
                return;
            }

            // 最终回退到英语或第一个可用语言
            if (languageDicts.TryGetValue("en-US", out var englishDict))
            {
                appDictionaries.Add(englishDict);
                _currentResourceDicts[assemblyName] = englishDict;
                Debug.WriteLine($"Using en-US as fallback for {assemblyName}");
            }
            else if (languageDicts.Count > 0)
            {
                var firstDict = languageDicts.Values.First();
                appDictionaries.Add(firstDict);
                _currentResourceDicts[assemblyName] = firstDict;
                Debug.WriteLine($"Using first available language for {assemblyName}");
            }
        }

        /// <summary>
        /// 验证语言代码是否有效
        /// </summary>
        /// <param name="code">待验证的语言代码</param>
        private static bool IsValidLanguageCode(string code)
        {
            try
            {
                var culture = CultureInfo.GetCultureInfo(code);
                return culture != null;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 规范化语言代码格式
        /// </summary>
        private static string NormalizeLanguageCode(string code)
        {
            try
            {
                var culture = CultureInfo.GetCultureInfo(code);
                return culture.Name;
            }
            catch
            {
                return code;
            }
        }

        /// <summary>
        /// 获取本地化资源字符串
        /// </summary>
        /// <param name="key">资源键</param>
        /// <param name="defaultValue">默认值，如果资源不存在则返回此值</param>
        /// <returns>本地化的字符串值</returns>
        public static string GetLocalizedString(string key, string defaultValue = null)
        {
            if (string.IsNullOrWhiteSpace(key))
                return defaultValue ?? string.Empty;

            try
            {
                // 尝试从应用程序资源中获取
                if (Application.Current?.Resources?.Contains(key) == true)
                {
                    var resource = Application.Current.Resources[key];
                    if (resource is string stringValue)
                        return stringValue;
                }

                // 如果应用程序资源中没有，尝试从当前加载的资源字典中获取
                foreach (var assembly in _loadedAssemblies)
                {
                    if (_currentResourceDicts.TryGetValue(assembly, out var resourceDict))
                    {
                        if (resourceDict.Contains(key))
                        {
                            var resource = resourceDict[key];
                            if (resource is string stringValue)
                                return stringValue;
                        }
                    }
                }

                return defaultValue ?? key;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting localized string for key '{key}': {ex.Message}");
                return defaultValue ?? key;
            }
        }
    }
}
