using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Wpf;
using System.IO;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Resources;
using System.Xml.Linq;
using TWPFX.Animations;
using TWPFX.Controls.Button.SegoeButton;
using TWPFX.Controls.Icon.SegoeIcon;
using TWPFX.Controls.Notification.InfoBar;

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

        // 单行代码高度（根据实际字体大小调整，默认1.5倍字体大小）
        public double LineHeight { get; set; } = 18; // 假设字体16px，行高24px（1.5倍）

        // 代码块上下边距（用于调整整体高度）
        public double VerticalPadding { get; set; } = 16; // 上下各8px，共16px

        // 最小高度（避免行数过少时高度过小）
        public double MinCodeHeight { get; set; } = 60;

        // 最大高度（超过时显示滚动条）
        public double MaxCodeHeight { get; set; } = 400;

        #endregion

        // HTML 模板
        private string htmlTemplate = @"<!DOCTYPE html>
            <html>
            <head>
                <style>
                html, body {{
                    margin: 0;
                    padding: 0;
                    width: 100%;
                    height: 100%;
                    background-color: transparent !important;
                }}
                pre {{
                    margin: 0 !important;
                    padding: 0 !important;
                    width: 100%;
                    height: 100%;
                }}
                code {{
                    display: block;
                    width: 100%;
                    height: 100%;
                    box-sizing: border-box;
                    overflow: hidden !important;
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
        private readonly TSegoeButton _copyButton = new();
        private Grid _containerGrid = new();
        private bool _isWebViewInitialized = false; // 新增：初始化状态标志


        public TCodeBlock()
        {
            // 创建容器Grid，并设置行列定义（关键修改）
            _containerGrid = new Grid();
            // 不需要额外定义行列，直接通过对齐方式控制位置

            // 初始化WebView2的布局：填充整个Grid
            _webView.HorizontalAlignment = HorizontalAlignment.Stretch;
            _webView.VerticalAlignment = VerticalAlignment.Stretch;
            Panel.SetZIndex(_webView, 0); // 底层

            // 初始化复制按钮的布局：固定在右上角（关键修改）
            _copyButton.HorizontalAlignment = HorizontalAlignment.Right; // 右对齐
            _copyButton.VerticalAlignment = VerticalAlignment.Top;      // 上对齐
            _copyButton.Margin = new Thickness(0, 8, 8, 0);             // 右上角边距（上8，右8）

            // 将控件添加到Grid
            _containerGrid.Children.Add(_webView);
            _containerGrid.Children.Add(_copyButton);

            Content = _containerGrid;

            // 初始化按钮样式
            InitializeCopyButton();
            // 加载事件处理
            Loaded += OnLoaded;
        }

        private void InitializeCopyButton()
        {
            _copyButton.Glyph = TSegoeIconType.Copy;
            _copyButton.GlyphSize = 14;
            _copyButton.Width = 26;
            _copyButton.Height = 26;
            _copyButton.Clicked += CopyButton_Click;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            // 每次加载都更新高度
            UpdateHeightByLineCount();

            if (!_isWebViewInitialized)
            {
                await InitializeWebView2();
            }
            else
            {
                // WebView2 已初始化，直接更新代码和样式
                UpdateCode();
                UpdateCodeStyle();
            }
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
                    _isWebViewInitialized = true; // 标记为已初始化
                }
            };
        }

        private void CopyButton_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Code) && sender != null)
                {
                    TSegoeButton button = (TSegoeButton)sender;
                    button.CreateAnimationSequence()
                    .AddStep(targetProperty: "GlyphSize", from: 14, to: 1, durationMs: 300) // 第一步：缩小图标 (20 -> 1)
                    .AddStep(targetProperty: "GlyphSize", from: null, to: 14, durationMs: 300, preAction: () => { button.Glyph = TSegoeIconType.CheckMark; }) // 第二步：放大图标并切换为勾选图标 (当前值 -> 16)
                    .AddStep(targetProperty: "GlyphSize", from: null, to: 1, durationMs: 300, preAction: () =>
                    {
                        Task.Run(() => {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                try
                                {
                                    Clipboard.SetText(Code);
                                }
                                catch (Exception ex) { }
                            });
                        });
                    }) // 第三步：延迟2秒后缩小图标 (当前值 -> 1)
                    .AddStep(targetProperty: "GlyphSize", from: null, to: 14, durationMs: 300, preAction: () => { button.Glyph = TSegoeIconType.Copy; }) // 第四步：恢复图标大小并切换为复制图标 (当前值 -> 16)
                    .Run(restartIfRunning: false);
                }
            }
            catch (Exception ex)
            {
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

        public async void UpdateCode()
        {
            if (_webView.CoreWebView2 != null)
             {
                // 对代码进行完整转义
                string escapedCode = Code
                .Replace("\\", "\\\\")   // 反斜杠
                .Replace("\"", "\\\"")   // 双引号
                .Replace("\r", "\\r")    // 回车符
                .Replace("\n", "\\n")    // 换行符
                .Replace("\t", "\\t")    // 制表符
                .Replace("'", "\\'");    // 单引号
                await _webView.ExecuteScriptAsync($"updateCode(\"{escapedCode}\");");
            }
        }

        /// <summary>
        /// 根据代码行数计算并更新高度
        /// </summary>
        private void UpdateHeightByLineCount()
        {
            if (string.IsNullOrEmpty(Code))
            {
                // 代码为空时使用最小高度
                SetHeight(MinCodeHeight);
                return;
            }

            // 1. 计算行数（按换行符分割）
            int lineCount = Code.Split("\n").Length;

            // 2. 计算总高度（行数×单行高度 + 上下边距）
            double calculatedHeight = lineCount * LineHeight + VerticalPadding;

            // 3. 限制高度在最小和最大之间
            double finalHeight = Math.Clamp(calculatedHeight, MinCodeHeight, MaxCodeHeight);

            // 4. 设置高度
            SetHeight(finalHeight);
        }


        /// <summary>
        /// 同步设置控件和WebView2的高度
        /// </summary>
        private void SetHeight(double height)
        {
            // 设置当前控件高度
            this.Height = height;
            // 同步WebView2高度（确保内容区域匹配）
            _webView.Height = height;
            // 确保容器Grid高度同步
            _containerGrid.Height = height;
        }

        public void UpdateCodeStyle()
        {
            if (_webView.CoreWebView2 != null)
            {
                string css = LoadResourceContent($"/TWPFX;component/Assets/Highlight/styles/{CodeStyle.ToCss()}").Replace("\n", "");
                string cssContent = $"updateStyle(`{css}`);";
                // 执行脚本
                _webView.ExecuteScriptAsync(cssContent);
            }
        }

        private static void OnCodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TCodeBlock codeBlock)
            {
                codeBlock.UpdateCode();
                // 代码变化时重新计算高度（新增）
                codeBlock.UpdateHeightByLineCount();
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
