using System.Diagnostics;
using System.Windows.Media;
using TWPFX.Animations;
using TWPFX.Controls.Button.SegoeButton;
using TWPFX.Controls.Icon.SegoeIcon;
using TWPFX.Controls.Notification.InfoBar;
using TWPFX_Gallery.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;
using Wpf.Ui.Controls;

namespace TWPFX_Gallery.Views.Pages
{
    public partial class DashboardPage : INavigableView<DashboardViewModel>
    {
        public DashboardViewModel ViewModel { get; }

        public DashboardPage(DashboardViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }

        private void button_Clicked(object sender, EventArgs e)
        {
            //button.CreateAnimationSequence()
            //        // 第一步：缩小图标 (20 -> 1)
            //        .AddStep(
            //            targetProperty: "GlyphSize",
            //            from: 20,
            //            to: 1,
            //            durationMs: 300
            //        )
            //        // 第二步：放大图标并切换为勾选图标 (当前值 -> 16)
            //        .AddStep(
            //            targetProperty: "GlyphSize",
            //            from: null, // 自动使用当前值
            //            to: 16,
            //            durationMs: 300,
            //            preAction: () =>
            //            {
            //                button.Glyph = TSegoeIconType.CheckMark; 
            //            }
            //        )
            //        // 第三步：延迟2秒后缩小图标 (当前值 -> 1)
            //        .AddStep(
            //            targetProperty: "GlyphSize",
            //            from: null,
            //            to: 1,
            //            durationMs: 100,
            //            delayMs: 2000 // 关键延迟控制
            //        )
            //        // 第四步：恢复图标大小并切换为复制图标 (当前值 -> 16)
            //        .AddStep(
            //            targetProperty: "GlyphSize",
            //            from: null,
            //            to: 16,
            //            durationMs: 100,
            //            preAction: () =>
            //            {
            //                button.Glyph = TSegoeIconType.Copy; // 最终图标切换
            //            }
            //        )
            //        .Run(); // 启动动画序列
            button.CreateAnimationSequence()
       // 第一步：缩小图标 (20 -> 1)
       .AddStep(
           targetProperty: "GlyphSize",
           from: 20,
           to: 1,
           durationMs: 300,
           preAction: () => Debug.WriteLine("开始缩小动画"),
           postAction: () => Debug.WriteLine("缩小完成")
       )
       // 第二步：放大图标并切换为勾选图标 (当前值 -> 16)
       .AddStep(
           targetProperty: "GlyphSize",
           from: null, // 自动使用当前值
           to: 16,
           durationMs: 300,
           preAction: () =>
           {
               button.Glyph = TSegoeIconType.CheckMark; // 动画前切换图标
               Debug.WriteLine("图标已切换为CheckMark");
           },
           postAction: () => Debug.WriteLine("放大完成")
       )
       // 第三步：延迟2秒后缩小图标 (当前值 -> 1)
       .AddStep(
           targetProperty: "GlyphSize",
           from: null,
           to: 1,
           durationMs: 100,
           delayMs: 2000, // 关键延迟控制
           preAction: () => Debug.WriteLine("开始延迟缩小"),
           postAction: () => Debug.WriteLine("延迟缩小完成")
       )
       // 第四步：恢复图标大小并切换为复制图标 (当前值 -> 16)
       .AddStep(
           targetProperty: "GlyphSize",
           from: null,
           to: 16,
           durationMs: 100,
           preAction: () =>
           {
               button.Glyph = TSegoeIconType.Copy; // 最终图标切换
               Debug.WriteLine("图标已恢复为Copy");
           },
           postAction: () => Debug.WriteLine("动画序列全部完成")
       )
       .Run(); // 启动动画序列
        }
    }
}
