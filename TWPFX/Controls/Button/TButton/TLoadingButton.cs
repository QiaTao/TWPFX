using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Threading;
using TWPFX.Controls.Progress;
using TWPFX.Service;

namespace TWPFX.Controls.Button.TButton
{
    /// <summary>
    /// 加载按钮控件，继承自TThemeButton，左侧放置loadingRing控件
    /// </summary>
    public class TLoadingButton : TThemeButton
    {
        #region 依赖属性

        // 加载状态属性
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(TLoadingButton),
                new PropertyMetadata(false, OnLoadingPropertyChanged));

        public static readonly DependencyProperty LoadingRingSizeProperty =
            DependencyProperty.Register(nameof(LoadingRingSize), typeof(double), typeof(TLoadingButton),
                new PropertyMetadata(16.0, OnContentPropertyChanged));

        public static readonly DependencyProperty LoadingRingStrokeThicknessProperty =
            DependencyProperty.Register(nameof(LoadingRingStrokeThickness), typeof(double), typeof(TLoadingButton),
                new PropertyMetadata(2.0, OnContentPropertyChanged));

        public static readonly DependencyProperty LoadingRingVisibilityProperty =
            DependencyProperty.Register(nameof(LoadingRingVisibility), typeof(Visibility), typeof(TLoadingButton),
                new PropertyMetadata(Visibility.Collapsed, OnContentPropertyChanged));

        public static readonly DependencyProperty LoadingRingColorProperty =
            DependencyProperty.Register(nameof(LoadingRingColor), typeof(Brush), typeof(TLoadingButton),
                new PropertyMetadata(Brushes.White, OnStylePropertyChanged));

        public static readonly DependencyProperty HoverLoadingRingColorProperty =
            DependencyProperty.Register(nameof(HoverLoadingRingColor), typeof(Brush), typeof(TLoadingButton),
                new PropertyMetadata(Brushes.White, OnStylePropertyChanged));

        public static readonly DependencyProperty PressedLoadingRingColorProperty =
            DependencyProperty.Register(nameof(PressedLoadingRingColor), typeof(Brush), typeof(TLoadingButton),
                new PropertyMetadata(Brushes.White, OnStylePropertyChanged));

        #endregion

        #region CLR属性包装器

        // 加载状态属性
        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, value);
        }

        public double LoadingRingSize
        {
            get => (double)GetValue(LoadingRingSizeProperty);
            set => SetValue(LoadingRingSizeProperty, value);
        }

        public double LoadingRingStrokeThickness
        {
            get => (double)GetValue(LoadingRingStrokeThicknessProperty);
            set => SetValue(LoadingRingStrokeThicknessProperty, value);
        }

        public Visibility LoadingRingVisibility
        {
            get => (Visibility)GetValue(LoadingRingVisibilityProperty);
            set => SetValue(LoadingRingVisibilityProperty, value);
        }

        public Brush LoadingRingColor
        {
            get => (Brush)GetValue(LoadingRingColorProperty);
            set => SetValue(LoadingRingColorProperty, value);
        }

        public Brush HoverLoadingRingColor
        {
            get => (Brush)GetValue(HoverLoadingRingColorProperty);
            set => SetValue(HoverLoadingRingColorProperty, value);
        }

        public Brush PressedLoadingRingColor
        {
            get => (Brush)GetValue(PressedLoadingRingColorProperty);
            set => SetValue(PressedLoadingRingColorProperty, value);
        }

        #endregion

        #region 私有字段

        private CancellationTokenSource _currentTaskCancellationTokenSource;
        private bool _isManualCancellation = false;

        #endregion

        public TLoadingButton()
        {
            InitializeLoadingButton();
        }

        protected virtual void InitializeLoadingButton()
        {
            // 应用基础样式
            ApplyBaseStyle();
            ApplyLayout();
            
            // 初始化完成后更新内容
            UpdateContent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            // 模板应用后更新内容
            UpdateContent();
        }

        #region 任务执行方法

        /// <summary>
        /// 执行耗时后台任务
        /// </summary>
        /// <typeparam name="T">任务返回结果的类型</typeparam>
        /// <param name="taskFunc">要执行的任务函数</param>
        /// <param name="timeout">超时时间（毫秒），默认30秒，0表示无超时</param>
        /// <param name="onCompleted">任务完成时的回调函数</param>
        /// <param name="onTimeout">任务超时时的回调函数</param>
        /// <param name="onError">任务出错时的回调函数</param>
        /// <returns>任务执行结果</returns>
        public async Task<T> ExecuteTaskAsync<T>(
            Func<CancellationToken, Task<T>> taskFunc,
            int timeout = 30000,
            Action<T> onCompleted = null,
            Action onTimeout = null,
            Action<Exception> onError = null)
        {
            // 取消之前的任务（如果存在）
            CancelCurrentTask();

            // 创建新的取消令牌
            _currentTaskCancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _currentTaskCancellationTokenSource.Token;
            _isManualCancellation = false; // 重置手动取消标志

            // 设置超时
            if (timeout > 0)
            {
                _currentTaskCancellationTokenSource.CancelAfter(timeout);
            }

            try
            {
                // 设置加载状态
                await Dispatcher.InvokeAsync(() => IsLoading = true);

                // 执行任务
                var result = await taskFunc(cancellationToken);

                // 任务成功完成
                await Dispatcher.InvokeAsync(() =>
                {
                    IsLoading = false;
                    onCompleted?.Invoke(result);
                });

                return result;
            }
            catch (OperationCanceledException)
            {
                // 任务被取消（超时或手动取消）
                await Dispatcher.InvokeAsync(() =>
                {
                    IsLoading = false;
                    // 使用手动取消标志来区分
                    if (_isManualCancellation)
                    {
                        onError?.Invoke(new OperationCanceledException());
                    }
                    else
                    {
                        onTimeout?.Invoke();
                    }
                });
                throw;
            }
            catch (Exception ex)
            {
                // 任务执行出错
                await Dispatcher.InvokeAsync(() =>
                {
                    IsLoading = false;
                    onError?.Invoke(ex);
                });
                throw;
            }
            finally
            {
                // 清理取消令牌
                _currentTaskCancellationTokenSource?.Dispose();
                _currentTaskCancellationTokenSource = null;
            }
        }

        /// <summary>
        /// 执行无返回值的耗时后台任务
        /// </summary>
        /// <param name="taskFunc">要执行的任务函数</param>
        /// <param name="timeout">超时时间（毫秒），默认30秒，0表示无超时</param>
        /// <param name="onCompleted">任务完成时的回调函数</param>
        /// <param name="onTimeout">任务超时时的回调函数</param>
        /// <param name="onError">任务出错时的回调函数</param>
        public async Task ExecuteTaskAsync(
            Func<CancellationToken, Task> taskFunc,
            int timeout = 30000,
            Action onCompleted = null,
            Action onTimeout = null,
            Action<Exception> onError = null)
        {
            await ExecuteTaskAsync(async (cancellationToken) =>
            {
                await taskFunc(cancellationToken);
                return true; // 返回默认值
            }, timeout, onCompleted != null ? (result) => onCompleted() : null, onTimeout, onError);
        }

        /// <summary>
        /// 取消当前正在执行的任务
        /// </summary>
        public void CancelCurrentTask()
        {
            if (_currentTaskCancellationTokenSource != null && !_currentTaskCancellationTokenSource.Token.IsCancellationRequested)
            {
                _isManualCancellation = true;
                _currentTaskCancellationTokenSource.Cancel();
            }
        }

        /// <summary>
        /// 检查是否有任务正在执行
        /// </summary>
        public bool IsTaskRunning => _currentTaskCancellationTokenSource != null && !_currentTaskCancellationTokenSource.Token.IsCancellationRequested;

        #endregion

        #region 属性变更回调

        private static void OnStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TLoadingButton button)
            {
                button.ApplyBaseStyle();
            }
        }

        private static void OnLayoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TLoadingButton button)
            {
                button.ApplyLayout();
                button.UpdateContent(); // 布局变化时也要更新内容
            }
        }

        private static void OnContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TLoadingButton button)
            {
                button.UpdateContent();
            }
        }

        private static void OnLoadingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TLoadingButton button)
            {
                button.UpdateLoadingState();
            }
        }

        #endregion

        protected override void UpdateContent()
        {
            base.UpdateContent();

            if (IsLoading)
            {
                double width = Width > 0 ? Width : ButtonWidth;
                double height = Height > 0 ? Height : ButtonHeight;
                double minSize = Math.Min(width, height);

                LoadingRingColor = TextColor;

                // 创建加载环元素并设置为Content
                var loadingRing = new TProgressRing
                {
                    Height = minSize / 2,
                    Width = minSize / 2,
                    Radius = minSize / 5,
                    StrokeThickness = LoadingRingStrokeThickness,
                    StrokeColor = LoadingRingColor,
                    IsIndeterminate = true,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                Content = loadingRing;
            }
            else
            {
                Content = null;
            }
        }

        protected virtual void UpdateLoadingState()
        {
            // 更新加载环可见性
            LoadingRingVisibility = IsLoading ? Visibility.Visible : Visibility.Collapsed;

            // 更新按钮状态
            IsEnabled = !IsLoading;

            // 更新内容（包括间距）
            UpdateContent();
        }
    }
} 