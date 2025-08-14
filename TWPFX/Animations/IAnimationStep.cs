using System;
using System.Threading.Tasks;
using System.Windows;

namespace TWPFX.Animations
{
    /// <summary>
    /// 动画步骤接口
    /// </summary>
    public interface IAnimationStep
    {
        Task ExecuteAsync(FrameworkElement target);
        Action BeforeAction { get; set; }
        Action AfterAction { get; set; }
        int DelayAfterMs { get; set; }
    }
} 