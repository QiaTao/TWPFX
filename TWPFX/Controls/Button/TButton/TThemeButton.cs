using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TWPFX.Service;

namespace TWPFX.Controls.Button.TButton
{
    /// <summary>
    /// 主题按钮控件，继承自TBaseButton，提供ButtonStyle和Appearance样式
    /// </summary>
    public class TThemeButton : TBaseButton
    {
        #region 依赖属性

        public static readonly DependencyProperty ButtonStyleProperty =
            DependencyProperty.Register(nameof(ButtonStyle), typeof(ButtonStyle), typeof(TThemeButton),
                new PropertyMetadata(ButtonStyle.Solid, OnButtonStyleChanged));

        public static readonly DependencyProperty AppearanceProperty =
            DependencyProperty.Register(nameof(Appearance), typeof(ButtonAppearance), typeof(TThemeButton),
                new PropertyMetadata(ButtonAppearance.Default, OnAppearanceChanged));

        #endregion

        #region CLR属性包装器

        public ButtonStyle ButtonStyle
        {
            get => (ButtonStyle)GetValue(ButtonStyleProperty);
            set => SetValue(ButtonStyleProperty, value);
        }

        public ButtonAppearance Appearance
        {
            get => (ButtonAppearance)GetValue(AppearanceProperty);
            set => SetValue(AppearanceProperty, value);
        }

        #endregion

        public TThemeButton()
        {

        }

        protected override void InitializeBaseButton()
        {
            // 初始化时应用样式
            ApplyButtonStyle();
            base.InitializeBaseButton();
        }

        #region 属性变更回调

        private static void OnButtonStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TThemeButton button)
            {
                button.ApplyButtonStyle();
            }
        }

        private static void OnAppearanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TThemeButton button)
            {
                button.ApplyButtonStyle();
            }
        }

        #endregion

        protected virtual void ApplyButtonStyle()
        {
            var (backgroundColor, textColor, borderColor, hoverBackgroundColor, hoverTextColor, hoverBorderColor,
                    pressBackgroundColor, pressTextColor, pressBorderColor, cornerRadius) = GetAppearanceColors();

            // 直接设置TBaseButton的依赖属性
            BackgroundColor = backgroundColor;
            TextColor = textColor;
            BorderColor = borderColor;
            BorderThickness = 1;
            CornerRadius = cornerRadius;
            
            // 应用悬停和按下状态颜色
            HoverBackgroundColor = hoverBackgroundColor;
            HoverTextColor = hoverTextColor;
            HoverBorderColor = hoverBorderColor;

            PressedBackgroundColor = pressBackgroundColor;
            PressedTextColor = pressTextColor;
            PressedBorderColor = pressBorderColor;

            // 对于Circle样式，确保尺寸设置正确
            if (ButtonStyle == ButtonStyle.Circle)
            {
                double minsize = Math.Min(ActualWidth > 0 ? ActualWidth : Width, ActualHeight > 0 ? ActualHeight : Height);
                if (minsize > 0)
                {
                    ButtonWidth = ButtonHeight = minsize;
                }
            }
        }

        protected virtual (Brush backgroundColor, Brush textColor, Brush borderColor,
                Brush hoverBackgroundColor, Brush hoverTextColor, Brush hoverBorderColor,
                Brush pressBackgroundColor, Brush pressTextColor, Brush pressBorderColor, double cornerRadius) GetAppearanceColors()
        {
            // 使用TThemeService获取颜色资源，添加防护代码
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

            Brush backgroundColor = blackBrush;  // 背景色
            Brush textColor = whiteBrush;  // 文本颜色
            Brush borderColor = blackBrush;  // 边框颜色
            Brush hoverBackgroundColor = lightBlackBrush;  // 悬停背景色
            Brush hoverTextColor = lightWhiteBrush;  // 悬停文本颜色
            Brush hoverBorderColor = lightBlackBrush;  // 悬停边框颜色
            Brush pressBackgroundColor = blackBrush;  // 按下背景色
            Brush pressTextColor = whiteBrush;  // 按下文本颜色
            Brush pressBorderColor = blackBrush;  // 按下边框颜色
            double cornerRadius = 6; // 圆角边框

            if (ButtonStyle == ButtonStyle.Solid)
            {
                switch (Appearance)
                {
                    case ButtonAppearance.Default:
                        var brighterBlackBrush = TThemeService.GetBrush("TColorBlack200");
                        backgroundColor = borderColor = pressBackgroundColor = pressBorderColor = lightBlackBrush;
                        hoverBackgroundColor = hoverBorderColor = brighterBlackBrush;
                        break;
                    case ButtonAppearance.System:
                        backgroundColor = borderColor = pressBackgroundColor = pressBorderColor = systemBrush;
                        hoverBackgroundColor = hoverBorderColor = lightSystemBrush;
                        break;
                    case ButtonAppearance.Primary:
                        backgroundColor = borderColor = pressBackgroundColor = pressBorderColor = primaryBrush;
                        hoverBackgroundColor = hoverBorderColor = lightPrimaryBrush;
                        break;
                    case ButtonAppearance.Info:
                        backgroundColor = borderColor = pressBackgroundColor = pressBorderColor = infoBrush;
                        hoverBackgroundColor = hoverBorderColor = lightInfoBrush;
                        break;
                    case ButtonAppearance.Success:
                        backgroundColor = borderColor = pressBackgroundColor = pressBorderColor = successBrush;
                        hoverBackgroundColor = hoverBorderColor = lightSuccessBrush;
                        break;
                    case ButtonAppearance.Warning:
                        backgroundColor = borderColor = pressBackgroundColor = pressBorderColor = warningBrush;
                        hoverBackgroundColor = hoverBorderColor = lightWarningBrush;
                        break;
                    case ButtonAppearance.Danger:
                        backgroundColor = borderColor = pressBackgroundColor = pressBorderColor = dangerBrush;
                        hoverBackgroundColor = hoverBorderColor = lightDangerBrush;
                        break;
                }
            }
            else if (ButtonStyle == ButtonStyle.Outlined)
            {
                backgroundColor = hoverTextColor = pressBackgroundColor = whiteBrush;
                borderColor = pressBorderColor = greyBrush;

                textColor = Appearance switch
                {
                    ButtonAppearance.Default => hoverBackgroundColor = hoverBorderColor = pressTextColor = lightBlackBrush,
                    ButtonAppearance.System => hoverBackgroundColor = hoverBorderColor = pressTextColor = systemBrush,
                    ButtonAppearance.Primary => hoverBackgroundColor = hoverBorderColor = pressTextColor = primaryBrush,
                    ButtonAppearance.Info => hoverBackgroundColor = hoverBorderColor = pressTextColor = infoBrush,
                    ButtonAppearance.Success => hoverBackgroundColor = hoverBorderColor = pressTextColor = successBrush,
                    ButtonAppearance.Warning => hoverBackgroundColor = hoverBorderColor = pressTextColor = warningBrush,
                    ButtonAppearance.Danger => hoverBackgroundColor = hoverBorderColor = pressTextColor = dangerBrush,
                    _ => hoverBackgroundColor = hoverBorderColor = pressTextColor = blackBrush,
                };
            }
            else if (ButtonStyle == ButtonStyle.Circle) {
                double minsize = Math.Min(ButtonWidth, Width);
                ButtonWidth = ButtonHeight = minsize;  // 设置属性而不是直接设置Width/Height
                cornerRadius = minsize / 2;
                Text = "";
                textColor = hoverTextColor = pressTextColor = whiteBrush;
                switch (Appearance) {
                    case ButtonAppearance.Default:
                        backgroundColor = borderColor = pressBackgroundColor = pressBorderColor = blackBrush;
                        hoverBackgroundColor = hoverBorderColor = lightBlackBrush;
                        break;
                    case ButtonAppearance.System:
                        backgroundColor = borderColor = pressBackgroundColor = pressBorderColor = systemBrush;
                        hoverBackgroundColor = hoverBorderColor = lightSystemBrush;
                        break;
                    case ButtonAppearance.Primary:
                        backgroundColor = borderColor = pressBackgroundColor = pressBorderColor = primaryBrush;
                        hoverBackgroundColor = hoverBorderColor = lightPrimaryBrush;
                        break;
                    case ButtonAppearance.Info:
                        backgroundColor = borderColor = pressBackgroundColor = pressBorderColor = infoBrush;
                        hoverBackgroundColor = hoverBorderColor = lightInfoBrush;
                        break;
                    case ButtonAppearance.Success:
                        backgroundColor = borderColor = pressBackgroundColor = pressBorderColor = successBrush;
                        hoverBackgroundColor = hoverBorderColor = lightSuccessBrush;
                        break;
                    case ButtonAppearance.Warning:
                        backgroundColor = borderColor = pressBackgroundColor = pressBorderColor = warningBrush;
                        hoverBackgroundColor = hoverBorderColor = lightWarningBrush;
                        break;
                    case ButtonAppearance.Danger:
                        backgroundColor = borderColor = pressBackgroundColor = pressBorderColor = dangerBrush;
                        hoverBackgroundColor = hoverBorderColor = lightDangerBrush;
                        break;
                }
            }
            else if (ButtonStyle == ButtonStyle.Filled)
            {
                // 获取更浅的颜色变体
                var paleBlackBrush = TThemeService.GetBrush("TColorBlack100");
                var brighterBlackBrush = TThemeService.GetBrush("TColorBlack200");
                var paleSystemBrush = TThemeService.GetBrush("TColorSystem100");
                var brighterSystemBrush = TThemeService.GetBrush("TColorSystem200");

                var palePrimaryBrush = TThemeService.GetBrush("TColorPrimary100");
                var brighterPrimaryBrush = TThemeService.GetBrush("TColorPrimary200");
                var paleInfoBrush = TThemeService.GetBrush("TColorInfo100");
                var brighterInfoBrush = TThemeService.GetBrush("TColorInfo200");
                var paleSuccessBrush = TThemeService.GetBrush("TColorSuccess100");
                var brighterSuccessBrush = TThemeService.GetBrush("TColorSuccess200");
                var paleWarningBrush = TThemeService.GetBrush("TColorWarning100");
                var brighterWarningBrush = TThemeService.GetBrush("TColorWarning200");
                var paleDangerBrush = TThemeService.GetBrush("TColorDanger100");
                var brighterDangerBrush = TThemeService.GetBrush("TColorDanger200");

                switch (Appearance)
                {
                    case ButtonAppearance.Default:
                        textColor = hoverTextColor = pressTextColor = blackBrush;
                        backgroundColor = borderColor = pressBackgroundColor = pressBorderColor = paleBlackBrush;
                        hoverBackgroundColor = hoverBorderColor = brighterBlackBrush;
                        break;
                    case ButtonAppearance.System:
                        textColor = hoverTextColor = pressTextColor = systemBrush;
                        backgroundColor = borderColor = pressBackgroundColor = pressBorderColor = paleSystemBrush;
                        hoverBackgroundColor = hoverBorderColor = brighterSystemBrush;
                        break;
                    case ButtonAppearance.Primary:
                        textColor = hoverTextColor = pressTextColor = primaryBrush;
                        backgroundColor = borderColor = pressBackgroundColor = pressBorderColor = palePrimaryBrush;
                        hoverBackgroundColor = hoverBorderColor = brighterPrimaryBrush;
                        break;
                    case ButtonAppearance.Info:
                        textColor = hoverTextColor = pressTextColor = infoBrush;
                        backgroundColor = borderColor = pressBackgroundColor = pressBorderColor = paleInfoBrush;
                        hoverBackgroundColor = hoverBorderColor = brighterInfoBrush;
                        break;
                    case ButtonAppearance.Success:
                        textColor = hoverTextColor = pressTextColor = successBrush;
                        backgroundColor = borderColor = pressBackgroundColor = pressBorderColor = paleSuccessBrush;
                        hoverBackgroundColor = hoverBorderColor = brighterSuccessBrush;
                        break;
                    case ButtonAppearance.Warning:
                        textColor = hoverTextColor = pressTextColor = warningBrush;
                        backgroundColor = borderColor = pressBackgroundColor = pressBorderColor = paleWarningBrush;
                        hoverBackgroundColor = hoverBorderColor = brighterWarningBrush;
                        break;
                    case ButtonAppearance.Danger:
                        textColor = hoverTextColor = pressTextColor = dangerBrush;
                        backgroundColor = borderColor = pressBackgroundColor = pressBorderColor = paleDangerBrush;
                        hoverBackgroundColor = hoverBorderColor = brighterDangerBrush;
                        break;
                }
            }
            return (backgroundColor, textColor, borderColor,
                    hoverBackgroundColor, hoverTextColor, hoverBorderColor,
                    pressBackgroundColor, pressTextColor, pressBorderColor, cornerRadius);
        }
    }
} 