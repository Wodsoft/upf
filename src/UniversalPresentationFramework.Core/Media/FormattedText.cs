using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Wodsoft.UI.Media.TextFormatting;

namespace Wodsoft.UI.Media
{
    public class FormattedText
    {
        #region Construction

        /// <summary>
        /// Construct a FormattedText object.
        /// </summary>
        /// <param name="textToFormat">String of text to be displayed.</param>
        /// <param name="culture">Culture of text.</param>
        /// <param name="flowDirection">Flow direction of text.</param>
        /// <param name="typeface">Type face used to display text.</param>
        /// <param name="emSize">Font em size in visual units (1/96 of an inch).</param>
        /// <param name="foreground">Foreground brush used to render text.</param>
        /// <param name="pixelsPerDip">DPI scale on which to render text</param>
        public FormattedText(
            string textToFormat,
            CultureInfo culture,
            FlowDirection flowDirection,
            Typeface typeface,
            float emSize,
            Brush foreground,
            float pixelsPerDip) : this(
                textToFormat,
                culture,
                flowDirection,
                typeface,
                emSize,
                foreground,
                null,
                TextFormattingMode.Ideal,
                pixelsPerDip
                )

        {
        }


        /// <summary>
        /// Construct a FormattedText object.
        /// </summary>
        /// <param name="textToFormat">String of text to be displayed.</param>
        /// <param name="culture">Culture of text.</param>
        /// <param name="flowDirection">Flow direction of text.</param>
        /// <param name="typeface">Type face used to display text.</param>
        /// <param name="emSize">Font em size in visual units (1/96 of an inch).</param>
        /// <param name="foreground">Foreground brush used to render text.</param>
        /// <param name="numberSubstitution">Number substitution behavior to apply to the text; can be null,
        /// in which case the default number number method for the text culture is used.</param>
        /// <param name="pixelsPerDip">DPI scale on which to render text.</param>
        public FormattedText(
            string textToFormat,
            CultureInfo culture,
            FlowDirection flowDirection,
            Typeface typeface,
            float emSize,
            Brush foreground,
            NumberSubstitution numberSubstitution,
            float pixelsPerDip) : this(
                textToFormat,
                culture,
                flowDirection,
                typeface,
                emSize,
                foreground,
                numberSubstitution,
                TextFormattingMode.Ideal,
                pixelsPerDip
                )
        {
        }

        /// <summary>
        /// Construct a FormattedText object.
        /// </summary>
        /// <param name="textToFormat">String of text to be displayed.</param>
        /// <param name="culture">Culture of text.</param>
        /// <param name="flowDirection">Flow direction of text.</param>
        /// <param name="typeface">Type face used to display text.</param>
        /// <param name="emSize">Font em size in visual units (1/96 of an inch).</param>
        /// <param name="foreground">Foreground brush used to render text.</param>
        /// <param name="numberSubstitution">Number substitution behavior to apply to the text; can be null,
        /// in which case the default number number method for the text culture is used.</param>
        /// <param name="pixelsPerDip">DPI scale on which to render text.</param>
        public FormattedText(
            string textToFormat,
            CultureInfo culture,
            FlowDirection flowDirection,
            Typeface typeface,
            float emSize,
            Brush foreground,
            NumberSubstitution? numberSubstitution,
            TextFormattingMode textFormattingMode,
            float pixelsPerDip)
        {
            if (textToFormat == null)
                throw new ArgumentNullException("textToFormat");

            if (typeface == null)
                throw new ArgumentNullException("typeface");

            ValidateCulture(culture);
            ValidateFlowDirection(flowDirection, "flowDirection");
            ValidateFontSize(emSize);
            _pixelsPerDip = pixelsPerDip;

            _textFormattingMode = textFormattingMode;
            _text = textToFormat;
            GenericTextRunProperties runProps = new GenericTextRunProperties(
                typeface,
                emSize,
                12.0f, // default hinting size
                _pixelsPerDip,
                null, // decorations
                foreground,
                null, // highlight background
                BaselineAlignment.Baseline,
                culture,
                numberSubstitution
                );

            _defaultParaProps = new GenericTextParagraphProperties(
                flowDirection,
                TextAlignment.Left,
                false,
                false,
                runProps,
                TextWrapping.WrapWithOverflow,
                0, // line height not specified
                0 // indentation not specified
                );

            _formatRuns = new ObjectSpans<GenericTextRunProperties>(textToFormat.Length, runProps);
            _textSource = new FormattedTextSource(this);
            InvalidateMetrics();

            _linebreaks = new List<int>();
        }

        /// <summary>
        /// Returns the string of text to be displayed
        /// </summary>
        public string Text
        {
            get { return _text; }
        }

        /// <summary>
        /// Sets the PixelsPerDip at which this text should be rendered. Must be set when creating FormattedObject and updated when DPI changes.
        /// </summary>
        public float PixelsPerDip
        {
            get { return _pixelsPerDip; }
            set
            {
                _pixelsPerDip = value;
                //_defaultParaProps.DefaultTextRunProperties.PixelsPerDip = _pixelsPerDip;
            }
        }

        #endregion

        #region Formatting properties

        private static void ValidateCulture(CultureInfo culture)
        {
            if (culture == null)
                throw new ArgumentNullException("culture");
        }

        private static void ValidateFontSize(float emSize)
        {
            if (emSize <= 0)
                throw new ArgumentOutOfRangeException("emSize", "Em size must be greater than zero.");

            if (emSize > _MaxFontEmSize)
                throw new ArgumentOutOfRangeException("emSize", $"Em size must be less than {_MaxFontEmSize}.");

            if (float.IsNaN(emSize))
                throw new ArgumentOutOfRangeException("emSize", "Em size can't be NaN.");
        }

        private static void ValidateFlowDirection(FlowDirection flowDirection, string parameterName)
        {
            if ((int)flowDirection < 0 || (int)flowDirection > (int)FlowDirection.RightToLeft)
                throw new InvalidEnumArgumentException(parameterName, (int)flowDirection, typeof(FlowDirection));
        }

        private int ValidateRange(int startIndex, int count)
        {
            if (startIndex < 0 || startIndex > _text.Length)
                throw new ArgumentOutOfRangeException("startIndex");

            int limit = startIndex + count;

            if (count < 0 || limit < startIndex || limit > _text.Length)
                throw new ArgumentOutOfRangeException("count");

            return limit - 1;
        }

        private void InvalidateMetrics()
        {
            //_metrics = null;
            _minWidth = float.MinValue;
        }

        /// <summary>
        /// Sets foreground brush used for drawing text
        /// </summary>
        /// <param name="foregroundBrush">Foreground brush</param>
        public void SetForegroundBrush(Brush foregroundBrush)
        {
            SetForegroundBrush(foregroundBrush, 0, _text.Length);
        }

        /// <summary>
        /// Sets foreground brush used for drawing text
        /// </summary>
        /// <param name="foregroundBrush">Foreground brush</param>
        /// <param name="startIndex">The start index of initial character to apply the change to.</param>
        /// <param name="count">The number of characters the change should be applied to.</param>
        public void SetForegroundBrush(Brush foregroundBrush, int startIndex, int count)
        {
            int end = ValidateRange(startIndex, count);
            _formatRuns.SetValue(startIndex, end, (runProps) => new GenericTextRunProperties(
                runProps.Typeface,
                runProps.FontRenderingEmSize,
                runProps.FontHintingEmSize,
                runProps.PixelsPerDip,
                runProps.TextDecorations,
                foregroundBrush,
                runProps.BackgroundBrush,
                runProps.BaselineAlignment,
                runProps.CultureInfo,
                runProps.NumberSubstitution
            ));
        }

        /// <summary>
        /// Sets or changes the font family for the text object 
        /// </summary>
        /// <param name="fontFamily">Font family name</param>
        public void SetFontFamily(string fontFamily)
        {
            SetFontFamily(fontFamily, 0, _text.Length);
        }

        /// <summary>
        /// Sets or changes the font family for the text object 
        /// </summary>
        /// <param name="fontFamily">Font family name</param>
        /// <param name="startIndex">The start index of initial character to apply the change to.</param>
        /// <param name="count">The number of characters the change should be applied to.</param>
        public void SetFontFamily(string fontFamily, int startIndex, int count)
        {
            if (fontFamily == null)
                throw new ArgumentNullException("fontFamily");

            SetFontFamily(new FontFamily(fontFamily), startIndex, count);
        }

        /// <summary>
        /// Sets or changes the font family for the text object 
        /// </summary>
        /// <param name="fontFamily">Font family</param>
        public void SetFontFamily(FontFamily fontFamily)
        {
            SetFontFamily(fontFamily, 0, _text.Length);
        }

        /// <summary>
        /// Sets or changes the font family for the text object 
        /// </summary>
        /// <param name="fontFamily">Font family</param>
        /// <param name="startIndex">The start index of initial character to apply the change to.</param>
        /// <param name="count">The number of characters the change should be applied to.</param>
        public void SetFontFamily(FontFamily fontFamily, int startIndex, int count)
        {
            if (fontFamily == null)
                throw new ArgumentNullException("fontFamily");

            int end = ValidateRange(startIndex, count);
            _formatRuns.SetValue(startIndex, end, (runProps) => new GenericTextRunProperties(
                new Typeface(fontFamily, runProps.Typeface.Style, runProps.Typeface.Weight, runProps.Typeface.Stretch),
                runProps.FontRenderingEmSize,
                runProps.FontHintingEmSize,
                runProps.PixelsPerDip,
                runProps.TextDecorations,
                runProps.ForegroundBrush,
                runProps.BackgroundBrush,
                runProps.BaselineAlignment,
                runProps.CultureInfo,
                runProps.NumberSubstitution
            ));
            InvalidateMetrics();
        }


        /// <summary>
        /// Sets or changes the font em size measured in MIL units
        /// </summary>
        /// <param name="emSize">Font em size</param>
        public void SetFontSize(float emSize)
        {
            SetFontSize(emSize, 0, _text.Length);
        }

        /// <summary>
        /// Sets or changes the font em size measured in MIL units
        /// </summary>
        /// <param name="emSize">Font em size</param>
        /// <param name="startIndex">The start index of initial character to apply the change to.</param>
        /// <param name="count">The number of characters the change should be applied to.</param>
        public void SetFontSize(float emSize, int startIndex, int count)
        {
            ValidateFontSize(emSize);

            int end = ValidateRange(startIndex, count);
            _formatRuns.SetValue(startIndex, end, (runProps) => new GenericTextRunProperties(
                runProps.Typeface,
                emSize,
                runProps.FontHintingEmSize,
                runProps.PixelsPerDip,
                runProps.TextDecorations,
                runProps.ForegroundBrush,
                runProps.BackgroundBrush,
                runProps.BaselineAlignment,
                runProps.CultureInfo,
                runProps.NumberSubstitution
            ));
            InvalidateMetrics();
        }

        /// <summary>
        /// Sets or changes the culture for the text object.
        /// </summary>
        /// <param name="culture">The new culture for the text object.</param>
        public void SetCulture(CultureInfo culture)
        {
            SetCulture(culture, 0, _text.Length);
        }

        /// <summary>
        /// Sets or changes the culture for the text object.
        /// </summary>
        /// <param name="culture">The new culture for the text object.</param>
        /// <param name="startIndex">The start index of initial character to apply the change to.</param>
        /// <param name="count">The number of characters the change should be applied to.</param>
        public void SetCulture(CultureInfo culture, int startIndex, int count)
        {
            ValidateCulture(culture);

            int end = ValidateRange(startIndex, count);
            _formatRuns.SetValue(startIndex, end, (runProps) => new GenericTextRunProperties(
                runProps.Typeface,
                runProps.FontRenderingEmSize,
                runProps.FontHintingEmSize,
                runProps.PixelsPerDip,
                runProps.TextDecorations,
                runProps.ForegroundBrush,
                runProps.BackgroundBrush,
                runProps.BaselineAlignment,
                culture,
                runProps.NumberSubstitution
            ));
            InvalidateMetrics();
        }

        /// <summary>
        /// Sets or changes the number substitution behavior for the text.
        /// </summary>
        /// <param name="numberSubstitution">Number substitution behavior to apply to the text; can be null,
        /// in which case the default number substitution method for the text culture is used.</param>
        public void SetNumberSubstitution(
            NumberSubstitution numberSubstitution
            )
        {
            SetNumberSubstitution(numberSubstitution, 0, _text.Length);
        }

        /// <summary>
        /// Sets or changes the number substitution behavior for a range of text.
        /// </summary>
        /// <param name="numberSubstitution">Number substitution behavior to apply to the text; can be null,
        /// in which case the default number substitution method for the text culture is used.</param>
        /// <param name="startIndex">The start index of initial character to apply the change to.</param>
        /// <param name="count">The number of characters the change should be applied to.</param>
        public void SetNumberSubstitution(
            NumberSubstitution numberSubstitution,
            int startIndex,
            int count
            )
        {
            int end = ValidateRange(startIndex, count);
            _formatRuns.SetValue(startIndex, end, (runProps) => new GenericTextRunProperties(
                runProps.Typeface,
                runProps.FontRenderingEmSize,
                runProps.FontHintingEmSize,
                runProps.PixelsPerDip,
                runProps.TextDecorations,
                runProps.ForegroundBrush,
                runProps.BackgroundBrush,
                runProps.BaselineAlignment,
                runProps.CultureInfo,
                numberSubstitution
            ));
            InvalidateMetrics();
        }

        /// <summary>
        /// Sets or changes the font weight
        /// </summary>
        /// <param name="weight">Font weight</param>
        public void SetFontWeight(FontWeight weight)
        {
            SetFontWeight(weight, 0, _text.Length);
        }

        /// <summary>
        /// Sets or changes the font weight
        /// </summary>
        /// <param name="weight">Font weight</param>
        /// <param name="startIndex">The start index of initial character to apply the change to.</param>
        /// <param name="count">The number of characters the change should be applied to.</param>
        public void SetFontWeight(FontWeight weight, int startIndex, int count)
        {
            int end = ValidateRange(startIndex, count);
            _formatRuns.SetValue(startIndex, end, (runProps) => new GenericTextRunProperties(
                new Typeface(runProps.Typeface.FontFamily, runProps.Typeface.Style, weight, runProps.Typeface.Stretch),
                runProps.FontRenderingEmSize,
                runProps.FontHintingEmSize,
                runProps.PixelsPerDip,
                runProps.TextDecorations,
                runProps.ForegroundBrush,
                runProps.BackgroundBrush,
                runProps.BaselineAlignment,
                runProps.CultureInfo,
                runProps.NumberSubstitution
            ));
            InvalidateMetrics();
        }

        /// <summary>
        /// Sets or changes the font style
        /// </summary>
        /// <param name="style">Font style</param>
        public void SetFontStyle(FontStyle style)
        {
            SetFontStyle(style, 0, _text.Length);
        }

        /// <summary>
        /// Sets or changes the font style
        /// </summary>
        /// <param name="style">Font style</param>
        /// <param name="startIndex">The start index of initial character to apply the change to.</param>
        /// <param name="count">The number of characters the change should be applied to.</param>
        public void SetFontStyle(FontStyle style, int startIndex, int count)
        {
            int end = ValidateRange(startIndex, count);
            _formatRuns.SetValue(startIndex, end, (runProps) => new GenericTextRunProperties(
                new Typeface(runProps.Typeface.FontFamily, style, runProps.Typeface.Weight, runProps.Typeface.Stretch),
                runProps.FontRenderingEmSize,
                runProps.FontHintingEmSize,
                runProps.PixelsPerDip,
                runProps.TextDecorations,
                runProps.ForegroundBrush,
                runProps.BackgroundBrush,
                runProps.BaselineAlignment,
                runProps.CultureInfo,
                runProps.NumberSubstitution
            ));
            InvalidateMetrics();
        }

        /// <summary>
        /// Sets or changes the font stretch
        /// </summary>
        /// <param name="stretch">Font stretch</param>
        public void SetFontStretch(FontStretch stretch)
        {
            SetFontStretch(stretch, 0, _text.Length);
        }

        /// <summary>
        /// Sets or changes the font stretch
        /// </summary>
        /// <param name="stretch">Font stretch</param>
        /// <param name="startIndex">The start index of initial character to apply the change to.</param>
        /// <param name="count">The number of characters the change should be applied to.</param>
        public void SetFontStretch(FontStretch stretch, int startIndex, int count)
        {
            int end = ValidateRange(startIndex, count);
            _formatRuns.SetValue(startIndex, end, (runProps) => new GenericTextRunProperties(
                new Typeface(runProps.Typeface.FontFamily, runProps.Typeface.Style, runProps.Typeface.Weight, stretch),
                runProps.FontRenderingEmSize,
                runProps.FontHintingEmSize,
                runProps.PixelsPerDip,
                runProps.TextDecorations,
                runProps.ForegroundBrush,
                runProps.BackgroundBrush,
                runProps.BaselineAlignment,
                runProps.CultureInfo,
                runProps.NumberSubstitution
            ));
            InvalidateMetrics();
        }

        /// <summary>
        /// Sets or changes the type face
        /// </summary>
        /// <param name="typeface">Typeface</param>
        public void SetFontTypeface(Typeface typeface)
        {
            SetFontTypeface(typeface, 0, _text.Length);
        }

        /// <summary>
        /// Sets or changes the type face
        /// </summary>
        /// <param name="typeface">Typeface</param>
        /// <param name="startIndex">The start index of initial character to apply the change to.</param>
        /// <param name="count">The number of characters the change should be applied to.</param>
        public void SetFontTypeface(Typeface typeface, int startIndex, int count)
        {
            int end = ValidateRange(startIndex, count);
            _formatRuns.SetValue(startIndex, end, (runProps) => new GenericTextRunProperties(
                typeface,
                runProps.FontRenderingEmSize,
                runProps.FontHintingEmSize,
                runProps.PixelsPerDip,
                runProps.TextDecorations,
                runProps.ForegroundBrush,
                runProps.BackgroundBrush,
                runProps.BaselineAlignment,
                runProps.CultureInfo,
                runProps.NumberSubstitution
            ));
            InvalidateMetrics();
        }

        /// <summary>
        /// Sets or changes the text decorations
        /// </summary>
        /// <param name="textDecorations">Text decorations</param>
        public void SetTextDecorations(TextDecorationCollection textDecorations)
        {
            SetTextDecorations(textDecorations, 0, _text.Length);
        }

        /// <summary>
        /// Sets or changes the text decorations
        /// </summary>
        /// <param name="textDecorations">Text decorations</param>
        /// <param name="startIndex">The start index of initial character to apply the change to.</param>
        /// <param name="count">The number of characters the change should be applied to.</param>
        public void SetTextDecorations(TextDecorationCollection textDecorations, int startIndex, int count)
        {
            int end = ValidateRange(startIndex, count);
            _formatRuns.SetValue(startIndex, end, (runProps) => new GenericTextRunProperties(
                runProps.Typeface,
                runProps.FontRenderingEmSize,
                runProps.FontHintingEmSize,
                runProps.PixelsPerDip,
                textDecorations,
                runProps.ForegroundBrush,
                runProps.BackgroundBrush,
                runProps.BaselineAlignment,
                runProps.CultureInfo,
                runProps.NumberSubstitution
            ));
        }

        #endregion

        #region Line Enumerator

        private struct LineEnumerator : IEnumerator<TextLine>, IDisposable
        {
            int _textStorePosition;
            int _lineCount;
            float _totalHeight;
            TextLine? _currentLine;
            TextLine? _nextLine;
            TextFormatter _formatter;
            FormattedText _that;

            // these are needed because _currentLine can be disposed before the next MoveNext() call
            float _previousHeight;
            int _previousLength;

            internal LineEnumerator(FormattedText text)
            {
                _previousHeight = 0;
                _previousLength = 0;

                _textStorePosition = 0;
                _lineCount = 0;
                _totalHeight = 0;
                _currentLine = null;
                _nextLine = null;
                _formatter = GenericTextFormatter.Default;
                _that = text;
            }

            public void Dispose()
            {
                if (_currentLine != null)
                {
                    _currentLine.Dispose();
                    _currentLine = null;
                }

                if (_nextLine != null)
                {
                    _nextLine.Dispose();
                    _nextLine = null;
                }
            }

            internal int Position
            {
                get
                {
                    return _textStorePosition;
                }
            }

            internal int Length
            {
                get
                {
                    return _previousLength;
                }
            }

            /// <summary>
            /// Gets the current text line in the collection
            /// </summary>
            public TextLine Current
            {
                get
                {
                    return _currentLine ?? TextLine.Empty;
                }
            }

            /// <summary>
            /// Gets the current text line in the collection
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            /// <summary>
            /// Gets the paragraph width used to format the current text line
            /// </summary>
            internal float CurrentParagraphWidth
            {
                get
                {
                    return MaxLineLength(_lineCount);
                }
            }

            private float MaxLineLength(int line)
            {
                if (_that._maxTextWidths == null)
                    return _that._maxTextWidth;
                return _that._maxTextWidths[Math.Min(line, _that._maxTextWidths.Length - 1)];
            }

            /// <summary>
            /// Advances the enumerator to the next text line of the collection
            /// </summary>
            /// <returns>true if the enumerator was successfully advanced to the next element;
            /// false if the enumerator has passed the end of the collection</returns>
            public bool MoveNext()
            {
                if (_currentLine == null)
                {   // this is the first line
                    if (_that._text.Length == 0)
                        return false;

                    _currentLine = FormatLine(
                        _that._textSource,
                        _textStorePosition,
                        MaxLineLength(_lineCount),
                        _that._defaultParaProps);

                    if (_currentLine == TextLine.Empty)
                        return false;

                    // check if this line fits the text height
                    if (_totalHeight + _currentLine.Height > _that._maxTextHeight)
                    {
                        _currentLine.Dispose();
                        _currentLine = null;
                        return false;
                    }
                    Debug.Assert(_nextLine == null);
                }
                else
                {
                    // there is no next line or it didn't fit
                    // either way we're finished
                    if (_nextLine == null)
                        return false;

                    _totalHeight += _previousHeight;
                    _textStorePosition += _previousLength;
                    ++_lineCount;

                    _currentLine = _nextLine;
                    _nextLine = null;
                }

                if (_that._trimming != TextTrimming.None && _currentLine.HasOverflowed)
                    _currentLine = _currentLine.Collapse(_that._trimming, MaxLineLength(_lineCount), false, out _nextLine);

                // this line is guaranteed to fit the text height
                Debug.Assert(_totalHeight + _currentLine.Height <= _that._maxTextHeight);

                // now, check if the next line fits, we need to do this on this iteration
                // because we might need to add ellipsis to the current line
                // as a result of the next line measurement

                // maybe there is no next line at all
                var currentTextRuns = _currentLine.GetTextRunSpans();
                var lastTextRun = currentTextRuns[currentTextRuns.Count - 1];
                var lastTextPosition = lastTextRun.Start + lastTextRun.Length;
                if (lastTextPosition < _that._text.Length)
                {
                    bool nextLineFits;

                    if (_lineCount + 1 >= _that._maxLineCount)
                        nextLineFits = false;
                    else
                    {
                        if (_nextLine == null)
                            _nextLine = FormatLine(
                                _that._textSource,
                                lastTextPosition,
                                MaxLineLength(_lineCount + 1),
                                _that._defaultParaProps);
                        nextLineFits = (_totalHeight + _currentLine.Height + _nextLine.Height <= _that._maxTextHeight);
                    }

                    if (!nextLineFits)
                    {
                        // next line doesn't fit
                        if (_nextLine != null)
                        {
                            _nextLine.Dispose();
                            _nextLine = null;
                        }

                        if (_that._trimming != TextTrimming.None && !_currentLine.HasCollapsed)
                        {
                            // recreate the current line with ellipsis added
                            // Note: Paragraph ellipsis is not supported today. We'll workaround
                            // it here by faking a non-wrap text on finite column width.
                            TextWrapping currentWrap = _that._defaultParaProps.TextWrapping;
                            _that._defaultParaProps.SetTextWrapping(TextWrapping.NoWrap);

                            _currentLine.Dispose();
                            _currentLine = FormatLine(
                                _that._textSource,
                                _textStorePosition,
                                MaxLineLength(_lineCount),
                                _that._defaultParaProps);

                            _that._defaultParaProps.SetTextWrapping(currentWrap);
                        }
                    }
                }
                _previousHeight = _currentLine.Height;
                _previousLength = _currentLine.Length;
                return true;
            }


            /// <summary>
            /// Wrapper of TextFormatter.FormatLine that auto-collapses the line if needed.
            /// </summary>
            private TextLine FormatLine(TextSource textSource, int textSourcePosition, float maxLineLength, TextParagraphProperties paraProps)
            {
                TextLine line = _formatter.FormatLine(
                    textSource,
                    textSourcePosition,
                    maxLineLength,
                    paraProps);

                if (line == TextLine.Empty)
                    return line;

                //if (_that._trimming != TextTrimming.None && line.HasOverflowed && line.Length > 0)
                //{
                //    // what I really need here is the last displayed text run of the line
                //    // textSourcePosition + line.Length - 1 works except the end of paragraph case,
                //    // where line length includes the fake paragraph break run
                //    Debug.Assert(_that._text.Length > 0 && textSourcePosition + line.Length <= _that._text.Length + 1);

                //    TextLine collapsedLine = line.Collapse(_that._trimming, maxLineLength);

                //    if (collapsedLine != line)
                //    {
                //        line.Dispose();
                //        line = collapsedLine;
                //    }
                //}
                return line;
            }


            /// <summary>
            /// Sets the enumerator to its initial position,
            /// which is before the first element in the collection
            /// </summary>
            public void Reset()
            {
                _textStorePosition = 0;
                _lineCount = 0;
                _totalHeight = 0;
                _currentLine = null;
                _nextLine = null;
            }
        }

        private class FormattedTextSource : TextSource
        {
            private readonly FormattedText _formattedText;

            public FormattedTextSource(FormattedText formattedText)
            {
                _formattedText = formattedText;
            }

            public override TextRun GetTextRun(int textSourceCharacterIndex)
            {
                if (textSourceCharacterIndex < 0 || textSourceCharacterIndex >= _formattedText._text.Length)
                    throw new ArgumentOutOfRangeException(nameof(textSourceCharacterIndex));
                var span = _formattedText._formatRuns.FindSpan(textSourceCharacterIndex);
                return new FormattedTextRun(_formattedText, textSourceCharacterIndex, span.End - textSourceCharacterIndex + 1, span.Value);
            }

            public override int Length => _formattedText._text.Length;
        }

        private class FormattedTextRun : TextRun
        {
            private readonly FormattedText _formattedText;
            private readonly int _start;
            private readonly int _length;
            private readonly GenericTextRunProperties _properties;

            public FormattedTextRun(FormattedText formattedText, int start, int length, GenericTextRunProperties properties)
            {
                _formattedText = formattedText;
                _start = start;
                _length = length;
                _properties = properties;
            }

            public override ReadOnlySpan<char> Characters => _formattedText._text.AsSpan(_start, Length);

            public override int Length => _length;

            public override TextRunProperties Properties => _properties;

            public override bool IsEndOfNewLine => _formattedText._text[_start + Length - 1] == '\n';

            public override int Start => _start;

            public override int GetNextLineBreakPosition()
            {
                var end = _start + _length;
                if (_formattedText._calculatedLinebreak < end)
                {
                    var span = _formattedText._text.AsSpan().Slice(_formattedText._calculatedLinebreak, end - _formattedText._calculatedLinebreak);
                    int index = span.IndexOf('\n');
                    if (index == -1)
                    {
                        _formattedText._calculatedLinebreak = end;
                        return -1;
                    }
                    else
                    {
                        _formattedText._calculatedLinebreak += index;
                        _formattedText._linebreaks.Add(_formattedText._calculatedLinebreak);
                        _formattedText._calculatedLinebreak++;
                        return index;
                    }
                }
                else
                {
                    var i = _formattedText._linebreaks.FindIndex(t => t >= _start && t < end);
                    if (i == -1)
                        return -1;
                    return _formattedText._linebreaks[i];
                }
            }

            public override TextRun Slice(int start, int length)
            {
                if (start < 0)
                    throw new ArgumentOutOfRangeException(nameof(start));
                if (start + length > _length)
                    throw new ArgumentOutOfRangeException(nameof(length));
                return new FormattedTextRun(_formattedText, _start + start, length, _properties);
            }
        }

        /// <summary>
        /// Returns an enumerator that can iterate through the text line collection
        /// </summary>
        private LineEnumerator GetEnumerator()
        {
            return new LineEnumerator(this);
        }

        private void AdvanceLineOrigin(ref Point lineOrigin, TextLine currentLine)
        {
            float height = currentLine.Height;
            // advance line origin according to the flow direction
            switch (_defaultParaProps.FlowDirection)
            {
                case FlowDirection.LeftToRight:
                case FlowDirection.RightToLeft:
                    lineOrigin.Y += height;
                    break;
            }
        }

        #endregion

        #region Measurement and layout properties

        private class CachedMetrics
        {
            // vertical
            public float Height;
            public float Baseline;

            // horizontal
            public float Width;
            public float WidthIncludingTrailingWhitespace;

            // vertical bounding box metrics
            public float Extent;
            public float OverhangAfter;

            // horizontal bounding box metrics
            public float OverhangLeading;
            public float OverhangTrailing;
        }

        /// <summary>
        /// Defines the flow direction
        /// </summary>
        public FlowDirection FlowDirection
        {
            set
            {
                ValidateFlowDirection(value, "value");
                _defaultParaProps.SetFlowDirection(value);
                InvalidateMetrics();
            }
            get
            {
                return _defaultParaProps.FlowDirection;
            }
        }

        /// <summary>
        /// Defines the alignment of text within the column
        /// </summary>
        public TextAlignment TextAlignment
        {
            set
            {
                _defaultParaProps.SetTextAlignment(value);
                InvalidateMetrics();
            }
            get
            {
                return _defaultParaProps.TextAlignment;
            }
        }

        /// <summary>
        /// Gets or sets the height of, or the spacing between, each line where
        /// zero represents the default line height.
        /// </summary>
        public float LineHeight
        {
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", "LineHeight can not be negative.");

                _defaultParaProps.SetLineHeight(value);
                InvalidateMetrics();
            }
            get
            {
                return _defaultParaProps.LineHeight;
            }
        }

        /// <summary>
        /// The MaxTextWidth property defines the alignment edges for the FormattedText.
        /// For example, left aligned text is wrapped such that the leftmost glyph alignment point
        /// on each line falls exactly on the left edge of the rectangle.
        /// Note that for many fonts, especially in italic style, some glyph strokes may extend beyond the edges of the alignment rectangle.
        /// For this reason, it is recommended that clients draw text with at least 1/6 em (i.e of the font size) unused margin space either side.
        /// Zero value of MaxTextWidth is equivalent to the maximum possible paragraph width.
        /// </summary>
        public float MaxTextWidth
        {
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", "MaxTextWidth can not be negative.");
                _maxTextWidth = value;
                InvalidateMetrics();
            }
            get
            {
                return _maxTextWidth;
            }
        }

        /// <summary>
        /// Sets the array of lengths,
        /// which will be applied to each line of text in turn.
        /// If the text covers more lines than there are entries in the length array,
        /// the last entry is reused as many times as required.
        /// The maxTextWidths array overrides the MaxTextWidth property.
        /// </summary>
        /// <param name="maxTextWidths">The max text width array</param>
        public void SetMaxTextWidths(float[] maxTextWidths)
        {
            if (maxTextWidths == null || maxTextWidths.Length <= 0)
                throw new ArgumentNullException("maxTextWidths");
            _maxTextWidths = maxTextWidths;
            InvalidateMetrics();
        }

        /// <summary>
        /// Obtains a copy of the array of lengths,
        /// which will be applied to each line of text in turn.
        /// If the text covers more lines than there are entries in the length array,
        /// the last entry is reused as many times as required.
        /// The maxTextWidths array overrides the MaxTextWidth property.
        /// </summary>
        /// <returns>The copy of max text width array</returns>
        public IReadOnlyList<float>? GetMaxTextWidths()
        {
            return _maxTextWidths?.AsReadOnly();
        }

        /// <summary>
        /// Sets the maximum length of a column of text.
        /// The last line of text displayed is the last whole line that will fit within this limit,
        /// or the nth line as specified by MaxLineCount, whichever occurs first.
        /// Use the Trimming property to control how the omission of text is indicated.
        /// </summary>
        public float MaxTextHeight
        {
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value", "MaxTextHeight must be greater than zero.");

                if (float.IsNaN(value))
                    throw new ArgumentOutOfRangeException("value", "MaxTextHeight can not be NaN.");

                _maxTextHeight = value;
                InvalidateMetrics();
            }
            get
            {
                return _maxTextHeight;
            }
        }

        /// <summary>
        /// Defines the maximum number of lines to display.
        /// The last line of text displayed is the lineCount-1'th line,
        /// or the last whole line that will fit within the count set by MaxTextHeight,
        /// whichever occurs first.
        /// Use the Trimming property to control how the omission of text is indicated
        /// </summary>
        public int MaxLineCount
        {
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value", "MaxLineCount must be greater than zero.");
                _maxLineCount = value;
                InvalidateMetrics();
            }
            get
            {
                return _maxLineCount;
            }
        }


        /// <summary>
        /// Defines how omission of text is indicated.
        /// CharacterEllipsis trimming allows partial words to be displayed,
        /// while WordEllipsis removes whole words to fit.
        /// Both guarantee to include an ellipsis ('...') at the end of the lines
        /// where text has been trimmed as a result of line and column limits.
        /// </summary>
        public TextTrimming Trimming
        {
            set
            {
                if ((int)value < 0 || (int)value > (int)TextTrimming.WordEllipsis)
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(TextTrimming));

                _trimming = value;
                if (_trimming == TextTrimming.None)
                {
                    // if trimming is disabled, enforce emergency wrap
                    _defaultParaProps.SetTextWrapping(TextWrapping.Wrap);
                }
                else
                {
                    _defaultParaProps.SetTextWrapping(TextWrapping.WrapWithOverflow);
                }

                InvalidateMetrics();
            }
            get
            {
                return _trimming;
            }
        }

        /// <summary>
        /// Lazily initializes the cached metrics EXCEPT for black box metrics and
        /// returns the CachedMetrics structure.
        /// </summary>
        private CachedMetrics Metrics
        {
            get
            {
                if (_metrics == null)
                {
                    // We need to obtain the metrics. DON'T compute black box metrics here because
                    // they probably won't be needed and computing them requires GlyphRun creation. 
                    // In the common case where a client measures and then draws, we'll format twice 
                    // but create GlyphRuns only during drawing.

                    _metrics = DrawAndCalculateMetrics(
                        null,           // drawing context
                        new Point(),    // drawing offset
                        false);         // don't calculate black box metrics
                }
                return _metrics;
            }
        }

        /// <summary>
        /// Lazily initializes the cached metrics INCLUDING black box metrics and
        /// returns the CachedMetrics structure.
        /// </summary>
        private CachedMetrics BlackBoxMetrics
        {
            get
            {
                if (_metrics == null || float.IsNaN(_metrics.Extent))
                {
                    // We need to obtain the metrics, including black box metrics.

                    _metrics = DrawAndCalculateMetrics(
                        null,           // drawing context
                        new Point(),    // drawing offset
                        true);          // calculate black box metrics
                }
                return _metrics;
            }
        }


        /// <summary>
        /// The distance from the top of the first line to the bottom of the last line.
        /// </summary>
        public float Height
        {
            get
            {
                return Metrics.Height;
            }
        }

        /// <summary>
        /// The distance from the topmost black pixel of the first line
        /// to the bottommost black pixel of the last line. 
        /// </summary>
        public float Extent
        {
            get
            {
                return BlackBoxMetrics.Extent;
            }
        }

        /// <summary>
        /// The distance from the top of the first line to the baseline of the first line.
        /// </summary>
        public float Baseline
        {
            get
            {
                return Metrics.Baseline;
            }
        }

        /// <summary>
        /// The distance from the bottom of the last line to the extent bottom.
        /// </summary>
        public float OverhangAfter
        {
            get
            {
                return BlackBoxMetrics.OverhangAfter;
            }
        }

        /// <summary>
        /// The maximum distance from the leading black pixel to the leading alignment point of a line.
        /// </summary>
        public float OverhangLeading
        {
            get
            {
                return BlackBoxMetrics.OverhangLeading;
            }
        }

        /// <summary>
        /// The maximum distance from the trailing black pixel to the trailing alignment point of a line.
        /// </summary>
        public float OverhangTrailing
        {
            get
            {
                return BlackBoxMetrics.OverhangTrailing;
            }
        }

        /// <summary>
        /// The maximum advance width between the leading and trailing alignment points of a line,
        /// excluding the width of whitespace characters at the end of the line.
        /// </summary>
        public float Width
        {
            get
            {
                return Metrics.Width;
            }
        }

        /// <summary>
        /// The maximum advance width between the leading and trailing alignment points of a line,
        /// including the width of whitespace characters at the end of the line.
        /// </summary>
        public float WidthIncludingTrailingWhitespace
        {
            get
            {
                return Metrics.WidthIncludingTrailingWhitespace;
            }
        }

        ///// <summary>
        ///// The minimum line width that can be specified without causing any word to break. 
        ///// </summary>
        //public float MinWidth
        //{
        //    get
        //    {
        //        if (_minWidth != float.MinValue)
        //            return _minWidth;

        //        _minWidth = GenericTextFormatter.Default.FormatMinMaxParagraphWidth(
        //            _textSource,
        //            0,  // textSourceCharacterIndex
        //            _defaultParaProps
        //            ).MinWidth;

        //        return _minWidth;
        //    }
        //}

        #endregion

        #region Drawing

        /// <summary>
        /// Draws the text object
        /// </summary>
        internal void Draw(DrawingContext dc, Point origin)
        {
            Point lineOrigin = origin;

            if (_metrics != null && !float.IsNaN(_metrics.Extent))
            {
                // we can't use foreach because it requires GetEnumerator and associated classes to be public
                // foreach (TextLine currentLine in this)

                using (LineEnumerator enumerator = GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        using (TextLine currentLine = enumerator.Current)
                        {
                            currentLine.Draw(dc, lineOrigin, InvertAxes.None);
                            AdvanceLineOrigin(ref lineOrigin, currentLine);
                        }
                    }
                }
            }
            else
            {
                // Calculate metrics as we draw to avoid formatting again if we need metrics later; we compute
                // black box metrics too because these are already known as a side-effect of drawing

                _metrics = DrawAndCalculateMetrics(dc, origin, true);
            }
        }

        private CachedMetrics DrawAndCalculateMetrics(DrawingContext? dc, Point drawingOffset, bool getBlackBoxMetrics)
        {
            // The calculation for FormattedText.Width and Overhangs was wrong for Right and Center alignment.
            // Thus the fix of this bug is based on the fact that FormattedText always had 0 indent and no 
            // TextMarkerProperties. These assumptions enabled us to remove TextLine.Start from the calculation 
            // of the Width. TextLine.Start caused the calculation of FormattedText to be incorrect in cases 
            // of Right and Center alignment because it took on -ve values when ParagraphWidth was 0 (which indicates infinite width). 
            // This was a result of how TextFormatter interprets TextLine.Start. In the simplest case, it computes 
            // TextLine.Start as Paragraph Width - Line Width (for Right alignment).
            // So, the following two Debug.Asserts verify that the assumptions over which the bug fix was made are still valid 
            // and not changed by adding features to FormattedText. Incase these assumptions were invalidated, the bug fix 
            // should be revised and it will possibly involve alot of changes elsewhere.
            Debug.Assert(_defaultParaProps.Indent == 0.0, "FormattedText was assumed to always have 0 indent. This assumption has changed and thus the calculation of Width and Overhangs should be revised.");
            //Debug.Assert(_defaultParaProps.TextMarkerProperties == null, "FormattedText was assumed to always have no TextMarkerProperties. This assumption has changed and thus the calculation of Width and Overhangs should be revised.");
            CachedMetrics metrics = new CachedMetrics();

            if (_text.Length == 0)
            {
                return metrics;
            }

            // we can't use foreach because it requires GetEnumerator and associated classes to be public
            // foreach (TextLine currentLine in this)

            using (LineEnumerator enumerator = GetEnumerator())
            {
                bool first = true;

                float accBlackBoxLeft, accBlackBoxTop, accBlackBoxRight, accBlackBoxBottom;
                accBlackBoxLeft = accBlackBoxTop = float.MaxValue;
                accBlackBoxRight = accBlackBoxBottom = float.MinValue;

                Point origin = new Point(0, 0);

                // Holds the TextLine.Start of the longest line. Thus it will hold the minimum value 
                // of TextLine.Start among all the lines that forms the text. The overhangs (leading and trailing) 
                // are calculated with an offset as a result of the same issue with TextLine.Start. 
                // So, we compute this offset and remove it later from the values of the overhangs.
                float lineStartOfLongestLine = float.MaxValue;
                while (enumerator.MoveNext())
                {
                    // enumerator will dispose the currentLine
                    using (TextLine currentLine = enumerator.Current)
                    {
                        // if we're drawing, do it first as this will compute black box metrics as a side-effect
                        if (dc != null)
                        {
                            currentLine.Draw(
                                dc,
                                new Point(origin.X + drawingOffset.X, origin.Y + drawingOffset.Y),
                                InvertAxes.None
                                );
                        }

                        if (getBlackBoxMetrics)
                        {
                            float blackBoxLeft = origin.X + currentLine.Start + currentLine.OverhangLeading;
                            float blackBoxRight = origin.X + currentLine.Start + currentLine.Width - currentLine.OverhangTrailing;
                            float blackBoxBottom = origin.Y + currentLine.Height + currentLine.OverhangAfter;
                            float blackBoxTop = blackBoxBottom - currentLine.Extent;

                            accBlackBoxLeft = Math.Min(accBlackBoxLeft, blackBoxLeft);
                            accBlackBoxRight = Math.Max(accBlackBoxRight, blackBoxRight);
                            accBlackBoxBottom = Math.Max(accBlackBoxBottom, blackBoxBottom);
                            accBlackBoxTop = Math.Min(accBlackBoxTop, blackBoxTop);

                            metrics.OverhangAfter = currentLine.OverhangAfter;
                        }

                        metrics.Height += currentLine.Height;
                        metrics.Width = Math.Max(metrics.Width, currentLine.Width);
                        metrics.WidthIncludingTrailingWhitespace = Math.Max(metrics.WidthIncludingTrailingWhitespace, currentLine.WidthIncludingTrailingWhitespace);
                        lineStartOfLongestLine = Math.Min(lineStartOfLongestLine, currentLine.Start);

                        if (first)
                        {
                            metrics.Baseline = currentLine.Baseline;
                            first = false;
                        }
                        AdvanceLineOrigin(ref origin, currentLine);
                    }
                }

                if (getBlackBoxMetrics)
                {
                    metrics.Extent = accBlackBoxBottom - accBlackBoxTop;
                    metrics.OverhangLeading = accBlackBoxLeft - lineStartOfLongestLine;
                    metrics.OverhangTrailing = metrics.Width - (accBlackBoxRight - lineStartOfLongestLine);
                }
                else
                {
                    // indicate that black box metrics are not known
                    metrics.Extent = float.NaN;
                }
            }

            return metrics;
        }

        #endregion

        #region Fields

        const float _MaxFontEmSize = Constants.RealInfiniteWidth / Constants.GreatestMutiplierOfEm;

        // properties and format runs
        private readonly string _text;
        private float _pixelsPerDip = 1f;
        private readonly ObjectSpans<GenericTextRunProperties> _formatRuns;
        private readonly List<int> _linebreaks;

        private int _calculatedLinebreak;

        private GenericTextParagraphProperties _defaultParaProps;

        private float _maxTextWidth;
        private float[]? _maxTextWidths;
        private float _maxTextHeight = float.MaxValue;
        private int _maxLineCount = int.MaxValue;
        private TextTrimming _trimming = TextTrimming.WordEllipsis;

        private readonly TextFormattingMode _textFormattingMode;

        // text source callbacks
        private readonly FormattedTextSource _textSource;

        // cached metrics
        private CachedMetrics? _metrics;
        private float _minWidth;

        #endregion
    }
}
