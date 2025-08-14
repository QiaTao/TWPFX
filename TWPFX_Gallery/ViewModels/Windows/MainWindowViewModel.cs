using System.Collections.ObjectModel;
using TWPFX_Gallery.Views.Pages.Design;
using TWPFX_Gallery.Views.Pages.BasicInput;
using Wpf.Ui.Controls;

namespace TWPFX_Gallery.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _applicationTitle = "TWPFX_Gallery";

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Home",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(Views.Pages.DashboardPage)
            },
            new NavigationViewItem()
            {
                Content = "Design guidance",
                Icon = new SymbolIcon { Symbol = SymbolRegular.DesignIdeas24 },
                MenuItemsSource = new object[]
                {
                    new NavigationViewItem("TLottieIcon", typeof(TLottieIconPage)),
                    new NavigationViewItem("TSegoeIcon", typeof(TSegoeIconPage)),
                    new NavigationViewItem("TColorPalette", typeof(TColorPalettePage)),
                }
            },
            new NavigationViewItemSeparator(),
            new NavigationViewItem()
            {
                Content = "Basic Input",
                Icon = new SymbolIcon { Symbol = SymbolRegular.CheckboxChecked24 },
                MenuItemsSource = new object[]
                {
                    new NavigationViewItem("TButton", typeof(TButtonPage)),
                    new NavigationViewItem("TRotateAnimation", typeof(TRotateAnimationPage)),
                }
            },
            new NavigationViewItem("TControlExampleExpander", SymbolRegular.Button20, typeof(TControlExampleExpanderPage)),
            new NavigationViewItem()
            {
                Content = "Data",
                Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
                TargetPageType = typeof(Views.Pages.DataPage)
            },
             new NavigationViewItem()
            {
                Content = "TSegoeIcon",
                Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
                TargetPageType = typeof(Views.Pages.Design.TSegoeIconPage)
            },
        };

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Settings",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.Pages.SettingsPage)
            }
        };

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new()
        {
            new MenuItem { Header = "Home", Tag = "tray_home" }
        };
    }
}
