using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using TWPFX.Controls.Overlay;

namespace TWPFX.Controls.Button.TButton
{
    /// <summary>
    /// 按钮基类，提供基础的样式属性和模板支持
    /// 左侧区域可以放置任意控件，右侧是文本
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

        public static readonly DependencyProperty BorderPaddingProperty =
            DependencyProperty.Register(nameof(BorderPadding), typeof(Thickness), typeof(TBaseButton),
                new PropertyMetadata(new Thickness(6.0), OnLayoutPropertyChanged));

        public static readonly DependencyProperty ContentSpacingProperty =
            DependencyProperty.Register(nameof(ContentSpacing), typeof(double), typeof(TBaseButton),
                new PropertyMetadata(6.0, OnLayoutPropertyChanged));

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

        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register(nameof(TextAlignment), typeof(HorizontalAlignment), typeof(TBaseButton),
                new PropertyMetadata(HorizontalAlignment.Left, OnContentPropertyChanged));

        public static readonly DependencyProperty ContentOrderProperty =
            DependencyProperty.Register(nameof(ContentOrder), typeof(ContentOrder), typeof(TBaseButton),
                new PropertyMetadata(ContentOrder.ContentFirst, OnContentPropertyChanged));

        // 宽度自适应属性
        public static readonly DependencyProperty WidthAutoAdaptationProperty =
            DependencyProperty.Register(nameof(WidthAutoAdaptation), typeof(bool), typeof(TBaseButton),
                new PropertyMetadata(false, OnWidthAutoAdaptationChanged));

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

        public Thickness BorderPadding
        {
            get => (Thickness)GetValue(BorderPaddingProperty);
            set => SetValue(BorderPaddingProperty, value);
        }

        public double ContentSpacing
        {
            get => (double)GetValue(ContentSpacingProperty);
            set => SetValue(ContentSpacingProperty, value);
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

        public HorizontalAlignment TextAlignment
        {
            get => (HorizontalAlignment)GetValue(TextAlignmentProperty);
            set => SetValue(TextAlignmentProperty, value);
        }

        public ContentOrder ContentOrder
        {
            get => (ContentOrder)GetValue(ContentOrderProperty);
            set => SetValue(ContentOrderProperty, value);
        }

        public bool WidthAutoAdaptation
        {
            get => (bool)GetValue(WidthAutoAdaptationProperty);
            set => SetValue(WidthAutoAdaptationProperty, value);
        }

        #endregion

        #region 私有字段

        private System.Windows.Controls.TextBlock? _textElement;
        private ContentPresenter? _leftContent;

        #endregion

        public TBaseButton()
        {
            // 设置渲染优化
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.HighQuality);
            RenderOptions.SetEdgeMode(this, EdgeMode.Unspecified);
            UseLayoutRounding = true;
            SnapsToDevicePixels = true;
            Width = 71;
            Height = 31;
            // 启用禁用时的鼠标手势
            TDisabledBehavior.SetEnableDisabledCursor(this, true);
            
            InitializeBaseButton();
        }

        protected virtual void InitializeBaseButton()
        {
            // 应用基础样式
            ControlTemplate? template = Application.Current.Resources["TBaseButtonTemplate"] as ControlTemplate;
            Template = template;
            ApplyBaseStyle();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            _textElement = GetTemplateChild("textElement") as System.Windows.Controls.TextBlock;
            _leftContent = GetTemplateChild("leftContent") as ContentPresenter;
            
            if (_textElement != null)
            {
                // 应用渲染优化
                RenderOptions.SetBitmapScalingMode(_textElement, BitmapScalingMode.HighQuality);
                RenderOptions.SetEdgeMode(_textElement, EdgeMode.Unspecified);
                RenderOptions.SetClearTypeHint(_textElement, ClearTypeHint.Enabled);
                _textElement.UseLayoutRounding = true;
                TextOptions.SetTextFormattingMode(_textElement, TextFormattingMode.Display);
                TextOptions.SetTextRenderingMode(_textElement, TextRenderingMode.ClearType);
                
                // 应用TextAlignment
                _textElement.HorizontalAlignment = TextAlignment;
            }
            
            // 如果启用了宽度自适应，应用自适应宽度
            if (WidthAutoAdaptation)
            {
                ApplyWidthAutoAdaptation();
            }

            ApplyContentSpacing();

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
                if (button.Template != null &&
                    button.Template != button.FindResource("TBaseButtonTemplate") as ControlTemplate)
                    return;
                    
                if (e.Property == ButtonWidthProperty && (double.IsNaN(button.Width) || button.Width == 0))
                {
                    button.Width = button.ButtonWidth;
                }
                else if (e.Property == ButtonHeightProperty && (double.IsNaN(button.Height) || button.Height == 0))
                {
                    button.Height = button.ButtonHeight;
                }
                else if (e.Property == ContentSpacingProperty || e.Property == ContentOrderProperty)
                {
                    button.ApplyContentSpacing();
                }
            }
        }

        private static void OnContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TBaseButton button)
            {
                button.UpdateContent();
                
                // 如果启用了宽度自适应，重新计算宽度
                if (button.WidthAutoAdaptation)
                {
                    button.ApplyWidthAutoAdaptation();
                }
                button.ApplyContentSpacing();
            }
        }

        private static void OnWidthAutoAdaptationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TBaseButton button)
            {
                button.ApplyWidthAutoAdaptation();
                button.ApplyContentSpacing();
            }
        }

        #endregion

        protected virtual void ApplyBaseStyle()
        {
            // 如果使用了自定义模板，不应用基础样式
            if (Template != null && Template != this.FindResource("TBaseButtonTemplate") as ControlTemplate)
                return;

            if (_textElement == null) return;

            // 应用基础样式
            Background = BackgroundColor;
            Foreground = ForegroundColor;
            BorderBrush = BorderColor;

            // 应用文本样式（不设置Foreground，让模板触发器处理）
            _textElement.FontSize = FontSize;
            _textElement.FontWeight = FontWeight;
            _textElement.HorizontalAlignment = TextAlignment;
        }

        protected virtual void ApplyLayout()
        {
            if (Width == 0 && !WidthAutoAdaptation)
            {
                Width = ButtonWidth;
            }
            if (double.IsNaN(Height) || Height == 0)
            {
                Height = ButtonHeight;
            }
        }

        protected virtual void UpdateContent()
        {
            if (_textElement != null)
            {
                _textElement.Text = Text;
                _textElement.Visibility = !string.IsNullOrEmpty(Text) ? Visibility.Visible : Visibility.Collapsed;
                TextOptions.SetTextFormattingMode(this, TextFormattingMode.Display);
                TextOptions.SetTextRenderingMode(this, TextRenderingMode.ClearType);
                RenderOptions.SetEdgeMode(this, EdgeMode.Unspecified);
            }
        }

        protected virtual void ApplyContentSpacing()
        {
            if (_textElement != null)
            {
                // 根据ContentOrder设置不同的Margin
                if (ContentOrder == ContentOrder.TextFirst)
                {
                    _textElement.Margin = new Thickness(0, 0, ContentSpacing, 0);
                    if (_leftContent != null)
                    {
                        _leftContent.Margin = new Thickness(ContentSpacing, 0, 0, 0);
                    }
                }
                else
                {
                    _textElement.Margin = new Thickness(0);
                    if (_leftContent != null)
                    {
                        _leftContent.Margin = new Thickness(0, 0, ContentSpacing, 0);
                    }
                }
            }
        }

        protected virtual void ApplyWidthAutoAdaptation()
        {
            if (!WidthAutoAdaptation) return;

            // 延迟执行，确保布局已完成
            Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    // 临时设置Width为Auto以测量内容
                    double originalWidth = Width;
                    Width = double.NaN; // Auto
                    
                    // 强制布局更新以获取实际所需宽度
                    Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    Size desiredSize = DesiredSize;
                    
                    // 设置宽度为内容所需宽度加上一些边距
                    double contentWidth = desiredSize.Width;
                    double padding = BorderPadding.Left + BorderPadding.Right;
                    Width = contentWidth + padding;
                }
                catch (Exception)
                {
                    // 如果出现异常，恢复原始宽度
                    Width = ButtonWidth;
                }
            }), System.Windows.Threading.DispatcherPriority.Loaded);
        }
    }
}