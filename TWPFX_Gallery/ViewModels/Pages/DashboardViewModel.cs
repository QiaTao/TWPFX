


using TWPFX.Controls.Notification.InfoBar;

namespace TWPFX_Gallery.ViewModels.Pages
{
    public partial class DashboardViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _counter = 0;

        [RelayCommand]
        private void OnCounterIncrement()
        {
            Counter++;
            TInfoBar.Info("提示", "这是一条短提示", TInfoBarPosition.TOP, 10000);
            TInfoBar.Success("提示", "这是一条短提示", TInfoBarPosition.TOP_LEFT, 10000);
            TInfoBar.Warning("提示", "这是一条短提示", TInfoBarPosition.TOP_RIGHT, 10000);
            TInfoBar.Error("提示", "这是一条短提示", TInfoBarPosition.BOTTOM_RIGHT, 10000);

        }
    }
}
