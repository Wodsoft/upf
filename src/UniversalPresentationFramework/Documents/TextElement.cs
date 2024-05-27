using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Markup;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Documents
{
    public abstract class TextElement : FrameworkContentElement, IAddChild
    {
        #region Properties

        public static readonly DependencyProperty FontFamilyProperty =
                DependencyProperty.RegisterAttached(
                        "FontFamily",
                        typeof(FontFamily),
                        typeof(TextElement),
                        new FrameworkPropertyMetadata(
                                SystemFonts.MessageFontFamily,
                                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits),
                        new ValidateValueCallback(IsValidFontFamily));
        private static bool IsValidFontFamily(object? o)
        {
            return o is FontFamily;
        }
        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty)!; }
            set { SetValue(FontFamilyProperty, value); }
        }
        public static void SetFontFamily(DependencyObject element, FontFamily value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(FontFamilyProperty, value);
        }
        public static FontFamily GetFontFamily(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (FontFamily)element.GetValue(FontFamilyProperty)!;
        }

        public static readonly DependencyProperty FontStyleProperty =
                DependencyProperty.RegisterAttached(
                        "FontStyle",
                        typeof(FontStyle),
                        typeof(TextElement),
                        new FrameworkPropertyMetadata(
                                SystemFonts.MessageFontStyle,
                                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
        public FontStyle FontStyle
        {
            get { return (FontStyle)GetValue(FontStyleProperty)!; }
            set { SetValue(FontStyleProperty, value); }
        }
        public static void SetFontStyle(DependencyObject element, FontStyle value)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            element.SetValue(FontStyleProperty, value);
        }
        public static FontStyle GetFontStyle(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            return (FontStyle)element.GetValue(FontStyleProperty)!;
        }

        public static readonly DependencyProperty FontWeightProperty =
                DependencyProperty.RegisterAttached(
                        "FontWeight",
                        typeof(FontWeight),
                        typeof(TextElement),
                        new FrameworkPropertyMetadata(
                                SystemFonts.MessageFontWeight,
                                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
        public FontWeight FontWeight
        {
            get { return (FontWeight)GetValue(FontWeightProperty)!; }
            set { SetValue(FontWeightProperty, value); }
        }
        public static void SetFontWeight(DependencyObject element, FontWeight value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            element.SetValue(FontWeightProperty, value);
        }
        public static FontWeight GetFontWeight(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            return (FontWeight)element.GetValue(FontWeightProperty)!;
        }

        public static readonly DependencyProperty FontStretchProperty =
                DependencyProperty.RegisterAttached(
                        "FontStretch",
                        typeof(FontStretch),
                        typeof(TextElement),
                        new FrameworkPropertyMetadata(
                                FontStretches.Normal,
                                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
        public FontStretch FontStretch
        {
            get { return (FontStretch)GetValue(FontStretchProperty)!; }
            set { SetValue(FontStretchProperty, value); }
        }
        public static void SetFontStretch(DependencyObject element, FontStretch value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            element.SetValue(FontStretchProperty, value);
        }
        public static FontStretch GetFontStretch(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            return (FontStretch)element.GetValue(FontStretchProperty)!;
        }

        public static readonly DependencyProperty FontSizeProperty =
                DependencyProperty.RegisterAttached(
                        "FontSize",
                        typeof(float),
                        typeof(TextElement),
                        new FrameworkPropertyMetadata(
                                SystemFonts.MessageFontSize,
                                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits),
                        new ValidateValueCallback(IsValidFontSize));
        private static bool IsValidFontSize(object? value)
        {
            if (value == null)
                return false;
            float fontSize = (float)value;
            float minFontSize = 1;
            float maxFontSize = 1000000;

            if (float.IsNaN(fontSize))
            {
                return false;
            }
            if (fontSize < minFontSize)
            {
                return false;
            }
            if (fontSize > maxFontSize)
            {
                return false;
            }
            return true;
        }
        public float FontSize
        {
            get { return (float)GetValue(FontSizeProperty)!; }
            set { SetValue(FontSizeProperty, value); }
        }
        public static void SetFontSize(DependencyObject element, float value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            element.SetValue(FontSizeProperty, value);
        }
        public static float GetFontSize(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            return (float)element.GetValue(FontSizeProperty)!;
        }

        public static readonly DependencyProperty ForegroundProperty =
                DependencyProperty.RegisterAttached(
                        "Foreground",
                        typeof(Brush),
                        typeof(TextElement),
                        new FrameworkPropertyMetadata(
                                Brushes.Black,
                                FrameworkPropertyMetadataOptions.AffectsRender |
                                FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender |
                                FrameworkPropertyMetadataOptions.Inherits));
        public Brush? Foreground
        {
            get { return (Brush?)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }
        public static void SetForeground(DependencyObject element, Brush? value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            element.SetValue(ForegroundProperty, value);
        }
        public static Brush? GetForeground(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            return (Brush?)element.GetValue(ForegroundProperty);
        }

        public static readonly DependencyProperty BackgroundProperty =
                DependencyProperty.Register("Background",
                        typeof(Brush),
                        typeof(TextElement),
                        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
        public Brush? Background
        {
            get { return (Brush?)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }


        #endregion

        #region Dependency Value

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
        }

        #endregion

        public void AddChild(object value)
        {
            throw new NotImplementedException();
        }

        public void AddText(string text)
        {
            throw new NotImplementedException();
        }
    }
}
