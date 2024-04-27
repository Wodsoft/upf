using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.TextFormatting
{
    public abstract class TextLine : IDisposable
    {
        /// <summary>
        /// Clean up text line internal resource
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Client to draw the line
        /// </summary>
        /// <param name="drawingContext">drawing context</param>
        /// <param name="origin">drawing origin</param>
        /// <param name="inversion">indicate the inversion of the drawing surface</param>
        public abstract void Draw(DrawingContext drawingContext, Point origin, InvertAxes inversion);

        ///// <summary>
        ///// Client to get an array of bounding rectangles of a range of characters within a text line.
        ///// </summary>
        ///// <param name="firstTextSourceCharacterIndex">index of first character of specified range</param>
        ///// <param name="textLength">number of characters of the specified range</param>
        ///// <returns>an array of bounding rectangles.</returns>
        //public abstract IReadOnlyList<TextBounds> GetTextBounds(int firstTextSourceCharacterIndex, int textLength);

        /// <summary>
        /// Client to get a collection of TextRun span objects within a line
        /// </summary>
        public abstract IReadOnlyList<TextRun> GetTextRunSpans();

        public abstract TextLine Collapse(TextTrimming trimming, float maxLineLength);

        public abstract TextLine Collapse(TextTrimming trimming, float maxLineLength, out TextLine? collapsedLine);

        ///// <summary>
        ///// Client to get IndexedGlyphRuns enumerable to enumerate each IndexedGlyphRun object 
        ///// in the line. Through IndexedGlyphRun client can obtain glyph information of 
        ///// a text source character. 
        ///// </summary>
        //public abstract IEnumerable<IndexedGlyphRun> GetIndexedGlyphRuns();

        /// <summary>
        /// Client to get a boolean value indicates whether content of the line overflows 
        /// the specified paragraph width.
        /// </summary>
        public abstract bool HasOverflowed { get; }

        /// <summary>
        /// Client to get a boolean value indicates whether a line has been collapsed
        /// </summary>
        public abstract bool HasCollapsed { get; }

        public bool IsEndOfNewLine { get; }

        /// <summary>
        /// Client to get the number of text source positions of this line
        /// </summary>
        public abstract int Length { get; }

        /// <summary>
        /// Client to get the number of whitespace characters at the end of the line.
        /// </summary>
        public abstract int TrailingWhitespaceLength { get; }


        ///// <summary>
        ///// Client to get the number of characters following the last character 
        ///// of the line that may trigger reformatting of the current line.
        ///// </summary>
        //public abstract int DependentLength { get; }


        ///// <summary>
        ///// Client to get the number of newline characters at line end
        ///// </summary>
        //public abstract int NewlineLength { get; }


        /// <summary>
        /// Client to get distance from paragraph start to line start
        /// </summary>
        public abstract float Start { get; }


        /// <summary>
        /// Client to get the total width of this line
        /// </summary>
        public abstract float Width { get; }


        /// <summary>
        /// Client to get the total width of this line including width of whitespace characters at the end of the line.
        /// </summary>
        public abstract float WidthIncludingTrailingWhitespace { get; }


        /// <summary>
        /// Client to get the height of the line
        /// </summary>
        public abstract float Height { get; }


        /// <summary>
        /// Client to get the height of the text (or other content) in the line; this property may differ from the Height
        /// property if the client specified the line height
        /// </summary>
        public abstract float TextHeight { get; }


        /// <summary>
        /// Client to get the height of the actual black of the line
        /// </summary>
        public abstract float Extent { get; }


        /// <summary>
        /// Client to get the distance from top to baseline of this text line
        /// </summary>
        public abstract float Baseline { get; }


        /// <summary>
        /// Client to get the distance from the top of the text (or other content) to the baseline of this text line;
        /// this property may differ from the Baseline property if the client specified the line height
        /// </summary>
        public abstract float TextBaseline { get; }


        /// <summary>
        /// Client to get the distance from the before edge of line height 
        /// to the baseline of marker of the line if any.
        /// </summary>
        public abstract float MarkerBaseline { get; }


        /// <summary>
        /// Client to get the overall height of the list items marker of the line if any.
        /// </summary>
        public abstract float MarkerHeight { get; }


        /// <summary>
        /// Client to get the distance covering all black preceding the leading edge of the line.
        /// </summary>
        public abstract float OverhangLeading { get; }


        /// <summary>
        /// Client to get the distance covering all black following the trailing edge of the line.
        /// </summary>
        public abstract float OverhangTrailing { get; }


        /// <summary>
        /// Client to get the distance from the after edge of line height to the after edge of the extent of the line.
        /// </summary>
        public abstract float OverhangAfter { get; }

        public static TextLine Empty { get; } = new EmptyTextLine();
    }

    /// <summary>
    /// Indicate the inversion of axes of the drawing surface
    /// </summary>
    [Flags]
    public enum InvertAxes
    {
        /// <summary>
        /// Drawing surface is not inverted in either axis
        /// </summary>
        None = 0,

        /// <summary>
        /// Drawing surface is inverted in horizontal axis
        /// </summary>
        Horizontal = 1,

        /// <summary>
        /// Drawing surface is inverted in vertical axis
        /// </summary>
        Vertical = 2,

        /// <summary>
        /// Drawing surface is inverted in both axes
        /// </summary>
        Both = (Horizontal | Vertical),
    }
}
