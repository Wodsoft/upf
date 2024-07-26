using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Documents
{
    [ContentProperty("Inlines")]
    public class Paragraph : Block
    {
        private readonly InlineCollection _inlines;
        private Documents.ParagraphLayout _inlineLayout;

        #region Constructors

        public Paragraph()
        {
            _inlines = new InlineCollection(this, TextElementNode);
            _inlineLayout = new Documents.ParagraphLayout(this, _inlines);
        }

        public Paragraph(Inline inline) : this()
        {
            _inlines.Add(inline);
        }

        #endregion

        #region Properties

        public InlineCollection Inlines => _inlines;


        /// <summary>
        /// DependencyProperty for <see cref="TextDecorations" /> property.
        /// </summary>
        public static readonly DependencyProperty TextDecorationsProperty =
                Inline.TextDecorationsProperty.AddOwner(
                        typeof(Paragraph),
                        new FrameworkPropertyMetadata(TextDecorationCollection.Empty,
                                FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// The TextDecorations property specifies decorations that are added to the text of an element.
        /// </summary>
        public TextDecorationCollection? TextDecorations
        {
            get { return (TextDecorationCollection?)GetValue(TextDecorationsProperty); }
            set { SetValue(TextDecorationsProperty, value); }
        }

        /// <summary>
        /// DependencyProperty for <see cref="TextIndent" /> property.
        /// </summary>
        public static readonly DependencyProperty TextIndentProperty =
                DependencyProperty.Register(
                        "TextIndent",
                        typeof(float),
                        typeof(Paragraph),
                        new FrameworkPropertyMetadata(
                                0f,
                                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender),
                        new ValidateValueCallback(IsValidTextIndent));
        private static bool IsValidTextIndent(object? o)
        {
            float indent = (float)o!;
            float maxIndent = 1000000;
            float minIndent = -maxIndent;
            if (float.IsNaN(indent))
            {
                return false;
            }
            if (indent < minIndent || indent > maxIndent)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// The TextIndent property specifies the indentation of the first line of a paragraph.
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        public float TextIndent
        {
            get { return (float)GetValue(TextIndentProperty)!; }
            set { SetValue(TextIndentProperty, value); }
        }

        /// <summary>
        /// DependencyProperty for <see cref="MinOrphanLines" /> property.
        /// </summary>
        public static readonly DependencyProperty MinOrphanLinesProperty =
                DependencyProperty.Register(
                        "MinOrphanLines",
                        typeof(int),
                        typeof(Paragraph),
                        new FrameworkPropertyMetadata(
                                0,
                                FrameworkPropertyMetadataOptions.AffectsParentMeasure),
                        new ValidateValueCallback(IsValidMinOrphanLines));
        private static bool IsValidMinOrphanLines(object? o)
        {
            int value = (int)o!;
            const int maxLines = 1000000;
            return (value >= 0 && value <= maxLines);
        }
        /// <summary>
        /// The MinOrphanLines is the minimum number of lines that
        /// can be left behind when a paragraph is broken on a page break 
        /// or column break.
        /// </summary>
        public int MinOrphanLines
        {
            get { return (int)GetValue(MinOrphanLinesProperty)!; }
            set { SetValue(MinOrphanLinesProperty, value); }
        }

        /// <summary>
        /// DependencyProperty for <see cref="MinWidowLines" /> property.
        /// </summary>
        public static readonly DependencyProperty MinWidowLinesProperty =
                DependencyProperty.Register(
                        "MinWidowLines",
                        typeof(int),
                        typeof(Paragraph),
                        new FrameworkPropertyMetadata(
                                0,
                                FrameworkPropertyMetadataOptions.AffectsParentMeasure),
                        new ValidateValueCallback(IsValidMinWidowLines));
        private static bool IsValidMinWidowLines(object? o)
        {
            int value = (int)o!;
            const int maxLines = 1000000;
            return (value >= 0 && value <= maxLines);
        }
        /// <summary>
        /// The MinWidowLines is the minimum number of lines after a break
        /// to be put on the next page or column.
        /// </summary>
        public int MinWidowLines
        {
            get { return (int)GetValue(MinWidowLinesProperty)!; }
            set { SetValue(MinWidowLinesProperty, value); }
        }

        /// <summary>
        /// DependencyProperty for <see cref="KeepWithNext" /> property.
        /// </summary>
        public static readonly DependencyProperty KeepWithNextProperty =
                DependencyProperty.Register(
                        "KeepWithNext",
                        typeof(bool),
                        typeof(Paragraph),
                        new FrameworkPropertyMetadata(
                                false,
                                FrameworkPropertyMetadataOptions.AffectsParentMeasure));
        /// <summary>
        /// The KeepWithNext property indicates that this paragraph should be kept with
        /// the next paragraph in the track.  (This also implies that the paragraph itself
        /// will not be broken.)
        /// </summary>
        public bool KeepWithNext
        {
            get { return (bool)GetValue(KeepWithNextProperty)!; }
            set { SetValue(KeepWithNextProperty, value); }
        }

        /// <summary>
        /// DependencyProperty for <see cref="KeepTogether" /> property.
        /// </summary>
        public static readonly DependencyProperty KeepTogetherProperty =
                DependencyProperty.Register(
                        "KeepTogether",
                        typeof(bool),
                        typeof(Paragraph),
                        new FrameworkPropertyMetadata(
                                false,
                                FrameworkPropertyMetadataOptions.AffectsParentMeasure));
        /// <summary>
        /// The KeepTogether property indicates that all the text in the paragraph
        /// should be kept together.
        /// </summary>
        public bool KeepTogether
        {
            get { return (bool)GetValue(KeepTogetherProperty)!; }
            set { SetValue(KeepTogetherProperty, value); }
        }

        #endregion

        #region Layout

        private IBlockLayout? _layout;
        public override IBlockLayout Layout => _layout ??= new ParagraphLayout(this, _inlines);

        #endregion
    }
}
