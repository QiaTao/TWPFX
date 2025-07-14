using Microsoft.Web.WebView2.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Resources;
using System.Windows.Xps.Packaging;

namespace TWPFX.Controls.TextBlock.CodeBlock
{
    public class TCodeBlock : UserControl
    {
        #region 依赖属性
        public static readonly DependencyProperty CodeProperty =
        DependencyProperty.Register("Code", typeof(string), typeof(TCodeBlock),
            new PropertyMetadata(string.Empty, OnCodeChanged));

        public static readonly DependencyProperty CodeStyleProperty =
        DependencyProperty.Register("CodeStyle", typeof(TCodeBlockStyle), typeof(TCodeBlock),
            new PropertyMetadata(TCodeBlockStyle.Github_Dark, OnCodeStyleChanged));

        public static readonly DependencyProperty ShowCopyButtonProperty =
        DependencyProperty.Register("ShowCopyButton", typeof(bool), typeof(TCodeBlock),
            new PropertyMetadata(true, OnShowCopyButtonChanged));

        public string Code
        {
            get => (string)GetValue(CodeProperty);
            set => SetValue(CodeProperty, value);
        }

        public TCodeBlockStyle CodeStyle
        {
            get => (TCodeBlockStyle)GetValue(CodeStyleProperty);
            set => SetValue(CodeStyleProperty, value);
        }

        public bool ShowCopyButton
        {
            get => (bool)GetValue(ShowCopyButtonProperty);
            set => SetValue(ShowCopyButtonProperty, value);
        }

        #endregion

        // HTML 模板
        private string htmlTemplate = @"<!DOCTYPE html>
        <html>
        <head>
            <style>
            body {{
                margin: 0;
                padding: 0;
                background-color: transparent !important;
            }}
            </style>
            <style id='code-style'>
                {0}
            </style>
            <script>
                {1}
            </script>
            <script>
                function updateCode(newCode) {{
                    const codeBlock = document.querySelector('pre code');
                    if (codeBlock) {{
                        codeBlock.textContent = newCode;
                        document.querySelectorAll('code[data-highlighted]').forEach(el => {{
                                        el.removeAttribute('data-highlighted');
                                    }});
                        hljs.highlightAll();
                    }}
                }}
                function updateStyle(newCss) {{
                    const styleElement = document.getElementById('code-style');
                    if (styleElement) {{
                        styleElement.innerHTML = newCss;
                    }}
                }}
            </script>
        </head>
        <body>
            <pre><code></code></pre>
        </body>
        </html>";
        private string jsResource = "/TWPFX;component/Assets/Highlight/highlight.js";
        private readonly WebView2CompositionControl _webView = new();
        private readonly System.Windows.Controls.Button _copyButton = new();
        private Grid _containerGrid = new();

        public TCodeBlock()
        {
            // 创建容器Grid
            _containerGrid.Children.Add(_webView);
            _containerGrid.Children.Add(_copyButton);

            Content = _containerGrid;

            // 初始化按钮样式
            InitializeCopyButton();
            // 初始化WebView2
            Loaded += async (s, e) => await InitializeWebView2();
        }

        private void InitializeCopyButton()
        {
            _copyButton.Content = "Copy";
            _copyButton.Padding = new Thickness(4, 2, 4, 2);
            _copyButton.Margin = new Thickness(0, 4, 4, 0);
            _copyButton.HorizontalAlignment = HorizontalAlignment.Right;
            _copyButton.VerticalAlignment = VerticalAlignment.Top;
            _copyButton.Background = Brushes.Transparent;
            _copyButton.BorderBrush = Brushes.Gray;
            _copyButton.Foreground = Brushes.White;
            _copyButton.Cursor = Cursors.Hand;
            _copyButton.Click += CopyButton_Click;
            _containerGrid.Background = new SolidColorBrush(Colors.Transparent);
        }

        private async Task InitializeWebView2()
        {
            await _webView.EnsureCoreWebView2Async();
            // 禁用默认右键菜单
            _webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            string html = string.Format(htmlTemplate, string.Empty, LoadResourceContent(jsResource));
            _webView.NavigateToString(html);
            _webView.NavigationCompleted += (s, e) =>
            {
                if (e.IsSuccess)
                {
                    UpdateCode();
                    UpdateCodeStyle();
                }
            };
        }


        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Code))
                {
                    Clipboard.SetText(Code);

                    // Visual feedback
                    _copyButton.Content = "Copied!";
                    var timer = new System.Windows.Threading.DispatcherTimer
                    {
                        Interval = TimeSpan.FromSeconds(1.5)
                    };
                    timer.Tick += (s, args) =>
                    {
                        _copyButton.Content = "Copy";
                        timer.Stop();
                    };
                    timer.Start();
                }
            }
            catch (Exception ex)
            {
                _copyButton.Content = "Error";
                Console.WriteLine($"Failed to copy to clipboard: {ex.Message}");
            }
        }

        private static string LoadResourceContent(string resourcePath)
        {
            Uri uri = new(resourcePath, UriKind.Relative);
            StreamResourceInfo streamInfo = Application.GetResourceStream(uri) ?? throw new FileNotFoundException($"Resource not found: {resourcePath}");
            using StreamReader reader = new(streamInfo.Stream);
            return reader.ReadToEnd();
        }

        public void UpdateCode() {
            if (_webView.CoreWebView2 != null)
            {
                _webView.ExecuteScriptAsync($"updateCode(\"{Code}\");");
            }
        }

        public void UpdateCodeStyle()
        {
            if (_webView.CoreWebView2 != null)
            {
                string css = LoadResourceContent($"/TWPFX;component/Assets/Highlight/styles/{CodeStyle.ToCss()}").Replace("\n", "");
                _webView.ExecuteScriptAsync($"updateStyle(\"{css}\");");
            }
        }

        private static void OnCodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TCodeBlock codeBlock)
            {
                codeBlock.UpdateCode();
            }
        }

        private static void OnCodeStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TCodeBlock codeBlock)
            {
                codeBlock.UpdateCodeStyle();
            }
        }

        private static void OnShowCopyButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TCodeBlock codeBlock && codeBlock._copyButton != null)
            {
                codeBlock._copyButton.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
            }
        }

    }
}
