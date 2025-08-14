using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TWPFX.Service;

namespace TWPFX_Gallery.Controls.Design
{
    public partial class TColorPaletteCardViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _selectedIndex = 4;

        [ObservableProperty]
        private Brush _selectedColor = TThemeService.GetBrush("TColorSystem400");

        [ObservableProperty]
        private string _title = "System Color";

        [ObservableProperty]
        private string _colorName = TThemeService.GetBrush("TColorSystem400").ToString();

        [ObservableProperty]
        private string _colorHex = TThemeService.GetBrush("TColorSystem400").ToString();

        public ObservableCollection<ColorItem> Colors { get; } = new();

        public TColorPaletteCardViewModel()
        {
            InitializeColors(ColorScheme.System);
        }

        public void InitializeColors(ColorScheme scheme)
        {
            Colors.Clear();
            var colorData = GetColorData(scheme);
            
            for (int i = 0; i < colorData.Length; i++)
            {
                Colors.Add(new ColorItem
                {
                    Index = i + 1,
                    Brush = colorData[i].Brush,
                    Name = colorData[i].Name,
                    Hex = colorData[i].Brush.ToString()
                });
            }

            Title = GetSchemeTitle(scheme);
            UpdateSelectedColor();
        }

        public void UpdateSelectedColor()
        {
            if (SelectedIndex > 0 && SelectedIndex <= Colors.Count)
            {
                var selectedItem = Colors[SelectedIndex - 1];
                SelectedColor = selectedItem.Brush;
                ColorName = selectedItem.Name;
                ColorHex = selectedItem.Hex;
            }
        }

        private static (Brush Brush, string Name)[] GetColorData(ColorScheme scheme)
        {
            return scheme switch
            {
                ColorScheme.System =>
                [
                    (TThemeService.GetBrush("TColorSystem100"), "TColorSystem100"),
                    (TThemeService.GetBrush("TColorSystem200"), "TColorSystem200"),
                    (TThemeService.GetBrush("TColorSystem300"), "TColorSystem300"),
                    (TThemeService.GetBrush("TColorSystem400"), "TColorSystem400"),
                    (TThemeService.GetBrush("TColorSystem500"), "TColorSystem500"),
                    (TThemeService.GetBrush("TColorSystem600"), "TColorSystem600"),
                    (TThemeService.GetBrush("TColorSystem700"), "TColorSystem700")
                ],
                ColorScheme.Primary =>
                [
                    (TThemeService.GetBrush("TColorPrimary100"), "TColorPrimary100"),
                    (TThemeService.GetBrush("TColorPrimary200"), "TColorPrimary200"),
                    (TThemeService.GetBrush("TColorPrimary300"), "TColorPrimary300"),
                    (TThemeService.GetBrush("TColorPrimary400"), "TColorPrimary400"),
                    (TThemeService.GetBrush("TColorPrimary500"), "TColorPrimary500"),
                    (TThemeService.GetBrush("TColorPrimary600"), "TColorPrimary600"),
                    (TThemeService.GetBrush("TColorPrimary700"), "TColorPrimary700")
                ],
                ColorScheme.Info =>
                [
                    (TThemeService.GetBrush("TColorInfo100"), "TColorInfo100"),
                    (TThemeService.GetBrush("TColorInfo200"), "TColorInfo200"),
                    (TThemeService.GetBrush("TColorInfo300"), "TColorInfo300"),
                    (TThemeService.GetBrush("TColorInfo400"), "TColorInfo400"),
                    (TThemeService.GetBrush("TColorInfo500"), "TColorInfo500"),
                    (TThemeService.GetBrush("TColorInfo600"), "TColorInfo600"),
                    (TThemeService.GetBrush("TColorInfo700"), "TColorInfo700")
                ],
                ColorScheme.Success =>
                [
                    (TThemeService.GetBrush("TColorSuccess100"), "TColorSuccess100"),
                    (TThemeService.GetBrush("TColorSuccess200"), "TColorSuccess200"),
                    (TThemeService.GetBrush("TColorSuccess300"), "TColorSuccess300"),
                    (TThemeService.GetBrush("TColorSuccess400"), "TColorSuccess400"),
                    (TThemeService.GetBrush("TColorSuccess500"), "TColorSuccess500"),
                    (TThemeService.GetBrush("TColorSuccess600"), "TColorSuccess600"),
                    (TThemeService.GetBrush("TColorSuccess700"), "TColorSuccess700")
                ],
                ColorScheme.Warning =>
                [
                    (TThemeService.GetBrush("TColorWarning100"), "TColorWarning100"),
                    (TThemeService.GetBrush("TColorWarning200"), "TColorWarning200"),
                    (TThemeService.GetBrush("TColorWarning300"), "TColorWarning300"),
                    (TThemeService.GetBrush("TColorWarning400"), "TColorWarning400"),
                    (TThemeService.GetBrush("TColorWarning500"), "TColorWarning500"),
                    (TThemeService.GetBrush("TColorWarning600"), "TColorWarning600"),
                    (TThemeService.GetBrush("TColorWarning700"), "TColorWarning700")
                ],
                ColorScheme.Danger =>
                [
                    (TThemeService.GetBrush("TColorDanger100"), "TColorDanger100"),
                    (TThemeService.GetBrush("TColorDanger200"), "TColorDanger200"),
                    (TThemeService.GetBrush("TColorDanger300"), "TColorDanger300"),
                    (TThemeService.GetBrush("TColorDanger400"), "TColorDanger400"),
                    (TThemeService.GetBrush("TColorDanger500"), "TColorDanger500"),
                    (TThemeService.GetBrush("TColorDanger600"), "TColorDanger600"),
                    (TThemeService.GetBrush("TColorDanger700"), "TColorDanger700")
                ],
                _ => throw new ArgumentException($"Unknown color scheme: {scheme}")
            };
        }

        private static string GetSchemeTitle(ColorScheme scheme)
        {
            return scheme switch
            {
                ColorScheme.System => "System Color",
                ColorScheme.Primary => "Primary Color",
                ColorScheme.Info => "Info Color",
                ColorScheme.Success => "Success Color",
                ColorScheme.Warning => "Warning Color",
                ColorScheme.Danger => "Danger Color",
                _ => "Unknown Color"
            };
        }
    }

    public partial class ColorItem : ObservableObject
    {
        [ObservableProperty]
        private int _index;

        [ObservableProperty]
        private Brush _brush;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _hex;
    }
}
