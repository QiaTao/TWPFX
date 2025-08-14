using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TWPFX.Controls.Icon.SegoeIcon;
using TWPFX.Controls.Notification.InfoBar;
using TWPFX.Service;

namespace TWPFX.Controls.Button.TButton
{
    /// <summary>
    /// 图标按钮控件，继承自TThemeButton，左侧放置TSegoeIcon图标
    /// </summary>
    public class TIconButton : TThemeButton
    {
        #region 依赖属性

        // 图标属性
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(TSegoeIconType), typeof(TIconButton),
                new PropertyMetadata(TSegoeIconType.None, OnIconPropertyChanged));

        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register(nameof(IconSize), typeof(double), typeof(TIconButton),
                new PropertyMetadata(16.0, OnIconPropertyChanged));

        public static readonly DependencyProperty IconColorProperty =
            DependencyProperty.Register(nameof(IconColor), typeof(Brush), typeof(TIconButton),
                new PropertyMetadata(Brushes.White, OnIconPropertyChanged));

        public static readonly DependencyProperty HoverIconColorProperty =
            DependencyProperty.Register(nameof(HoverIconColor), typeof(Brush), typeof(TIconButton),
                new PropertyMetadata(Brushes.White, OnIconPropertyChanged));

        public static readonly DependencyProperty PressedIconColorProperty =
            DependencyProperty.Register(nameof(PressedIconColor), typeof(Brush), typeof(TIconButton),
                new PropertyMetadata(Brushes.White, OnIconPropertyChanged));

        #endregion

        #region CLR属性包装器

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

        #endregion

        private bool isMouseDown = false;

        public TIconButton()
        {
            // 监听按钮状态变化
            MouseEnter += (s, e) => UpdateIconColorForState();
            MouseLeave += (s, e) => { isMouseDown = false; UpdateIconColorForState(); };
            PreviewMouseDown += (s, e) => { isMouseDown = true; UpdateIconColorForState(); };
            PreviewMouseUp += (s, e) => { isMouseDown = false; UpdateIconColorForState(); };
        }

        private void UpdateIconColorForState()
        {
            if (Content is not TSegoeIcon iconElement)
                return;

            var targetColor = isMouseDown
                ? PressedIconColor
                : IsMouseOver
                    ? HoverIconColor
                    : IconColor;

            iconElement.Foreground = targetColor;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // 更新图标内容
            UpdateIconContent();
            
            // 延迟更新按钮尺寸，确保控件已完全加载
            Dispatcher.BeginInvoke(new Action(() =>
            {
                UpdateButtonSize();
            }), System.Windows.Threading.DispatcherPriority.Loaded);
        }

        #region 属性变更回调

        private static void OnIconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TIconButton button)
            {
                if(e.Property == IconProperty || e.Property == IconSizeProperty)
                    button.UpdateIconContent();
                if (e.Property == ButtonStyleProperty || e.Property == AppearanceProperty)
                {
                    button.UpdateIconColors();
                    // 延迟更新按钮尺寸
                    button.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        button.UpdateButtonSize();
                    }), System.Windows.Threading.DispatcherPriority.Loaded);
                }
            }
        }

        #endregion

        protected virtual void UpdateIconContent()
        {
            if (Icon != TSegoeIconType.None)
            {
                // 创建图标元素并设置为Content
                var iconElement = new TSegoeIcon
                {
                    Glyph = Icon,
                    GlyphSize = IconSize,
                    Foreground = IconColor
                };
                Content = iconElement;
                UpdateIconColors();
                UpdateButtonSize();
                iconElement.Foreground = IconColor;
            }
            else
            {
                Content = null;
            }
        }

        protected virtual void UpdateIconColors()
        {
            if (Content is TSegoeIcon)
            {
                // 根据Appearance设置图标颜色
                var whiteBrush = TThemeService.GetBrush("TColorWhite400");
                var blackBrush = TThemeService.GetBrush("TColorBlack400");
                var systemBrush = TThemeService.GetBrush("TColorSystem400");
                var primaryBrush = TThemeService.GetBrush("TColorPrimary400");
                var infoBrush = TThemeService.GetBrush("TColorInfo400");
                var successBrush = TThemeService.GetBrush("TColorSuccess400");
                var warningBrush = TThemeService.GetBrush("TColorWarning400");
                var dangerBrush = TThemeService.GetBrush("TColorDanger400");

                Brush iconColor = whiteBrush;
                Brush hoverIconColor = whiteBrush;
                Brush pressedIconColor = whiteBrush;

                if (ButtonStyle == ButtonStyle.Solid)
                {
                    switch (Appearance)
                    {
                        case ButtonAppearance.Default:
                            iconColor = whiteBrush;
                            break;
                        case ButtonAppearance.System:
                            iconColor = whiteBrush;
                            break;
                        case ButtonAppearance.Primary:
                            iconColor = whiteBrush;
                            break;
                        case ButtonAppearance.Info:
                            iconColor = whiteBrush;
                            break;
                        case ButtonAppearance.Success:
                            iconColor = whiteBrush;
                            break;
                        case ButtonAppearance.Warning:
                            iconColor = whiteBrush;
                            break;
                        case ButtonAppearance.Danger:
                            iconColor = whiteBrush;
                            break;
                    }
                }
                else if (ButtonStyle == ButtonStyle.Outlined)
                {
                    switch (Appearance)
                    {
                        case ButtonAppearance.Default:
                            iconColor = blackBrush;
                            pressedIconColor = blackBrush;
                            break;
                        case ButtonAppearance.System:
                            iconColor = systemBrush;
                            pressedIconColor = systemBrush;
                            break;
                        case ButtonAppearance.Primary:
                            iconColor = primaryBrush;
                            pressedIconColor = primaryBrush;
                            break;
                        case ButtonAppearance.Info:
                            iconColor = infoBrush;
                            pressedIconColor = infoBrush;
                            break;
                        case ButtonAppearance.Success:
                            iconColor = successBrush;
                            pressedIconColor = successBrush;
                            break;
                        case ButtonAppearance.Warning:
                            iconColor = warningBrush;
                            pressedIconColor = warningBrush;
                            break;
                        case ButtonAppearance.Danger:
                            iconColor = dangerBrush;
                            pressedIconColor = dangerBrush;
                            break;
                    }
                }
                else if (ButtonStyle == ButtonStyle.Filled)
                {
                    var darkBlackBrush = TThemeService.GetBrush("TColorBlack500");
                    var darkSystemBrush = TThemeService.GetBrush("TColorSystem500");
                    var darkPrimaryBrush = TThemeService.GetBrush("TColorPrimary500");
                    var darkInfoBrush = TThemeService.GetBrush("TColorInfo500");
                    var darkSuccesBrush = TThemeService.GetBrush("TColorSuccess500");
                    var darkWarningBrush = TThemeService.GetBrush("TColorWarning500");
                    var darkDangerBrush = TThemeService.GetBrush("TColorDanger500");
                    switch (Appearance)
                    {
                        case ButtonAppearance.Default:
                            iconColor = hoverIconColor = blackBrush;
                            pressedIconColor = darkBlackBrush;
                            break;
                        case ButtonAppearance.System:
                            iconColor = hoverIconColor = systemBrush;
                            pressedIconColor = darkSystemBrush;
                            break;
                        case ButtonAppearance.Primary:
                            iconColor = hoverIconColor = primaryBrush;
                            pressedIconColor = darkPrimaryBrush;
                            break;
                        case ButtonAppearance.Info:
                            iconColor = hoverIconColor = infoBrush;
                            pressedIconColor = darkInfoBrush;
                            break;
                        case ButtonAppearance.Success:
                            iconColor = hoverIconColor = successBrush;
                            pressedIconColor = darkSuccesBrush;
                            break;
                        case ButtonAppearance.Warning:
                            iconColor = hoverIconColor = warningBrush;
                            pressedIconColor = darkWarningBrush;
                            break;
                        case ButtonAppearance.Danger:
                            iconColor = hoverIconColor = dangerBrush;
                            pressedIconColor = darkDangerBrush;
                            break;
                    }
                }

                IconColor = iconColor;
                HoverIconColor = hoverIconColor;
                PressedIconColor = pressedIconColor;

            }
        }

        protected virtual void UpdateButtonSize()
        {
            if (Content is TSegoeIcon)
            { 
                if (ButtonStyle == ButtonStyle.Circle)
                {
                    // 优先使用设置的Width和Height，如果为0则使用默认值
                    double width = Width > 0 ? Width : ButtonWidth;
                    double height = Height > 0 ? Height : ButtonHeight;
                    double minSize = Math.Min(width, height);
                    
                    // 如果minSize仍然为0，使用默认的按钮尺寸
                    if (minSize <= 0)
                    {
                        minSize = Math.Min(ButtonWidth, ButtonHeight);
                    }
                    
                    Text = "";
                    Width = Height = minSize;
                    CornerRadius = (int)minSize / 2;

                    ContentSpacing = 0;
                    BorderPadding = new Thickness(0);
                    ContentOrder = ContentOrder.TextFirst;
                }
            }
        }
    }
}