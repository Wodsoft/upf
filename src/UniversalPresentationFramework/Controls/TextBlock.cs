using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Controls.Primitives;
using Wodsoft.UI.Data;
using Wodsoft.UI.Documents;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Controls
{
    [ContentProperty("Inlines")]
    public partial class TextBlock : FrameworkElement
    {
        private readonly InlineCollection _inlines;
        private readonly TextBlockTextContainer _textContainer;
        private readonly Run _textRun;
        private string _text = string.Empty;
        private int _selectionStart, _selectionLength;

        #region Constructor

        static TextBlock()
        {
            _TextViewerTemplate = new TextBlockTemplate();
            FrameworkElementFactory textViewer = new FrameworkElementFactory(typeof(RichTextViewer));
            textViewer.SetValue(MarginProperty, new TemplateBindingExpression(new TemplateBindingExtension(PaddingProperty)));
            textViewer.SetValue(TextViewer.TextAlignmentProperty, new TemplateBindingExpression(new TemplateBindingExtension(TextAlignmentProperty)));
            textViewer.SetValue(TextViewer.TextTrimmingProperty, new TemplateBindingExpression(new TemplateBindingExtension(TextTrimmingProperty)));
            textViewer.SetValue(TextViewer.TextWrappingProperty, new TemplateBindingExpression(new TemplateBindingExtension(TextWrappingProperty)));
            textViewer.Seal();
            _TextViewerTemplate.VisualTree = textViewer;
        }

        public TextBlock()
        {
            _textContainer = new TextBlockTextContainer(this);
            _inlines = new InlineCollection(this, _textContainer.Root.FirstChildNode!);
            _textRun = new Run();
            _textRun.SetBinding(Run.TextProperty, new Binding { Source = this, Path = new PropertyPath("Text"), Mode = BindingMode.OneWay });
            AddLogicalChild(_textRun);
            ((TextBlockRootNode)_textContainer.Root).AddNode(_textRun.TextElementNode);
        }

        #endregion

        #region Template

        private static readonly FrameworkTemplate _TextViewerTemplate;

        protected override FrameworkTemplate? GetTemplate()
        {
            return _TextViewerTemplate;
        }

        protected override void OnApplyTemplate()
        {
            ((RichTextViewer)TemplatedChild!).InitializeViewer(_textContainer, this);
        }

        #endregion

        #region Properties

        public static readonly DependencyProperty BackgroundProperty =
                TextElement.BackgroundProperty.AddOwner(
                        typeof(TextBlock),
                        new FrameworkPropertyMetadata(
                                null,
                                FrameworkPropertyMetadataOptions.AffectsRender));
        public Brush? Background
        {
            get { return (Brush?)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        /// <summary>
        /// DependencyProperty for <see cref="BaselineOffset" /> property.
        /// </summary>
        public static readonly DependencyProperty BaselineOffsetProperty =
                DependencyProperty.RegisterAttached(
                        "BaselineOffset",
                        typeof(float),
                        typeof(TextBlock),
                        new FrameworkPropertyMetadata(
                                float.NaN,
                                new PropertyChangedCallback(OnBaselineOffsetChanged)));
        /// <summary>
        /// The BaselineOffset property provides an adjustment to baseline offset
        /// </summary>
        public float BaselineOffset
        {
            get { return (float)GetValue(BaselineOffsetProperty)!; }
            set { SetValue(BaselineOffsetProperty, value); }
        }
        /// <summary>
        /// Writes the attached property BaselineOffset to the given element.
        /// </summary>
        /// <param name="element">The element to which to write the attached property.</param>
        /// <param name="value">The property value to set</param>
        public static void SetBaselineOffset(DependencyObject element, float value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(BaselineOffsetProperty, value);
        }
        /// <summary>
        /// Reads the attached property from the given element
        /// </summary>
        /// <param name="element">The element to which to read the attached property.</param>
        public static float GetBaselineOffset(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (float)element.GetValue(BaselineOffsetProperty)!;
        }
        private static void OnBaselineOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Set up our baseline changed event

            //fire event!
            //TextElement te = TextElement.ContainerTextElementField.GetValue(d);

            //if (te != null)
            //{
            //    DependencyObject parent = te.TextContainer.Parent;
            //    TextBlock tb = parent as TextBlock;
            //    if (tb != null)
            //    {
            //        tb.OnChildBaselineOffsetChanged(d);
            //    }
            //    //else
            //    //{
            //    //    FlowDocument fd = parent as FlowDocument;
            //    //    if (fd != null && d is UIElement)
            //    //    {
            //    //        fd.OnChildDesiredSizeChanged((UIElement)d);
            //    //    }
            //    //}
            //}
        }

        public static readonly DependencyProperty TextProperty =
                DependencyProperty.Register(
                        "Text",
                        typeof(string),
                        typeof(TextBlock),
                        new FrameworkPropertyMetadata(
                                string.Empty,
                                FrameworkPropertyMetadataOptions.AffectsMeasure |
                                FrameworkPropertyMetadataOptions.AffectsRender,
                                new PropertyChangedCallback(OnTextChanged),
                                new CoerceValueCallback(CoerceText)));
        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OnTextChanged((TextBlock)d, (string)e.NewValue!);
        }
        private static object CoerceText(DependencyObject d, object? value)
        {
            TextBlock textblock = (TextBlock)d;

            if (value == null)
                value = string.Empty;
            //if (textblock._complexContent != null &&
            //    !textblock.CheckFlags(Flags.TextContentChanging) &&
            //    (string)value == (string)textblock.GetValue(TextProperty))
            //{
            //    // If the new value equals the old value, then the property
            //    // system will optimize out the call to OnTextChanged.  We can't
            //    // skip this call because there's ambiguity between the TextProperty
            //    // view of content and actual content -- we might have a new
            //    // value even if strings match.
            //    //
            //    // E.g.: content = <Image/>, TextProperty == " "
            //    // Now setting TextProperty = " " really changes content, replacing
            //    // the Image with a space char.
            //    OnTextChanged(d, (string)value);
            //}

            return value;
        }
        public string? Text
        {
            get { return (string?)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        private static void OnTextChanged(TextBlock text, string newText)
        {
            text._text = newText;
            //if (text.CheckFlags(Flags.TextContentChanging))
            //{
            //    // The update originated in a TextContainer change -- don't update
            //    // the TextContainer a second time.
            //    return;
            //}

            //if (text._complexContent == null)
            //{
            //    text._contentCache = (newText != null) ? newText : String.Empty;
            //}
            //else
            //{
            //    text.SetFlags(true, Flags.TextContentChanging);
            //    try
            //    {
            //        bool exceptionThrown = true;

            //        Invariant.Assert(text._contentCache == null, "Content cache should be null when complex content exists.");

            //        text._complexContent.TextContainer.BeginChange();
            //        try
            //        {
            //            ((TextContainer)text._complexContent.TextContainer).DeleteContentInternal((TextPointer)text._complexContent.TextContainer.Start, (TextPointer)text._complexContent.TextContainer.End);
            //            InsertTextRun(text._complexContent.TextContainer.End, newText, /*whitespacesIgnorable:*/true);
            //            exceptionThrown = false;
            //        }
            //        finally
            //        {
            //            text._complexContent.TextContainer.EndChange();

            //            if (exceptionThrown)
            //            {
            //                text.ClearLineMetrics();
            //            }
            //        }
            //    }
            //    finally
            //    {
            //        text.SetFlags(false, Flags.TextContentChanging);
            //    }
            //}
        }

        public static readonly DependencyProperty FontFamilyProperty =
                TextElement.FontFamilyProperty.AddOwner(typeof(TextBlock));
        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty)!; }
            set { SetValue(FontFamilyProperty, value); }
        }
        /// <summary>
        /// DependencyProperty setter for <see cref="FontFamily" /> property.
        /// </summary>
        /// <param name="element">The element to which to write the attached property.</param>
        /// <param name="value">The property value to set</param>
        public static void SetFontFamily(DependencyObject element, FontFamily value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(FontFamilyProperty, value);
        }
        /// <summary>
        /// DependencyProperty getter for <see cref="FontFamily" /> property.
        /// </summary>
        /// <param name="element">The element from which to read the attached property.</param>
        public static FontFamily GetFontFamily(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (FontFamily)element.GetValue(FontFamilyProperty)!;
        }

        public static readonly DependencyProperty FontStyleProperty =
                TextElement.FontStyleProperty.AddOwner(typeof(TextBlock));
        /// <summary>
        /// The FontStyle property requests normal, italic, and oblique faces within a font family.
        /// </summary>
        public FontStyle FontStyle
        {
            get { return (FontStyle)GetValue(FontStyleProperty)!; }
            set { SetValue(FontStyleProperty, value); }
        }
        /// <summary>
        /// DependencyProperty setter for <see cref="FontStyle" /> property.
        /// </summary>
        /// <param name="element">The element to which to write the attached property.</param>
        /// <param name="value">The property value to set</param>
        public static void SetFontStyle(DependencyObject element, FontStyle value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(FontStyleProperty, value);
        }
        /// <summary>
        /// DependencyProperty getter for <see cref="FontStyle" /> property.
        /// </summary>
        /// <param name="element">The element from which to read the attached property.</param>
        public static FontStyle GetFontStyle(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (FontStyle)element.GetValue(FontStyleProperty)!;
        }

        public static readonly DependencyProperty FontWeightProperty =
                TextElement.FontWeightProperty.AddOwner(typeof(TextBlock));
        /// <summary>
        /// The FontWeight property specifies the weight of the font.
        /// </summary>
        public FontWeight FontWeight
        {
            get { return (FontWeight)GetValue(FontWeightProperty)!; }
            set { SetValue(FontWeightProperty, value); }
        }
        /// <summary>
        /// DependencyProperty setter for <see cref="FontWeight" /> property.
        /// </summary>
        /// <param name="element">The element to which to write the attached property.</param>
        /// <param name="value">The property value to set</param>
        public static void SetFontWeight(DependencyObject element, FontWeight value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(FontWeightProperty, value);
        }

        /// <summary>
        /// DependencyProperty getter for <see cref="FontWeight" /> property.
        /// </summary>
        /// <param name="element">The element from which to read the attached property.</param>
        public static FontWeight GetFontWeight(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (FontWeight)element.GetValue(FontWeightProperty)!;
        }

        public static readonly DependencyProperty FontStretchProperty =
                TextElement.FontStretchProperty.AddOwner(typeof(TextBlock));
        /// <summary>
        /// The FontStretch property selects a normal, condensed, or extended face from a font family.
        /// </summary>
        public FontStretch FontStretch
        {
            get { return (FontStretch)GetValue(FontStretchProperty)!; }
            set { SetValue(FontStretchProperty, value); }
        }
        /// <summary>
        /// DependencyProperty setter for <see cref="FontStretch" /> property.
        /// </summary>
        /// <param name="element">The element to which to write the attached property.</param>
        /// <param name="value">The property value to set</param>
        public static void SetFontStretch(DependencyObject element, FontStretch value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(FontStretchProperty, value);
        }
        /// <summary>
        /// DependencyProperty getter for <see cref="FontStretch" /> property.
        /// </summary>
        /// <param name="element">The element from which to read the attached property.</param>
        public static FontStretch GetFontStretch(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (FontStretch)element.GetValue(FontStretchProperty)!;
        }

        public static readonly DependencyProperty FontSizeProperty =
                TextElement.FontSizeProperty.AddOwner(
                        typeof(TextBlock));
        /// <summary>
        /// The FontSize property specifies the size of the font.
        /// </summary>
        [TypeConverter(typeof(FontSizeConverter))]
        public float FontSize
        {
            get { return (float)GetValue(FontSizeProperty)!; }
            set { SetValue(FontSizeProperty, value); }
        }
        /// <summary>
        /// DependencyProperty setter for <see cref="FontSize" /> property.
        /// </summary>
        /// <param name="element">The element to which to write the attached property.</param>
        /// <param name="value">The property value to set</param>
        public static void SetFontSize(DependencyObject element, float value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(FontSizeProperty, value);
        }

        /// <summary>
        /// DependencyProperty getter for <see cref="FontSize" /> property.
        /// </summary>
        /// <param name="element">The element from which to read the attached property.</param>
        [TypeConverter(typeof(FontSizeConverter))]
        public static float GetFontSize(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (float)element.GetValue(FontSizeProperty)!;
        }

        public static readonly DependencyProperty ForegroundProperty =
                TextElement.ForegroundProperty.AddOwner(
                        typeof(TextBlock));
        /// <summary>
        /// The Foreground property specifies the foreground brush of an element's text content.
        /// </summary>
        public Brush? Foreground
        {
            get { return (Brush?)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        /// <summary>
        /// DependencyProperty setter for <see cref="Foreground" /> property.
        /// </summary>
        /// <param name="element">The element to which to write the attached property.</param>
        /// <param name="value">The property value to set</param>
        public static void SetForeground(DependencyObject element, Brush? value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(ForegroundProperty, value);
        }
        /// <summary>
        /// DependencyProperty getter for <see cref="Foreground" /> property.
        /// </summary>
        /// <param name="element">The element from which to read the attached property.</param>
        public static Brush? GetForeground(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (Brush?)element.GetValue(ForegroundProperty);
        }

        public static readonly DependencyProperty TextDecorationsProperty =
                Inline.TextDecorationsProperty.AddOwner(
                        typeof(TextBlock),
                        new FrameworkPropertyMetadata(TextDecorationCollection.Empty, FrameworkPropertyMetadataOptions.AffectsRender));
        /// <summary>
        /// The TextDecorations property specifies decorations that are added to the text of an element.
        /// </summary>
        public TextDecorationCollection? TextDecorations
        {
            get { return (TextDecorationCollection?)GetValue(TextDecorationsProperty); }
            set { SetValue(TextDecorationsProperty, value); }
        }


        /// <summary>
        /// DependencyProperty for <see cref="LineHeight" /> property.
        /// </summary>
        public static readonly DependencyProperty LineHeightProperty =
                Block.LineHeightProperty.AddOwner(typeof(TextBlock));
        /// <summary>
        /// The LineHeight property specifies the height of each generated line box.
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        public float LineHeight
        {
            get { return (float)GetValue(LineHeightProperty)!; }
            set { SetValue(LineHeightProperty, value); }
        }
        /// <summary>
        /// DependencyProperty setter for <see cref="LineHeight" /> property.
        /// </summary>
        /// <param name="element">The element to which to write the attached property.</param>
        /// <param name="value">The property value to set</param>
        public static void SetLineHeight(DependencyObject element, float value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(LineHeightProperty, value);
        }
        /// <summary>
        /// DependencyProperty getter for <see cref="LineHeight" /> property.
        /// </summary>
        /// <param name="element">The element from which to read the attached property.</param>
        [TypeConverter(typeof(LengthConverter))]
        public static float GetLineHeight(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (float)element.GetValue(LineHeightProperty)!;
        }

        /// <summary>
        /// DependencyProperty for <see cref="LineStackingStrategy" /> property.
        /// </summary>
        public static readonly DependencyProperty LineStackingStrategyProperty =
                Block.LineStackingStrategyProperty.AddOwner(typeof(TextBlock));
        /// <summary>
        /// The LineStackingStrategy property specifies how lines are placed
        /// </summary>
        public LineStackingStrategy LineStackingStrategy
        {
            get { return (LineStackingStrategy)GetValue(LineStackingStrategyProperty)!; }
            set { SetValue(LineStackingStrategyProperty, value); }
        }
        /// <summary>
        /// DependencyProperty setter for <see cref="LineStackingStrategy" /> property.
        /// </summary>
        /// <param name="element">The element to which to write the attached property.</param>
        /// <param name="value">The property value to set</param>
        public static void SetLineStackingStrategy(DependencyObject element, LineStackingStrategy value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(LineStackingStrategyProperty, value);
        }
        /// <summary>
        /// DependencyProperty getter for <see cref="LineStackingStrategy" /> property.
        /// </summary>
        /// <param name="element">The element from which to read the attached property.</param>
        public static LineStackingStrategy GetLineStackingStrategy(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (LineStackingStrategy)element.GetValue(LineStackingStrategyProperty)!;
        }


        /// <summary>
        /// DependencyProperty for <see cref="Padding" /> property.
        /// </summary>
        public static readonly DependencyProperty PaddingProperty =
                Block.PaddingProperty.AddOwner(
                        typeof(TextBlock),
                        new FrameworkPropertyMetadata(
                                new Thickness(),
                                FrameworkPropertyMetadataOptions.AffectsMeasure));
        /// <summary>
        /// The Padding property specifies the padding of the element.
        /// </summary>
        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty)!; }
            set { SetValue(PaddingProperty, value); }
        }

        /// <summary>
        /// DependencyProperty for <see cref="TextAlignment" /> property.
        /// </summary>
        public static readonly DependencyProperty TextAlignmentProperty =
                Block.TextAlignmentProperty.AddOwner(typeof(TextBlock));
        /// <summary>
        /// The TextAlignment property specifies horizontal alignment of the content.
        /// </summary>
        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty)!; }
            set { SetValue(TextAlignmentProperty, value); }
        }
        /// <summary>
        /// DependencyProperty setter for <see cref="TextAlignment" /> property.
        /// </summary>
        /// <param name="element">The element to which to write the attached property.</param>
        /// <param name="value">The property value to set</param>
        public static void SetTextAlignment(DependencyObject element, TextAlignment value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(TextAlignmentProperty, value);
        }
        /// <summary>
        /// DependencyProperty getter for <see cref="TextAlignment" /> property.
        /// </summary>
        /// <param name="element">The element from which to read the attached property.</param>
        public static TextAlignment GetTextAlignment(DependencyObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (TextAlignment)element.GetValue(TextAlignmentProperty)!;
        }

        public static readonly DependencyProperty TextTrimmingProperty =
                DependencyProperty.Register(
                        "TextTrimming",
                        typeof(TextTrimming),
                        typeof(TextBlock),
                        new FrameworkPropertyMetadata(
                                TextTrimming.None,
                                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender),
                        new ValidateValueCallback(IsValidTextTrimming));
        private static bool IsValidTextTrimming(object? o)
        {
            TextTrimming value = (TextTrimming)o!;
            return value == TextTrimming.CharacterEllipsis
                || value == TextTrimming.None
                || value == TextTrimming.WordEllipsis;
        }
        /// <summary>
        /// The TextTrimming property specifies the trimming behavior situation
        /// in case of clipping some textual content caused by overflowing the line's box.
        /// </summary>
        public TextTrimming TextTrimming
        {
            get { return (TextTrimming)GetValue(TextTrimmingProperty)!; }
            set { SetValue(TextTrimmingProperty, value); }
        }

        public static readonly DependencyProperty TextWrappingProperty =
                DependencyProperty.Register(
                        "TextWrapping",
                        typeof(TextWrapping),
                        typeof(TextBlock),
                        new FrameworkPropertyMetadata(
                                TextWrapping.NoWrap,
                                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender),
                        new ValidateValueCallback(IsValidTextWrap));
        private static bool IsValidTextWrap(object? o)
        {
            TextWrapping value = (TextWrapping)o!;
            return value == TextWrapping.Wrap
                || value == TextWrapping.NoWrap
                || value == TextWrapping.WrapWithOverflow;
        }
        /// <summary>
        /// The TextWrapping property controls whether or not text wraps
        /// when it reaches the flow edge of its containing block box.
        /// </summary>
        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty)!; }
            set { SetValue(TextWrappingProperty, value); }
        }

        /// <summary>
        /// DependencyProperty for hyphenation property.
        /// </summary>
        public static readonly DependencyProperty IsHyphenationEnabledProperty =
                Block.IsHyphenationEnabledProperty.AddOwner(typeof(TextBlock));
        /// <summary>
        /// CLR property for hyphenation
        /// </summary>
        public bool IsHyphenationEnabled
        {
            get { return (bool)GetValue(IsHyphenationEnabledProperty)!; }
            set { SetValue(IsHyphenationEnabledProperty, value); }
        }

        public static readonly DependencyProperty IsTextSelectionEnabledProperty =
            DependencyProperty.Register("IsTextSelectionEnabled", typeof(bool), typeof(TextBlock), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));
        public bool IsTextSelectionEnabled { get => (bool)GetValue(IsTextSelectionEnabledProperty)!; set => SetValue(IsTextSelectionEnabledProperty, value); }

        public TextPointer SelectionStart => _textContainer.SelectionStart;

        public TextPointer SelectionEnd => _textContainer.SelectionEnd;

        public static readonly DependencyProperty SelectionBrushProperty = TextBoxBase.SelectionBrushProperty.AddOwner(typeof(TextBlock));
        public Brush? SelectionBrush
        {
            get { return (Brush?)GetValue(SelectionBrushProperty); }
            set { SetValue(SelectionBrushProperty, value); }
        }

        public static readonly DependencyProperty SelectionTextBrushProperty = TextBoxBase.SelectionTextBrushProperty.AddOwner(typeof(TextBlock));
        public Brush? SelectionTextBrush
        {
            get { return (Brush?)GetValue(SelectionTextBrushProperty); }
            set { SetValue(SelectionTextBrushProperty, value); }
        }

        public InlineCollection Inlines => _inlines;

        #endregion

        #region Events

        public static readonly RoutedEvent SelectionChangedEvent = TextBoxBase.SelectionChangedEvent.AddOwner(typeof(TextBlock));
        public event RoutedEventHandler SelectionChanged { add { AddHandler(SelectionChangedEvent, value); } remove { RemoveHandler(SelectionChangedEvent, value); } }


        public static readonly RoutedEvent TextChangedEvent = TextBoxBase.TextChangedEvent.AddOwner(typeof(TextBlock));
        public event TextChangedEventHandler TextChanged { add { AddHandler(TextChangedEvent, value); } remove { RemoveHandler(TextChangedEvent, value); } }

        #endregion

        #region Layout

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (TemplatedChild != null)
            {
                TemplatedChild.Arrange(new Rect(finalSize));
            }
            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (TemplatedChild == null)
                return new Size(0.0f, 0.0f);
            TemplatedChild.Measure(availableSize);
            return TemplatedChild.DesiredSize;
        }

        #endregion

        #region Render

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (drawingContext == null)
                return;

            var background = Background;
            if (background != null)
                drawingContext.DrawRectangle(background, null, new Rect(0, 0, RenderSize.Width, RenderSize.Height));
        }

        #endregion

        #region Selection

        public void Select(TextPointer start, TextPointer end)
        {
            if (!IsTextSelectionEnabled)
                return;
            _textContainer.Select(start, end);
        }

        public void SelectAll()
        {
            if (!IsTextSelectionEnabled)
                return;
            _textContainer.Select(_textContainer.DocumentStart, _textContainer.DocumentEnd);
        }

        #endregion
    }
}
