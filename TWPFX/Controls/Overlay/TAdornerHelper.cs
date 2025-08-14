using System.Windows;
using System.Windows.Documents;

namespace TWPFX.Controls.Overlay
{
    /// <summary>
    /// 通用装饰器辅助类
    /// 用于管理控件的禁用状态装饰器
    /// </summary>
    public static class TAdornerHelper
    {
        private static readonly DependencyProperty DisabledAdornerProperty =
            DependencyProperty.RegisterAttached("DisabledAdorner", typeof(TDisabledAdorner), typeof(TAdornerHelper));

        public static TDisabledAdorner GetDisabledAdorner(DependencyObject obj)
        {
            return (TDisabledAdorner)obj.GetValue(DisabledAdornerProperty);
        }

        public static void SetDisabledAdorner(DependencyObject obj, TDisabledAdorner value)
        {
            obj.SetValue(DisabledAdornerProperty, value);
        }

        public static TDisabledAdorner GetOrAddDisabledAdorner(this UIElement element)
        {
            var adorner = GetDisabledAdorner(element);
            if (adorner == null)
            {
                adorner = new TDisabledAdorner(element);
                SetDisabledAdorner(element, adorner);

                // 获取AdornerLayer并添加装饰器
                var adornerLayer = AdornerLayer.GetAdornerLayer(element);
                if (adornerLayer != null)
                {
                    adornerLayer.Add(adorner);
                }
            }
            return adorner;
        }

        public static void RemoveDisabledAdorner(this UIElement element)
        {
            var adorner = GetDisabledAdorner(element);
            if (adorner != null)
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(element);
                if (adornerLayer != null)
                {
                    adornerLayer.Remove(adorner);
                }
                SetDisabledAdorner(element, null);
            }
        }
    }
} 