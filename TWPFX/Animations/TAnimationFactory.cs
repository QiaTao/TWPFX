using System.Windows;
using System.Windows.Media.Animation;

namespace TWPFX.Animations
{
    /// <summary>
    /// 动画工厂 - 创建具体动画
    /// </summary>
    public static class TAnimationFactory
    {
        public static IAnimationStep CreateRotateAnimation(
            double fromAngle, double toAngle, int durationMs = 300,
            double centerX = 0.5, double centerY = 0.5,
            IEasingFunction easingFunction = null)
        {
            return new RotateAnimationStep(fromAngle, toAngle, durationMs, centerX, centerY, easingFunction);
        }

        public static IAnimationStep CreateOpacityAnimation(
            double fromOpacity, double toOpacity, int durationMs = 300,
            IEasingFunction easingFunction = null)
        {
            return new DoublePropertyAnimationStep(UIElement.OpacityProperty, fromOpacity, toOpacity, durationMs, easingFunction);
        }

        public static IAnimationStep CreateSizeAnimation(
            DependencyProperty property, double? fromValue, double toValue,
            int durationMs = 300, IEasingFunction easingFunction = null)
        {
            return new DoublePropertyAnimationStep(property, fromValue, toValue, durationMs, easingFunction);
        }

        public static IAnimationStep CreateCustomAnimation(
            string propertyPath, object fromValue, object toValue,
            int durationMs = 300, IEasingFunction easingFunction = null)
        {
            return new CustomPropertyAnimationStep(propertyPath, fromValue, toValue, durationMs, easingFunction);
        }
    }
} 