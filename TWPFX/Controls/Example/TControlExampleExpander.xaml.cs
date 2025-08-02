using System.Windows;
using System.Windows.Controls;

namespace TWPFX.Controls.Example
{
    /// <summary>
    /// TControlExampleExpander.xaml 的交互逻辑
    /// </summary>
    public partial class TControlExampleExpander : UserControl
    {
        public TControlExampleExpander()
        {
            InitializeComponent();
        }

        #region 依赖属性

        /// <summary>
        /// 标题依赖属性
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(
                nameof(Header),
                typeof(string),
                typeof(TControlExampleExpander),
                new PropertyMetadata(string.Empty));

        /// <summary>
        /// 标题
        /// </summary>
        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        /// <summary>
        /// 内容依赖属性
        /// </summary>
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(
                nameof(Content),
                typeof(object),
                typeof(TControlExampleExpander),
                new PropertyMetadata(null));

        /// <summary>
        /// 内容
        /// </summary>
        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        /// <summary>
        /// 代码依赖属性
        /// </summary>
        public static readonly DependencyProperty CodeProperty =
            DependencyProperty.Register(
                nameof(Code),
                typeof(string),
                typeof(TControlExampleExpander),
                new PropertyMetadata(string.Empty));

        /// <summary>
        /// 代码
        /// </summary>
        public string Code
        {
            get => (string)GetValue(CodeProperty);
            set => SetValue(CodeProperty, value);
        }

        /// <summary>
        /// 代码语言依赖属性
        /// </summary>
        public static readonly DependencyProperty LangProperty =
            DependencyProperty.Register(
                nameof(Lang),
                typeof(string),
                typeof(TControlExampleExpander),
                new PropertyMetadata("C#"));

        /// <summary>
        /// 代码
        /// </summary>
        public string Lang
        {
            get => (string)GetValue(LangProperty);
            set => SetValue(LangProperty, value);
        }

        #endregion

        #region 事件处理

        private void DisableCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // 禁用示例控件
            if (Content is FrameworkElement element)
            {
                element.IsEnabled = false;
            }
        }

        private void DisableCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // 启用示例控件
            if (Content is FrameworkElement element)
            {
                element.IsEnabled = true;
            }
        }

        #endregion
    }
} 