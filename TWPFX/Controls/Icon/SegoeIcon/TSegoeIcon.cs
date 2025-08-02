using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media;

namespace TWPFX.Controls.Icon.SegoeIcon
{
    public class TSegoeIcon : FrameworkElement
    {
        #region 依赖属性

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(TSegoeIcon),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, OnDependencyPropertyChanged));

        public static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register(nameof(FontFamily), typeof(FontFamily), typeof(TSegoeIcon),
            new FrameworkPropertyMetadata(new FontFamily("Segoe MDL2 Assets"), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, OnDependencyPropertyChanged));

        public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register(nameof(FontSize), typeof(double), typeof(TSegoeIcon),
           new FrameworkPropertyMetadata(16.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, OnDependencyPropertyChanged));

        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(nameof(Foreground), typeof(Brush), typeof(TSegoeIcon),
            new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender, OnDependencyPropertyChanged));

        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register(nameof(Glyph), typeof(TSegoeIconType), typeof(TSegoeIcon),
            new FrameworkPropertyMetadata(TSegoeIconType.None, OnGlyphPropertyChanged));

        public static readonly DependencyProperty GlyphSizeProperty = DependencyProperty.Register(nameof(GlyphSize), typeof(double), typeof(TSegoeIcon),
            new FrameworkPropertyMetadata(16.0, OnGlyphSizePropertyChanged));

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(nameof(Description), typeof(string), typeof(TSegoeIcon));

        #endregion

        #region 属性变更回调

        private static void OnDependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TSegoeIcon segoeIcon = (TSegoeIcon)d;
            if (e.Property == ForegroundProperty)
            {
                if (e.OldValue is Brush oldBrush && !oldBrush.IsFrozen)
                    oldBrush.Changed -= segoeIcon.OnBrushChanged;
                if (e.NewValue is Brush newBrush && !newBrush.IsFrozen)
                    newBrush.Changed += segoeIcon.OnBrushChanged;
            }
            segoeIcon.InvalidateFormattedText();
        }

        private void OnBrushChanged(object sender, EventArgs e)
        {
            InvalidateFormattedText();
        }

        private static void OnGlyphPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TSegoeIcon segoeIcon = (TSegoeIcon)d;
            segoeIcon.Text = segoeIcon.Glyph.ToUnicodeChar();
            segoeIcon.Description = segoeIcon.Glyph.GetDescription();
        }

        private static void OnGlyphSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TSegoeIcon segoeIcon = (TSegoeIcon)d;
            segoeIcon.FontSize = segoeIcon.GlyphSize;
        }

        #endregion

        #region CLR属性包装器

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public FontFamily FontFamily
        {
            get => (FontFamily)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }

        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public Brush Foreground
        {
            get => (Brush)GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }

        public TSegoeIconType Glyph
        {
            get => (TSegoeIconType)GetValue(GlyphProperty);
            set => SetValue(GlyphProperty, value);
        }

        public double GlyphSize
        {
            get => (double)GetValue(GlyphSizeProperty);
            set => SetValue(GlyphSizeProperty, value);
        }

        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        #endregion

        private FormattedText _formattedText;
        private Brush _lastForeground;

        private void InvalidateFormattedText()
        {
            _formattedText = CreateFormattedText(); // 立即重建而不是设为null
            InvalidateMeasure(); // 强制重新测量
            InvalidateVisual(); // 强制重绘
        }

        static TSegoeIcon()
        {
            // 启用高质量渲染选项
            TextOptions.TextRenderingModeProperty.OverrideMetadata(
                typeof(TSegoeIcon),
                new FrameworkPropertyMetadata(TextRenderingMode.ClearType));

            TextOptions.TextFormattingModeProperty.OverrideMetadata(
                typeof(TSegoeIcon),
                new FrameworkPropertyMetadata(TextFormattingMode.Ideal));
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (string.IsNullOrEmpty(Text))
                return new Size(0, 0);

            _formattedText = CreateFormattedText();
            return new Size(_formattedText.Width, _formattedText.Height);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (_formattedText == null || string.IsNullOrEmpty(Text))
                return;

            // 应用布局偏移补偿
            var offset = new Vector(
                (RenderSize.Width - _formattedText.Width) / 2,
                (RenderSize.Height - _formattedText.Height) / 2);

            // 使用高质量渲染
            drawingContext.PushTransform(new TranslateTransform(offset.X, offset.Y));
            drawingContext.DrawText(_formattedText, new Point(0, 0));
            drawingContext.Pop();
        }

        private FormattedText CreateFormattedText()
        {
            var formattedText = new FormattedText(
                Text,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(FontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal),
                FontSize,
                Foreground,
                VisualTreeHelper.GetDpi(this).PixelsPerDip);

            return formattedText;
        }
    }
}
