using LottieSharp.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TWPFX.Controls.Icon.LottieIcon;
using TWPFX.Controls.Notification.InfoBar;
using TWPFX_Gallery.Controls.Icon;
using Wpf.Ui.Controls;

namespace TWPFX_Gallery.Views.Pages.Design
{

    /// <summary>
    /// TLottieIconography.xaml 的交互逻辑
    /// </summary>
    public partial class TLottieIconographyPage : Page, INotifyPropertyChanged
    {
        #region 数据源

        private TLottieIconStyle _pStyle = TLottieIconStyle.Regular;
        public TLottieIconStyle PStyle
        {
            get { return _pStyle; }
            set { _pStyle = value; OnPropertyChanged(nameof(PStyle)); }
        }

        private TLottieIconAnimationMode _pAnimationMode = TLottieIconAnimationMode.OnHover;
        public TLottieIconAnimationMode PAnimationMode
        {
            get { return _pAnimationMode; }
            set { _pAnimationMode = value; OnPropertyChanged(nameof(PAnimationMode)); }
        }

        private TLottieIconItem _selectedItem;
        public TLottieIconItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                UpdateCodeStrings();
            }
        }

        private string _xamlString;
        public string XamlString
        {
            get { return _xamlString; }
            set
            {
                _xamlString = value;
                OnPropertyChanged(nameof(XamlString));
            }
        }

        private string _cSharpString;
        public string CSharpString
        {
            get { return _cSharpString; }
            set
            {
                _cSharpString = value;
                OnPropertyChanged(nameof(CSharpString));
            }
        }

        #endregion

        public ObservableCollection<TLottieIconItem> AllItems { get; } = [];
        public ObservableCollection<string> SuggestItems { get; set; } = [];

        public TLottieIconographyPage()
        {
            InitializeComponent();
            DataContext = this;
            LoadItems();
        }

        private void LoadItems()
        {
            foreach (TLottieIconType type in Enum.GetValues(typeof(TLottieIconType)))
            {
                if (type == TLottieIconType.None) continue;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    TLottieIconItem item = new()
                    {
                        IType = type,
                        Margin = new Thickness(0, 0, 12, 12)
                    };
                    item.clicked += (s, e) =>
                    {
                        if (item != SelectedItem)
                        {
                            if (SelectedItem != null)
                                SelectedItem.IsSelected = false;
                            item.IsSelected = true;
                            SelectedItem = item;
                        }
                    };
                    wrapPanel.Children.Add(item);
                    AllItems.Add(item);
                    SuggestItems.Add(type.ToString());
                });
            }
            SelectedItem = AllItems[0];
            AllItems[0].IsSelected = true;
            suggestBox.OriginalItemsSource = SuggestItems;
            suggestBox.QuerySubmitted += SuggestBox_QuerySubmitted;
            suggestBox.SuggestionChosen += SuggestBox_SuggestionChosen;
        }

        private void SuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem == null) return;
            foreach (TLottieIconItem item in AllItems)
            {
                if (item.IType.ToString().Contains(args.SelectedItem.ToString()))
                {
                    item.Visibility = Visibility.Visible;
                }
                else {
                    item.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void SuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            foreach (TLottieIconItem item in AllItems)
            {
                if (item.IType.ToString().Contains(args.QueryText))
                {
                    item.Visibility = Visibility.Visible;
                }
                else
                {
                    item.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void UpdateCodeStrings()
        {
            if (SelectedItem != null)
            {
                XamlString = $"<LottieIcon\r\n    IconType=\"{SelectedItem.IType}\"\r\n    IconStyle=\"{PStyle}\"\r\n    AnimationMode=\"{PAnimationMode}\"/>";
                CSharpString = $"LottieIcon lottieIcon = new() {{ \r\n    IconType = LottieIconType.{SelectedItem.IType},\r\n    IconStyle = LottieIconStyle.{PStyle},\r\n    AnimationMode = LottieIconAnimationMode.{PAnimationMode}\r\n}};";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (propertyName == nameof(PStyle))
            {
                foreach (TLottieIconItem item in AllItems)
                {
                    item.IStyle = PStyle;
                }
                UpdateCodeStrings();
            }
            if (propertyName == nameof(PAnimationMode))
            {
                foreach (TLottieIconItem item in AllItems)
                {
                    item.IAnimationMode = PAnimationMode;
                }
                UpdateCodeStrings();
            }
        }
    }
}
