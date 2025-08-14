using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace TWPFX.Controls.Overlay
{
    /// <summary>
    /// 通用禁用状态装饰器
    /// 用于在控件禁用时显示自定义鼠标手势
    /// </summary>
    public class TDisabledAdorner : Adorner
    {
        private readonly VisualCollection _collection;
        private readonly Grid _grid;
        private readonly Border _border;

        protected override int VisualChildrenCount => _collection.Count;

        protected override Visual GetVisualChild(int index) => _collection[index];

        public TDisabledAdorner(UIElement adornedElement) : base(adornedElement)
        {
            _collection = new VisualCollection(this);
            _grid = new Grid();
            _border = new Border()
            {
                Background = Brushes.Transparent,
                Cursor = Cursors.No, // 使用禁止光标
            };

            _grid.Children.Add(_border);
            _collection.Add(_grid);
        }

        public void SetEnable(bool isEnable)
        {
            _border.IsHitTestVisible = !isEnable;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            return base.MeasureOverride(constraint);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _grid.Arrange(new Rect(finalSize));
            return base.ArrangeOverride(finalSize);
        }
    }
} 