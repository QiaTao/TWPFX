using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TWPFX.Controls.Icon.LottieIcon;

namespace TWPFX.Controls.Button.LottieButton
{
    public class TLottieButton : TLottieIcon
    {
        #region 依赖属性

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius), typeof(CornerRadius), typeof(TLottieButton), new FrameworkPropertyMetadata(new CornerRadius(4), FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
            nameof(Background), typeof(Brush), typeof(TLottieButton), new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty HoverBackgroundProperty = DependencyProperty.Register(
            nameof(HoverBackground), typeof(Brush), typeof(TLottieButton), new FrameworkPropertyMetadata((SolidColorBrush)Application.Current.Resources["TButtonBackgroundHover"], FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register(
            nameof(BorderBrush), typeof(Brush), typeof(TLottieButton), new FrameworkPropertyMetadata((SolidColorBrush)Application.Current.Resources["TButtonBorderDefault"], FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register(
            nameof(BorderThickness), typeof(Thickness), typeof(TLottieButton), new FrameworkPropertyMetadata(new Thickness(1), FrameworkPropertyMetadataOptions.AffectsRender));
        #endregion

        #region CLR属性包装器
        public CornerRadius CornerRadius { get => (CornerRadius)GetValue(CornerRadiusProperty); set => SetValue(CornerRadiusProperty, value); }
        public Brush Background { get => (Brush)GetValue(BackgroundProperty); set => SetValue(BackgroundProperty, value); }
        public Brush HoverBackground { get => (Brush)GetValue(HoverBackgroundProperty); set => SetValue(HoverBackgroundProperty, value); }
        public Brush BorderBrush { get => (Brush)GetValue(BorderBrushProperty); set => SetValue(BorderBrushProperty, value); }
        public Thickness BorderThickness { get => (Thickness)GetValue(BorderThicknessProperty); set => SetValue(BorderThicknessProperty, value); }

        #endregion

        #region 状态管理

        private bool _isMouseOver;
        private bool _isPressed;

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            _isMouseOver = true;
            InvalidateVisual();
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            _isMouseOver = false;
            _isPressed = false;
            InvalidateVisual();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            _isPressed = true;
            InvalidateVisual();
            e.Handled = true;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            if (_isPressed)
            {
                _isPressed = false;
                Clicked?.Invoke(this, EventArgs.Empty);
                InvalidateVisual();
            }
            e.Handled = true;
        }

        #endregion

        #region 渲染

        protected override void OnRender(DrawingContext drawingContext)
        {
            var background = _isMouseOver ? HoverBackground : Background;
            var borderPen = new Pen(BorderBrush, BorderThickness.Left);
            var rect = new Rect(0, 0, ActualWidth, ActualHeight);
            var roundedRect = new RectangleGeometry(rect, CornerRadius.TopLeft, CornerRadius.TopLeft);
            drawingContext.DrawGeometry(background, borderPen, roundedRect);
            base.OnRender(drawingContext);
        }

        #endregion

        public event EventHandler Clicked;

        static TLottieButton() { }
    }
}