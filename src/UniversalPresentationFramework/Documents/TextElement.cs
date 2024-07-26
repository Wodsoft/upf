using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Controls.Primitives;
using Wodsoft.UI.Markup;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Documents
{
    public abstract class TextElement : FrameworkContentElement
    {
        #region Appearance Properties

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
        [TypeConverter(typeof(FontSizeConverter))]
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
            var metadata = e.Metadata;
            if (metadata == null)
                metadata = e.Property.GetMetadata(GetType());
            if (metadata is FrameworkPropertyMetadata fMetadata && fMetadata.Flags.HasFlag(FrameworkPropertyMetadataOptions.AffectsRender))
            {

            }
        }

        #endregion

        #region Relationship

        private TextPointer? _contentStart, _contentEnd, _elementStart, _elementEnd;
        private TextTreeTextElementNode? _textElementNode;

        public TextPointer ContentStart
        {
            get
            {
                if (_contentStart is null)
                {
                    _contentStart = new TextPointer(EnsureTextContainer(), TextElementNode, ElementEdge.AfterStart, LogicalDirection.Forward);
                }
                return _contentStart;
            }
        }

        public TextPointer ContentEnd
        {
            get
            {
                if (_contentEnd is null)
                    _contentEnd = new TextPointer(EnsureTextContainer(), TextElementNode, ElementEdge.BeforeEnd, LogicalDirection.Backward);
                return _contentEnd;
            }
        }

        public TextPointer ElementStart
        {
            get
            {
                if (_elementStart is null)
                    _elementStart = new TextPointer(EnsureTextContainer(), TextElementNode, ElementEdge.BeforeStart, LogicalDirection.Forward);
                return _elementStart;
            }
        }

        public TextPointer ElementEnd
        {
            get
            {
                if (_elementEnd is null)
                    _elementEnd = new TextPointer(EnsureTextContainer(), TextElementNode, ElementEdge.AfterEnd, LogicalDirection.Backward);
                return _elementEnd;
            }
        }

        protected TextElement? NextElement => (TextElementNode.NextNode as TextTreeTextElementNode)?.TextElement;

        protected TextElement? PreviousElement => (TextElementNode.PreviousNode as TextTreeTextElementNode)?.TextElement;

        protected TextElement? FirstChildElement => (TextElementNode.FirstChildNode as TextTreeTextElementNode)?.TextElement;

        protected TextElement? LastChildElement => (TextElementNode.LastChildNode as TextTreeTextElementNode)?.TextElement;

        protected internal virtual int ContentCount => 0;

        public TextTreeTextElementNode TextElementNode
        {
            get
            {
                if (_textElementNode == null)
                    _textElementNode = CreateTextElementNode();
                return _textElementNode;
            }
        }

        protected virtual TextTreeTextElementNode CreateTextElementNode() => new TextTreeTextElementNode(this);

        private IReadOnlyTextContainer EnsureTextContainer()
        {
            TextTreeNode node = TextElementNode;
            while (node.ParentNode != null)
                node = node.ParentNode;
            if (node is TextTreeRootNode rootNode)
                return rootNode.TextContainer;
            return new ReadOnlyTextContainer(node);
        }

        #endregion
    }
}
