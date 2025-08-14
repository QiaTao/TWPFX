using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using TWPFX.Controls.Icon.LottieIcon;
using Wpf.Ui.Controls;
using TWPFX.Controls.Notification.Loading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace TWPFX_Gallery.Views.Pages.Design
{
    /// <summary>
    /// Lottie图标项的ViewModel，用于数据绑定
    /// </summary>
    public class LottieIconItemViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 图标类型
        /// </summary>
        private TLottieIconType _type;
        public TLottieIconType Type
        {
            get => _type;
            set
            {
                if (_type != value)
                {
                    _type = value;
                    OnPropertyChanged(nameof(Type));
                }
            }
        }
        
        /// <summary>
        /// 图标样式
        /// </summary>
        private TLottieIconStyle _style = TLottieIconStyle.Regular;
        public TLottieIconStyle Style
        {
            get => _style;
            set
            {
                if (_style != value)
                {
                    _style = value;
                    OnPropertyChanged(nameof(Style));
                }
            }
        }
        
        /// <summary>
        /// 动画模式
        /// </summary>
        private TLottieIconAnimationMode _animationMode = TLottieIconAnimationMode.OnHover;
        public TLottieIconAnimationMode AnimationMode
        {
            get => _animationMode;
            set
            {
                if (_animationMode != value)
                {
                    _animationMode = value;
                    OnPropertyChanged(nameof(AnimationMode));
                }
            }
        }
        
        /// <summary>
        /// 是否被选中
        /// </summary>
        private bool _isSelected = false;
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(nameof(IsSelected)); }
        }
        
        /// <summary>
        /// 属性变更事件
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
        
        /// <summary>
        /// 触发属性变更事件
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// TLottieIconPage.xaml 的交互逻辑
    /// 展示Lottie图标库，支持搜索、样式切换和动画模式设置
    /// </summary>
    public partial class TLottieIconPage : Page, INotifyPropertyChanged
    {
        #region 数据源

        /// <summary>
        /// 当前选中的图标样式
        /// </summary>
        private TLottieIconStyle _pStyle = TLottieIconStyle.Regular;
        public TLottieIconStyle PStyle
        {
            get { return _pStyle; }
            set { _pStyle = value; OnPropertyChanged(nameof(PStyle)); UpdateAllItemsStyle(); }
        }

        /// <summary>
        /// 当前选中的动画模式
        /// </summary>
        private TLottieIconAnimationMode _pAnimationMode = TLottieIconAnimationMode.OnHover;
        public TLottieIconAnimationMode PAnimationMode
        {
            get { return _pAnimationMode; }
            set { _pAnimationMode = value; OnPropertyChanged(nameof(PAnimationMode)); UpdateAllItemsAnimationMode(); }
        }

        /// <summary>
        /// 当前选中的图标项
        /// </summary>
        private LottieIconItemViewModel _selectedItem;
        public LottieIconItemViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                UpdateCodeStrings();
            }
        }

        /// <summary>
        /// XAML代码字符串
        /// </summary>
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

        /// <summary>
        /// C#代码字符串
        /// </summary>
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

        /// <summary>
        /// 搜索文本
        /// </summary>
        private string _searchText = "";
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(nameof(SearchText)); }
        }

        #endregion

        /// <summary>
        /// 所有图标项的集合
        /// </summary>
        public ObservableCollection<LottieIconItemViewModel> IconItems { get; } = [];
        
        /// <summary>
        /// 过滤后的图标项集合
        /// </summary>
        public ObservableCollection<LottieIconItemViewModel> FilteredIconItems { get; } = [];
        
        /// <summary>
        /// 过滤视图
        /// </summary>
        private ICollectionView _filteredView;

        /// <summary>
        /// 上一个选中的图标项
        /// </summary>
        private LottieIconItemViewModel _lastSelectedItem = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public TLottieIconPage()
        {
            InitializeComponent();
            DataContext = this;
            // 显示加载动画并开始数据加载
            TLoading.Show(LoadItemsAsync());
        }

        /// <summary>
        /// 异步加载图标数据
        /// </summary>
        private async Task LoadItemsAsync()
        {
            // 在后台线程准备数据
            var itemsToAdd = await Task.Run(() =>
            {
                var items = new List<LottieIconItemViewModel>();
                foreach (TLottieIconType type in Enum.GetValues(typeof(TLottieIconType)))
                {
                    if (type == TLottieIconType.None) continue;
                    var item = new LottieIconItemViewModel
                    {
                        Type = type,
                        Style = TLottieIconStyle.Regular, // Default style
                        AnimationMode = TLottieIconAnimationMode.OnHover // Default animation mode
                    };
                    items.Add(item);
                }
                return items;
            });

            // 在主线程分批更新UI，避免长时间阻塞
            const int batchSize = 50;
            for (int i = 0; i < itemsToAdd.Count; i += batchSize)
            {
                var batch = itemsToAdd.Skip(i).Take(batchSize);
                foreach (var item in batch)
                {
                    IconItems.Add(item);
                    FilteredIconItems.Add(item);
                }
                
                // 让UI有机会响应
                if (i + batchSize < itemsToAdd.Count)
                {
                    await Task.Delay(1);
                }
            }

            // 初始化CollectionView
            _filteredView = System.Windows.Data.CollectionViewSource.GetDefaultView(FilteredIconItems);

            if (IconItems.Count > 0)
            {
                SelectedItem = IconItems[0];
                IconItems[0].IsSelected = true;
                _lastSelectedItem = IconItems[0];

                // 等待UI渲染完成后再设置边框
                await Task.Delay(100);
                Dispatcher.BeginInvoke(() =>
                {
                    var border = FindBorderForItem(IconItems[0]);
                    if (border != null)
                    {
                        border.BorderBrush = (Brush)border.FindResource("TColorSystem400");
                        border.BorderThickness = new Thickness(2);
                    }
                });
            }
        }

        /// <summary>
        /// 更新所有图标项的样式
        /// </summary>
        private void UpdateAllItemsStyle()
        {
            foreach (var item in IconItems)
            {
                item.Style = PStyle;
            }
            if (SelectedItem != null)
            {
                SelectedItem.Style = PStyle;
            }
            UpdateCodeStrings();
        }

        /// <summary>
        /// 更新所有图标项的动画模式
        /// </summary>
        private void UpdateAllItemsAnimationMode()
        {
            foreach (var item in IconItems)
            {
                item.AnimationMode = PAnimationMode;
            }
            if (SelectedItem != null)
            {
                SelectedItem.AnimationMode = PAnimationMode;
            }
            UpdateCodeStrings();
        }

        /// <summary>
        /// 执行搜索功能
        /// </summary>
        /// <param name="query">搜索查询</param>
        private void PerformSearch(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                // 空搜索时清除过滤器，显示所有项
                _filteredView.Filter = null;
                return;
            }

            // 设置过滤器，只显示匹配项
            _filteredView.Filter = item =>
            {
                if (item is LottieIconItemViewModel vm)
                {
                    return vm.Type.ToString().Contains(query, StringComparison.OrdinalIgnoreCase);
                }
                return false;
            };

            // 选中第一个匹配项
            var firstMatch = _filteredView.Cast<LottieIconItemViewModel>().FirstOrDefault();
            if (firstMatch != null)
            {
                SelectItem(firstMatch);
            }
        }

        /// <summary>
        /// 选中指定的图标项
        /// </summary>
        /// <param name="match">要选中的图标项</param>
        private void SelectItem(LottieIconItemViewModel match)
        {
            if (match == null) return;

            // 只重置上一个选中项
            if (_lastSelectedItem != null && _lastSelectedItem != match)
            {
                _lastSelectedItem.IsSelected = false;
                // 使用Dispatcher确保UI已生成后再还原边框
                Dispatcher.BeginInvoke(() =>
                {
                    var previousBorder = FindBorderForItem(_lastSelectedItem);
                    if (previousBorder != null)
                    {
                        previousBorder.BorderBrush = (Brush)previousBorder.FindResource("TCardBorderDefault");
                        previousBorder.BorderThickness = new Thickness(1);
                    }
                });
            }

            match.IsSelected = true;
            // 确保选中项的样式和动画模式与当前设置一致
            match.Style = PStyle;
            match.AnimationMode = PAnimationMode;
            SelectedItem = match;
            _lastSelectedItem = match;

            // 设置Border样式
            Dispatcher.BeginInvoke(() =>
            {
                var currentBorder = FindBorderForItem(match);
                if (currentBorder != null)
                {
                    currentBorder.BorderBrush = (Brush)currentBorder.FindResource("TColorSystem400");
                    currentBorder.BorderThickness = new Thickness(2);
                }
            });
        }

        /// <summary>
        /// 更新代码字符串
        /// </summary>
        private void UpdateCodeStrings()
        {
            if (SelectedItem != null)
            {
                XamlString = $"<lottieicon:TLottieIcon\r\n    IconType=\"{SelectedItem.Type}\"\r\n    IconStyle=\"{SelectedItem.Style}\"\r\n    AnimationMode=\"{SelectedItem.AnimationMode}\"/>";
                CSharpString = $"var lottieIcon = new TLottieIcon {{ \r\n    IconType = TLottieIconType.{SelectedItem.Type},\r\n    IconStyle = TLottieIconStyle.{SelectedItem.Style},\r\n    AnimationMode = TLottieIconAnimationMode.{SelectedItem.AnimationMode}\r\n}};";
            }
        }

        /// <summary>
        /// 图标项鼠标点击事件处理
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">鼠标事件参数</param>
        private void IconItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border currentBorder && currentBorder.DataContext is LottieIconItemViewModel vm)
            {
                // 重置之前选中项的边框样式
                if (SelectedItem != null && SelectedItem != vm)
                {
                    var previousBorder = FindBorderForItem(SelectedItem);
                    if (previousBorder != null)
                    {
                        previousBorder.BorderBrush = (Brush)previousBorder.FindResource("TCardBorderDefault");
                        previousBorder.BorderThickness = new Thickness(1);
                    }
                }
                
                // 重置所有项的IsSelected状态
                foreach (var item in IconItems)
                {
                    item.IsSelected = false;
                }
                
                // 设置当前项为选中状态，并确保样式和动画模式正确
                vm.IsSelected = true;
                vm.Style = PStyle;
                vm.AnimationMode = PAnimationMode;
                SelectedItem = vm;
                
                // 设置当前Border的样式
                currentBorder.BorderBrush = (Brush)currentBorder.FindResource("TColorSystem400");
                currentBorder.BorderThickness = new Thickness(2);
            }
        }

        /// <summary>
        /// 查找指定图标项对应的Border控件
        /// </summary>
        /// <param name="item">图标项</param>
        /// <returns>对应的Border控件，如果未找到则返回null</returns>
        private Border FindBorderForItem(LottieIconItemViewModel item)
        {
            var itemsControl = iconItemsControl;
            if (itemsControl != null)
            {
                for (int i = 0; i < itemsControl.Items.Count; i++)
                {
                    var container = itemsControl.ItemContainerGenerator.ContainerFromIndex(i);
                    if (container is ContentPresenter presenter)
                    {
                        if (presenter.DataContext == item)
                        {
                            return presenter.ContentTemplate.FindName("border", presenter) as Border;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 搜索文本框按键事件处理
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">按键事件参数</param>
        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PerformSearch(SearchText);
            }
        }

        /// <summary>
        /// 搜索按钮点击事件处理
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">路由事件参数</param>
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            PerformSearch(SearchText);
        }

        /// <summary>
        /// 属性变更事件
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
        
        /// <summary>
        /// 触发属性变更事件
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
