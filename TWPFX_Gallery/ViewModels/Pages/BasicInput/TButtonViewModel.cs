using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using TWPFX.Controls.Notification.InfoBar;
using TWPFX.Controls.Button.TButton;
using TWPFX.Service;
using TWPFX_Gallery.Services;

namespace TWPFX_Gallery.ViewModels.Pages.BasicInput
{
    public partial class TButtonViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _tBaseButtonCode = "<TBaseButton\r\n" +
            "\tBackgroundColor=\"#21222d\"\r\n" +
            "\tBorderColor=\"#eaeaea\"\r\n" +
            "\tCornerRadius=\"6\"\r\n" +
            "\tFontSize=\"14\"\r\n" +
            "\tFontWeight=\"Bold\"\r\n" +
            "\tHoverBackgroundColor=\"#373842\"\r\n" +
            "\tHoverBorderColor=\"#eaeaea\"\r\n" +
            "\tHoverTextColor=\"#eaeaeb\"\r\n" +
            "\tPressedBackgroundColor=\"#73747b\"\r\n" +
            "\tPressedBorderColor=\"#eaeaea\"\r\n" +
            "\tPressedTextColor=\"#eaeaeb\"\r\n" +
            "\tText=\"{DynamicResource TButton_TextButton}\"\r\n" +
            "\tTextColor=\"#ffffff\" />";

        [ObservableProperty]
        private string _tThemeButtonCode = "<TThemeButton\r\n" +
            "\tAppearance=\"Default\"\r\n" +
            "\tButtonStyle=\"Solid\"\r\n" +
            "\tText=\"{DynamicResource TButton_TextDefault}\"/>";

        [ObservableProperty]
        private string _tIconButtonCode = "<TIconButton\r\n" +
            "\tAppearance=\"Default\"\r\n" +
            "\tButtonStyle=\"Solid\"\r\n" +
            "\tBorderPadding=\"18,0\"\r\n" +
            "\tIcon=\"HeartFill\"\r\n" +
            "\tText=\"{DynamicResource TButton_TextDefault}\"/>";

        [ObservableProperty]
        private string _tLoadingButtonCode = "<TLoadingButton\r\n" +
            "\tAppearance=\"Default\"\r\n" +
            "\tButtonStyle=\"Solid\"\r\n" +
            "\tText=\"{DynamicResource TButton_TextGetLicence}\"\r\n" +
            "\tWidthAutoAdaptation=\"True\"\r\n" +
            "\tClick=\"TLoadingButton_Click\"/>";

        [RelayCommand]
        private void OnButtonClick()
        {
            TInfoBar.Info(MovieQuoteService.GetRandomMovieQuote(), duration: 5000);
        }
    }
}
