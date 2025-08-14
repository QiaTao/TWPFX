
using System.Windows.Controls;
using TWPFX.Controls.Notification.InfoBar;
using TWPFX.Service;
using TWPFX_Gallery.Services;
using TWPFX_Gallery.ViewModels.Pages;
using TWPFX_Gallery.Views.Windows;
using Wpf.Ui;
using Wpf.Ui.Abstractions.Controls;
using Wpf.Ui.Controls;
using Button = System.Windows.Controls.Button;


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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            switch (button.Content)
            {
                case "英文":
                    TLocalizationService.ChangeLanguage("en-US");
                    TInfoBar.Info("Info", "切换成功", duration: 400000);
                    TInfoBar.Info("切换成功", duration: 400000);
                    break;
                case "日文":
                    TLocalizationService.ChangeLanguage("ja-jp");
                    TInfoBar.Success("Success", "切换成功", duration: 400000);
                    TInfoBar.Success("切换成功", duration: 400000);
                    break;
                case "简体中文":
                    TLocalizationService.ChangeLanguage("zh-CN");
                    TInfoBar.Warning("Warning", "切换成功", duration: 400000);
                    TInfoBar.Warning("切换成功", duration: 400000);
                    break;
                case "繁体中文":
                    TLocalizationService.ChangeLanguage("zh-TW");
                    TInfoBar.Error("Error", "切换成功", duration: 400000);
                    TInfoBar.Error("切换成功", duration: 400000);
                    break;
                case "韩语":
                    TLocalizationService.ChangeLanguage("ko-KR");
                    TInfoBar.Clear();
                    break;
                case "弹窗":
                    TInfoBar.Info("Info", $"1111", duration:30000);
                    break;
            }
        }
    }
}
