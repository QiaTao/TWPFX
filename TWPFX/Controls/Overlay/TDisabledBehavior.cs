using System.Windows;

namespace TWPFX.Controls.Overlay
{
    /// <summary>
    /// 通用禁用行为类
    /// 为任何控件提供禁用时的鼠标手势功能
    /// </summary>
    public static class TDisabledBehavior
    {
        public static readonly DependencyProperty EnableDisabledCursorProperty =
            DependencyProperty.RegisterAttached("EnableDisabledCursor", typeof(bool), typeof(TDisabledBehavior),
                new PropertyMetadata(false, OnEnableDisabledCursorChanged));

        public static bool GetEnableDisabledCursor(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableDisabledCursorProperty);
        }

        public static void SetEnableDisabledCursor(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableDisabledCursorProperty, value);
        }

        private static void OnEnableDisabledCursorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element)
            {
                bool enable = (bool)e.NewValue;
                if (enable)
                {
                    // 监听IsEnabled属性变化
                    element.IsEnabledChanged += OnElementIsEnabledChanged;
                    // 立即处理当前状态
                    HandleIsEnabledChanged(element, element.IsEnabled);
                }
                else
                {
                    // 移除监听
                    element.IsEnabledChanged -= OnElementIsEnabledChanged;
                    // 移除装饰器
                    element.RemoveDisabledAdorner();
                }
            }
        }

        private static void OnElementIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is UIElement element)
            {
                HandleIsEnabledChanged(element, (bool)e.NewValue);
            }
        }

        private static void HandleIsEnabledChanged(UIElement element, bool isEnabled)
        {
            if (!isEnabled)
            {
                // 禁用时添加装饰器
                var adorner = element.GetOrAddDisabledAdorner();
                adorner.SetEnable(false);
            }
            else
            {
                // 启用时移除装饰器
                element.RemoveDisabledAdorner();
            }
        }
    }
} 