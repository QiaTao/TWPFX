using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TWPFX.Controls.Icon.SegoeIcon;

namespace TWPFX.Controls.Button.TButton
{
    /// <summary>
    /// 按钮基类，提供所有可配置的样式属性
    /// </summary>
    public class TBaseButton : System.Windows.Controls.Button
    {
        #region 依赖属性

        // 基础样式属性
        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register(nameof(BackgroundColor), typeof(Brush), typeof(TBaseButton),
                new PropertyMetadata(Brushes.White, OnStylePropertyChanged));

        public static readonly DependencyProperty ForegroundColorProperty =
            DependencyProperty.Register(nameof(ForegroundColor), typeof(Brush), typeof(TBaseButton),
                new PropertyMetadata(Brushes.Black, OnStylePropertyChanged));

        public static readonly DependencyProperty BorderColorProperty =
            DependencyProperty.Register(nameof(BorderColor), typeof(Brush), typeof(TBaseButton),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(240, 240, 240)), OnStylePropertyChanged));

        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register(nameof(BorderThickness), typeof(double), typeof(TBaseButton),
                new PropertyMetadata(1.0, OnStylePropertyChanged));

        // 悬停状态属性
        public static readonly DependencyProperty HoverBackgroundColorProperty =
            DependencyProperty.Register(nameof(HoverBackgroundColor), typeof(Brush), typeof(TBaseButton),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(51, 51, 51)), OnStylePropertyChanged));

        public static readonly DependencyProperty HoverForegroundColorProperty =
            DependencyProperty.Register(nameof(HoverForegroundColor), typeof(Brush), typeof(TBaseButton),
                new PropertyMetadata(Brushes.White, OnStylePropertyChanged));

        public static readonly DependencyProperty HoverBorderColorProperty =
            DependencyProperty.Register(nameof(HoverBorderColor), typeof(Brush), typeof(TBaseButton),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(51, 51, 51)), OnStylePropertyChanged));

        // 按下状态属性
        public static readonly DependencyProperty PressedBackgroundColorProperty =
            DependencyProperty.Register(nameof(PressedBackgroundColor), typeof(Brush), typeof(TBaseButton),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(85, 85, 85)), OnStylePropertyChanged));

        public static readonly DependencyProperty PressedForegroundColorProperty =
            DependencyProperty.Register(nameof(PressedForegroundColor), typeof(Brush), typeof(TBaseButton),
                new PropertyMetadata(Brushes.White, OnStylePropertyChanged));

        public static readonly DependencyProperty PressedBorderColorProperty =
            DependencyProperty.Register(nameof(PressedBorderColor), typeof(Brush), typeof(TBaseButton),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(85, 85, 85)), OnStylePropertyChanged));

        // 尺寸和布局属性
        public static readonly DependencyProperty ButtonWidthProperty =
            DependencyProperty.Register(nameof(ButtonWidth), typeof(double), typeof(TBaseButton),
                new PropertyMetadata(71.0, OnLayoutPropertyChanged));

        public static readonly DependencyProperty ButtonHeightProperty =
            DependencyProperty.Register(nameof(ButtonHeight), typeof(double), typeof(TBaseButton),
                new PropertyMetadata(31.0, OnLayoutPropertyChanged));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(double), typeof(TBaseButton),
                new PropertyMetadata(6.0, OnLayoutPropertyChanged));

        public static readonly DependencyProperty PaddingProperty =
            DependencyProperty.Register(nameof(Padding), typeof(double), typeof(TBaseButton),
                new PropertyMetadata(6.0, OnLayoutPropertyChanged));

        // 图标属性
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(TSegoeIconType), typeof(TBaseButton),
                new PropertyMetadata(TSegoeIconType.None, OnContentPropertyChanged));

        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register(nameof(IconSize), typeof(double), typeof(TBaseButton),
                new PropertyMetadata(16.0, OnContentPropertyChanged));

        public static readonly DependencyProperty IconColorProperty =
            DependencyProperty.Register(nameof(IconColor), typeof(Brush), typeof(TBaseButton),
                new PropertyMetadata(Brushes.Black, OnStylePropertyChanged));

        public static readonly DependencyProperty HoverIconColorProperty =
            DependencyProperty.Register(nameof(HoverIconColor), typeof(Brush), typeof(TBaseButton),
                new PropertyMetadata(Brushes.White, OnStylePropertyChanged));

        public static readonly DependencyProperty PressedIconColorProperty =
            DependencyProperty.Register(nameof(PressedIconColor), typeof(Brush), typeof(TBaseButton),
                new PropertyMetadata(Brushes.White, OnStylePropertyChanged));

        // 文本属性
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(TBaseButton),
                new PropertyMetadata(string.Empty, OnContentPropertyChanged));

        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register(nameof(FontSize), typeof(double), typeof(TBaseButton),
                new PropertyMetadata(12.0, OnContentPropertyChanged));

        public static readonly DependencyProperty FontWeightProperty =
            DependencyProperty.Register(nameof(FontWeight), typeof(FontWeight), typeof(TBaseButton),
                new PropertyMetadata(FontWeights.Bold, OnContentPropertyChanged));

        public static readonly DependencyProperty TextColorProperty =
            DependencyProperty.Register(nameof(TextColor), typeof(Brush), typeof(TBaseButton),
                new PropertyMetadata(Brushes.Black, OnStylePropertyChanged));

        public static readonly DependencyProperty HoverTextColorProperty =
            DependencyProperty.Register(nameof(HoverTextColor), typeof(Brush), typeof(TBaseButton),
                new PropertyMetadata(Brushes.White, OnStylePropertyChanged));

        public static readonly DependencyProperty PressedTextColorProperty =
            DependencyProperty.Register(nameof(PressedTextColor), typeof(Brush), typeof(TBaseButton),
                new PropertyMetadata(Brushes.White, OnStylePropertyChanged));

        #endregion

        #region CLR属性包装器

        // 基础样式属性
        public Brush BackgroundColor
        {
            get => (Brush)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }

        public Brush ForegroundColor
        {
            get => (Brush)GetValue(ForegroundColorProperty);
            set => SetValue(ForegroundColorProperty, value);
        }

        public Brush BorderColor
        {
            get => (Brush)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public double BorderThickness
        {
            get => (double)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }

        // 悬停状态属性
        public Brush HoverBackgroundColor
        {
            get => (Brush)GetValue(HoverBackgroundColorProperty);
            set => SetValue(HoverBackgroundColorProperty, value);
        }

        public Brush HoverForegroundColor
        {
            get => (Brush)GetValue(HoverForegroundColorProperty);
            set => SetValue(HoverForegroundColorProperty, value);
        }

        public Brush HoverBorderColor
        {
            get => (Brush)GetValue(HoverBorderColorProperty);
            set => SetValue(HoverBorderColorProperty, value);
        }

        // 按下状态属性
        public Brush PressedBackgroundColor
        {
            get => (Brush)GetValue(PressedBackgroundColorProperty);
            set => SetValue(PressedBackgroundColorProperty, value);
        }

        public Brush PressedForegroundColor
        {
            get => (Brush)GetValue(PressedForegroundColorProperty);
            set => SetValue(PressedForegroundColorProperty, value);
        }

        public Brush PressedBorderColor
        {
            get => (Brush)GetValue(PressedBorderColorProperty);
            set => SetValue(PressedBorderColorProperty, value);
        }

        // 尺寸和布局属性
        public double ButtonWidth
        {
            get => (double)GetValue(ButtonWidthProperty);
            set => SetValue(ButtonWidthProperty, value);
        }

        public double ButtonHeight
        {
            get => (double)GetValue(ButtonHeightProperty);
            set => SetValue(ButtonHeightProperty, value);
        }

        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public double Padding
        {
            get => (double)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }

        // 图标属性
        public TSegoeIconType Icon
        {
            get => (TSegoeIconType)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }

        public Brush IconColor
        {
            get => (Brush)GetValue(IconColorProperty);
            set => SetValue(IconColorProperty, value);
        }

        public Brush HoverIconColor
        {
            get => (Brush)GetValue(HoverIconColorProperty);
            set => SetValue(HoverIconColorProperty, value);
        }

        public Brush PressedIconColor
        {
            get => (Brush)GetValue(PressedIconColorProperty);
            set => SetValue(PressedIconColorProperty, value);
        }

        // 文本属性
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public FontWeight FontWeight
        {
            get => (FontWeight)GetValue(FontWeightProperty);
            set => SetValue(FontWeightProperty, value);
        }

        public Brush TextColor
        {
            get => (Brush)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public Brush HoverTextColor
        {
            get => (Brush)GetValue(HoverTextColorProperty);
            set => SetValue(HoverTextColorProperty, value);
        }

        public Brush PressedTextColor
        {
            get => (Brush)GetValue(PressedTextColorProperty);
            set => SetValue(PressedTextColorProperty, value);
        }

        #endregion

        #region 私有字段

        protected TSegoeIcon _iconElement;
        protected System.Windows.Controls.TextBlock _textElement;
        protected StackPanel _contentPanel;

        #endregion

        public TBaseButton()
        {
            InitializeBaseButton();
        }

        protected virtual void InitializeBaseButton()
        {
            // 应用基础样式
            ControlTemplate? template = Application.Current.Resources["TBaseButtonTemplate"] as ControlTemplate;
            Template = template;
            ApplyBaseStyle();
            ApplyLayout();  
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
        }

        #region 属性变更回调

        private static void OnStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TBaseButton button)
            {
                // 如果使用了自定义模板，不应用基础样式
                if (button.Template != null && 
                    button.Template != button.FindResource("TBaseButtonTemplate") as ControlTemplate)
                    return;
                    
                button.ApplyBaseStyle();
            }
        }

        private static void OnLayoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TBaseButton button)
            {
                // 如果使用了自定义模板，不应用布局
                if (button.Template != null && 
                    button.Template != button.FindResource("TBaseButtonTemplate") as ControlTemplate)
                    return;
                    
                button.ApplyLayout();
            }
        }

        private static void OnContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TBaseButton button)
            {
                button.UpdateContent();
            }
        }

        #endregion

        protected virtual void ApplyBaseStyle()
        {
            // 如果使用了自定义模板，不应用基础样式
            if (Template != null && Template != this.FindResource("TBaseButtonTemplate") as ControlTemplate)
                return;

            if (_iconElement == null || _textElement == null) return;

            // 应用基础样式
            Background = BackgroundColor;
            Foreground = ForegroundColor;
            BorderBrush = BorderColor;
            // 注意：这里不需要设置BorderThickness，因为模板会处理

            // 应用图标和文本颜色
            _iconElement.Foreground = IconColor;
            _textElement.Foreground = TextColor;
            _textElement.FontSize = FontSize;
            _textElement.FontWeight = FontWeight;
        }

        protected virtual void ApplyLayout()
        {
            Width = ButtonWidth;
            Height = ButtonHeight;
        }

        protected virtual void UpdateContent()
        {
            if (_iconElement != null)
            {
                _iconElement.Glyph = Icon;
                _iconElement.GlyphSize = IconSize;
                _iconElement.Visibility = Icon != TSegoeIconType.None ? Visibility.Visible : Visibility.Collapsed;
            }

            if (_textElement != null)
            {
                _textElement.Text = Text;
                _textElement.Visibility = !string.IsNullOrEmpty(Text) ? Visibility.Visible : Visibility.Collapsed;
                TextOptions.SetTextFormattingMode(this, TextFormattingMode.Display);
                TextOptions.SetTextRenderingMode(this, TextRenderingMode.ClearType);
                RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);
            }

            // 更新间距
            if (_textElement != null && _iconElement != null)
            {
                _textElement.Margin = Icon != TSegoeIconType.None && !string.IsNullOrEmpty(Text)
                    ? new Thickness(6, 0, 0, 0)
                    : new Thickness(0);
            }
        }
    }
}