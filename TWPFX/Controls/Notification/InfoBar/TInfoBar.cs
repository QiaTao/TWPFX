using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;

namespace TWPFX.Controls.Notification.InfoBar
{
    public static class TInfoBar
    {
        private static TInfoBarAdorner? _infoBarAdorner;

        /// <summary>
        /// 创建或推送信息提示条到装饰层。
        /// </summary>
        /// <param name="title">提示条的标题。</param>
        /// <param name="content">提示条的内容。</param>
        /// <param name="severity">提示条的严重等级。</param>
        /// <param name="position">提示条的位置。</param>
        /// <param name="duration">提示条的显示持续时间。</param>
        /// <param name="isShadowEnabled">是否显示阴影</param>
        /// <param name="playMode">动画播放模式。</param>
        /// <param name="action">可选的操作按钮。</param>
        static void CreateAdorner(string title, string content, TInfoBarSeverity severity = TInfoBarSeverity.Success,
            TInfoBarPosition position = TInfoBarPosition.TOP_RIGHT, int duration = 2000, bool isShadowEnabled = false, TInfoBarPlayMode playMode = TInfoBarPlayMode.Once, ButtonBase? action = null)
        {
            if (_infoBarAdorner != null)
            {
                _infoBarAdorner.Push(title, content, severity, position, duration, isShadowEnabled, playMode, action);
                return;
            }
            var owner = GetDefaultWindow(); // 直接获取默认窗口
            var layer = GetAdornerLayer(owner) ?? throw new Exception("AdornerLayer not found.");
            _infoBarAdorner = new TInfoBarAdorner(layer);
            layer.Add(_infoBarAdorner);
            _infoBarAdorner.Push(title, content, severity, position, duration, isShadowEnabled, playMode, action);
        }

        /// <summary>
        /// 显示信息类型的提示条。
        /// </summary>
        /// <param name="title">提示条的标题。</param>
        /// <param name="content">提示条的内容。</param>
        /// <param name="position">提示条的位置。</param>
        /// <param name="duration">提示条的显示持续时间。</param>
        /// <param name="isShadowEnabled">是否显示阴影</param>
        /// <param name="playMode">动画播放模式。</param>
        /// <param name="action">可选的操作按钮。</param>
        public static void Info(string title, string content, TInfoBarPosition position = TInfoBarPosition.TOP_RIGHT,
            int duration = 2000, bool isShadowEnabled = false, TInfoBarPlayMode playMode = TInfoBarPlayMode.Once, ButtonBase? action = null)
        {
            CreateAdorner(title: title, content: content, position: position, duration: duration, isShadowEnabled: isShadowEnabled, playMode: playMode, action: action, severity: TInfoBarSeverity.Info);
        }

        /// <summary>
        /// 显示信息类型的提示条(标题默认为时间)。
        /// </summary>
        /// <param name="content">提示条的内容。</param>
        /// <param name="position">提示条的位置。</param>
        /// <param name="duration">提示条的显示持续时间。</param>
        /// <param name="isShadowEnabled">是否显示阴影</param>
        /// <param name="playMode">动画播放模式。</param>
        /// <param name="action">可选的操作按钮。</param>
        public static void Info(string content, TInfoBarPosition position = TInfoBarPosition.TOP_RIGHT,
            int duration = 2000, bool isShadowEnabled = false, TInfoBarPlayMode playMode = TInfoBarPlayMode.Once, ButtonBase? action = null)
        {

            CreateAdorner(title: DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), content: content, position: position, duration: duration, isShadowEnabled: isShadowEnabled, playMode: playMode, action: action, severity: TInfoBarSeverity.Info);
        }

        /// <summary>
        /// 显示成功类型的提示条。
        /// </summary>
        /// <param name="title">提示条的标题。</param>
        /// <param name="content">提示条的内容。</param>
        /// <param name="position">提示条的位置。</param>
        /// <param name="duration">提示条的显示持续时间。</param>
        /// <param name="isShadowEnabled">是否显示阴影</param>
        /// <param name="playMode">动画播放模式。</param>
        /// <param name="action">可选的操作按钮。</param>
        public static void Success(string title, string content, TInfoBarPosition position = TInfoBarPosition.TOP_RIGHT,
            int duration = 2000, bool isShadowEnabled = false, TInfoBarPlayMode playMode = TInfoBarPlayMode.Once, ButtonBase? action = null)
        {
            CreateAdorner(title: title, content: content, position: position, duration: duration, isShadowEnabled: isShadowEnabled, playMode: playMode, action: action, severity: TInfoBarSeverity.Success);
        }

        /// <summary>
        /// 显示成功类型的提示条(标题默认为时间)。
        /// </summary>
        /// <param name="content">提示条的内容。</param>
        /// <param name="position">提示条的位置。</param>
        /// <param name="duration">提示条的显示持续时间。</param>
        /// <param name="isShadowEnabled">是否显示阴影</param>
        /// <param name="playMode">动画播放模式。</param>
        /// <param name="action">可选的操作按钮。</param>
        public static void Success(string content, TInfoBarPosition position = TInfoBarPosition.TOP_RIGHT,
            int duration = 2000, bool isShadowEnabled = false, TInfoBarPlayMode playMode = TInfoBarPlayMode.Once, ButtonBase? action = null)
        {
            CreateAdorner(title: DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), content: content, position: position, duration: duration, isShadowEnabled: isShadowEnabled, playMode: playMode, action: action, severity: TInfoBarSeverity.Success);
        }

        /// <summary>
        /// 显示警告类型的提示条。
        /// </summary>
        /// <param name="title">提示条的标题。</param>
        /// <param name="content">提示条的内容。</param>
        /// <param name="position">提示条的位置。</param>
        /// <param name="duration">提示条的显示持续时间。</param>
        /// <param name="isShadowEnabled">是否显示阴影</param>
        /// <param name="playMode">动画播放模式。</param>
        /// <param name="action">可选的操作按钮。</param>
        public static void Warning(string title, string content, TInfoBarPosition position = TInfoBarPosition.TOP_RIGHT,
            int duration = 2000, bool isShadowEnabled = false, TInfoBarPlayMode playMode = TInfoBarPlayMode.Once, ButtonBase? action = null)
        {
            CreateAdorner(title: title, content: content, position: position, duration: duration, isShadowEnabled: isShadowEnabled, playMode: playMode, action: action, severity: TInfoBarSeverity.Warning);
        }

        /// <summary>
        /// 显示警告类型的提示条(标题默认为时间)。
        /// </summary>
        /// <param name="content">提示条的内容。</param>
        /// <param name="position">提示条的位置。</param>
        /// <param name="duration">提示条的显示持续时间。</param>
        /// <param name="isShadowEnabled">是否显示阴影</param>
        /// <param name="playMode">动画播放模式。</param>
        /// <param name="action">可选的操作按钮。</param>
        public static void Warning(string content, TInfoBarPosition position = TInfoBarPosition.TOP_RIGHT,
            int duration = 2000, bool isShadowEnabled = false, TInfoBarPlayMode playMode = TInfoBarPlayMode.Once, ButtonBase? action = null)
        {
            CreateAdorner(title: DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), content: content, position: position, duration: duration, isShadowEnabled: isShadowEnabled, playMode: playMode, action: action, severity: TInfoBarSeverity.Warning);
        }

        /// <summary>
        /// 显示错误类型的提示条。
        /// </summary>
        /// <param name="title">提示条的标题。</param>
        /// <param name="content">提示条的内容。</param>
        /// <param name="position">提示条的位置。</param>
        /// <param name="duration">提示条的显示持续时间。</param>
        /// <param name="isShadowEnabled">是否显示阴影</param>
        /// <param name="playMode">动画播放模式。</param>
        /// <param name="action">可选的操作按钮。</param>
        public static void Error(string title, string content, TInfoBarPosition position = TInfoBarPosition.TOP_RIGHT,
            int duration = 2000, bool isShadowEnabled = false, TInfoBarPlayMode playMode = TInfoBarPlayMode.Once, ButtonBase? action = null)
        {
            CreateAdorner(title: title, content: content, position: position, duration: duration, isShadowEnabled: isShadowEnabled, playMode: playMode, action: action, severity: TInfoBarSeverity.Error);
        }

        /// <summary>
        /// 显示错误类型的提示条(标题默认为时间)。
        /// </summary>
        /// <param name="content">提示条的内容。</param>
        /// <param name="position">提示条的位置。</param>
        /// <param name="duration">提示条的显示持续时间。</param>
        /// <param name="isShadowEnabled">是否显示阴影</param>
        /// <param name="playMode">动画播放模式。</param>
        /// <param name="action">可选的操作按钮。</param>
        public static void Error(string content, TInfoBarPosition position = TInfoBarPosition.TOP_RIGHT,
            int duration = 2000, bool isShadowEnabled = false, TInfoBarPlayMode playMode = TInfoBarPlayMode.Once, ButtonBase? action = null)
        {
            CreateAdorner(title: DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), content: content, position: position, duration: duration, isShadowEnabled: isShadowEnabled, playMode: playMode, action: action, severity: TInfoBarSeverity.Error);
        }

        /// <summary>
        /// 清除所有显示的提示条。
        /// </summary>
        public static void Clear()
        {
            _infoBarAdorner?.Clear();
        }

        /// <summary>
        /// 获取默认的活动窗口。
        /// </summary>
        /// <returns>默认窗口。</returns>
        public static Window GetDefaultWindow()
        {
            Window window = null;
            if (Application.Current != null && Application.Current.Windows.Count > 0)
            {
                window = Application.Current.Windows.OfType<Window>().FirstOrDefault(o => o.IsActive);
                window ??= Enumerable.FirstOrDefault(Application.Current.Windows.OfType<Window>());
            }
            return window;
        }

        /// <summary>
        /// 获取指定视觉对象的装饰层。
        /// </summary>
        /// <param name="visual">要获取装饰层的视觉对象。</param>
        /// <returns>装饰层，如果未找到则返回 null。</returns>
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
