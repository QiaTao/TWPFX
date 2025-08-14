using System.Windows.Media;
using TWPFX_Gallery.ViewModels.Pages;

namespace TWPFX_Gallery.ViewModels.Pages.Design
{
    public partial class TColorPaletteViewModel : ObservableObject
    {
        [ObservableProperty]
        public string _title = "Brand Colors";

        [ObservableProperty]
        public string _hexCode = "#409EFF";

        [ObservableProperty]
        public Brush _background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF409EFF"));

        [ObservableProperty]
        public IEnumerable<Brush> _colors=
        [
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE1F5FE")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB3E5FC")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF81D4FA")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4FC3F7")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF29B6F6")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF03A9F4")),
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0288D1"))
        ];
    }
} 