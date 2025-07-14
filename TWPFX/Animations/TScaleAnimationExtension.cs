using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows;
using System.Reflection;

namespace TWPFX.Animations
{

    public static class ScaleAnimationExtensions
    {
        #region 附加属性
        // 目标属性名
        public static readonly DependencyProperty TargetPropertyNameProperty =
            DependencyProperty.RegisterAttached(
                "TargetPropertyName",
                typeof(string),
                typeof(ScaleAnimationExtensions),
                new PropertyMetadata(null));

        // 动画初始值（可为空）
        public static readonly DependencyProperty InitialValueProperty =
            DependencyProperty.RegisterAttached(
                "InitialValue",
                typeof(double?),
                typeof(ScaleAnimationExtensions),
                new PropertyMetadata(null));

        // 动画目标值
        public static readonly DependencyProperty TargetValueProperty =
            DependencyProperty.RegisterAttached(
                "TargetValue",
                typeof(double),
                typeof(ScaleAnimationExtensions),
                new PropertyMetadata(0.0));

        // 动画持续时间（毫秒）
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.RegisterAttached(
                "Duration",
                typeof(int),
                typeof(ScaleAnimationExtensions),
                new PropertyMetadata(300));

        // 缓动函数
        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.RegisterAttached(
                "EasingFunction",
                typeof(IEasingFunction),
                typeof(ScaleAnimationExtensions),
                new PropertyMetadata(new CubicEase { EasingMode = EasingMode.EaseOut }));

        // 动画完成回调
        public static readonly DependencyProperty AnimationCompletedCallbackProperty =
            DependencyProperty.RegisterAttached(
                "AnimationCompletedCallback",
                typeof(Action),
                typeof(ScaleAnimationExtensions),
                new PropertyMetadata(null));
        #endregion

        #region Get/Set方法
        // Get/Set 方法
        public static string GetTargetPropertyName(DependencyObject obj) =>
               (string)obj.GetValue(TargetPropertyNameProperty);
        public static void SetTargetPropertyName(DependencyObject obj, string value) =>
            obj.SetValue(TargetPropertyNameProperty, value);

        public static double? GetInitialValue(DependencyObject obj) =>
                (double?)obj.GetValue(InitialValueProperty);
        public static void SetInitialValue(DependencyObject obj, double? value) =>
            obj.SetValue(InitialValueProperty, value);

        public static double GetTargetValue(DependencyObject obj) =>
            (double)obj.GetValue(TargetValueProperty);
        public static void SetTargetValue(DependencyObject obj, double value) =>
            obj.SetValue(TargetValueProperty, value);

        public static int GetDuration(DependencyObject obj) =>
            (int)obj.GetValue(DurationProperty);
        public static void SetDuration(DependencyObject obj, int value) =>
            obj.SetValue(DurationProperty, value);

        public static IEasingFunction GetEasingFunction(DependencyObject obj) =>
            (IEasingFunction)obj.GetValue(EasingFunctionProperty);
        public static void SetEasingFunction(DependencyObject obj, IEasingFunction value) =>
            obj.SetValue(EasingFunctionProperty, value);

        public static Action GetAnimationCompletedCallback(DependencyObject obj) =>
            (Action)obj.GetValue(AnimationCompletedCallbackProperty);
        public static void SetAnimationCompletedCallback(DependencyObject obj, Action value) =>
            obj.SetValue(AnimationCompletedCallbackProperty, value);

        public static void RemoveAnimationCompletedCallback(FrameworkElement element)
        {
            // 1. 获取当前回调
            Action callback = GetAnimationCompletedCallback(element);
            if (callback == null) return;

            // 2. 解除事件绑定
            string propertyName = GetTargetPropertyName(element);
            DependencyProperty dp = GetDependencyProperty(element.GetType(), propertyName);

            // 停止当前动画并获取最后一次动画值
            object currentValue = element.GetValue(dp);
            element.BeginAnimation(dp, null); // 解除动画绑定
            element.SetValue(dp, currentValue); // 保留当前值

            // 3. 清除回调属性
            SetAnimationCompletedCallback(element, null);
        }

        #endregion

        #region 动画控制

        /// <summary>
        /// 启动目标元素的动画
        /// </summary>
        /// <param name="element">动画目标元素</param>
        public static void StartAnimation(FrameworkElement element)
        {
            string propertyName = GetTargetPropertyName(element);
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException("TargetPropertyName must be set!");

            DependencyProperty dp = GetDependencyProperty(element.GetType(), propertyName) ?? throw new InvalidOperationException($"Dependency property '{propertyName}' not found.");

            // 获取初始值逻辑
            double? initialValue = GetInitialValue(element);
            double startValue = initialValue ?? (double)element.GetValue(dp); // 若未指定则用当前值

            var animation = new DoubleAnimation
            {
                From = startValue, // 设置动画起始值
                To = GetTargetValue(element),
                Duration = TimeSpan.FromMilliseconds(GetDuration(element)),
                EasingFunction = GetEasingFunction(element)
            };

            // 回调处理
            Action callback = GetAnimationCompletedCallback(element);
            if (callback != null)
            {
                animation.Completed += (sender, e) =>
                    element.Dispatcher.Invoke(callback);
            }

            element.BeginAnimation(dp, animation);
        }

        /// <summary>
        /// 停止目标元素的动画
        /// </summary>
        /// <param name="element">动画目标元素</param>
        /// <param name="keepCurrentValue">是否保留当前值（默认false）</param>
        public static void StopAnimation(FrameworkElement element, bool keepCurrentValue = false)
        {
            string propertyName = GetTargetPropertyName(element);
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException("TargetPropertyName must be set!");

            // 获取目标依赖属性
            DependencyProperty dp = GetDependencyProperty(element.GetType(), propertyName) ?? throw new InvalidOperationException($"Dependency property '{propertyName}' not found.");

            // 停止动画并处理当前值
            if (keepCurrentValue)
            {
                // 获取当前动画值并应用为属性静态值
                object currentValue = element.GetValue(dp);
                element.BeginAnimation(dp, null); // 解除动画绑定
                element.SetValue(dp, currentValue); // 保留当前值
            }
            else
            {
                element.BeginAnimation(dp, null); // 直接停止动画，属性值回退到初始状态
            }
        }

        // 通过反射获取依赖属性
        private static DependencyProperty GetDependencyProperty(Type type, string propertyName)
        {
            var field = type.GetField(
                propertyName + "Property",
                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy
            );
            return field?.GetValue(null) as DependencyProperty;
        }

        #endregion
    }
}
