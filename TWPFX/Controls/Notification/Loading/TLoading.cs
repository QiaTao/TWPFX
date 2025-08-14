using System.Windows;
using System.Threading.Tasks;
using TWPFX.Controls.Overlay;
using TWPFX.Controls.Progress;
using TWPFX.Service;

namespace TWPFX.Controls.Notification.Loading
{
    public static class TLoading
    {
        private static TProgressRing? _progressRing;
        private static bool _canClose;

        public static void Show(bool canClose = false)
        {
            if (_progressRing != null) return;
            _canClose = canClose;
            _progressRing = new TProgressRing
            {
                Width = 64,
                Height = 64,
                IsIndeterminate = true,
                StrokeColor = TThemeService.GetBrush("TColorSystem400")
            };
            if (canClose)
            {
                _progressRing.MouseLeftButtonUp += ProgressRing_MouseLeftButtonUp;
            }
            TMaskService.ShowMask(_progressRing);
        }

        public static async void Show(Task task, bool canClose = false)
        {
            Show(canClose);
            try
            {
                await task;
            }
            finally
            {
                Hide();
            }
        }

        public static void Hide()
        {
            TMaskService.HideMask();
            if (_progressRing != null && _canClose)
            {
                _progressRing.MouseLeftButtonUp -= ProgressRing_MouseLeftButtonUp;
            }
            _progressRing = null;
        }

        private static void ProgressRing_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Hide();
        }
    }
} 