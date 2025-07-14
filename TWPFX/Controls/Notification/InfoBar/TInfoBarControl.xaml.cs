using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TWPFX.Controls.Notification.InfoBar
{
    /// <summary>
    /// TInfoBarControl.xaml 的交互逻辑
    /// </summary>
    public partial class TInfoBarControl : UserControl, INotifyPropertyChanged
    {
        #region 数据源
        private string _title;
        /// <summary> 标题 </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }

        private string _infoContent;
        /// <summary> 内容 </summary>
        public string InfoContent
        {
            get { return _infoContent; }
            set { _infoContent = value; OnPropertyChanged(nameof(InfoContent)); }
        }

        private TInfoBarSeverity _severity;
        /// <summary> 严重等级 </summary>
        public TInfoBarSeverity Severity
        {
            get { return _severity; }
            set { _severity = value; OnPropertyChanged(nameof(Severity)); }
        }

        private TInfoBarPosition _position;
        /// <summary> 显示位置 </summary>
        public TInfoBarPosition Position
        {
            get { return _position; }
            set { _position = value; OnPropertyChanged(nameof(Position)); }
        }

        private int _duration;
        /// <summary> 持续时间 </summary>
        public int Duration
        {
            get { return _duration; }
            set { _duration = value; OnPropertyChanged(nameof(Duration)); }
        }

        private string _backgroundColor;
        /// <summary> 背景色 </summary>
        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; OnPropertyChanged(nameof(BackgroundColor)); }
        }

        private string _resourcePath;
        /// <summary> 资源路径 </summary>
        public string ResourcePath
        {
            get { return _resourcePath; }
            set { _resourcePath = value; OnPropertyChanged(nameof(ResourcePath)); }
        }

        private TInfoBarPlayMode _playMode;
        /// <summary> 动画播放模式 </summary>
        public TInfoBarPlayMode PlayMode
        {
            get { return _playMode; }
            set { _playMode = value; OnPropertyChanged(nameof(PlayMode)); }
        }

        private int _repeatCount;
        /// <summary> 动画循环次数 </summary>
        public int RepeatCount
        {
            get { return _repeatCount; }
            set { _repeatCount = value; OnPropertyChanged(nameof(RepeatCount)); }
        }

        private bool _autoPlay;
        /// <summary> 动画自动播放 </summary>
        public bool AutoPlay
        {
            get { return _autoPlay; }
            set { _autoPlay = value; OnPropertyChanged(nameof(AutoPlay)); }
        }

        private string _orientation;
        /// <summary> 布局方向 </summary>
        public string Orientation
        {
            get { return _orientation; }
            set { _orientation = value; OnPropertyChanged(nameof(Orientation)); }
        }

        private ButtonBase? _action;
        /// <summary> 操作按钮 </summary>
        public ButtonBase? Action
        {
            get { return _action; }
            set { _action = value; OnPropertyChanged(nameof(Action)); }
        }
        #endregion

        public event EventHandler<TInfoBarPosition> Closed;
        private DispatcherTimer closeTimer;

        public TInfoBarControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        // 控件加载时触发滑入动画
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PlaySlideInAnimation();
            StartTimer();
        }

        private void StartTimer()
        {
            if (Duration > 0)
            {
                closeTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(Duration)
                };
                closeTimer.Tick += (s, e) => { Close(); };
                closeTimer.Start();
            }
        }

        private void PlaySlideInAnimation()
        {
            var transform = new TranslateTransform();
            RootBorder.RenderTransform = transform;
            var storyboard = new Storyboard();
            // 根据方向设置初始位置
            switch (Position)
            {
                case TInfoBarPosition.TOP_LEFT:
                    transform.X = -50;
                    break;
                case TInfoBarPosition.BOTTOM_LEFT:
                    transform.X = -50;
                    break;
                case TInfoBarPosition.TOP_RIGHT:
                    transform.X = 50;
                    break;
                case TInfoBarPosition.BOTTOM_RIGHT:
                    transform.X = 50;
                    break;
                case TInfoBarPosition.TOP:
                    transform.Y = -20;
                    break;
                case TInfoBarPosition.BOTTOM:
                    transform.Y = 20;
                    break;
            }

            // 透明度动画
            var opacityAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(333),
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            Storyboard.SetTarget(opacityAnimation, RootBorder);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(UIElement.OpacityProperty));
            storyboard.Children.Add(opacityAnimation);

            // 主位移动画
            var mainAnimation = new DoubleAnimation
            {
                To = Position switch
                {
                    TInfoBarPosition.TOP_LEFT => 8,    // 从左到右滑动
                    TInfoBarPosition.BOTTOM_LEFT => 8,    // 从左到右滑动
                    TInfoBarPosition.TOP_RIGHT => -8,   // 从右到左滑动
                    TInfoBarPosition.BOTTOM_RIGHT => -8,   // 从右到左滑动
                    TInfoBarPosition.TOP => 8,    // 从上往下滑动
                    TInfoBarPosition.BOTTOM => -8,  // 从下往上滑动
                },
                Duration = TimeSpan.FromMilliseconds(333),
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };

            string propertyPath = Position == TInfoBarPosition.TOP_LEFT || Position == TInfoBarPosition.TOP_RIGHT
                || Position == TInfoBarPosition.BOTTOM_LEFT || Position == TInfoBarPosition.BOTTOM_RIGHT
                ? "(UIElement.RenderTransform).(TranslateTransform.X)"
                : "(UIElement.RenderTransform).(TranslateTransform.Y)";

            Storyboard.SetTarget(mainAnimation, RootBorder);
            Storyboard.SetTargetProperty(mainAnimation, new PropertyPath(propertyPath));
            storyboard.Children.Add(mainAnimation);

            // 回弹动画
            var bounceAnimation = new DoubleAnimation
            {
                To = 0,
                BeginTime = TimeSpan.FromMilliseconds(333),
                Duration = TimeSpan.FromMilliseconds(200),
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            Storyboard.SetTarget(bounceAnimation, RootBorder);
            Storyboard.SetTargetProperty(bounceAnimation, new PropertyPath(propertyPath));
            storyboard.Children.Add(bounceAnimation);

            storyboard.Begin();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public async void Close()
        {
            closeTimer?.Stop(); // 关闭前停止定时器
            var fadeOutStoryboard = (Storyboard)FindResource("FadeOutStoryboard");
            fadeOutStoryboard.Begin(RootBorder);
            await Task.Delay(200);
            Closed?.Invoke(this, Position);
        }

        public event PropertyChangedEventHandler? PropertyChanged;  // 实现接口

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (propertyName == nameof(Severity))
            {
                switch (Severity)
                {
                    case TInfoBarSeverity.Info:
                        BackgroundColor = "#f4f4f4";
                        ResourcePath = "pack://application:,,,/TWPFX;component/Assets/InfoBar/info.json";
                        break;
                    case TInfoBarSeverity.Success:
                        BackgroundColor = "#dff6dd";
                        ResourcePath = "pack://application:,,,/TWPFX;component/Assets/InfoBar/success.json";
                        break;
                    case TInfoBarSeverity.Warning:
                        BackgroundColor = "#fff4ce";
                        ResourcePath = "pack://application:,,,/TWPFX;component/Assets/InfoBar/warning.json";
                        break;
                    case TInfoBarSeverity.Error:
                        BackgroundColor = "#fde7e9";
                        ResourcePath = "pack://application:,,,/TWPFX;component/Assets/InfoBar/error.json";
                        break;
                }
            }
            if (propertyName == nameof(PlayMode))
            {
                switch (PlayMode)
                {
                    case TInfoBarPlayMode.None:
                        AutoPlay = false;
                        RepeatCount = 0;
                        break;
                    case TInfoBarPlayMode.Once:
                        AutoPlay = true;
                        RepeatCount = 0;
                        break;
                    case TInfoBarPlayMode.Always:
                        AutoPlay = true;
                        RepeatCount = -1;
                        break;
                }
            }
            if (propertyName == nameof(InfoContent))
            {
                if (GetStringWidth(InfoContent) > 16)
                {
                    Orientation = "Vertical";
                    textBlock_content.Margin = new Thickness(0, 0, 0, 6);
                }
                else
                {
                    Orientation = "Horizontal";
                }
                Orientation = GetStringWidth(InfoContent) > 16 ? "Vertical" : "Horizontal";
            }
            if (propertyName == nameof(Action))
            {
                if (Action != null)
                {
                    panel.Margin = new Thickness(0, 10, 0, 0);
                    panel.Children.Add(Action);
                }
            }
        }

        private int GetStringWidth(string text)
        {
            int length = 0;
            foreach (char c in text)
            {
                // 判断字符是否为中文（或其他双字节字符）
                if (c >= '\u4e00' && c <= '\u9fff') // 中文 Unicode 范围
                {
                    length += 2;
                }
                else
                {
                    length += 1;
                }
            }
            return length;
        }
    }
}
