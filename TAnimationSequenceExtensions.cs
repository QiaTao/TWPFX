using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows;
using System.Diagnostics;

namespace TWPFX.Animations
{
    public static class TAnimationSequenceExtensions
    {
        /// <summary>
        /// 动画序列步骤的配置容器
        /// </summary>
        public class AnimationSequence
        {
            private readonly FrameworkElement _target;  // 动画目标元素
            private readonly List<AnimationStep> _steps = [];  // 动画步骤列表
            private Storyboard _storyboard;  // 管理的Storyboard实例
            private bool _isRunning = false;  // 是否正在运行

            public AnimationSequence(FrameworkElement target) => _target = target;

            /// <summary>
            /// 添加动画步骤
            /// </summary>
            /// <param name="targetProperty">目标依赖属性名（如"Width"）</param>
            /// <param name="from">起始值（null表示使用当前值）</param>
            /// <param name="to">目标值</param>
            /// <param name="durationMs">持续时间（毫秒）</param>
            /// <param name="preAction">动画开始前执行的操作</param>
            /// <param name="postAction">动画结束后执行的操作</param>
            /// <param name="delayMs">相对上一动画的延迟（毫秒）</param>
            /// <returns>当前序列实例（支持链式调用）</returns>
            public AnimationSequence AddStep(string targetProperty, double? from, double to, int durationMs, Action preAction = null, Action postAction = null, int delayMs = 0)
            {
                _steps.Add(new AnimationStep
                {
                    TargetProperty = targetProperty,
                    From = from,
                    To = to,
                    DurationMs = durationMs,
                    PreAction = preAction,
                    PostAction = postAction,
                    DelayMs = delayMs
                });
                return this;
            }

            /// <summary>
            /// 执行动画序列
            /// </summary>
            /// <param name="onCompleted">全部完成后的回调</param>
            /// <param name="restartIfRunning">动画正在运行时是否强制重新开始</param>
            public void Run(Action onCompleted = null, bool restartIfRunning = false)
            {
                if (_isRunning)
                {
                    if (restartIfRunning)
                    {
                        _storyboard?.Stop();
                    }
                    else
                    {
                        return;
                    }
                }

                _isRunning = true;

                // 清理旧动画资源
                _storyboard?.Stop();
                _storyboard = new Storyboard();
                TimeSpan currentBeginTime = TimeSpan.Zero;

                foreach (var step in _steps)
                {
                    var animation = new DoubleAnimation
                    {
                        From = step.From,
                        To = step.To,
                        Duration = TimeSpan.FromMilliseconds(step.DurationMs),
                        BeginTime = currentBeginTime + TimeSpan.FromMilliseconds(step.DelayMs),
                        EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                    };

                    // 正确的做法：直接将动画添加到Storyboard，而不是使用时钟
                    _storyboard.Children.Add(animation);
                    Storyboard.SetTarget(animation, _target);
                    Storyboard.SetTargetProperty(animation, new PropertyPath(step.TargetProperty));

                    // 处理preAction
                    if (step.PreAction != null)
                    {
                        // 使用BeginTime设置预操作的触发时间
                        _target.Dispatcher.BeginInvoke(
                            System.Windows.Threading.DispatcherPriority.Normal,
                            new Action(() =>
                            {
                                // 计算预操作应该执行的时间
                                var preActionTime = animation.BeginTime.GetValueOrDefault();

                                // 使用计时器触发预操作
                                var timer = new System.Windows.Threading.DispatcherTimer
                                {
                                    Interval = preActionTime,
                                    IsEnabled = true
                                };

                                timer.Tick += (s, e) =>
                                {
                                    timer.Stop();
                                    step.PreAction?.Invoke();
                                };
                            }));
                    }

                    // 处理postAction
                    if (step.PostAction != null)
                    {
                        // 计算动画结束时间
                        var endTime = animation.BeginTime.GetValueOrDefault() + animation.Duration.TimeSpan;

                        // 使用计时器触发后操作
                        _target.Dispatcher.BeginInvoke(
                            System.Windows.Threading.DispatcherPriority.Normal,
                            new Action(() =>
                            {
                                var timer = new System.Windows.Threading.DispatcherTimer
                                {
                                    Interval = endTime,
                                    IsEnabled = true
                                };

                                timer.Tick += (s, e) =>
                                {
                                    timer.Stop();
                                    step.PostAction?.Invoke();
                                };
                            }));
                    }

                    currentBeginTime = animation.BeginTime.GetValueOrDefault() +
                                     (animation.Duration.HasTimeSpan ?
                                      animation.Duration.TimeSpan :
                                      TimeSpan.Zero);
                }

                _storyboard.Completed += (s, e) => onCompleted?.Invoke();
                _storyboard.Begin();
            }

            private struct AnimationStep
            {
                public string TargetProperty;
                public double? From;
                public double To;
                public int DurationMs;
                public Action PreAction;
                public Action PostAction;
                public int DelayMs;
            }
        }

        /// <summary>
        /// 创建动画序列（入口扩展方法）
        /// </summary>
        /// <param name="element">动画目标控件</param>
        public static AnimationSequence CreateAnimationSequence(this FrameworkElement element)
            => new AnimationSequence(element);
    }
}