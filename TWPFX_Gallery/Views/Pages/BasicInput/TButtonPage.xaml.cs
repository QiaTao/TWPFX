using System.Windows;
using TWPFX_Gallery.ViewModels.Pages.BasicInput;
using Wpf.Ui.Abstractions.Controls;
using System.Threading.Tasks;
using System.Threading;
using System;
using TWPFX.Controls.Button.TButton;
using System.Diagnostics;
using System.Text;
using TWPFX.Controls.Notification.InfoBar;

namespace TWPFX_Gallery.Views.Pages.BasicInput
{
    /// <summary>
    /// TButtonPage.xaml 的交互逻辑
    /// </summary>
    public partial class TButtonPage : INavigableView<TButtonViewModel>
    {
        public TButtonViewModel ViewModel { get; }

        public TButtonPage(TButtonViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void TLoadingButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is TLoadingButton loadingButton)
            {
                string tag = loadingButton.Tag?.ToString();
                
                switch (tag)
                {
                    case "normal":
                        _ = loadingButton.ExecuteTaskAsync<string>(
                        async (cancellationToken) =>
                        {
                            // 模拟耗时操作
                            await Task.Delay(3000, cancellationToken);
                            return GenerateRandomString();
                        },
                        timeout: 5000, // 5秒超时
                        onCompleted: (result) =>
                        {
                            TInfoBar.Success($"{result}", duration: 5000);
                        },
                        onTimeout: () =>
                        {
                            TInfoBar.Warning(Application.Current.Resources["TButton_TextGetLicenceResultTimeout"]?.ToString() ?? "Timed out", duration: 5000);
                        },
                        onError: (ex) =>
                        {
                            TInfoBar.Error($"{ex.Message}", duration: 5000);
                        });
                        break;
                    case "timeout":
                        _ = loadingButton.ExecuteTaskAsync<string>(
                        async (cancellationToken) =>
                        {
                            // 模拟耗时操作，会超时
                            await Task.Delay(8000, cancellationToken);
                            return GenerateRandomString();
                        },
                        timeout: 3000, // 3秒超时
                        onCompleted: (result) =>
                        {
                            TInfoBar.Success($"{result}", duration: 5000);
                        },
                        onTimeout: () =>
                        {
                            TInfoBar.Warning(Application.Current.Resources["TButton_TextGetLicenceResultTimeout"]?.ToString() ?? "Timed out", duration: 5000);
                        },
                        onError: (ex) =>
                        {
                            if (ex is OperationCanceledException)
                            {
                                TInfoBar.Info(Application.Current.Resources["TButton_TextGetLicenceResultCancel"]?.ToString() ?? "Task has been manually cancelled", duration: 5000);
                            }
                            else
                            {
                                TInfoBar.Error($"{ex.Message}", duration: 5000);
                            }
                        });
                         break;
                     case "exception":
                         _ = loadingButton.ExecuteTaskAsync<string>(
                         async (cancellationToken) =>
                         {
                             // 模拟耗时操作
                             await Task.Delay(2000, cancellationToken);
                             // 模拟异常情况
                             throw new InvalidOperationException(Application.Current.Resources["TButton_TextGetLicenceResultNoPermissions"]?.ToString() ?? "Current user has no permissions");
                         },
                         timeout: 5000, // 5秒超时
                         onCompleted: (result) =>
                         {
                             TInfoBar.Success($"{result}", duration: 5000);
                         },
                         onTimeout: () =>
                         {
                             TInfoBar.Warning(Application.Current.Resources["TButton_TextGetLicenceResultTimeout"]?.ToString() ?? "Timed out", duration: 5000);
                         },
                         onError: (ex) =>
                         {
                             if (ex is OperationCanceledException)
                             {
                                 TInfoBar.Info(Application.Current.Resources["TButton_TextGetLicenceResultCancel"]?.ToString() ?? "Task has been manually cancelled", duration: 5000);
                             }
                             else
                             {
                                 TInfoBar.Error($"{ex.Message}", duration: 5000);
                             }
                         });
                          break;
                      case "cancel":
                          _ = loadingButton.ExecuteTaskAsync<string>(
                          async (cancellationToken) =>
                          {
                              // 模拟长时间操作
                              for (int i = 0; i < 10; i++)
                              {
                                  cancellationToken.ThrowIfCancellationRequested();
                                  await Task.Delay(1000, cancellationToken); // 每秒检查一次
                              }
                              return GenerateRandomString();
                          },
                          timeout: 15000, // 15秒超时
                          onCompleted: (result) =>
                          {
                              TInfoBar.Success($"{result}", duration: 5000);
                          },
                          onTimeout: () =>
                          {
                              TInfoBar.Warning(Application.Current.Resources["TButton_TextGetLicenceResultTimeout"]?.ToString() ?? "Timed out", duration: 5000);
                          },
                          onError: (ex) =>
                          {
                              if (ex is OperationCanceledException)
                              {
                                  TInfoBar.Info(Application.Current.Resources["TButton_TextGetLicenceResultCancel"]?.ToString() ?? "Task has been manually cancelled", duration: 5000);
                              }
                              else
                              {
                                  TInfoBar.Error($"{ex.Message}", duration: 5000);
                              }
                          });

                          // 3秒后手动取消任务
                          _ = Task.Delay(3000).ContinueWith(_ =>
                          {
                              loadingButton.Dispatcher.InvokeAsync(() =>
                              {
                                  loadingButton.CancelCurrentTask();
                              });
                          });
                          break;
                  }
            }
        }

        public static string GenerateRandomString()
        {
            string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringBuilder = new StringBuilder(20); // 预先指定容量，提高性能
            for (int i = 0; i < 20; i++)
            {
                // 从字符集中随机取一个字符
                int randomIndex = new Random().Next(Chars.Length);
                stringBuilder.Append(Chars[randomIndex]);
            }
            return stringBuilder.ToString();
        }

        private void TLoadingButton_NoResult_Click(object sender, RoutedEventArgs e)
        {
            if (sender is TLoadingButton loadingButton)
            {
                // 示例2：执行无返回值的任务
                _ = loadingButton.ExecuteTaskAsync(
                    async (cancellationToken) =>
                    {
                        // 模拟耗时操作
                        await Task.Delay(2000, cancellationToken);
                        // 可以在这里执行一些操作，比如保存文件、发送请求等
                    },
                    timeout: 10000, // 10秒超时
                    onCompleted: () =>
                    {
                        MessageBox.Show("无返回值任务执行完成！", "成功");
                    },
                    onTimeout: () =>
                    {
                        MessageBox.Show("无返回值任务超时！", "超时");
                    },
                    onError: (ex) =>
                    {
                        MessageBox.Show($"无返回值任务出错：{ex.Message}", "错误");
                    });
            }
        }

        private void TLoadingButton_Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (sender is TLoadingButton loadingButton)
            {
                // 示例3：演示如何取消任务
                _ = loadingButton.ExecuteTaskAsync(
                    async (cancellationToken) =>
                    {
                        // 模拟长时间运行的任务
                        for (int i = 0; i < 100; i++)
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            await Task.Delay(100, cancellationToken);
                        }
                    },
                    timeout: 30000, // 30秒超时
                    onCompleted: () =>
                    {
                        MessageBox.Show("长时间任务完成！", "成功");
                    },
                    onTimeout: () =>
                    {
                        MessageBox.Show("长时间任务超时！", "超时");
                    },
                    onError: (ex) =>
                    {
                        if (ex is OperationCanceledException)
                        {
                            MessageBox.Show("任务被取消！", "取消");
                        }
                        else
                        {
                            MessageBox.Show($"任务出错：{ex.Message}", "错误");
                        }
                    });

                // 3秒后取消任务
                Task.Delay(3000).ContinueWith(_ =>
                {
                    loadingButton.Dispatcher.Invoke(() =>
                    {
                        loadingButton.CancelCurrentTask();
                    });
                });
            }
        }
    }
} 