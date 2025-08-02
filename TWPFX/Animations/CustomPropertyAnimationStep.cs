using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace TWPFX.Animations
{
    internal class CustomPropertyAnimationStep : AnimationStepBase
    {
        private readonly string _propertyPath;
        private readonly object _fromValue;
        private readonly object _toValue;
        private readonly int _durationMs;
        private readonly IEasingFunction _easingFunction;

        public CustomPropertyAnimationStep(
            string propertyPath, object fromValue, object toValue,
            int durationMs, IEasingFunction easingFunction)
        {
            _propertyPath = propertyPath;
            _fromValue = fromValue;
            _toValue = toValue;
            _durationMs = durationMs;
            _easingFunction = easingFunction;
        }

        protected override async Task ExecuteAnimationAsync(FrameworkElement target)
        {
            var property = GetDependencyProperty(target, _propertyPath) ?? throw new ArgumentException($"Property '{_propertyPath}' not found on target element");
            var tcs = new TaskCompletionSource<bool>();

            if (property.PropertyType == typeof(double))
            {
                // 如果fromValue为null，则获取当前值
                double fromValue = _fromValue == null ? 
                    (double)target.GetValue(property) : 
                    (double)_fromValue;

                var animation = new DoubleAnimation
                {
                    From = fromValue,
                    To = (double)_toValue,
                    Duration = TimeSpan.FromMilliseconds(_durationMs),
                    EasingFunction = _easingFunction ?? new CubicEase { EasingMode = EasingMode.EaseOut },
                    FillBehavior = FillBehavior.HoldEnd
                };

                animation.Completed += (s, e) => tcs.SetResult(true);
                target.BeginAnimation(property, animation);
            }
            else if (property.PropertyType == typeof(Color))
            {
                // 特殊处理Brush类型的属性
                if (_propertyPath.Contains("."))
                {
                    // 处理Brush.Color动画
                    var brush = GetBrushFromPropertyPath(target, _propertyPath);
                    
                    // 确定起始颜色
                    Color fromColor;
                    if (_fromValue == null)
                    {
                        // 如果fromValue为null，从当前Brush获取颜色
                        if (brush is SolidColorBrush solidBrush)
                        {
                            fromColor = solidBrush.Color;
                        }
                        else
                        {
                            fromColor = Colors.Transparent; // 默认值
                        }
                    }
                    else
                    {
                        fromColor = (Color)_fromValue;
                    }
                    
                    if (brush == null)
                    {
                        // 创建新的可修改Brush
                        brush = new SolidColorBrush(fromColor);
                        SetBrushToPropertyPath(target, _propertyPath, brush);
                    }
                    else if (brush.IsFrozen || !brush.CanFreeze)
                    {
                        // 如果Brush已冻结或不可修改，创建新的可修改Brush
                        brush = new SolidColorBrush(fromColor);
                        SetBrushToPropertyPath(target, _propertyPath, brush);
                    }

                    var animation = new ColorAnimation
                    {
                        From = fromColor,
                        To = (Color)_toValue,
                        Duration = TimeSpan.FromMilliseconds(_durationMs),
                        EasingFunction = _easingFunction,
                        FillBehavior = FillBehavior.HoldEnd
                    };

                    animation.Completed += (s, e) => tcs.SetResult(true);
                    brush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
                else
                {
                    var animation = new ColorAnimation
                    {
                        From = (Color)_fromValue,
                        To = (Color)_toValue,
                        Duration = TimeSpan.FromMilliseconds(_durationMs),
                        EasingFunction = _easingFunction,
                        FillBehavior = FillBehavior.HoldEnd
                    };

                    animation.Completed += (s, e) => tcs.SetResult(true);
                    target.BeginAnimation(property, animation);
                }
            }
            else
            {
                throw new NotSupportedException($"Property type {property.PropertyType.Name} is not supported for animation");
            }

            await tcs.Task;
        }

        private Brush GetBrushFromPropertyPath(FrameworkElement element, string propertyPath)
        {
            try
            {
                var parts = propertyPath.Split(new[] { '.' }, 2);
                var prop = GetDependencyProperty(element, parts[0]);
                if (prop == null) return null;

                var value = element.GetValue(prop);
                if (value is Brush brush) return brush;
                if (value == null) return null;

                // 处理类似 "Background.(SolidColorBrush.Color)" 的情况
                if (parts.Length > 1 && parts[1].StartsWith("(") && parts[1].EndsWith(")"))
                {
                    var subPropName = parts[1].Trim('(', ')').Split('.')[1];
                    var subProp = value.GetType().GetProperty(subPropName);
                    return subProp?.GetValue(value) as Brush;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        private void SetBrushToPropertyPath(FrameworkElement element, string propertyPath, Brush brush)
        {
            var parts = propertyPath.Split(new[] { '.' }, 2);
            var prop = GetDependencyProperty(element, parts[0]);
            if (prop == null) return;

            if (parts.Length == 1)
            {
                element.SetValue(prop, brush);
            }
            else if (parts[1].StartsWith("(") && parts[1].EndsWith(")"))
            {
                var subPropName = parts[1].Trim('(', ')').Split('.')[1];
                var currentValue = element.GetValue(prop);
                if (currentValue == null)
                {
                    currentValue = Activator.CreateInstance(prop.PropertyType);
                    element.SetValue(prop, currentValue);
                }

                var subProp = currentValue.GetType().GetProperty(subPropName);
                subProp?.SetValue(currentValue, brush);
            }
        }

        private DependencyProperty GetDependencyProperty(FrameworkElement element, string propertyPath)
        {
            // 处理复杂属性路径（如Background.(SolidColorBrush.Color)）
            if (propertyPath.Contains("."))
            {
                var parts = propertyPath.Split(new[] { '.' }, 2);
                var property = GetDependencyProperty(element, parts[0]);

                if (property == null) return null;

                var value = element.GetValue(property);
                if (value == null) return null;

                var subProperty = GetSubProperty(value.GetType(), parts[1]);
                return subProperty;
            }

            // 查找标准依赖属性
            var fieldInfo = element.GetType().GetField(
                propertyPath + "Property",
                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            if (fieldInfo != null)
                return (DependencyProperty)fieldInfo.GetValue(null);

            // 查找附加属性
            var parts2 = propertyPath.Split('.');
            if (parts2.Length == 2)
            {
                var ownerType = Type.GetType($"System.Windows.Controls.{parts2[0]}, PresentationFramework") ??
                               Type.GetType($"System.Windows.{parts2[0]}, PresentationFramework");

                if (ownerType != null)
                {
                    fieldInfo = ownerType.GetField(
                        parts2[1] + "Property",
                        BindingFlags.Public | BindingFlags.Static);

                    if (fieldInfo != null)
                        return (DependencyProperty)fieldInfo.GetValue(null);
                }
            }

            return null;
        }

        private DependencyProperty GetSubProperty(Type type, string propertyName)
        {
            var fieldInfo = type.GetField(
                propertyName + "Property",
                BindingFlags.Public | BindingFlags.Static);

            return fieldInfo?.GetValue(null) as DependencyProperty;
        }
    }
} 