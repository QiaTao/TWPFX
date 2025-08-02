using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Controls;

namespace TWPFX.Controls.Overlay
{
    /// <summary>
    /// 通用全屏遮罩Adorner，可自定义内容（如进度环、弹窗等）全屏显示。
    /// </summary>
    public class TMaskAdorner : Adorner
    {
        private readonly VisualCollection _visualChildren;
        private readonly UIElement _content;

        public TMaskAdorner(UIElement adornedElement, UIElement content)
            : base(adornedElement)
        {
            _visualChildren = new VisualCollection(this);
            _content = content;
            _visualChildren.Add(_content);
        }

        protected override int VisualChildrenCount => _visualChildren.Count;
        protected override Visual GetVisualChild(int index) => _visualChildren[index];

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (_content is FrameworkElement fe)
            {
                fe.Width = finalSize.Width;
                fe.Height = finalSize.Height;
            }
            _content.Arrange(new Rect(new Point(0, 0), finalSize));
            return finalSize;
        }
    }
} 