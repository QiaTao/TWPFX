using System.Windows;
using TWPFX_Gallery.ViewModels.Pages.BasicInput;
using Wpf.Ui.Abstractions.Controls;

namespace TWPFX_Gallery.Views.Pages.BasicInput
{
    /// <summary>
    /// TButtonPage.xaml 的交互逻辑
    /// </summary>
    public partial class TButtonPage : INavigableView<TButtonViewModel>
    {
        public TButtonViewModel ViewModel { get; }

        public TButtonPage(TButtonViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

    }
} 