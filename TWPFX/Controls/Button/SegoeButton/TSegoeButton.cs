using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using TWPFX.Controls.Icon.SegoeIcon;
using System.Windows.Controls;

namespace TWPFX.Controls.Button.SegoeButton
{
    public class TSegoeButton: TSegoeIcon
    {
        #region 依赖属性

        // 圆角半径
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(TSegoeButton),
                new FrameworkPropertyMetadata(new CornerRadius(4), FrameworkPropertyMetadataOptions.AffectsRender));

        // 按钮背景色
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register(
                nameof(Background),
                typeof(Brush),
                typeof(TSegoeButton),
                new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.AffectsRender));

        // 鼠标悬停背景色
        public static readonly DependencyProperty HoverBackgroundProperty =
            DependencyProperty.Register(
                nameof(HoverBackground),
                typeof(Brush),
                typeof(TSegoeButton),
                new FrameworkPropertyMetadata((SolidColorBrush)Application.Current.Resources["TButtonBackgroundHover"], FrameworkPropertyMetadataOptions.AffectsRender));

        // 边框颜色
        public static readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.Register(
                nameof(BorderBrush),
                typeof(Brush),
                typeof(TSegoeButton),
                new FrameworkPropertyMetadata((SolidColorBrush)Application.Current.Resources["TButtonBorderDefault"], FrameworkPropertyMetadataOptions.AffectsRender));

        // 边框厚度
        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register(
                nameof(BorderThickness),
                typeof(Thickness),
                typeof(TSegoeButton),
                new FrameworkPropertyMetadata(new Thickness(1), FrameworkPropertyMetadataOptions.AffectsRender));

        #endregion

        #region CLR属性包装器

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public Brush Background
        {
            get => (Brush)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        public Brush HoverBackground
        {
            get => (Brush)GetValue(HoverBackgroundProperty);
            set => SetValue(HoverBackgroundProperty, value);
        }

        public Brush BorderBrush
        {
            get => (Brush)GetValue(BorderBrushProperty);
            set => SetValue(BorderBrushProperty, value);
        }

        public Thickness BorderThickness
        {
            get => (Thickness)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }

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
            e.Handled = true; // 阻止事件冒泡
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            if (_isPressed)
            {
                _isPressed = false;
                Clicked?.Invoke(this, EventArgs.Empty); // 触发点击事件
                InvalidateVisual();
            }
            e.Handled = true;
        }

        #endregion

        #region 渲染

        protected override void OnRender(DrawingContext drawingContext)
        {
            // 绘制圆角背景
            var background = _isMouseOver ? HoverBackground : Background;

            var borderPen = new Pen(BorderBrush, BorderThickness.Left);

            var rect = new Rect(0, 0, ActualWidth, ActualHeight);
            var roundedRect = new RectangleGeometry(rect, CornerRadius.TopLeft, CornerRadius.TopLeft);

            drawingContext.DrawGeometry(background, borderPen, roundedRect);

            // 调用基类渲染图标
            base.OnRender(drawingContext);
        }

        #endregion

        public event EventHandler Clicked;

        static TSegoeButton()
        {

        }
    }
}
