using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWPFX.Controls.Notification.InfoBar;
using TWPFX.Service;
using TWPFX_Gallery.Services;

namespace TWPFX_Gallery.ViewModels.Pages.BasicInput
{
    public partial class TButtonViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _tBaseButtonCode = "<TBaseButton\r\n" +
            "\tBackgroundColor=\"#21222d\"\r\n" +
            "\tBorderBrush=\"#eaeaea\"\r\n" +
            "\tHoverBackgroundColor=\"#373842\"\r\n" +
            "\tHoverBorderColor=\"#eaeaea\"\r\n" +
            "\tHoverIconColor=\"#eaeaeb\"\r\n" +
            "\tHoverTextColor=\"#eaeaeb\"\r\n" +
            "\tIcon=\"Settings\"\r\n" +
            "\tIconColor=\"#ffffff\"\r\n" +
            "\tPressedBackgroundColor=\"#73747b\"\r\n" +
            "\tPressedBorderColor=\"#eaeaea\"\r\n" +
            "\tPressedIconColor=\"#eaeaeb\"\r\n" +
            "\tPressedTextColor=\"#eaeaeb\"\r\n" +
            "\tText=\"设置\"\r\n" +
            "\tTextColor=\"#ffffff\" />";

        [ObservableProperty]
        private string _tButtonCode = "<TButton\r\n" +
            "\tAppearance=\"Default\"\r\n" +
            "\tButtonStyle=\"Solid\"\r\n" +
            "\tIcon=\"ReportDocument\"\r\n" +
            "\tText=\"Default\"/>\r\n\r\n" +
            "<TButton\r\n" +
            "\tAppearance=\"Default\"\r\n" +
            "\tButtonStyle=\"Outlined\"\r\n" +
            "\tIcon=\"ReportDocument\"\r\n" +
            "\tText=\"Default\"/>\r\n\r\n" +
            "<TButton\r\n" +
            "\tAppearance=\"Default\"\r\n" +
            "\tButtonStyle=\"Filled\"\r\n" +
            "\tIcon=\"ReportDocument\"\r\n" +
            "\tText=\"Default\"/>\r\n\r\n" +
            "<TButton\r\n" +
            "\tAppearance=\"Default\"\r\n" +
            "\tButtonStyle=\"Circle\"\r\n" +
            "\tIcon=\"Add\"\r\n" +
            "\tText=\"Default\"/>" 
            ;

        [ObservableProperty]
        private string _tLoadingButtonCode = "<TLoadingButton\r\n" +
            "\tBackgroundColor=\"#21222d\"\r\n" +
            "\tBorderBrush=\"#eaeaea\"\r\n" +
            "\tFontSize=\"14\"\r\n" +
            "\tFontWeight=\"Bold\"\r\n" +
            "\tIsLoading=\"True\"\r\n" +
            "\tLoadingRingColor=\"#ffffff\"\r\n" +
            "\tText=\"提交\"\r\n" +
            "\tTextColor=\"#ffffff\" />\r\n\r\n" +
            "<TLoadingButton\r\n" +
            "\tBackgroundColor=\"#6CCB5F\"\r\n" +
            "\tBorderBrush=\"#6CCB5F\"\r\n" +
            "\tFontSize=\"14\"\r\n" +
            "\tFontWeight=\"Bold\"\r\n" +
            "\tIsLoading=\"True\"\r\n" +
            "\tLoadingRingColor=\"#ffffff\"\r\n" +
            "\tText=\"成功\"\r\n" +
            "\tTextColor=\"#ffffff\" />";

        [RelayCommand]
        private void OnButtonClick()
        {
            TInfoBar.Info(MovieQuoteService.GetRandomMovieQuote(), duration:5000);
        }
    }
}
