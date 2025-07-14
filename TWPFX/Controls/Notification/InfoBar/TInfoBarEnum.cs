using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWPFX.Controls.Notification.InfoBar
{
    /// <summary>
    /// 信息提示条的位置枚举
    /// </summary>
    public enum TInfoBarPosition
    {
        TOP_LEFT,
        TOP,
        TOP_RIGHT,
        BOTTOM_LEFT,
        BOTTOM,
        BOTTOM_RIGHT
    }

    /// <summary>
    /// 信息提示条的严重等级。
    /// </summary>
    public enum TInfoBarSeverity
    {
        Info,
        Success,
        Warning,
        Error
    }

    /// <summary>
    /// 信息提示条的动画播放模式。
    /// </summary>
    public enum TInfoBarPlayMode
    {
        /// <summary> 总是播放动画</summary>
        Always,

        /// <summary> 仅播放一次动画（后续不再播放）</summary>
        Once,

        /// <summary> 不播放动画（直接应用最终状态）</summary>
        None
    }
}
