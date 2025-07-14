using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows;

namespace TWPFX.Animations
{
    public static class TAnimationSequenceExtensions
    {
        /// <summary>
        /// 动画序列步骤的配置容器（内部类）
        /// </summary>
        public class AnimationSequence
        {
            private readonly FrameworkElement _target;  // 动画目标元素
            private readonly List<AnimationStep> _steps = [];  // 动画步骤列表
            private Storyboard _storyboard;  // 管理的Storyboard实例

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
            public AnimationSequence AddStep(
                string targetProperty,
                double? from,
                double to,
                int durationMs,
                Action preAction = null,
                Action postAction = null,
                int delayMs = 0
            )
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
            public void Run(Action onCompleted = null)
            {
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

                    // 创建动画时钟来精确控制preAction
                    var clock = animation.CreateClock();
                    bool isPreActionFired = false;

                    // 绑定到时钟状态变化事件
                    clock.CurrentStateInvalidated += (s, e) =>
                    {
                        var currentClock = s as AnimationClock;
                        if (currentClock.CurrentState == ClockState.Active && !isPreActionFired)
                        {
                            _target.Dispatcher.Invoke(() =>
                            {
                                step.PreAction?.Invoke();
                                isPreActionFired = true;
                            });
                        }
                    };

                    if (step.PostAction != null)
                    {
                        clock.Completed += (s, e) =>
                            _target.Dispatcher.Invoke(step.PostAction);
                    }

                    // 将时钟而非动画添加到Storyboard
                    _storyboard.Children.Add(clock.Timeline);
                    Storyboard.SetTarget(clock.Timeline, _target);
                    Storyboard.SetTargetProperty(clock.Timeline, new PropertyPath(step.TargetProperty));

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
