using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace TWPFX.Controls.Icon.LottieIcon
{
    public class TLottieIconExtension : MarkupExtension
    {
        // 支持通过构造函数传递参数
        public TLottieIconExtension(TLottieIconType iconType)
        {
            IconType = iconType;
        }

        // 支持属性赋值
        [ConstructorArgument("iconType")]
        public TLottieIconType IconType { get; set; } = TLottieIconType.None;

        public TLottieIconStyle IconStyle { get; set; } = TLottieIconStyle.Regular;
        public TLottieIconAnimationMode AnimationMode { get; set; } = TLottieIconAnimationMode.OnHover;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new TLottieIcon
            {
                IconType = IconType,
                IconStyle = IconStyle,
                AnimationMode = AnimationMode
            };
        }
    }

    public static class LottieIconTypeExtensions
    {
        public static string ToRegular(this TLottieIconType value)
        {
            return $"pack://application:,,,/TWPFX;component/Assets/lordicon/regular/regular-{value}.json";
        }

        public static string ToSolid(this TLottieIconType value)
        {
            return $"pack://application:,,,/TWPFX;component/Assets/lordicon/solid/solid-{value}.json";
        }
    }
}
