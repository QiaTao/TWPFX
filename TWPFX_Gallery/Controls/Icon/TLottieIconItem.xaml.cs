using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TWPFX.Controls.Icon.LottieIcon;

namespace TWPFX_Gallery.Controls.Icon
{
    /// <summary>
    /// TLottieIconItem.xaml 的交互逻辑
    /// </summary>
    public partial class TLottieIconItem : UserControl
    {
        #region 依赖属性
        public static readonly DependencyProperty ITypeProperty = DependencyProperty.Register("IType", typeof(TLottieIconType), typeof(TLottieIconItem));
        public static readonly DependencyProperty IStyleProperty = DependencyProperty.Register("IStyle", typeof(TLottieIconStyle), typeof(TLottieIconItem));
        public static readonly DependencyProperty IAnimationModeProperty = DependencyProperty.Register("IAnimationMode", typeof(TLottieIconAnimationMode), typeof(TLottieIconItem), new PropertyMetadata(TLottieIconAnimationMode.OnHover, IAnimationModeropertyChangedCallback));
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(TLottieIconItem), new PropertyMetadata(false, IsSelectedropertyChangedCallback));

        public TLottieIconType IType
        {
            get
            {
                return (TLottieIconType)GetValue(ITypeProperty);
            }
            set
            {
                SetValue(ITypeProperty, value);
            }
        }

        public TLottieIconStyle IStyle
        {
            get
            {
                return (TLottieIconStyle)GetValue(IStyleProperty);
            }
            set
            {
                SetValue(IStyleProperty, value);
            }
        }

        public TLottieIconAnimationMode IAnimationMode
        {
            get
            {
                return (TLottieIconAnimationMode)GetValue(IAnimationModeProperty);
            }
            set
            {
                SetValue(IAnimationModeProperty, value);
            }
        }

        public bool IsSelected
        {
            get
            {
                return (bool)GetValue(IsSelectedProperty);
            }
            set
            {
                SetValue(IsSelectedProperty, value);
            }
        }

        #endregion

        public event EventHandler clicked;

        public TLottieIconItem()
        {
            DataContext = this;
            InitializeComponent();
        }

        private static void IsSelectedropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is TLottieIconItem item && e.NewValue is bool isSelected)
            {
                if (isSelected)
                {
                    item.border.BorderBrush = (Brush)item.border.FindResource("TSystemAccentColor");
                    item.border.BorderThickness = new Thickness(2);
                }
                else
                {
                    item.border.BorderBrush = (Brush)item.border.FindResource("TCardBorderDefault");
                    item.border.BorderThickness = new Thickness(1);
                }
            }
        }

        private static void IAnimationModeropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is TLottieIconItem item && e.NewValue is TLottieIconAnimationMode animationMode)
            {
                if (animationMode == TLottieIconAnimationMode.Never || animationMode == TLottieIconAnimationMode.OnHover)
                    item.lottieIcon.AnimationMode = TLottieIconAnimationMode.Never;
                if (animationMode == TLottieIconAnimationMode.Always)
                    item.lottieIcon.AnimationMode = TLottieIconAnimationMode.Always;
            }
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            if (IAnimationMode == TLottieIconAnimationMode.OnHover)
                lottieIcon.PlayAnimation();
        }

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            clicked?.Invoke(this, e);
        }
    }
}
