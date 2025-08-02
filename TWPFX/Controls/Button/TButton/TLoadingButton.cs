using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TWPFX.Controls.Progress;
using TWPFX.Service;

namespace TWPFX.Controls.Button.TButton
{
    /// <summary>
    /// 加载按钮控件，使用 TProgressRing 替代图标
    /// </summary>
    public class TLoadingButton : System.Windows.Controls.Button
    {
        #region 依赖属性

        // 基础样式属性
        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register(nameof(BackgroundColor), typeof(Brush), typeof(TLoadingButton),
                new PropertyMetadata(Brushes.White, OnStylePropertyChanged));

        public static readonly DependencyProperty ForegroundColorProperty =
            DependencyProperty.Register(nameof(ForegroundColor), typeof(Brush), typeof(TLoadingButton),
                new PropertyMetadata(Brushes.Black, OnStylePropertyChanged));

        public static readonly DependencyProperty BorderColorProperty =
            DependencyProperty.Register(nameof(BorderColor), typeof(Brush), typeof(TLoadingButton),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(240, 240, 240)), OnStylePropertyChanged));

        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register(nameof(BorderThickness), typeof(double), typeof(TLoadingButton),
                new PropertyMetadata(1.0, OnStylePropertyChanged));

        // 悬停状态属性
        public static readonly DependencyProperty HoverBackgroundColorProperty =
            DependencyProperty.Register(nameof(HoverBackgroundColor), typeof(Brush), typeof(TLoadingButton),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(51, 51, 51)), OnStylePropertyChanged));

        public static readonly DependencyProperty HoverForegroundColorProperty =
            DependencyProperty.Register(nameof(HoverForegroundColor), typeof(Brush), typeof(TLoadingButton),
                new PropertyMetadata(Brushes.White, OnStylePropertyChanged));

        public static readonly DependencyProperty HoverBorderColorProperty =
            DependencyProperty.Register(nameof(HoverBorderColor), typeof(Brush), typeof(TLoadingButton),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(51, 51, 51)), OnStylePropertyChanged));

        // 按下状态属性
        public static readonly DependencyProperty PressedBackgroundColorProperty =
            DependencyProperty.Register(nameof(PressedBackgroundColor), typeof(Brush), typeof(TLoadingButton),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(85, 85, 85)), OnStylePropertyChanged));

        public static readonly DependencyProperty PressedForegroundColorProperty =
            DependencyProperty.Register(nameof(PressedForegroundColor), typeof(Brush), typeof(TLoadingButton),
                new PropertyMetadata(Brushes.White, OnStylePropertyChanged));

        public static readonly DependencyProperty PressedBorderColorProperty =
            DependencyProperty.Register(nameof(PressedBorderColor), typeof(Brush), typeof(TLoadingButton),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(85, 85, 85)), OnStylePropertyChanged));

        // 尺寸和布局属性
        public static readonly DependencyProperty ButtonWidthProperty =
            DependencyProperty.Register(nameof(ButtonWidth), typeof(double), typeof(TLoadingButton),
                new PropertyMetadata(71.0, OnLayoutPropertyChanged));

        public static readonly DependencyProperty ButtonHeightProperty =
            DependencyProperty.Register(nameof(ButtonHeight), typeof(double), typeof(TLoadingButton),
                new PropertyMetadata(31.0, OnLayoutPropertyChanged));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(double), typeof(TLoadingButton),
                new PropertyMetadata(6.0, OnLayoutPropertyChanged));

        public static readonly DependencyProperty PaddingProperty =
            DependencyProperty.Register(nameof(Padding), typeof(double), typeof(TLoadingButton),
                new PropertyMetadata(6.0, OnLayoutPropertyChanged));

        // 外观属性
        public static readonly DependencyProperty AppearanceProperty =
            DependencyProperty.Register(nameof(Appearance), typeof(ButtonAppearance), typeof(TLoadingButton),
                new PropertyMetadata(ButtonAppearance.Default, OnAppearancePropertyChanged));

        // 加载状态属性
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(TLoadingButton),
                new PropertyMetadata(false, OnLoadingPropertyChanged));

        public static readonly DependencyProperty LoadingRingSizeProperty =
            DependencyProperty.Register(nameof(LoadingRingSize), typeof(double), typeof(TLoadingButton),
                new PropertyMetadata(16.0, OnContentPropertyChanged));

        public static readonly DependencyProperty LoadingRingStrokeThicknessProperty =
            DependencyProperty.Register(nameof(LoadingRingStrokeThickness), typeof(double), typeof(TLoadingButton),
                new PropertyMetadata(2.0, OnContentPropertyChanged));

        public static readonly DependencyProperty LoadingRingVisibilityProperty =
            DependencyProperty.Register(nameof(LoadingRingVisibility), typeof(Visibility), typeof(TLoadingButton),
                new PropertyMetadata(Visibility.Collapsed, OnContentPropertyChanged));

        public static readonly DependencyProperty LoadingRingColorProperty =
            DependencyProperty.Register(nameof(LoadingRingColor), typeof(Brush), typeof(TLoadingButton),
                new PropertyMetadata(Brushes.Black, OnStylePropertyChanged));

        public static readonly DependencyProperty HoverLoadingRingColorProperty =
            DependencyProperty.Register(nameof(HoverLoadingRingColor), typeof(Brush), typeof(TLoadingButton),
                new PropertyMetadata(Brushes.White, OnStylePropertyChanged));

        public static readonly DependencyProperty PressedLoadingRingColorProperty =
            DependencyProperty.Register(nameof(PressedLoadingRingColor), typeof(Brush), typeof(TLoadingButton),
                new PropertyMetadata(Brushes.White, OnStylePropertyChanged));

        // 文本属性
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(TLoadingButton),
                new PropertyMetadata(string.Empty, OnContentPropertyChanged));

        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register(nameof(FontSize), typeof(double), typeof(TLoadingButton),
                new PropertyMetadata(14.0, OnContentPropertyChanged));

        public static readonly DependencyProperty FontWeightProperty =
            DependencyProperty.Register(nameof(FontWeight), typeof(FontWeight), typeof(TLoadingButton),
                new PropertyMetadata(FontWeights.Bold, OnContentPropertyChanged));

        public static readonly DependencyProperty TextColorProperty =
            DependencyProperty.Register(nameof(TextColor), typeof(Brush), typeof(TLoadingButton),
                new PropertyMetadata(Brushes.Black, OnStylePropertyChanged));

        public static readonly DependencyProperty HoverTextColorProperty =
            DependencyProperty.Register(nameof(HoverTextColor), typeof(Brush), typeof(TLoadingButton),
                new PropertyMetadata(Brushes.White, OnStylePropertyChanged));

        public static readonly DependencyProperty PressedTextColorProperty =
            DependencyProperty.Register(nameof(PressedTextColor), typeof(Brush), typeof(TLoadingButton),
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

        // 外观属性
        public ButtonAppearance Appearance
        {
            get => (ButtonAppearance)GetValue(AppearanceProperty);
            set => SetValue(AppearanceProperty, value);
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

        // 加载状态属性
        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, value);
        }

        public double LoadingRingSize
        {
            get => (double)GetValue(LoadingRingSizeProperty);
            set => SetValue(LoadingRingSizeProperty, value);
        }

        public double LoadingRingStrokeThickness
        {
            get => (double)GetValue(LoadingRingStrokeThicknessProperty);
            set => SetValue(LoadingRingStrokeThicknessProperty, value);
        }

        public Visibility LoadingRingVisibility
        {
            get => (Visibility)GetValue(LoadingRingVisibilityProperty);
            set => SetValue(LoadingRingVisibilityProperty, value);
        }

        public Brush LoadingRingColor
        {
            get => (Brush)GetValue(LoadingRingColorProperty);
            set => SetValue(LoadingRingColorProperty, value);
        }

        public Brush HoverLoadingRingColor
        {
            get => (Brush)GetValue(HoverLoadingRingColorProperty);
            set => SetValue(HoverLoadingRingColorProperty, value);
        }

        public Brush PressedLoadingRingColor
        {
            get => (Brush)GetValue(PressedLoadingRingColorProperty);
            set => SetValue(PressedLoadingRingColorProperty, value);
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

        protected TProgressRing _loadingRing;
        protected System.Windows.Controls.TextBlock _textElement;
        protected StackPanel _contentPanel;

        #endregion

        public TLoadingButton()
        {
            InitializeLoadingButton();
        }

        protected virtual void InitializeLoadingButton()
        {
            // 应用基础样式
            ControlTemplate? template = Application.Current.Resources["TLoadingButtonTemplate"] as ControlTemplate;
            Template = template;
            ApplyAppearance();
            ApplyBaseStyle();
            ApplyLayout();
            
            // 初始化完成后更新内容
            UpdateContent();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            // 获取模板中的元素
            _loadingRing = GetTemplateChild("loadingRing") as TProgressRing;
            _textElement = GetTemplateChild("textElement") as System.Windows.Controls.TextBlock;
            _contentPanel = GetTemplateChild("contentPanel") as StackPanel;
            
            // 模板应用后更新内容
            UpdateContent();
        }

        #region 属性变更回调

        private static void OnStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TLoadingButton button)
            {
                // 如果使用了自定义模板，不应用基础样式
                if (button.Template != null && 
                    button.Template != button.FindResource("TLoadingButtonTemplate") as ControlTemplate)
                    return;
                    
                button.ApplyBaseStyle();
            }
        }

        private static void OnLayoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TLoadingButton button)
            {
                // 如果使用了自定义模板，不应用布局
                if (button.Template != null && 
                    button.Template != button.FindResource("TLoadingButtonTemplate") as ControlTemplate)
                    return;
                    
                button.ApplyLayout();
                button.UpdateContent(); // 布局变化时也要更新内容
            }
        }

        private static void OnContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TLoadingButton button)
            {
                button.UpdateContent();
            }
        }

        private static void OnAppearancePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TLoadingButton button)
            {
                button.ApplyAppearance();
            }
        }

        private static void OnLoadingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TLoadingButton button)
            {
                button.UpdateLoadingState();
            }
        }

        #endregion

        protected virtual void ApplyBaseStyle()
        {
            // 如果使用了自定义模板，不应用基础样式
            if (Template != null && Template != this.FindResource("TLoadingButtonTemplate") as ControlTemplate)
                return;

            if (_loadingRing == null || _textElement == null) return;

            // 应用基础样式
            Background = BackgroundColor;
            Foreground = ForegroundColor;
            BorderBrush = BorderColor;

            // 应用加载环和文本颜色
            _loadingRing.StrokeColor = LoadingRingColor;
            _textElement.Foreground = TextColor;
            _textElement.FontSize = FontSize;
            _textElement.FontWeight = FontWeight;
        }

        protected virtual void ApplyLayout()
        {
            Width = ButtonWidth;
            Height = ButtonHeight;
        }

        protected virtual void ApplyAppearance()
        {
            // 如果使用了自定义模板，不应用外观
            if (Template != null && Template != this.FindResource("TLoadingButtonTemplate") as ControlTemplate)
                return;

            var whiteBrush = TThemeService.GetBrush("TColorWhite400");
            var lightWhiteBrush = TThemeService.GetBrush("TColorWhite300");
            var blackBrush = TThemeService.GetBrush("TColorBlack400");
            var lightBlackBrush = TThemeService.GetBrush("TColorBlack300");
            var greyBrush = TThemeService.GetBrush("TColorGrey400");

            // 系统主题色
            var systemBrush = TThemeService.GetBrush("TColorSystem400");
            var lightSystemBrush = TThemeService.GetBrush("TColorSystem300");

            // 主要颜色
            var primaryBrush = TThemeService.GetBrush("TColorPrimary400");
            var lightPrimaryBrush = TThemeService.GetBrush("TColorPrimary300");

            // 状态颜色
            var infoBrush = TThemeService.GetBrush("TColorInfo400");
            var lightInfoBrush = TThemeService.GetBrush("TColorInfo300");
            var successBrush = TThemeService.GetBrush("TColorSuccess400");
            var lightSuccessBrush = TThemeService.GetBrush("TColorSuccess300");
            var warningBrush = TThemeService.GetBrush("TColorWarning400");
            var lightWarningBrush = TThemeService.GetBrush("TColorWarning300");
            var dangerBrush = TThemeService.GetBrush("TColorDanger400");
            var lightDangerBrush = TThemeService.GetBrush("TColorDanger300");

            TextColor = LoadingRingColor = PressedTextColor = PressedLoadingRingColor= whiteBrush; 
            HoverTextColor = HoverLoadingRingColor = lightWhiteBrush;  

            switch (Appearance)
            {
                case ButtonAppearance.Default:
                    var brighterBlackBrush = TThemeService.GetBrush("TColorBlack200");
                    BackgroundColor = BorderColor = PressedBackgroundColor = PressedBorderColor = lightBlackBrush;
                    HoverBackgroundColor = HoverBorderColor = brighterBlackBrush;
                    break;

                case ButtonAppearance.System:
                    BackgroundColor = BorderColor = PressedBackgroundColor = PressedBorderColor = systemBrush;
                    HoverBackgroundColor = HoverBorderColor = lightSystemBrush;
                    break;
                case ButtonAppearance.Primary:
                    BackgroundColor = BorderColor = PressedBackgroundColor = PressedBorderColor = primaryBrush;
                    HoverBackgroundColor = HoverBorderColor = lightPrimaryBrush;
                    break;
                case ButtonAppearance.Info:
                    BackgroundColor = BorderColor = PressedBackgroundColor = PressedBorderColor = infoBrush;
                    HoverBackgroundColor = HoverBorderColor = lightInfoBrush;
                    break;
                case ButtonAppearance.Success:
                    BackgroundColor = BorderColor = PressedBackgroundColor = PressedBorderColor = successBrush;
                    HoverBackgroundColor = HoverBorderColor = lightSuccessBrush;
                    break;
                case ButtonAppearance.Warning:
                    BackgroundColor = BorderColor = PressedBackgroundColor = PressedBorderColor = warningBrush;
                    HoverBackgroundColor = HoverBorderColor = lightWarningBrush;
                    break;
                case ButtonAppearance.Danger:
                    BackgroundColor = BorderColor = PressedBackgroundColor = PressedBorderColor = dangerBrush;
                    HoverBackgroundColor = HoverBorderColor = lightDangerBrush;
                    break;
            }
        }

        protected virtual void UpdateContent()
        {
            if (_loadingRing != null)
            {
                // 自动计算加载环大小为按钮高度的 1/4
                double ringSize = Height / 4.0;
                _loadingRing.Radius = ringSize; 
                _loadingRing.StrokeThickness = LoadingRingStrokeThickness;
                _loadingRing.Visibility = LoadingRingVisibility;
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
            if (_textElement != null && _loadingRing != null)
            {
                _textElement.Margin = IsLoading && !string.IsNullOrEmpty(Text)
                    ? new Thickness(6, 0, 0, 0)
                    : new Thickness(0);
            }
        }

        protected virtual void UpdateLoadingState()
        {
            // 更新加载环可见性
            LoadingRingVisibility = IsLoading ? Visibility.Visible : Visibility.Collapsed;

            if (_loadingRing != null)
            {
                _loadingRing.IsIndeterminate = IsLoading;
            }

            // 更新按钮状态
            IsEnabled = !IsLoading;

            // 更新内容（包括间距）
            UpdateContent();
        }
    }
} 