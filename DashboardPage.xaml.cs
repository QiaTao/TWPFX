using System.Diagnostics;
using System.Windows.Media;
using TWPFX.Animations;
using TWPFX.Controls.Button.SegoeButton;
using TWPFX.Controls.Icon.SegoeIcon;
using TWPFX.Controls.Notification.InfoBar;
using TWPFX_Gallery.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;
using Wpf.Ui.Controls;

namespace TWPFX_Gallery.Views.Pages
{
    public partial class DashboardPage : INavigableView<DashboardViewModel>
    {
        public DashboardViewModel ViewModel { get; }

        public DashboardPage(DashboardViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }

        private void button_Clicked(object sender, EventArgs e)
        {
        }
    }
}
