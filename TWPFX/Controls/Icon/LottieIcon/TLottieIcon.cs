using LottieSharp.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TWPFX.Controls.Icon.LottieIcon
{
    public class TLottieIcon : LottieAnimationView
    {
        public static readonly DependencyProperty IconTypeProperty = DependencyProperty.Register("IconType", typeof(TLottieIconType), typeof(TLottieIcon), new PropertyMetadata(TLottieIconType.None, IconTypePropertyChangedCallback));
        public static readonly DependencyProperty IconStyleProperty = DependencyProperty.Register("IconStyle", typeof(TLottieIconStyle), typeof(TLottieIcon), new PropertyMetadata(TLottieIconStyle.None, IconStylePropertyChangedCallback));
        public static readonly DependencyProperty AnimationModeProperty = DependencyProperty.Register("AnimationMode", typeof(TLottieIconAnimationMode), typeof(TLottieIcon), new PropertyMetadata(TLottieIconAnimationMode.OnHover, AnimationModePropertyChangedCallback));

        public TLottieIconType IconType
        {
            get
            {
                return (TLottieIconType)GetValue(IconTypeProperty);
            }
            set
            {
                SetValue(IconTypeProperty, value);
            }
        }

        public TLottieIconStyle IconStyle
        {
            get
            {
                return (TLottieIconStyle)GetValue(IconStyleProperty);
            }
            set
            {
                SetValue(IconStyleProperty, value);
            }
        }

        public TLottieIconAnimationMode AnimationMode
        {
            get
            {
                return (TLottieIconAnimationMode)GetValue(AnimationModeProperty);
            }
            set
            {
                SetValue(AnimationModeProperty, value);
            }
        }


        private static void IconTypePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is TLottieIcon lottieAnimationIcon && e.NewValue is TLottieIconType lottieIconType && lottieIconType != TLottieIconType.None)
            {
                lottieAnimationIcon.ResourcePath = lottieAnimationIcon.IconStyle is TLottieIconStyle.Solid ? lottieIconType.ToSolid() : lottieIconType.ToRegular();
                lottieAnimationIcon.InvalidateVisual();  // 强制更新 UI
            }
        }

        private static void IconStylePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is TLottieIcon lottieAnimationIcon && e.NewValue is TLottieIconStyle lottieIconStyle && lottieAnimationIcon.IconType != TLottieIconType.None)
            {
                lottieAnimationIcon.ResourcePath = lottieIconStyle is TLottieIconStyle.Solid ? lottieAnimationIcon.IconType.ToSolid() : lottieAnimationIcon.IconType.ToRegular();
                lottieAnimationIcon.InvalidateVisual();  // 强制更新 UI
            }
        }

        private static void AnimationModePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is TLottieIcon lottieAnimationIcon && e.NewValue is TLottieIconAnimationMode mode)
            {
                switch (mode)
                {
                    case TLottieIconAnimationMode.Never:
                        lottieAnimationIcon.AutoPlay = false;
                        lottieAnimationIcon.RepeatCount = 0;
                        break;
                    case TLottieIconAnimationMode.Always:
                        lottieAnimationIcon.AutoPlay = true;
                        lottieAnimationIcon.RepeatCount = -1;
                        lottieAnimationIcon.PlayAnimation();
                        break;
                    case TLottieIconAnimationMode.OnHover:
                        lottieAnimationIcon.AutoPlay = false;
                        lottieAnimationIcon.RepeatCount = 0;
                        break;
                }
            }
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (AnimationMode == TLottieIconAnimationMode.OnHover)
            {
                PlayAnimation();
            }
        }

    }
}
