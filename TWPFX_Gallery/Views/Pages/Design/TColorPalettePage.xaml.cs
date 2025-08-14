using TWPFX_Gallery.ViewModels.Pages.Design;
using Wpf.Ui.Abstractions.Controls;
using Wpf.Ui.Controls;

namespace TWPFX_Gallery.Views.Pages.Design
{
    /// <summary>
    /// TColorPalettePage.xaml 的交互逻辑
    /// </summary>
    public partial class TColorPalettePage : INavigableView<TColorPaletteViewModel>
    {

        public TColorPaletteViewModel ViewModel { get; }

        public TColorPalettePage(TColorPaletteViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
} 