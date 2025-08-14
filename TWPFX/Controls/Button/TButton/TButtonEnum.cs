using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWPFX.Controls.Button.TButton
{
    /// <summary>
    /// 按钮样式枚举
    /// </summary>
    public enum ButtonStyle
    {
        Solid,      // 实心
        Outlined,   // 轮廓
        Circle,     // 圆形
        Filled      // 填充
    }

    /// <summary>
    /// 按钮外观枚举
    /// </summary>
    public enum ButtonAppearance
    {
        Default,
        Primary,
        System,
        Info,
        Success,
        Warning,
        Danger
    }

    /// <summary>
    /// 内容顺序枚举
    /// </summary>
    public enum ContentOrder
    {
        /// <summary>
        /// 内容在前，文本在后（默认）
        /// </summary>
        ContentFirst,

        /// <summary>
        /// 文本在前，内容在后
        /// </summary>
        TextFirst
    }
}
