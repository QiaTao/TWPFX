using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.Windows.Data;
using System.Diagnostics;

namespace TWPFX.Controls.Progress
{
    /// <summary>
    /// Material风格的环形进度条控件，支持静态进度和动态不定长动画。
    /// </summary>
    public partial class TProgressRing : UserControl
    {
        #region 依赖属性
        /// <summary>
        /// 最小值（静态进度模式）
        /// </summary>
        public static readonly DependencyProperty MinProperty = DependencyProperty.Register(
            nameof(Min), typeof(double), typeof(TProgressRing), new PropertyMetadata(0.0, OnVisualPropertyChanged));
        /// <summary>
        /// 最大值（静态进度模式）
        /// </summary>
        public static readonly DependencyProperty MaxProperty = DependencyProperty.Register(
            nameof(Max), typeof(double), typeof(TProgressRing), new PropertyMetadata(100.0, OnVisualPropertyChanged));
        /// <summary>
        /// 当前进度值（静态进度模式）
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value), typeof(double), typeof(TProgressRing), new PropertyMetadata(0.0, OnVisualPropertyChanged));
        /// <summary>
        /// 圆环线宽
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            nameof(StrokeThickness), typeof(double), typeof(TProgressRing), new PropertyMetadata(6.0, OnVisualPropertyChanged));
        /// <summary>
        /// 圆环半径
        /// </summary>
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(
            nameof(Radius), typeof(double), typeof(TProgressRing), new PropertyMetadata(21.0, OnVisualPropertyChanged));
        /// <summary>
        /// 进度条颜色
        /// </summary>
        public static readonly DependencyProperty StrokeColorProperty = DependencyProperty.Register(
            nameof(StrokeColor), typeof(Brush), typeof(TProgressRing), new PropertyMetadata(Brushes.DeepSkyBlue, OnVisualPropertyChanged));
        /// <summary>
        /// 是否为不定长动画模式（true: 动画，false: 静态进度）
        /// </summary>
        public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register(
            nameof(IsIndeterminate), typeof(bool), typeof(TProgressRing), new PropertyMetadata(true, OnVisualPropertyChanged));
        /// <summary>
        /// 弧线起点（动画用）
        /// </summary>
        public static readonly DependencyProperty ArcStartPointProperty = DependencyProperty.Register(
            nameof(ArcStartPoint), typeof(Point), typeof(TProgressRing), new PropertyMetadata(new Point(0.5, 0), OnArcPointsChanged));
        /// <summary>
        /// 弧线终点（动画用）
        /// </summary>
        public static readonly DependencyProperty ArcEndPointProperty = DependencyProperty.Register(
            nameof(ArcEndPoint), typeof(Point), typeof(TProgressRing), new PropertyMetadata(new Point(0.5, 0), OnArcPointsChanged));
        #endregion

        // 属性封装
        public double Min { get => (double)GetValue(MinProperty); set => SetValue(MinProperty, value); }
        public double Max { get => (double)GetValue(MaxProperty); set => SetValue(MaxProperty, value); }
        public double Value { get => (double)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }
        public double StrokeThickness { get => (double)GetValue(StrokeThicknessProperty); set => SetValue(StrokeThicknessProperty, value); }
        public double Radius { get => (double)GetValue(RadiusProperty); set => SetValue(RadiusProperty, value); }
        public Brush StrokeColor { get => (Brush)GetValue(StrokeColorProperty); set => SetValue(StrokeColorProperty, value); }
        public bool IsIndeterminate { get => (bool)GetValue(IsIndeterminateProperty); set => SetValue(IsIndeterminateProperty, value); }
        public Point ArcStartPoint { get => (Point)GetValue(ArcStartPointProperty); set => SetValue(ArcStartPointProperty, value); }
        public Point ArcEndPoint { get => (Point)GetValue(ArcEndPointProperty); set => SetValue(ArcEndPointProperty, value); }

        // 动画相关字段
        private Storyboard _spinStoryboard; // 旋转和弧长动画主Storyboard
        private PointAnimationUsingKeyFrames _startPointAnimation; // 弧线起点动画
        private PointAnimationUsingKeyFrames _endPointAnimation;   // 弧线终点动画

        /// <summary>
        /// 构造函数，初始化控件
        /// </summary>
        public TProgressRing()
        {
            InitializeComponent();
            Loaded += (s, e) => InitializeProgress();
        }

        /// <summary>
        /// 初始化动画和初始状态
        /// </summary>
        private void InitializeProgress()
        {
            CreateStoryboard();
            UpdateIndeterminate();
            RefreshArc();
        }

        /// <summary>
        /// 创建Material风格的旋转+弧长动画Storyboard
        /// </summary>
        private void CreateStoryboard()
        {
            if (_spinStoryboard != null) return;
            _spinStoryboard = new Storyboard { RepeatBehavior = RepeatBehavior.Forever };
            // 旋转动画：Path整体360°匀速旋转
            var animation = new DoubleAnimation(0, 360, new Duration(TimeSpan.FromSeconds(1.5)));
            Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.RenderTransform).(RotateTransform.Angle)"));
            _spinStoryboard.Children.Add(animation);
            // Material风格弧长动画（增长-保持-收缩-循环）
            _startPointAnimation = new PointAnimationUsingKeyFrames
            {
                Duration = TimeSpan.FromSeconds(1.5)
            };
            _endPointAnimation = new PointAnimationUsingKeyFrames
            {
                Duration = TimeSpan.FromSeconds(1.5)
            };
            // 初始状态 (0秒)：起点0.5，终点0.51，形成一个很短的弧
            _startPointAnimation.KeyFrames.Add(new LinearPointKeyFrame(
                new Point(0.5, 0), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0))));
            _endPointAnimation.KeyFrames.Add(new LinearPointKeyFrame(
                new Point(0.5 + 0.01, 0), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0)))); // 从一个小弧开始
            // 增长阶段 (0-0.9秒)：终点从0.51增长到0.95，起点从0.5增长到0.7，弧长变长
            _startPointAnimation.KeyFrames.Add(new LinearPointKeyFrame(
                new Point(0.5 + 0.2, 0), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.9))));
            _endPointAnimation.KeyFrames.Add(new LinearPointKeyFrame(
                new Point(0.5 + 0.95, 0), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.9))));
            // 保持阶段 (0.9-1.2秒)：起点小幅前进，弧长基本不变，制造“拉伸”感
            _startPointAnimation.KeyFrames.Add(new LinearPointKeyFrame(
                new Point(0.5 + 0.25, 0), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.3))));
            // 收缩阶段 (0.9-1.5秒)：起点快速追上终点，弧长变短
            _startPointAnimation.KeyFrames.Add(new LinearPointKeyFrame(
                new Point(0.5 + 0.9, 0), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(1.5))));
            _endPointAnimation.KeyFrames.Add(new LinearPointKeyFrame(
                new Point(0.5 + 1.1, 0), KeyTime.FromTimeSpan(TimeSpan.FromSeconds(1.5))));
            // 绑定动画到依赖属性
            Storyboard.SetTargetProperty(_startPointAnimation,
                new PropertyPath("ArcStartPoint"));
            Storyboard.SetTargetProperty(_endPointAnimation,
                new PropertyPath("ArcEndPoint"));
            _spinStoryboard.Children.Add(_startPointAnimation);
            _spinStoryboard.Children.Add(_endPointAnimation);
        }

        /// <summary>
        /// ArcStartPoint/ArcEndPoint变化时，更新弧线几何
        /// </summary>
        private static void OnArcPointsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TProgressRing progress)
            {
                progress.UpdateArcGeometry();
            }
        }

        /// <summary>
        /// 依赖属性变化时刷新外观，切换动画/静态模式
        /// </summary>
        private static void OnVisualPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TProgressRing cp)
            {
                cp.RefreshArc();
                if (e.Property == IsIndeterminateProperty && cp.IsLoaded)
                    cp.UpdateIndeterminate();
            }
        }

        /// <summary>
        /// 根据IsIndeterminate切换动画或静态进度
        /// </summary>
        private void UpdateIndeterminate()
        {
            if (!IsLoaded || _spinStoryboard == null || Arc == null)
                return;
            // 绑定旋转动画目标
            if (_spinStoryboard.Children[0] is DoubleAnimation animation)
            {
                Storyboard.SetTarget(animation, Arc);
                Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.RenderTransform).(RotateTransform.Angle)"));
            }
            if (IsIndeterminate)
            {
                // 启动动画
                _spinStoryboard.Begin(this, true);
            }
            else
            {
                // 停止动画，重置角度，清除弧线动画，刷新静态进度
                _spinStoryboard.Stop(this);
                if (Arc.RenderTransform is RotateTransform rotateTransform)
                {
                    rotateTransform.Angle = 0;
                }
                // 清除ArcStartPoint和ArcEndPoint的动画
                BeginAnimation(ArcStartPointProperty, null);
                BeginAnimation(ArcEndPointProperty, null);
                // 停止动画后立即刷新圆弧显示静态进度
                RefreshArc();
            }
        }

        /// <summary>
        /// 根据ArcStartPoint/ArcEndPoint属性绘制动画弧线
        /// </summary>
        private void UpdateArcGeometry()
        {
            double r = Radius;
            double cx = r + StrokeThickness / 2;
            double cy = r + StrokeThickness / 2;
            // 将标准化坐标转换为实际坐标
            double startX = cx + r * Math.Cos(ArcStartPoint.X * 2 * Math.PI);
            double startY = cy + r * Math.Sin(ArcStartPoint.X * 2 * Math.PI);
            double endX = cx + r * Math.Cos(ArcEndPoint.X * 2 * Math.PI);
            double endY = cy + r * Math.Sin(ArcEndPoint.X * 2 * Math.PI);
            // 计算弧长是否大于半圆
            double angle = (ArcEndPoint.X - ArcStartPoint.X) * 360;
            if (angle < 0) angle += 360;
            bool isLargeArc = angle > 180;
            string arcData = $"M{startX},{startY} A{r},{r} 0 {(isLargeArc ? 1 : 0)},1 {endX},{endY}";
            Arc.Data = Geometry.Parse(arcData);
        }

        /// <summary>
        /// 刷新静态进度圆弧（IsIndeterminate=false时）
        /// </summary>
        private void RefreshArc()
        {
            // 计算当前进度百分比
            double percent = (Max > Min) ? (Value - Min) / (Max - Min) : 0;
            percent = Math.Max(0, Math.Min(1, percent));
            double angle = 360 * percent;
            double r = Radius;
            double cx = r + StrokeThickness / 2;
            double cy = r + StrokeThickness / 2;
            double startAngle = -90;
            double endAngle = startAngle + angle;
            double x0 = cx + r * Math.Cos(startAngle * Math.PI / 180);
            double y0 = cy + r * Math.Sin(startAngle * Math.PI / 180);
            double x1 = cx + r * Math.Cos(endAngle * Math.PI / 180);
            double y1 = cy + r * Math.Sin(endAngle * Math.PI / 180);
            bool isLargeArc = angle > 180;
            string arcData = $"M{x0},{y0} A{r},{r} 0 {(isLargeArc ? 1 : 0)},1 {x1},{y1}";
            Arc.Data = Geometry.Parse(arcData);
            Arc.Stroke = StrokeColor;
            Arc.StrokeThickness = StrokeThickness;
            Ellipse.StrokeThickness = StrokeThickness;
            // Ellipse.Width/Height 由XAML绑定
            Arc.Width = Arc.Height = 2 * r + StrokeThickness;
            Arc.Visibility = (IsIndeterminate || percent > 0) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}