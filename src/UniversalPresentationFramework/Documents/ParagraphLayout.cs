using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Controls;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Documents
{
    public class ParagraphLayout : IBlockLayout
    {
        private readonly TextElement _owner;
        private readonly IReadOnlyList<Inline> _inlines;
        private readonly List<Rect> _floatInlines;
        private readonly List<InlineLayoutItem> _items;
        private float _width, _height;

        public ParagraphLayout(TextElement owner, IReadOnlyList<Inline> inlines)
        {
            _owner = owner;
            _inlines = inlines;
            _floatInlines = new List<Rect>();
            _items = new List<InlineLayoutItem>();
        }

        //public TextWrapping TextWrapping { get; set; }

        //public TextTrimming TextTrimming { get; set; }

        public float Width => _width;

        public float Height => _height;

        public Span<InlineLayoutItem> Items => CollectionsMarshal.AsSpan(_items);

        public void Measure(Size availableSize)
        {
            _items.Clear();
            _floatInlines.Clear();
            float width = 0f, height = 0f;
            Rect space = FindNextSpace(availableSize, default);
            float spaceAvailableWidth = space.Width;
            bool lastIsFloat = false;
            var textWrapping = (TextWrapping)_owner.GetValue(TextBlock.TextWrappingProperty)!;
            var textTrimming = (TextTrimming)_owner.GetValue(TextBlock.TextTrimmingProperty)!;
            for (int i = 0; i < _inlines.Count; i++)
            {
                var inline = _inlines[i];
                InlineLayoutMeasureResult measureResult;
                TextPointer start = inline.ContentStart;
                TextPointer end = inline.ContentEnd;
                var layout = inline.Layout;
                var isFloat = layout.IsFloat;
                if (isFloat && !lastIsFloat)
                {
                    space.X = 0;
                    space.Width = availableSize.Width;
                    space = FindNextSpace(availableSize, space);
                    spaceAvailableWidth = space.Width;
                }
                else if (!isFloat && lastIsFloat)
                {
                    space.X = 0;
                    space.Width = availableSize.Width;
                    space.Height = 0;
                    space = FindNextSpace(availableSize, space);
                    spaceAvailableWidth = space.Width;
                }
                do
                {
                    //space = FindNextSpace(availableSize, space);
                    measureResult = layout.Measure(start, end, spaceAvailableWidth, spaceAvailableWidth == availableSize.Width, textWrapping, textTrimming);
                    if (!measureResult.Rect.IsEmpty)
                    {
                        measureResult.Rect.X += space.X + space.Width - spaceAvailableWidth;
                        measureResult.Rect.Y += space.Y;
                        if (isFloat)
                            _floatInlines.Add(measureResult.Rect);
                        if (measureResult.Rect.Right > width)
                            width = measureResult.Rect.Right;
                        if (measureResult.Rect.Bottom > height)
                            height = measureResult.Rect.Bottom;
                        if (measureResult.Rect.Height > space.Height)
                            space.Height = measureResult.Rect.Height;
                        _items.Add(new InlineLayoutItem(measureResult.Rect, measureResult.LineHeight, measureResult.Widths, inline, measureResult.Start, measureResult.End, isFloat));
                        if (measureResult.Overflow == null)
                        {
                            spaceAvailableWidth -= measureResult.Rect.Width;
                        }
                        else
                        {
                            start = measureResult.Overflow.Start;
                            end = measureResult.Overflow.End;
                            space = FindNextSpace(availableSize, space);
                            spaceAvailableWidth = space.Width;
                        }
                    }
                    else if (measureResult.Overflow != null)
                    {
                        space = FindNextSpace(availableSize, space);
                        spaceAvailableWidth = space.Width;
                    }
                }
                while (measureResult.Overflow != null);
                lastIsFloat = isFloat;
            }
            _width = width;
            _height = height;
        }

        private Rect FindNextSpace(Size availableSize, Rect lastSpace)
        {
            var spans = CollectionsMarshal.AsSpan(_floatInlines);
            /*
            |----------|------------|----------|
            |text      |float A     |text      |
            |----------|            |----------|
            |float B   |------------|float C   |
            |          |last space  |          |
            |----------|            |----------|
            |          |            |          |
            |----------|------------|----------|
            |            next space            |
            |----------------------------------|
            */
            float left, right = availableSize.Width, bottom = availableSize.Height, lastTop = 0;
            int i = 0, topIndex = 0;
            if (lastSpace.Right != availableSize.Width)
            {
                //find right space for this line.
                left = lastSpace.Right;
                for (; i < spans.Length; i++)
                {
                    ref var item = ref spans[i];
                    //skip float upper than last space
                    if (item.Bottom <= lastSpace.Top)
                        continue;
                    if (item.Top > lastTop)
                    {
                        lastTop = item.Top;
                        topIndex = i;
                    }
                    //cut current float
                    if (item.Left == left && right > item.Right)
                        left = item.Right;
                    if (item.Right == right && left < item.Left)
                        right = item.Left;
                    if (right == left)
                        break;
                    //cut right float
                    if (item.Left >= left && item.Left <= right)
                        right = item.Left;
                    //cut left float
                    if (item.Right <= right && item.Left >= left)
                        left = item.Right;
                    //find nestest bottom as space bottom
                    if (item.Bottom < bottom)
                        bottom = item.Bottom;
                    //stop at lower float
                    if (item.Top > lastSpace.Bottom)
                        break;
                }
                if (right - left != 0)
                    return new Rect(left, lastSpace.Top, right - left, 0);
            }
            i = topIndex;
            left = 0f;
            right = availableSize.Width;
            bottom = availableSize.Height;
            float top = lastSpace.Bottom;
            int shorterFloat = -1;
            //find bottom space for next line
            for (; i < spans.Length; i++)
            {
                ref var item = ref spans[i];
                //skip float upper than last space
                if (item.Bottom <= top)
                    continue;
                //this is full line float, go to next line
                if (item.Left == 0 && item.Right == availableSize.Width)
                {
                    top = item.Bottom;
                    bottom = availableSize.Height;
                    continue;
                }
                //this float is a new line, break here
                if (item.Top > bottom)
                    break;
                //find nestest bottom as space bottom
                if (item.Bottom < bottom)
                {
                    bottom = item.Bottom;
                    shorterFloat = i;
                }
                //this line full with float, we take shorter one
                if (item.Left == left && item.Right == right)
                {
                    item = ref spans[shorterFloat];
                    left = item.Left;
                    right = item.Right;
                    top = item.Bottom;
                    break;
                }
                //cut right float
                if (item.Left > left && item.Left <= right)
                    right = item.Left;
                //cut left float
                if (item.Right < right && item.Left >= left)
                    left = item.Right;
            }
            return new Rect(left, top, right - left, 0);
        }

        public TextPointer GetCharacterAtPoint(in Point point)
        {
            var items = CollectionsMarshal.AsSpan(_items);
            var y = point.Y;
            if (y > _height)
                y = _height;
            else if (y < 0)
                y = 0;
            int i = 0;
            //find inline which contains point
            for (; i < items.Length; i++)
            {
                ref var item = ref items[i];
                if (item.Rect.Bottom < y)
                    continue;
                if (item.Rect.Top > y && !item.IsFloat)
                    break;
                if (item.Rect.Left <= point.X && item.Rect.Right >= point.X)
                {
                    return item.Inline.Layout.GetCharacterAtPoint(new Point(point.X - item.Rect.X, point.Y - item.Rect.Y), item.Start, item.End);
                    //var length = 0f;
                    //var x = point.X - item.Rect.Left;
                    //int offset = 0;
                    //for (int j = 0; j < item.Widths.Length; j++)
                    //{
                    //    var width = item.Widths[j];
                    //    if (width == 0)
                    //        continue;
                    //    if (length + width > x)
                    //    {
                    //        if (x - length <= width / 2)
                    //            offset = j;
                    //        else
                    //            offset = j + 1;
                    //        break;
                    //    }
                    //    length += width;
                    //}
                    //if (offset == 0)
                    //    return item.Start;
                    //return item.Start.GetPositionAtOffset(offset, LogicalDirection.Forward)!;
                }
            }
            if (i == items.Length)
                i--;
            //find inline which near point at same line
            TextPointer? pointer = null;
            float distance = float.PositiveInfinity;
            for (; i >= 0; i--)
            {
                ref var item = ref items[i];
                if (item.IsFloat)
                    continue;
                if (item.Rect.Top > y)
                    continue;
                if (item.Rect.Bottom < y)
                    break;
                if (item.Rect.Left > point.X)
                {
                    var d = item.Rect.Left - point.X;
                    if (d < distance)
                    {
                        distance = d;
                        pointer = item.Start;
                    }
                }
                else if (item.Rect.Right < point.X)
                {
                    var d = point.X - item.Rect.Right;
                    if (d < distance)
                    {
                        distance = d;
                        pointer = item.End;
                    }
                }
            }
            if (pointer != null)
                return pointer;
            //find bottom inline
            i = items.Length - 1;
            for (; i >= 0; i--)
            {
                ref var item = ref items[i];
                if (item.IsFloat)
                    continue;
                if (point.X <= item.Rect.Left)
                    return item.Start;
                else if (point.X >= item.Rect.Right)
                    return item.End;
                else
                {
                    var length = 0f;
                    var x = point.X - item.Rect.Left;
                    int offset = 0;
                    for (int j = 0; j < item.Widths.Length; j++)
                    {
                        var width = item.Widths[j];
                        if (length + width > x)
                        {
                            if (x - length <= width / 2)
                                offset = j;
                            else
                                offset = j + 1;
                        }
                        length += width;
                    }
                    if (offset == 0)
                        return item.Start;
                    return item.Start.GetPositionAtOffset(offset, LogicalDirection.Forward)!;
                }
            }
            //return start
            return items[0].Start;
        }

        public bool GetCharacterRelateToCharacter(TextPointer pointer, in LogicalDirection direction, [NotNullWhen(true)] out TextPointer? position)
        {
            var items = CollectionsMarshal.AsSpan(_items);
            if (items.Length == 0 || pointer < items[0].Start || pointer > items[items.Length - 1].End)
            {
                position = null;
                return false;
            }
            int i = 0;
            float originY = -1, destY = -1, originX = -1;
            for (; i < items.Length; i++)
            {
                ref var item = ref items[i];
                if (item.End < pointer)
                    continue;
                originY = item.Rect.Y;
                originX = item.Rect.X + item.Widths.AsSpan().Slice(0, item.Start.GetOffsetToPosition(pointer)).Sum();
                break;
            }
            if (direction == LogicalDirection.Backward)
            {
                if (i == 0)
                {
                    position = null;
                    return false;
                }
                int upperStart = -1;
                for (; i >= 0; i--)
                {
                    ref var item = ref items[i];
                    //do not navigate to float inline
                    if (item.IsFloat)
                        continue;
                    //skip same line
                    if (item.Rect.Y == originY)
                        continue;
                    //mark line
                    if (destY == -1)
                    {
                        upperStart = i;
                        destY = item.Rect.Y;
                    }
                    else if (destY != item.Rect.Y)
                        break;
                    if (destY == item.Rect.Y && item.Rect.X >= originX && item.Rect.Y <= originX)
                    {
                        var length = 0f;
                        var x = originX - item.Rect.Left;
                        int offset = 0;
                        for (int j = 0; j < item.Widths.Length; j++)
                        {
                            var width = item.Widths[j];
                            if (length + width > x)
                            {
                                if (x - length <= width / 2)
                                    offset = i;
                                else
                                    offset = i + 1;
                            }
                            length += width;
                        }
                        if (offset == 0)
                            position = item.Start;
                        else
                            position = item.Start.GetPositionAtOffset(offset, LogicalDirection.Forward)!;
                        return true;
                    }
                }
                //no not float inline upper
                if (destY == -1)
                {
                    position = null;
                    return false;
                }
                float distance = float.PositiveInfinity;
                position = pointer;
                //get nestest upper inline
                for (; i <= upperStart; i++)
                {
                    ref var item = ref items[i];
                    if (item.Rect.Left > originX)
                    {
                        var d = originX - item.Rect.Left;
                        if (d < distance)
                        {
                            distance = d;
                            position = item.Start;
                        }
                    }
                    else if (item.Rect.Right < originX)
                    {
                        var d = item.Rect.Right - originX;
                        if (d < distance)
                        {
                            distance = d;
                            position = item.End;
                        }
                    }
                }
                return true;
            }
            else
            {
                if (i == items.Length - 1)
                {
                    position = null;
                    return false;
                }
                int bottomStart = -1;
                for (; i < items.Length; i++)
                {
                    ref var item = ref items[i];
                    //do not navigate to float inline
                    if (item.IsFloat)
                        continue;
                    //skip same line
                    if (item.Rect.Y == originY)
                        continue;
                    //mark line
                    if (destY == -1)
                    {
                        bottomStart = i;
                        destY = item.Rect.Y;
                    }
                    else if (destY != item.Rect.Y)
                        break;
                    if (destY == item.Rect.Y && item.Rect.X >= originX && item.Rect.Y <= originX)
                    {
                        var length = 0f;
                        var x = originX - item.Rect.Left;
                        int offset = 0;
                        for (int j = 0; j < item.Widths.Length; j++)
                        {
                            var width = item.Widths[j];
                            if (length + width > x)
                            {
                                if (x - length <= width / 2)
                                    offset = i;
                                else
                                    offset = i + 1;
                            }
                            length += width;
                        }
                        if (offset == 0)
                            position = item.Start;
                        else
                            position = item.Start.GetPositionAtOffset(offset, LogicalDirection.Forward)!;
                        return true;
                    }
                }
                //no not float inline upper
                if (destY == -1)
                {
                    position = null;
                    return false;
                }
                float distance = float.PositiveInfinity;
                position = pointer;
                //get nestest upper inline
                for (; i >= bottomStart; i--)
                {
                    ref var item = ref items[i];
                    if (item.Rect.Left > originX)
                    {
                        var d = originX - item.Rect.Left;
                        if (d < distance)
                        {
                            distance = d;
                            position = item.Start;
                        }
                    }
                    else if (item.Rect.Right < originX)
                    {
                        var d = item.Rect.Right - originX;
                        if (d < distance)
                        {
                            distance = d;
                            position = item.End;
                        }
                    }
                }
                return true;
            }
        }

        public Rect GetRectAtCharacter(TextPointer pointer)
        {
            var items = CollectionsMarshal.AsSpan(_items);
            if (items.Length == 0 || pointer < items[0].Start || pointer > items[items.Length - 1].End)
                return Rect.Empty;
            for (int i = 0; i < items.Length; i++)
            {
                ref var item = ref items[i];
                if (item.End < pointer)
                    continue;
                if (item.Start == pointer)
                    return new Rect(item.Rect.X, item.Rect.Y, 0, item.Rect.Height);
                else if (item.End == pointer)
                    return new Rect(item.Rect.Right, item.Rect.Y, 0, item.Rect.Height);
                else
                    return new Rect(item.Rect.X + item.Widths.AsSpan().Slice(0, item.Start.GetOffsetToPosition(pointer)).Sum(), item.Rect.Y, 0, item.Rect.Height);
            }
            return Rect.Empty;
        }

        public void Draw(DrawingContext drawingContext, in Point origin, in Rect clip)
        {
            var owner = _owner;
            var brush = owner.Background;
            if (brush != null)
                drawingContext.DrawRectangle(brush, null, new Rect(origin, new Size(_width, _height)));
            var items = CollectionsMarshal.AsSpan(_items);
            for (int i = 0; i < items.Length; i++)
            {
                ref var item = ref items[i];
                var rect = item.Rect;
                rect.X += origin.X;
                rect.Y += origin.Y;
                if (rect.Bottom < clip.Top)
                    continue;
                if (rect.Top >= clip.Bottom)
                    break;
                if (rect.Right <= clip.Left)
                    continue;
                if (rect.Left >= clip.Right)
                    continue;
                item.Inline.Layout.Draw(drawingContext, rect, clip, item.LineHeight, item.Widths, item.Start, item.End);
            }
        }
    }

    public struct InlineLayoutItem
    {
        public Rect Rect;
        public float LineHeight;
        public float[] Widths;
        public Inline Inline;
        public TextPointer Start;
        public TextPointer End;
        public bool IsFloat;

        public InlineLayoutItem(Rect rect, float lineHeight, float[] widths, Inline inline, TextPointer start, TextPointer end, bool isFloat)
        {
            Rect = rect;
            LineHeight = lineHeight;
            Widths = widths;
            Inline = inline;
            Start = start;
            End = end;
            IsFloat = isFloat;
        }
    }


}
