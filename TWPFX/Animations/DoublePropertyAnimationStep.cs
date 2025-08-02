using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace TWPFX.Animations
{
    internal class DoublePropertyAnimationStep : AnimationStepBase
    {
        private readonly DependencyProperty _property;
        private readonly double? _fromValue;
        private readonly double _toValue;
        private readonly int _durationMs;
        private readonly IEasingFunction _easingFunction;

        public DoublePropertyAnimationStep(
            DependencyProperty property, double? fromValue, double toValue,
            int durationMs, IEasingFunction easingFunction)
        {
            _property = property;
            _fromValue = fromValue;
            _toValue = toValue;
            _durationMs = durationMs;
            _easingFunction = easingFunction;
        }

        protected override async Task ExecuteAnimationAsync(FrameworkElement target)
        {
            var tcs = new TaskCompletionSource<bool>();

            // 如果fromValue为null，则获取当前值
            double fromValue = _fromValue ?? (double)target.GetValue(_property);

            var animation = new DoubleAnimation
            {
                From = fromValue,
                To = _toValue,
                Duration = TimeSpan.FromMilliseconds(_durationMs),
                EasingFunction = _easingFunction ?? new CubicEase { EasingMode = EasingMode.EaseOut },
                FillBehavior = FillBehavior.HoldEnd
            };

            animation.Completed += (s, e) => tcs.SetResult(true);
            target.BeginAnimation(_property, animation);

            await tcs.Task;
        }
    }
} 