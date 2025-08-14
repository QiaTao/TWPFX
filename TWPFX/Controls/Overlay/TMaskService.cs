using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Linq;

namespace TWPFX.Controls.Overlay
{
    /// <summary>
    /// 全局遮罩服务，可在Window上显示自定义内容（如进度环、弹窗等）+半透明遮罩。
    /// </summary>
    public static class TMaskService
    {
        private static TMaskAdorner? _maskAdorner;

        public static void ShowMask(UIElement content)
        {
            if (_maskAdorner != null) return;
            var owner = GetDefaultWindow();
            var layer = GetAdornerLayer(owner) ?? throw new System.Exception("AdornerLayer not found.");
            // 全屏Grid包裹内容
            var grid = new Grid
            {
                Background = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0)),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            grid.Children.Add(content);
            _maskAdorner = new TMaskAdorner(layer, grid);
            layer.Add(_maskAdorner);
        }

        public static void HideMask()
        {
            if (_maskAdorner != null)
            {
                var layer = AdornerLayer.GetAdornerLayer(_maskAdorner.AdornedElement);
                layer?.Remove(_maskAdorner);
                _maskAdorner = null;
            }
        }

        public static Window GetDefaultWindow()
        {
            Window window = null;
            if (Application.Current != null && Application.Current.Windows.Count > 0)
            {
                window = Application.Current.Windows.OfType<Window>().FirstOrDefault(o => o.IsActive);
                window ??= System.Linq.Enumerable.FirstOrDefault(Application.Current.Windows.OfType<Window>());
            }
            return window;
        }

        public static AdornerLayer GetAdornerLayer(Visual visual)
        {
            if (visual == null) return null;
            if (visual is AdornerDecorator decorator)
                return decorator.AdornerLayer;
            if (visual is ScrollContentPresenter presenter)
                return presenter.AdornerLayer;
            if (visual is Window window)
            {
                var visualContent = window.Content as Visual;
                return AdornerLayer.GetAdornerLayer(visualContent ?? visual);
            }
            return AdornerLayer.GetAdornerLayer(visual);
        }
    }
} 