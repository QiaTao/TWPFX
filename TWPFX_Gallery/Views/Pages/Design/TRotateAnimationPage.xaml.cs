using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TWPFX.Animations;

namespace TWPFX_Gallery.Views.Pages.Design
{
    /// <summary>
    /// TRotateAnimationPage.xaml 的交互逻辑
    /// </summary>
    public partial class TRotateAnimationPage : Page
    {

        public TRotateAnimationPage()
        {
            InitializeComponent();
        }

        private void RotateButton1_Click(object sender, RoutedEventArgs e)
        {
            // 测试重构后的动画系统
            rotateTarget1.CreateAnimationSequence().AddRotateStep(0, 360, 1000).Run();
        }
    }
} 