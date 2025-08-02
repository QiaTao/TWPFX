using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using TWPFX.Controls.Icon.SegoeIcon;
using TWPFX.Controls.Notification.Loading;
using Wpf.Ui.Controls;
using System.Collections.Generic; // Added for List

namespace TWPFX_Gallery.Views.Pages.Design
{
    /// <summary>
    /// Segoe图标项的ViewModel，用于数据绑定
    /// </summary>
    public class SegoeIconItemViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 图标字形类型
        /// </summary>
        public TSegoeIconType Glyph { get; set; }
        
        /// <summary>
        /// 图标名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 图标描述
        /// </summary>
        public string Description { get; set; }
        
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
    /// TSegoeIconPage.xaml 的交互逻辑
    /// 展示Segoe图标库，支持搜索和图标选择
    /// </summary>
    public partial class TSegoeIconPage : Page, INotifyPropertyChanged
    {
        /// <summary>
        /// 所有图标项的集合
        /// </summary>
        public ObservableCollection<SegoeIconItemViewModel> IconItems { get; } = [];
        
        /// <summary>
        /// 过滤后的图标项集合
        /// </summary>
        public ObservableCollection<SegoeIconItemViewModel> FilteredIconItems { get; } = [];
        
        /// <summary>
        /// 过滤视图
        /// </summary>
        private ICollectionView _filteredView;

        /// <summary>
        /// 当前选中的图标项
        /// </summary>
        private SegoeIconItemViewModel _selectedItem;
        public SegoeIconItemViewModel SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; OnPropertyChanged(nameof(SelectedItem)); UpdateCodeStrings(); }
        }

        /// <summary>
        /// XAML代码字符串
        /// </summary>
        private string _xamlString;
        public string XamlString
        {
            get => _xamlString;
            set { _xamlString = value; OnPropertyChanged(nameof(XamlString)); }
        }

        /// <summary>
        /// C#代码字符串
        /// </summary>
        private string _cSharpString;
        public string CSharpString
        {
            get => _cSharpString;
            set { _cSharpString = value; OnPropertyChanged(nameof(CSharpString)); }
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

        /// <summary>
        /// 上一个选中的图标项
        /// </summary>
        private SegoeIconItemViewModel _lastSelectedItem = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public TSegoeIconPage()
        {
            InitializeComponent();
            DataContext = this;
            
            // 立即显示加载动画，然后开始数据加载
            Dispatcher.BeginInvoke(async () =>
            {
                TLoading.Show(LoadItemsAsync());
            });
        }

        /// <summary>
        /// 异步加载图标数据
        /// </summary>
        private async Task LoadItemsAsync()
        {
            // 在后台线程准备数据
            var itemsToAdd = await Task.Run(() =>
            {
                var items = new List<SegoeIconItemViewModel>();
                foreach (TSegoeIconType type in Enum.GetValues(typeof(TSegoeIconType)))
                {
                    if (type == TSegoeIconType.None) continue;
                    var desc = type.GetDescription();
                    var item = new SegoeIconItemViewModel
                    {
                        Glyph = type,
                        Name = type.ToString(),
                        Description = desc
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
                        border.BorderBrush = (System.Windows.Media.Brush)border.FindResource("TColorSystem400");
                        border.BorderThickness = new System.Windows.Thickness(2);
                    }
                });
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
        /// 搜索文本框按下回车键事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">键盘事件参数</param>
        private void SearchTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                PerformSearch(SearchText);
            }
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
                if (item is SegoeIconItemViewModel vm)
                {
                    return vm.Name.Contains(query, StringComparison.OrdinalIgnoreCase);
                }
                return false;
            };

            // 选中第一个匹配项
            var firstMatch = _filteredView.Cast<SegoeIconItemViewModel>().FirstOrDefault();
            if (firstMatch != null)
            {
                SelectItem(firstMatch);
            }
        }

        /// <summary>
        /// 选中指定的图标项
        /// </summary>
        /// <param name="match">要选中的图标项</param>
        private void SelectItem(SegoeIconItemViewModel match)
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
                        previousBorder.BorderBrush = (System.Windows.Media.Brush)previousBorder.FindResource("TColorSystem400");
                        previousBorder.BorderThickness = new System.Windows.Thickness(1);
                    }
                });
            }

            match.IsSelected = true;
            SelectedItem = match;
            _lastSelectedItem = match;

            // 设置Border样式
            Dispatcher.BeginInvoke(() =>
            {
                var currentBorder = FindBorderForItem(match);
                if (currentBorder != null)
                {
                    currentBorder.BorderBrush = (System.Windows.Media.Brush)currentBorder.FindResource("TColorSystem400");
                    currentBorder.BorderThickness = new System.Windows.Thickness(2);
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
                XamlString = $"<TSegoeIcon\n    Glyph=\"{SelectedItem.Name}\"\n    GlyphSize=\"32\"/>";
                CSharpString = $"TSegoeIcon icon = new(){{\n    Glyph = TSegoeIconType.{SelectedItem.Name},\n    GlyphSize = 32\n}};";
            }
        }

        /// <summary>
        /// 图标项鼠标点击事件处理
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">鼠标事件参数</param>
        private void IconItem_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Border currentBorder && currentBorder.DataContext is SegoeIconItemViewModel vm)
            {
                // 重置之前选中项的边框样式
                if (SelectedItem != null && SelectedItem != vm)
                {
                    var previousBorder = FindBorderForItem(SelectedItem);
                    if (previousBorder != null)
                    {
                        previousBorder.BorderBrush = (System.Windows.Media.Brush)previousBorder.FindResource("TCardBorderDefault");
                        previousBorder.BorderThickness = new System.Windows.Thickness(1);
                    }
                }
                
                // 重置所有项的IsSelected状态
                foreach (var item in IconItems)
                {
                    item.IsSelected = false;
                }
                
                // 设置当前项为选中状态
                vm.IsSelected = true;
                SelectedItem = vm;
                
                // 设置当前Border的样式
                currentBorder.BorderBrush = (System.Windows.Media.Brush)currentBorder.FindResource("TColorSystem400");
                currentBorder.BorderThickness = new System.Windows.Thickness(2);
            }
        }

        /// <summary>
        /// 查找指定图标项对应的Border控件
        /// </summary>
        /// <param name="item">图标项</param>
        /// <returns>对应的Border控件，如果未找到则返回null</returns>
        private Border FindBorderForItem(SegoeIconItemViewModel item)
        {
            var itemsControl = iconItemsControl;
            if (itemsControl != null)
            {
                for (int i = 0; i < itemsControl.Items.Count; i++)
                {
                    var container = itemsControl.ItemContainerGenerator.ContainerFromIndex(i);
                    if (container is System.Windows.Controls.ContentPresenter presenter)
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
        /// 属性变更事件
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
        
        /// <summary>
        /// 触发属性变更事件
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
} 