using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWPFX.Controls.TextBlock.CodeBlock
{
    public enum TCodeBlockStyle
    {
        AndroidStudio,    // Android Studio 默认主题，深色背景搭配高饱和度彩色语法，适合Java/Kotlin开发者
        Dark,    // 通用深色主题，低亮度背景减少视觉疲劳，适合夜间或长期编码场景
        Default,    // 编辑器默认主题（通常为浅色），平衡可读性与普适性
        Github,     // GitHub 浅色主题，与网页版代码风格一致，适合开源项目文档展示
        Github_Dark,  // GitHub 深色主题，保留浅色主题的语法配色逻辑但适配暗背景
        GoogleCode,    // Google 代码规范主题，简洁明快的配色，强调代码结构清晰度
        Intellij_Light,   // IntelliJ IDEA 浅色主题，柔和的色彩对比，减少长时间编码的视觉刺激
        Qtcreator_Dark,   // Qt Creator 深色主题，冷色调为主，适合C++/嵌入式开发环境
        Qtcreator_Light,   // Qt Creator 浅色主题，中性配色方案，兼顾可读性和舒适度
        Stackoverflow_Dark,   // Stack Overflow 深色主题，橙黄色调突出关键词，适合问答场景代码展示
        Stackoverflow_Light,   // Stack Overflow 浅色主题，与网站问答页代码块风格一致
        VS2015  // Visual Studio 2015 主题，经典蓝色系，支持多语言高亮，适合Windows平台开发
    }
}