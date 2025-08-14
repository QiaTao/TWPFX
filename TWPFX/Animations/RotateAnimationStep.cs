using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace TWPFX.Animations
{
    internal class RotateAnimationStep : AnimationStepBase
    {
        private readonly double _fromAngle;
        private readonly double _toAngle;
        private readonly int _durationMs;
        private readonly double _centerX;
        private readonly double _centerY;
        private readonly IEasingFunction _easingFunction;

        public RotateAnimationStep(
            double fromAngle, double toAngle, int durationMs,
            double centerX, double centerY, IEasingFunction easingFunction)
        {
            _fromAngle = fromAngle;
            _toAngle = toAngle;
            _durationMs = durationMs;
            _centerX = centerX;
            _centerY = centerY;
            _easingFunction = easingFunction;
        }

        protected override async Task ExecuteAnimationAsync(FrameworkElement target)
        {
            var tcs = new TaskCompletionSource<bool>();

            // 确保元素已加载并获取实际尺寸
            if (!target.IsLoaded)
            {
                target.Loaded += (s, e) => ExecuteRotation(target, tcs);
            }
            else
            {
                ExecuteRotation(target, tcs);
            }

            await tcs.Task;
        }

        private void ExecuteRotation(FrameworkElement target, TaskCompletionSource<bool> tcs)
        {
            // 获取或创建旋转变换
            var rotateTransform = GetOrCreateRotateTransform(target);

            // 计算实际中心点（基于元素当前实际尺寸）
            UpdateRotationCenter(target, rotateTransform);

            // 创建动画
            var animation = new DoubleAnimation
            {
                From = _fromAngle,
                To = _toAngle,
                Duration = TimeSpan.FromMilliseconds(_durationMs),
                EasingFunction = _easingFunction ?? new CubicEase { EasingMode = EasingMode.EaseOut },
                FillBehavior = FillBehavior.HoldEnd
            };

            animation.Completed += (s, e) => tcs.SetResult(true);

            // 应用动画
            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, animation);
        }

        private RotateTransform GetOrCreateRotateTransform(FrameworkElement element)
        {
            // 处理变换组
            if (element.RenderTransform is TransformGroup group)
            {
                var rotate = group.Children.OfType<RotateTransform>().FirstOrDefault();
                if (rotate != null) return rotate;

                var newRotate = new RotateTransform();
                group.Children.Add(newRotate);
                return newRotate;
            }

            // 处理单独旋转变换
            if (element.RenderTransform is RotateTransform existingRotate)
            {
                return existingRotate;
            }

            // 处理其他变换类型
            if (element.RenderTransform != null)
            {
                var newGroup = new TransformGroup();
                newGroup.Children.Add(element.RenderTransform);
                var newRotate = new RotateTransform();
                newGroup.Children.Add(newRotate);
                element.RenderTransform = newGroup;
                return newRotate;
            }

            // 无现有变换
            var rotateTransform = new RotateTransform();
            element.RenderTransform = rotateTransform;
            return rotateTransform;
        }

        private void UpdateRotationCenter(FrameworkElement element, RotateTransform rotateTransform)
        {
            // 获取实际尺寸（优先使用Actual，其次使用声明值，最后使用默认值100）
            double width = element.ActualWidth > 0 ? element.ActualWidth :
                          (element.Width > 0 ? element.Width : 100);
            double height = element.ActualHeight > 0 ? element.ActualHeight :
                           (element.Height > 0 ? element.Height : 100);

            // 设置旋转中心（0.5表示中心）
            rotateTransform.CenterX = width * _centerX;
            rotateTransform.CenterY = height * _centerY;
        }
    }
} 