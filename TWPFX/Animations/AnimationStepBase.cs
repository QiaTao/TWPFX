using System;
using System.Threading.Tasks;
using System.Windows;

namespace TWPFX.Animations
{
    internal abstract class AnimationStepBase : IAnimationStep
    {
        public Action BeforeAction { get; set; }
        public Action AfterAction { get; set; }
        public int DelayAfterMs { get; set; }

        public async Task ExecuteAsync(FrameworkElement target)
        {
            BeforeAction?.Invoke();
            await ExecuteAnimationAsync(target);
            AfterAction?.Invoke();

            if (DelayAfterMs > 0)
                await Task.Delay(DelayAfterMs);
        }

        protected abstract Task ExecuteAnimationAsync(FrameworkElement target);
    }
} 