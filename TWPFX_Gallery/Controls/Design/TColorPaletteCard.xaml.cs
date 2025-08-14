using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TWPFX.Animations;
using TWPFX_Gallery.Controls.Design;

namespace TWPFX_Gallery.Controls.Design
{
    public enum ColorScheme { 
        System,
        Primary,
        Info,
        Success,
        Warning,
        Danger
    }

    /// <summary>
    /// TColorPaletteCard.xaml 的交互逻辑
    /// </summary>
    public partial class TColorPaletteCard : UserControl
    {
        public static readonly DependencyProperty ColorSchemeProperty =
        DependencyProperty.Register(
            "ColourScheme",     // 属性名称
            typeof(ColorScheme),   // 属性类型
            typeof(TColorPaletteCard), // 所属类型
            new PropertyMetadata(ColorScheme.System, OnColourSchemeChanged)); // 默认值，添加回调方法

        public ColorScheme ColorScheme
        {
            get { return (ColorScheme)GetValue(ColorSchemeProperty); }
            set { SetValue(ColorSchemeProperty, value); }
        }

        TColorPaletteCardViewModel ViewModel { get; set; }

        public TColorPaletteCard()
        {
            ViewModel = new();
            DataContext = ViewModel;
            InitializeComponent();
        }

        private void ColorItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is ColorItem colorItem)
            {
                ViewModel.SelectedIndex = colorItem.Index;
                ViewModel.UpdateSelectedColor();
            }
        }

        private static void OnColourSchemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var card = (TColorPaletteCard)d;
            card.ViewModel.InitializeColors(card.ColorScheme);
        }
    }
}
