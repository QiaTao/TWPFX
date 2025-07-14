using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TWPFX.Controls.TextBlock.CodeBlock
{
    public static class TCodeBlockStyleExtensions
    {
        public static string ToCss(this TCodeBlockStyle style)
        {
            var fieldInfo = style.GetType().GetField(style.ToString());
            var attribute = fieldInfo?.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? $"{style.ToString().ToLower()}.css";
        }
    }
}
