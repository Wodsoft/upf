using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Controls;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Documents
{
    public class Figure : AnchoredBlock
    {
        #region Constructors

        /// <summary>
        /// Initialized the new instance of a Figure
        /// </summary>
        public Figure()
        {
        }

        /// <summary>
        /// Initialized the new instance of a Figure specifying a Block added
        /// to a Figure as its first child.
        /// </summary>
        /// <param name="childBlock">
        /// Block added as a first initial child of the Figure.
        /// </param>
        public Figure(Block childBlock) : base(childBlock)
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// DependencyProperty for <see cref="HorizontalAnchor" /> property.
        /// </summary>
        public static readonly DependencyProperty HorizontalAnchorProperty =
                DependencyProperty.Register(
                        "HorizontalAnchor",
                        typeof(FigureHorizontalAnchor),
                        typeof(Figure),
                        new FrameworkPropertyMetadata(
                                FigureHorizontalAnchor.ColumnRight,
                                FrameworkPropertyMetadataOptions.AffectsParentMeasure),
                        new ValidateValueCallback(IsValidHorizontalAnchor));
        private static bool IsValidHorizontalAnchor(object? o)
        {
            FigureHorizontalAnchor value = (FigureHorizontalAnchor)o!;
            return value == FigureHorizontalAnchor.ContentCenter
                || value == FigureHorizontalAnchor.ContentLeft
                || value == FigureHorizontalAnchor.ContentRight
                || value == FigureHorizontalAnchor.PageCenter
                || value == FigureHorizontalAnchor.PageLeft
                || value == FigureHorizontalAnchor.PageRight
                || value == FigureHorizontalAnchor.ColumnCenter
                || value == FigureHorizontalAnchor.ColumnLeft
                || value == FigureHorizontalAnchor.ColumnRight;
            // || value == FigureHorizontalAnchor.CharacterCenter
            // || value == FigureHorizontalAnchor.CharacterLeft
            // || value == FigureHorizontalAnchor.CharacterRight;
        }
        public FigureHorizontalAnchor HorizontalAnchor
        {
            get { return (FigureHorizontalAnchor)GetValue(HorizontalAnchorProperty)!; }
            set { SetValue(HorizontalAnchorProperty, value); }
        }

        /// <summary>
        /// DependencyProperty for <see cref="VerticalAnchor" /> property.
        /// </summary>
        public static readonly DependencyProperty VerticalAnchorProperty =
                DependencyProperty.Register(
                        "VerticalAnchor",
                        typeof(FigureVerticalAnchor),
                        typeof(Figure),
                        new FrameworkPropertyMetadata(
                                FigureVerticalAnchor.ParagraphTop,
                                FrameworkPropertyMetadataOptions.AffectsParentMeasure),
                        new ValidateValueCallback(IsValidVerticalAnchor));
        private static bool IsValidVerticalAnchor(object? o)
        {
            FigureVerticalAnchor value = (FigureVerticalAnchor)o!;
            return value == FigureVerticalAnchor.ContentBottom
                || value == FigureVerticalAnchor.ContentCenter
                || value == FigureVerticalAnchor.ContentTop
                || value == FigureVerticalAnchor.PageBottom
                || value == FigureVerticalAnchor.PageCenter
                || value == FigureVerticalAnchor.PageTop
                || value == FigureVerticalAnchor.ParagraphTop;
            // || value == FigureVerticalAnchor.CharacterBottom
            // || value == FigureVerticalAnchor.CharacterCenter
            // || value == FigureVerticalAnchor.CharacterTop;
        }
        public FigureVerticalAnchor VerticalAnchor
        {
            get { return (FigureVerticalAnchor)GetValue(VerticalAnchorProperty)!; }
            set { SetValue(VerticalAnchorProperty, value); }
        }

        /// <summary>
        /// DependencyProperty for <see cref="HorizontalOffset" /> property.
        /// </summary>
        public static readonly DependencyProperty HorizontalOffsetProperty =
                DependencyProperty.Register(
                        "HorizontalOffset",
                        typeof(float),
                        typeof(Figure),
                        new FrameworkPropertyMetadata(
                                0f,
                                FrameworkPropertyMetadataOptions.AffectsParentMeasure),
                        new ValidateValueCallback(IsValidOffset));
        [TypeConverter(typeof(LengthConverter))]
        public float HorizontalOffset
        {
            get { return (float)GetValue(HorizontalOffsetProperty)!; }
            set { SetValue(HorizontalOffsetProperty, value); }
        }

        /// <summary>
        /// DependencyProperty for <see cref="VerticalOffset" /> property.
        /// </summary>
        public static readonly DependencyProperty VerticalOffsetProperty =
                DependencyProperty.Register(
                        "VerticalOffset",
                        typeof(float),
                        typeof(Figure),
                        new FrameworkPropertyMetadata(
                                0f,
                                FrameworkPropertyMetadataOptions.AffectsParentMeasure),
                        new ValidateValueCallback(IsValidOffset));
        [TypeConverter(typeof(LengthConverter))]
        public float VerticalOffset
        {
            get { return (float)GetValue(VerticalOffsetProperty)!; }
            set { SetValue(VerticalOffsetProperty, value); }
        }

        private static bool IsValidOffset(object? o)
        {
            float offset = (float)o!;
            float maxOffset = 1000000;
            float minOffset = -maxOffset;
            if (float.IsNaN(offset))
            {
                return false;
            }
            if (offset < minOffset || offset > maxOffset)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// DependencyProperty for <see cref="CanDelayPlacement" /> property.
        /// </summary>
        public static readonly DependencyProperty CanDelayPlacementProperty =
                DependencyProperty.Register(
                        "CanDelayPlacement",
                        typeof(bool),
                        typeof(Figure),
                        new FrameworkPropertyMetadata(
                                true,
                                FrameworkPropertyMetadataOptions.AffectsParentMeasure));
        public bool CanDelayPlacement
        {
            get { return (bool)GetValue(CanDelayPlacementProperty)!; }
            set { SetValue(CanDelayPlacementProperty, value); }
        }

        /// <summary>
        /// DependencyProperty for <see cref="WrapDirection" /> property.
        /// </summary>
        public static readonly DependencyProperty WrapDirectionProperty =
                DependencyProperty.Register(
                        "WrapDirection",
                        typeof(WrapDirection),
                        typeof(Figure),
                        new FrameworkPropertyMetadata(
                                WrapDirection.Both,
                                FrameworkPropertyMetadataOptions.AffectsParentMeasure),
                        new ValidateValueCallback(IsValidWrapDirection));
        private static bool IsValidWrapDirection(object? o)
        {
            WrapDirection value = (WrapDirection)o!;
            return value == WrapDirection.Both
                || value == WrapDirection.None
                || value == WrapDirection.Left
                || value == WrapDirection.Right;
        }
        public WrapDirection WrapDirection
        {
            get { return (WrapDirection)GetValue(WrapDirectionProperty)!; }
            set { SetValue(WrapDirectionProperty, value); }
        }

        /// <summary>
        /// DependencyProperty for <see cref="Width" /> property.
        /// </summary>
        public static readonly DependencyProperty WidthProperty =
                DependencyProperty.Register(
                        "Width",
                        typeof(FigureLength),
                        typeof(Figure),
                        new FrameworkPropertyMetadata(
                                new FigureLength(1f, FigureUnitType.Auto),
                                FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// The Width property specifies the width of the element.
        /// </summary>
        public FigureLength Width
        {
            get { return (FigureLength)GetValue(WidthProperty)!; }
            set { SetValue(WidthProperty, value); }
        }

        /// <summary>
        /// DependencyProperty for <see cref="Height" /> property.
        /// </summary>
        public static readonly DependencyProperty HeightProperty =
                DependencyProperty.Register(
                        "Height",
                        typeof(FigureLength),
                        typeof(Figure),
                        new FrameworkPropertyMetadata(
                                new FigureLength(1f, FigureUnitType.Auto),
                                FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// The Height property specifies the height of the element.
        /// </summary>
        public FigureLength Height
        {
            get { return (FigureLength)GetValue(HeightProperty)!; }
            set { SetValue(HeightProperty, value); }
        }

        #endregion

        #region Layout

        private FigureLayout? _layout;
        public override IInlineLayout Layout => _layout ??= new FigureLayout(this);

        private class FigureLayout : IInlineLayout
        {
            private readonly Figure _figure;
            private readonly List<TextPosition> _textPositions = new List<TextPosition>();
            private float _left, _top, _right, _bottom;

            public FigureLayout(Figure figure)
            {
                _figure = figure;
            }

            public bool IsFloat => true;

            public void Draw(DrawingContext drawingContext, in Rect origin, in Rect clip, in float lineHeight, ReadOnlySpan<float> widths, TextPointer start, TextPointer end)
            {
                float left = _left;
                float top = _top;
                float right = _right;
                float bottom = _bottom;
                var brush = _figure.Background;
                if (brush != null)
                {
                    var padding = _figure.Padding;
                    drawingContext.DrawRectangle(brush, null, new Rect(origin.X + padding.Left, origin.Y + padding.Top, right - left - padding.Left - padding.Right, bottom - top - padding.Top - padding.Bottom));
                }
                var spans = CollectionsMarshal.AsSpan(_textPositions);
                for (int i = 0; i < spans.Length; i++)
                {
                    ref var textPosition = ref spans[i];
                    var blockPoint = origin.TopLeft;
                    blockPoint.X += left;
                    blockPoint.Y += top + textPosition.Y;
                    textPosition.Block.Layout.Draw(drawingContext, blockPoint, clip);
                    //if ((textPosition.Y >= top && textPosition.Y < bottom) || (textPosition.Bottom > top && textPosition.Bottom <= bottom))
                    //{
                    //    if ((textPosition.X >= left && textPosition.X < right) || (textPosition.Right > left && textPosition.Right <= right) || (textPosition.X <= left && textPosition.Right >= right))
                    //    {
                    //        var clipLeft = MathF.Max(textPosition.X, left);
                    //        var clipTop = MathF.Max(textPosition.Y, top);
                    //        var clipRight = MathF.Min(textPosition.Right, right);
                    //        var clipBottom = MathF.Min(textPosition.Bottom, bottom);
                    //        var clip = new Rect(clipLeft - textPosition.X, clipTop - textPosition.Y, clipRight - clipLeft, clipBottom - clipTop);
                    //        textPosition.Block.Layout.Draw(drawingContext, new Point(textPosition.X + padding.Left, textPosition.Y + padding.Top), clip);
                    //    }
                    //}
                    //else
                    //    break;
                }
            }

            public TextPointer GetCharacterAtPoint(in Point point, TextPointer start, TextPointer end)
            {
                var spans = CollectionsMarshal.AsSpan(_textPositions);                
                for (int i = 0; i < spans.Length; i++)
                {
                    ref var textPosition = ref spans[i];
                    if (textPosition.Bottom < point.Y)
                        continue;
                    return textPosition.Block.Layout.GetCharacterAtPoint(new Point(point.X, point.Y - textPosition.Top));
                }
                return _figure.ContentEnd;
            }

            public InlineLayoutMeasureResult Measure(TextPointer start, TextPointer end, float availableWidth, bool isFullLine, TextWrapping textWrapping, TextTrimming textTrimming)
            {
                var figure = _figure;
                if (start != figure.ContentStart && start != figure.ElementStart)
                    throw new ArgumentOutOfRangeException("Start must be Figure element or content start.");
                if (end != figure.ContentEnd && end != figure.ElementEnd)
                    throw new ArgumentOutOfRangeException("End must be Figure element or content end.");

                var result = new InlineLayoutMeasureResult();
                result.Start = start;
                result.End = end;
                var figureWidth = MeasureLength(figure.Width);
                var margin = figure.Margin;
                var padding = figure.Padding;
                _left = margin.Left + padding.Left;
                _right = _left + figureWidth + margin.Right + padding.Right;
                _top = margin.Top + padding.Top;
                var desiredWidth = figureWidth + margin.Left + margin.Right + padding.Left + padding.Right;
                if (desiredWidth == 0f)
                {
                    result.Rect = Rect.Empty;
                    return result;
                }
                if (availableWidth < desiredWidth && !isFullLine)
                {
                    result.Rect = Rect.Empty;
                    result.Overflow = new TextRange(start, end);
                    return result;
                }
                var blocks = figure.Blocks;
                var y = 0f;
                var blockSize = new Size(figureWidth, float.PositiveInfinity);
                for (int i = 0; i < blocks.Count; i++)
                {
                    var block = blocks[i];
                    var layout = block.Layout;
                    layout.Measure(blockSize);
                    _textPositions.Add(new TextPosition(block, 0, y, layout.Width, layout.Height));
                    y += layout.Height;
                }
                _bottom = _top + y + margin.Bottom + padding.Bottom;
                if (start == figure.ElementStart && end == figure.ElementEnd)
                    result.Widths = [0, desiredWidth, 0];
                else if (start == figure.ElementStart)
                    result.Widths = [0, desiredWidth];
                else if (end == figure.ElementEnd)
                    result.Widths = [desiredWidth, 0];
                else
                    result.Widths = [desiredWidth];
                switch (figure.HorizontalAnchor)
                {
                    case FigureHorizontalAnchor.ContentLeft:
                    case FigureHorizontalAnchor.PageLeft:
                        result.Rect = new Rect(0, 0, desiredWidth, y);
                        break;
                    case FigureHorizontalAnchor.ContentCenter:
                    case FigureHorizontalAnchor.PageCenter:
                        if (availableWidth > desiredWidth)
                            result.Rect = new Rect((availableWidth - desiredWidth) / 2, 0, desiredWidth, y);
                        else
                            result.Rect = new Rect(0, 0, desiredWidth, y);
                        break;
                    case FigureHorizontalAnchor.ContentRight:
                    case FigureHorizontalAnchor.PageRight:
                        if (availableWidth > desiredWidth)
                            result.Rect = new Rect(availableWidth - desiredWidth, 0, desiredWidth, y);
                        else
                            result.Rect = new Rect(0, 0, desiredWidth, y);
                        break;
                    default:
                        throw new NotSupportedException($"Figure does not support anchor \"{figure.HorizontalAnchor}\" right now.");
                }
                return result;
            }

            private float MeasureLength(FigureLength length)
            {
                switch (length.FigureUnitType)
                {
                    case FigureUnitType.Pixel:
                        return length.Value;
                    default:
                        return 0f;
                }
            }


            private struct TextPosition
            {
                public TextPosition(Block block, float x, float y, float width, float height)
                {
                    Block = block;
                    X = x;
                    Y = y;
                    Width = width;
                    Height = height;
                }

                public Block Block;

                public float X;

                public float Y;

                public float Width;

                public float Height;

                public float Left => X;

                public float Top => Y;

                public float Right => X + Width;

                public float Bottom => Y + Height;
            }
        }

        #endregion
    }
}
