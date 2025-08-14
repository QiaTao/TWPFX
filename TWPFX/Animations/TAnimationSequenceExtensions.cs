using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace TWPFX.Animations
{
    /// <summary>
    /// 动画序列扩展方法
    /// </summary>
    public static class TAnimationSequenceExtensions
    {
        public static TAnimationSequence CreateAnimationSequence(this FrameworkElement element)
        {
            return new TAnimationSequence(element);
        }

        public static TAnimationSequence AddRotateStep(
            this TAnimationSequence sequence,
            double fromAngle, double toAngle, int durationMs = 300,
            double centerX = 0.5, double centerY = 0.5,
            IEasingFunction easingFunction = null,
            Action beforeAction = null, Action afterAction = null,
            int delayAfterMs = 0)
        {
            var step = TAnimationFactory.CreateRotateAnimation(
                fromAngle, toAngle, durationMs, centerX, centerY, easingFunction);

            step.BeforeAction = beforeAction;
            step.AfterAction = afterAction;
            step.DelayAfterMs = delayAfterMs;

            return sequence.AddStep(step);
        }

        public static TAnimationSequence AddOpacityStep(
            this TAnimationSequence sequence,
            double fromOpacity, double toOpacity, int durationMs = 300,
            IEasingFunction easingFunction = null,
            Action beforeAction = null, Action afterAction = null,
            int delayAfterMs = 0)
        {
            var step = TAnimationFactory.CreateOpacityAnimation(
                fromOpacity, toOpacity, durationMs, easingFunction);

            step.BeforeAction = beforeAction;
            step.AfterAction = afterAction;
            step.DelayAfterMs = delayAfterMs;

            return sequence.AddStep(step);
        }

        public static TAnimationSequence AddWidthStep(
            this TAnimationSequence sequence,
            double? fromWidth, double toWidth, int durationMs = 300,
            IEasingFunction easingFunction = null,
            Action beforeAction = null, Action afterAction = null,
            int delayAfterMs = 0)
        {
            var step = TAnimationFactory.CreateSizeAnimation(
                FrameworkElement.WidthProperty, fromWidth ?? 0, toWidth, durationMs, easingFunction);

            step.BeforeAction = beforeAction;
            step.AfterAction = afterAction;
            step.DelayAfterMs = delayAfterMs;

            return sequence.AddStep(step);
        }

        public static TAnimationSequence AddHeightStep(
            this TAnimationSequence sequence,
            double? fromHeight, double toHeight, int durationMs = 300,
            IEasingFunction easingFunction = null,
            Action beforeAction = null, Action afterAction = null,
            int delayAfterMs = 0)
        {
            var step = TAnimationFactory.CreateSizeAnimation(
                FrameworkElement.HeightProperty, fromHeight ?? 0, toHeight, durationMs, easingFunction);

            step.BeforeAction = beforeAction;
            step.AfterAction = afterAction;
            step.DelayAfterMs = delayAfterMs;

            return sequence.AddStep(step);
        }

        public static TAnimationSequence AddCustomStep(
            this TAnimationSequence sequence,
            string propertyPath, object fromValue, object toValue,
            int durationMs = 300, IEasingFunction easingFunction = null,
            Action beforeAction = null, Action afterAction = null,
            int delayAfterMs = 0)
        {
            var step = TAnimationFactory.CreateCustomAnimation(
                propertyPath, fromValue, toValue, durationMs, easingFunction);

            step.BeforeAction = beforeAction;
            step.AfterAction = afterAction;
            step.DelayAfterMs = delayAfterMs;

            return sequence.AddStep(step);
        }
    }
}
