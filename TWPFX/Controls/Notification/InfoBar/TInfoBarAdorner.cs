using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using SkiaSharp;
using System.Diagnostics;
using System.Windows.Threading;


namespace TWPFX.Controls.Notification.InfoBar
{
    /// <summary>
    /// 用于在装饰层显示信息提示条的装饰器
    /// </summary>
    public class TInfoBarAdorner : Adorner
    {
        private readonly VisualCollection _visualChildren; // 存储所有面板
        private FrameworkElement _adornedElement;
        private Dictionary<TInfoBarPosition, StackPanel> _panels = [];

        public TInfoBarAdorner(UIElement adornedElement) : base(adornedElement)
        {
            _adornedElement = adornedElement as FrameworkElement;
            _visualChildren = new VisualCollection(this); // 初始化视觉集合
        }

        /// <summary>
        /// 推送一个新的信息提示条到指定位置。
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="severity">严重等级</param>
        /// <param name="position">显示位置</param>
        /// <param name="duration">显示持续时间</param>
        /// <param name="playMode">动画播放模式</param>
        /// <param name="action">可选的操作按钮</param>
        internal void Push(string title, string content, TInfoBarSeverity severity = TInfoBarSeverity.Success,
            TInfoBarPosition position = TInfoBarPosition.TOP_RIGHT, int duration = 2000, TInfoBarPlayMode playMode = TInfoBarPlayMode.Once, ButtonBase? action = null)
        {
            if (!_panels.TryGetValue(position, out StackPanel? panel))
            {
                panel = new StackPanel { Width = 310 };
                _panels[position] = panel;
                _visualChildren.Add(panel); // 将面板添加到视觉树
                InvalidateArrange();        // 强制布局更新
                AdornerLayer.GetAdornerLayer(this)?.Update(); // 强制更新 AdornerLayer
            }
            TInfoBarControl infoBar = new()
            {
                Title = title,
                InfoContent = content,
                Duration = duration,
                Severity = severity,
                Position = position,
                PlayMode = playMode,
                Action = action
            };
            infoBar.Closed += InfoBarClosed;
            panel.Children.Add(infoBar);
        }

        private void InfoBarClosed(object? sender, TInfoBarPosition e)
        {
            if (sender == null) return;
            TInfoBarControl infobar = (TInfoBarControl)sender;
            _panels.TryGetValue(e, out StackPanel? _panel);
            if (_panel == null || !_panel.Children.Contains(infobar)) return;
            // 获取关闭的控件索引和高度
            double height = infobar.ActualHeight;

            // 创建动画
            infobar.IsHitTestVisible = false; // 防止动画期间被点击

            // 高度和透明度动画
            var heightAnimation = new DoubleAnimation
            {
                From = height,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.3),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };

            var opacityAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(0.3),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };

            // 同时运行高度和透明度动画
            Storyboard.SetTarget(heightAnimation, infobar);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath("Height"));

            Storyboard.SetTarget(opacityAnimation, infobar);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));

            var storyboard = new Storyboard();
            storyboard.Children.Add(heightAnimation);
            storyboard.Children.Add(opacityAnimation);

            // 动画完成后移除控件
            storyboard.Completed += (s, args) =>
            {
                if (_panel != null && _panel.Children.Contains(infobar))
                {
                    _panel.Children.Remove(infobar);
                    // 如果panel为空，启动30秒后清理的计时器
                    if (_panel.Children.Count == 0)
                    {
                        var timer = new DispatcherTimer
                        {
                            Interval = TimeSpan.FromSeconds(30)
                        };

                        timer.Tick += (t, e2) =>
                        {
                            timer.Stop();

                            // 再次检查是否仍然为空
                            if (_panel.Children.Count == 0)
                            {
                                _panels.Remove(e);
                                _visualChildren.Remove(_panel);
                                InvalidateArrange();
                                AdornerLayer.GetAdornerLayer(this)?.Update(); // 强制更新 AdornerLayer
                            }
                        };

                        timer.Start();
                    }
                }
            };

            storyboard.Begin();
        }

        /// <summary>
        /// 清除所有显示的信息提示条。
        /// </summary>
        internal void Clear()
        {
            // 遍历所有面板
            foreach (var panel in _panels.Values)
            {
                // 复制当前面板的子控件列表（避免修改集合时遍历异常）
                var children = panel.Children.Cast<UIElement>().ToList();

                // 遍历所有 TInfoBarControl 并触发关闭
                foreach (var child in children)
                {
                    if (child is TInfoBarControl infoBar)
                    {
                        infoBar.Close();
                    }
                }
            }
        }

        // 重写VisualChildrenCount以返回实际子元素数量
        protected override int VisualChildrenCount => _visualChildren.Count;

        // 重写GetVisualChild以访问视觉集合中的子元素
        protected override Visual GetVisualChild(int index) => _visualChildren[index];

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (_adornedElement == null) return finalSize;

            foreach (var kvp in _panels)
            {
                TInfoBarPosition position = kvp.Key;
                StackPanel panel = kvp.Value;
                double x = 0, y = 0;

                switch (position)
                {
                    case TInfoBarPosition.TOP_LEFT:
                        x = 20; // 与Push中的Margin保持一致
                        y = 20;
                        break;
                    case TInfoBarPosition.TOP:
                        x = (_adornedElement.ActualWidth - panel.DesiredSize.Width) / 2;
                        y = 20;
                        break;
                    case TInfoBarPosition.TOP_RIGHT:
                        x = _adornedElement.ActualWidth - panel.DesiredSize.Width - 20;
                        y = 20;
                        break;
                    case TInfoBarPosition.BOTTOM_LEFT:
                        x = 20;
                        y = _adornedElement.ActualHeight - panel.DesiredSize.Height - 20;
                        break;
                    case TInfoBarPosition.BOTTOM:
                        x = (_adornedElement.ActualWidth - panel.DesiredSize.Width) / 2;
                        y = _adornedElement.ActualHeight - panel.DesiredSize.Height - 20;
                        break;
                    case TInfoBarPosition.BOTTOM_RIGHT:
                        x = _adornedElement.ActualWidth - panel.DesiredSize.Width - 20;
                        y = _adornedElement.ActualHeight - panel.DesiredSize.Height - 20;
                        break;
                }

                panel.Arrange(new Rect(x, y, panel.DesiredSize.Width, panel.DesiredSize.Height));
            }

            return finalSize;
        }
    }
}
