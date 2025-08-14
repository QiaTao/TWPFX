using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TWPFX.Animations;
using TWPFX.Controls.Button.SegoeButton;
using TWPFX.Controls.Icon.SegoeIcon;

namespace TWPFX.Controls.TextBlock.CodeBlock
{
    /// <summary>
    /// TCodeBlock.xaml 的交互逻辑
    /// </summary>
    public partial class TCodeBlock : UserControl
    {
        public static readonly DependencyProperty CodeProperty = DependencyProperty.Register(
            nameof(Code), typeof(string), typeof(TCodeBlock), new PropertyMetadata(string.Empty, OnCodeChanged));

        public static readonly DependencyProperty LangProperty = DependencyProperty.Register(
            nameof(Lang), typeof(string), typeof(TCodeBlock), new PropertyMetadata("C#", OnCodeChanged));

        public static readonly DependencyProperty MaxCodeHeightProperty = DependencyProperty.Register(
            nameof(MaxCodeHeight), typeof(double), typeof(TCodeBlock), new PropertyMetadata(300.0));

        public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register(
            nameof(BorderThickness), typeof(double), typeof(TCodeBlock), new PropertyMetadata(1.0));

        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register(
            nameof(Padding), typeof(double), typeof(TCodeBlock), new PropertyMetadata(8.0));

        public string Code { get => (string)GetValue(CodeProperty); set => SetValue(CodeProperty, value); }
        public string Lang { get => (string)GetValue(LangProperty); set => SetValue(LangProperty, value); }
        public double MaxCodeHeight { get => (double)GetValue(MaxCodeHeightProperty); set => SetValue(MaxCodeHeightProperty, value); }
        public double BorderThickness { get => (double)GetValue(BorderThicknessProperty); set => SetValue(BorderThicknessProperty, value); }
        public double Padding { get => (double)GetValue(PaddingProperty); set => SetValue(PaddingProperty, value); }

        public TCodeBlock()
        {
            InitializeComponent();
            Loaded += (s, e) => UpdateHighlight();
        }

        private static void OnCodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TCodeBlock cb)
                cb.UpdateHighlight();
        }

        private void UpdateHighlight()
        {
            codeTextBlock.Inlines.Clear();
            if (string.IsNullOrEmpty(Code)) return;

            var lines = Code.Replace("\r\n", "\n").Split('\n');
            foreach (var line in lines)
            {
                foreach (var run in HighlightLine(line, Lang))
                    codeTextBlock.Inlines.Add(run);
                codeTextBlock.Inlines.Add(new LineBreak());
            }
        }

        private void CopyButton_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Code) && sender != null)
                {
                    TSegoeButton button = (TSegoeButton)sender;
                    button.CreateAnimationSequence()
                    .AddCustomStep(propertyPath: "GlyphSize", fromValue: 14.0, toValue: 1.0, durationMs: 300) // 第一步：缩小图标 (20 -> 1)
                    .AddCustomStep(propertyPath: "GlyphSize", fromValue: null, toValue: 14.0, durationMs: 300, beforeAction: () => {
                        button.Glyph = TSegoeIconType.CheckMark; 
                    }, afterAction: () => {
                        Task.Run(() =>
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                try
                                {
                                    Clipboard.SetText(Code);
                                }
                                catch (Exception ex) { }
                            });
                        });
                    }, delayAfterMs: 1500) // 第二步：放大图标并切换为勾选图标 (当前值 -> 16)
                    .AddCustomStep(propertyPath: "GlyphSize", fromValue: null, toValue: 1.0, durationMs: 300) // 第三步：延迟1.5秒后缩小图标 (当前值 -> 1)
                    .AddCustomStep(propertyPath: "GlyphSize", fromValue: null, toValue: 14.0, durationMs: 300, beforeAction: () => { button.Glyph = TSegoeIconType.Copy; }) // 第四步：恢复图标大小并切换为复制图标 (当前值 -> 16)
                    .Run();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to copy to clipboard: {ex.Message}");
            }
        }

        private static IEnumerable<Run> HighlightLine(string line, string lang)
        {
            // ===== 1. 颜色定义 =====
            var keywordBrush = Brushes.Blue;
            var commentBrush = Brushes.Green;
            var stringBrush = Brushes.Sienna;
            var numberBrush = Brushes.Purple;
            var typeBrush = Brushes.DarkCyan;       // 类/接口/结构体
            var methodBrush = Brushes.DarkViolet;   // 方法调用
            var propertyBrush = Brushes.DarkOrange; // 属性访问
            var parameterBrush = Brushes.Teal;      // 方法参数
            var annotationBrush = Brushes.Gray;     // 特性注解
            var defaultBrush = Brushes.Black;

            // ===== 2. 关键字集合 =====
            var csharpKeywords = new HashSet<string>
            {
                // 访问修饰符
                "public", "private", "protected", "internal", "sealed", "abstract", "static",
                // 类型定义
                "class", "interface", "struct", "enum", "namespace", "using", 
                // 基础类型
                "void", "int", "string", "bool", "var", "dynamic", "object", "decimal", "float", "double",
                // 流程控制
                "if", "else", "switch", "case", "default", "for", "foreach", "while", "do", "break", "continue",
                // 异常处理
                "try", "catch", "finally", "throw", "when",
                // 其他
                "new", "return", "await", "async", "lock", "this", "base", "partial", "readonly", "const"
            };

            // ===== 3. 正则表达式定义 =====
            var regexDict = new Dictionary<string, Regex>
            {
                // 注释（最高优先级）
                { "comment", new Regex(@"//.*|/\*.*?\*/|'''.*?'''", RegexOptions.Singleline) },
                // 字符串
                { "string", new Regex(@"""(?:\\.|[^""])*""|'(?:\\.|[^'])*'") },
                // 数字
                { "number", new Regex(@"\b\d+(\.\d+)?([eE][+-]?\d+)?\b") },
                // 类型声明
                { "typeDecl", new Regex(@"(class|interface|struct|enum|record)\s+([A-Za-z_][\w_]*)") },
                // 方法声明
                { "methodDecl", new Regex(@"\b([A-Za-z_][\w<>]*)\s+([A-Za-z_][\w_]*)\s*\(([^)]*)\)") },
                // 方法调用
                { "methodCall", new Regex(@"\b([A-Za-z_][\w_]*)\(([^)]*)\)") },
                // 实例化
                { "newInstance", new Regex(@"new\s+([A-Z][\w<>]*(?:\.[A-Z][\w<>]*)*)\s*\(") },
                // 属性访问
                { "propertyAccess", new Regex(@"([A-Za-z_][\w_]*)\.([A-Z][\w_]*)") },
                // 特性注解
                { "attribute", new Regex(@"\[([A-Z][\w_]*)(?:\([^)]*\))?\]") },
                // 泛型
                { "generic", new Regex(@"([A-Za-z_][\w_]*)\<([^>]+)\>") }
            };

            // ===== 4. 主处理逻辑 =====
            int pos = 0;
            while (pos < line.Length)
            {
                // 优先检查注释和字符串（它们不应该被其他规则匹配）
                var commentMatch = regexDict["comment"].Match(line, pos);
                if (commentMatch.Success && commentMatch.Index == pos)
                {
                    yield return new Run(commentMatch.Value) { Foreground = commentBrush };
                    pos += commentMatch.Length;
                    continue;
                }

                var stringMatch = regexDict["string"].Match(line, pos);
                if (stringMatch.Success && stringMatch.Index == pos)
                {
                    yield return new Run(stringMatch.Value) { Foreground = stringBrush };
                    pos += stringMatch.Length;
                    continue;
                }

                // 其他语法元素
                bool matched = false;

                // 类型声明
                var typeDeclMatch = regexDict["typeDecl"].Match(line, pos);
                if (typeDeclMatch.Success && typeDeclMatch.Index == pos)
                {
                    yield return new Run(typeDeclMatch.Groups[1].Value) { Foreground = keywordBrush };
                    yield return new Run(" ") { Foreground = defaultBrush };
                    yield return new Run(typeDeclMatch.Groups[2].Value) { Foreground = typeBrush };
                    pos += typeDeclMatch.Length;
                    matched = true;
                }

                // 方法声明/调用
                if (!matched)
                {
                    var methodMatch = regexDict["methodDecl"].Match(line, pos);
                    if (methodMatch.Success && methodMatch.Index == pos)
                    {
                        // 返回类型
                        if (!string.IsNullOrEmpty(methodMatch.Groups[1].Value))
                        {
                            yield return new Run(methodMatch.Groups[1].Value) { Foreground = typeBrush };
                            yield return new Run(" ") { Foreground = defaultBrush };
                        }

                        // 方法名
                        yield return new Run(methodMatch.Groups[2].Value) { Foreground = methodBrush };

                        // 参数
                        yield return new Run("(") { Foreground = defaultBrush };
                        yield return new Run(methodMatch.Groups[3].Value) { Foreground = parameterBrush };
                        yield return new Run(")") { Foreground = defaultBrush };

                        pos += methodMatch.Length;
                        matched = true;
                    }
                }

                // 实例化
                if (!matched)
                {
                    var newMatch = regexDict["newInstance"].Match(line, pos);
                    if (newMatch.Success && newMatch.Index == pos)
                    {
                        yield return new Run("new ") { Foreground = keywordBrush };
                        yield return new Run(newMatch.Groups[1].Value) { Foreground = typeBrush };
                        yield return new Run("(") { Foreground = defaultBrush };
                        pos += newMatch.Length;
                        matched = true;
                    }
                }

                // 属性访问
                if (!matched)
                {
                    var propMatch = regexDict["propertyAccess"].Match(line, pos);
                    if (propMatch.Success && propMatch.Index == pos)
                    {
                        yield return new Run(propMatch.Groups[1].Value) { Foreground = defaultBrush };
                        yield return new Run(".") { Foreground = defaultBrush };
                        yield return new Run(propMatch.Groups[2].Value) { Foreground = propertyBrush };
                        pos += propMatch.Length;
                        matched = true;
                    }
                }

                // 数字
                if (!matched)
                {
                    var numMatch = regexDict["number"].Match(line, pos);
                    if (numMatch.Success && numMatch.Index == pos)
                    {
                        yield return new Run(numMatch.Value) { Foreground = numberBrush };
                        pos += numMatch.Length;
                        matched = true;
                    }
                }

                // 关键字
                if (!matched)
                {
                    var word = GetNextWord(line, pos, out int wordLen);
                    if (wordLen > 0)
                    {
                        if (csharpKeywords.Contains(word))
                        {
                            yield return new Run(word) { Foreground = keywordBrush, FontWeight = FontWeights.Bold };
                        }
                        else if (word.StartsWith("@")) // 处理C#的verbatim标识符
                        {
                            yield return new Run(word) { Foreground = defaultBrush };
                        }
                        else if (char.IsUpper(word[0]) && pos > 0 && line[pos - 1] != '.')
                        {
                            yield return new Run(word) { Foreground = typeBrush }; // 假设大写开头的是类型
                        }
                        else
                        {
                            yield return new Run(word) { Foreground = defaultBrush };
                        }
                        pos += wordLen;
                        matched = true;
                    }
                }

                // 默认处理
                if (!matched)
                {
                    yield return new Run(line[pos].ToString()) { Foreground = defaultBrush };
                    pos++;
                }
            }
        }

        private static string GetNextWord(string line, int start, out int length)
        {
            length = 0;
            if (start >= line.Length) return string.Empty;

            // 处理C#的@标识符
            if (line[start] == '@' && start + 1 < line.Length)
            {
                int i = start + 1;
                while (i < line.Length && (char.IsLetterOrDigit(line[i]) || line[i] == '_'))
                    i++;
                length = i - start;
                return line.Substring(start, length);
            }

            // 普通单词
            if (!char.IsLetter(line[start])) return string.Empty;

            int j = start;
            while (j < line.Length && (char.IsLetterOrDigit(line[j]) || line[j] == '_' || line[j] == '<' || line[j] == '>'))
            {
                // 处理泛型
                if (line[j] == '<')
                {
                    int depth = 1;
                    while (j + 1 < line.Length && depth > 0)
                    {
                        j++;
                        if (line[j] == '<') depth++;
                        if (line[j] == '>') depth--;
                    }
                }
                j++;
            }
            length = j - start;
            return line.Substring(start, length);
        }
    }
}
