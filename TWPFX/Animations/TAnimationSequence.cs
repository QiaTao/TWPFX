using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace TWPFX.Animations
{
    /// <summary>
    /// 动画序列管理类
    /// </summary>
    public class TAnimationSequence
    {
        private readonly FrameworkElement _target;
        private readonly List<IAnimationStep> _steps = new List<IAnimationStep>();

        public TAnimationSequence(FrameworkElement target)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
        }

        public TAnimationSequence AddStep(IAnimationStep step)
        {
            if (step == null) throw new ArgumentNullException(nameof(step));
            _steps.Add(step);
            return this;
        }

        public async Task RunAsync()
        {
            foreach (var step in _steps)
            {
                await step.ExecuteAsync(_target);
            }
        }

        public async void Run(Action completed = null)
        {
            await RunAsync();
            completed?.Invoke();
        }
    }
} 