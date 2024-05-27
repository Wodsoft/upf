using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Documents;
using Wodsoft.UI.Input;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Controls.Primitives
{
    public class TextHolder : FrameworkElement
    {
        private ITextHost? _textHost;
        private float _offsetX, _offsetY;

        #region Properties

        public static readonly DependencyProperty CanContentScrollProperty = ScrollViewer.CanContentScrollProperty.AddOwner(typeof(TextHolder),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));
        public bool CanContentScroll
        {
            get { return (bool)GetValue(CanContentScrollProperty)!; }
            set { SetValue(CanContentScrollProperty, value); }
        }

        public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty = ScrollViewer.HorizontalScrollBarVisibilityProperty.AddOwner(typeof(TextHolder),
            new FrameworkPropertyMetadata(ScrollBarVisibility.Disabled, FrameworkPropertyMetadataOptions.AffectsRender));
        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get { return (ScrollBarVisibility)GetValue(HorizontalScrollBarVisibilityProperty)!; }
            set { SetValue(HorizontalScrollBarVisibilityProperty, value); }
        }

        public static readonly DependencyProperty VerticalScrollBarVisibilityProperty = ScrollViewer.VerticalScrollBarVisibilityProperty.AddOwner(typeof(TextHolder),
            new FrameworkPropertyMetadata(ScrollBarVisibility.Disabled, FrameworkPropertyMetadataOptions.AffectsRender));
        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get { return (ScrollBarVisibility)GetValue(VerticalScrollBarVisibilityProperty)!; }
            set { SetValue(VerticalScrollBarVisibilityProperty, value); }
        }

        public static readonly DependencyProperty TextWrappingProperty =
                TextBlock.TextWrappingProperty.AddOwner(
                        typeof(TextHolder),
                        new FrameworkPropertyMetadata(TextWrapping.NoWrap));
        public TextWrapping TextWrapping
        {
            get => (TextWrapping)GetValue(TextWrappingProperty)!;
            set => SetValue(TextWrappingProperty, value);
        }

        public static readonly DependencyProperty TextAlignmentProperty = Block.TextAlignmentProperty.AddOwner(typeof(TextHolder));
        public TextAlignment TextAlignment
        {
            get => (TextAlignment)GetValue(TextAlignmentProperty)!;
            set => SetValue(TextAlignmentProperty, value);
        }

        public static readonly DependencyProperty TextTrimmingProperty =
                TextBlock.TextTrimmingProperty.AddOwner(
                        typeof(TextHolder),
                        new FrameworkPropertyMetadata(TextTrimming.CharacterEllipsis));
        public TextTrimming TextTrimming
        {
            get => (TextTrimming)GetValue(TextTrimmingProperty)!;
            set => SetValue(TextWrappingProperty, value);
        }

        public static readonly DependencyProperty PaddingProperty = Control.PaddingProperty.AddOwner(typeof(TextHolder), new FrameworkPropertyMetadata(new Thickness(2, 0, 2, 0), FrameworkPropertyMetadataOptions.AffectsRender));
        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty)!; }
            set { SetValue(PaddingProperty, value); }
        }

        public static readonly DependencyProperty CaretBrushProperty = TextBoxBase.CaretBrushProperty.AddOwner(typeof(TextHolder), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
        public Brush? CaretBrush
        {
            get { return (Brush?)GetValue(CaretBrushProperty); }
            set { SetValue(CaretBrushProperty, value); }
        }

        #endregion

        #region Layout

        private List<TextPosition> _textPositions = new List<TextPosition>();

        protected override Size MeasureOverride(Size availableSize)
        {
            if (_textHost == null)
            {
                var root = LogicalRoot as FrameworkElement;
                if (root == null || root.TemplatedParent is not ITextHost textHost)
                    return Size.Empty;
                _textHost = textHost;
                _textHost.SelectionChanged += TextHost_SelectionChanged;
                _textHost.TextChanged += TextHost_TextChanged;
            }
            var padding = Padding;
            var textWrapping = TextWrapping;
            var textTrimming = TextTrimming;
            float width = padding.Left + padding.Right, height = padding.Top + padding.Bottom;
            foreach (var line in _textHost.Lines)
            {
                float lineHeight = float.IsNaN(line.LineHeight) ? 0f : line.LineHeight;
                float availableWidth = availableSize.Width;
                foreach (var run in line.Runs)
                {
                    if (!run.IsMeasured)
                        run.Measure();
                    if (float.IsNaN(line.LineHeight))
                        lineHeight = MathF.Max(lineHeight, run.Height);
                    if (run.Width > availableWidth)
                    {
                        if (textWrapping == TextWrapping.NoWrap)
                            return new Size(availableSize.Width, lineHeight);
                        ITextHostRun? right;
                        if (textTrimming == TextTrimming.None)
                        {
                            right = run;
                        }
                        else
                        {
                            run.Wrap(textTrimming, availableWidth, textWrapping == TextWrapping.WrapWithOverflow, out _, out right);
                        }
                        width = availableSize.Width;
                        height += lineHeight;
                        if (height >= availableSize.Height && width >= availableSize.Width)
                            return availableSize;
                        lineHeight = right!.Height;
                        availableWidth = availableSize.Width - right.Width;
                    }
                    else
                    {
                        availableWidth -= run.Width;
                    }
                }
                height += lineHeight;
                width = MathF.Max(width, availableSize.Width - availableWidth);
                if (height >= availableSize.Height && width >= availableSize.Width)
                    return availableSize;
            }
            return new Size(MathF.Min(width, availableSize.Width), MathF.Min(height, availableSize.Height));
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (_textHost == null)
                return;
            var textWrapping = TextWrapping;
            var textTrimming = TextTrimming;
            var flowDirection = FlowDirection;
            var renderSize = RenderSize;
            var padding = Padding;
            bool isHorizontalScrollable = CanContentScroll && HorizontalScrollBarVisibility != ScrollBarVisibility.Disabled;
            bool isVerticalScrollable = CanContentScroll && VerticalScrollBarVisibility != ScrollBarVisibility.Disabled;
            float left = _offsetX;
            float top = _offsetY;
            float right = left + renderSize.Width - padding.Left - padding.Right;
            drawingContext.PushClip(new RectangleGeometry(new Rect(padding.Left, padding.Top, renderSize.Width - padding.Left - padding.Right, renderSize.Height - padding.Top - padding.Bottom)));
            //float bottom = top + renderSize.Height;
            float x = 0f, y = 0f, baseline = 0f, lineHeight = 0f;
            var canDraw = (ITextHostRun run) => (x >= left && x < right) || (x + run.Width >= left && x + run.Width < right);
            _textPositions.Clear();
            if (flowDirection == FlowDirection.LeftToRight)
            {
                List<(ITextHostRun Run, float X)> lineRuns = new List<(ITextHostRun, float)>();
                var drawRuns = () =>
                {
                    //not overflow
                    if (!isVerticalScrollable || y > top)
                    {
                        foreach (var item in lineRuns)
                        {
                            item.Run.Draw(drawingContext, new Point(item.X + padding.Left, y + baseline + padding.Top));
                            _textPositions.Add(new TextPosition(item.Run, item.X, y, lineHeight));
                        }
                    }
                    lineRuns.Clear();
                };
                foreach (var line in _textHost.Lines)
                {
                    y += lineHeight;
                    if (float.IsNaN(line.LineHeight))
                    {
                        baseline = lineHeight = 0f;
                    }
                    else
                    {
                        baseline = line.Baseline;
                        lineHeight = line.LineHeight;
                    }
                    float availableWidth = _textHost.AcceptsReturn ? RenderSize.Width : float.PositiveInfinity;
                    foreach (var run in line.Runs)
                    {
                        if (!run.IsMeasured)
                            run.Measure();
                        if (float.IsNaN(line.LineHeight))
                        {
                            if (run.Height > lineHeight)
                            {
                                lineHeight = run.Height;
                                baseline = run.Baseline;
                            }
                        }
                        if (run.Width > availableWidth)
                        {
                            if (textWrapping == TextWrapping.NoWrap || textTrimming == TextTrimming.None)
                            {
                                if (!canDraw(run))
                                    lineRuns.Add((run, x - left));
                                drawRuns();
                                if (textWrapping == TextWrapping.NoWrap)
                                    //no wrap, go to next line
                                    break;
                                y += lineHeight;
                                //no trimming, go to next run
                                continue;
                            }
                            run.Wrap(textTrimming, availableWidth, textWrapping == TextWrapping.WrapWithOverflow, out var leftRun, out var rightRun);
                            if (leftRun != null && !canDraw(leftRun))
                                lineRuns.Add((leftRun!, x - left));
                            drawRuns();
                            if (y >= renderSize.Height)
                                return;
                            availableWidth = _textHost.AcceptsReturn ? RenderSize.Width : float.PositiveInfinity;
                            if (rightRun != null)
                            {
                                if (float.IsNaN(line.LineHeight))
                                {
                                    baseline = rightRun!.Baseline;
                                    lineHeight = rightRun!.Height;
                                }
                                else
                                {
                                    baseline = line.Baseline;
                                    lineHeight = line.LineHeight;
                                }
                                x = 0f;
                                if (rightRun.Width > left)
                                    lineRuns.Add((rightRun, 0));
                                availableWidth -= rightRun.Width;
                            }
                            else
                            {
                                if (float.IsNaN(line.LineHeight))
                                {
                                    baseline = lineHeight = 0f;
                                }
                                else
                                {
                                    baseline = line.Baseline;
                                    lineHeight = line.LineHeight;
                                }
                            }
                        }
                        else
                        {
                            availableWidth -= run.Width;
                            if (canDraw(run))
                                lineRuns.Add((run, x));
                            x += run.Width;
                        }
                    }
                    x = 0f;
                    drawRuns();
                }
            }
            drawingContext.Pop();
        }

        private void TextHost_TextChanged(object? sender, EventArgs e)
        {
            InvalidateMeasure();
        }

        private void TextHost_SelectionChanged(object? sender, EventArgs e)
        {
            InvalidateVisual();
        }

        #endregion

        #region Mouse Input

        public override Visual? HitTest(in Point point)
        {
            if (_textHost != null)
            {
                var renderSize = RenderSize;
                if (point.X >= 0 && point.X < renderSize.Width && point.Y >= 0 && point.Y < renderSize.Height)
                    return this;
            }
            return null;
        }

        private bool _mouseDown;
        private int _mousePositionStart;
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (_textHost != null && _textHost.IsSelectable)
            {
                _mouseDown = true;
                CaptureMouse();
                var padding = Padding;
                var point = e.GetPosition(this);
                point.X -= padding.Left;
                point.Y -= padding.Top;
                var position = GetTextPosition(point);
                if (position != _textHost.SelectionStart)
                    _textHost.Select(position, 0);
                _mousePositionStart = position;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_mouseDown)
            {
                var padding = Padding;
                var point = e.GetPosition(this);
                point.X -= padding.Left;
                point.Y -= padding.Top;
                var position = GetTextPosition(point);
                if (_mousePositionStart > position)
                    _textHost!.Select(position, _mousePositionStart - position);
                else
                    _textHost!.Select(_mousePositionStart, position - _mousePositionStart);
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (_mouseDown)
            {
                _mouseDown = false;
                ReleaseMouseCapture();
            }
        }

        private int GetTextPosition(in Point point)
        {
            if (_textPositions.Count == 0)
                return 0;
            var x = point.X;
            var y = point.Y;
            int i;
            float lastLine = float.PositiveInfinity;
            for (i = 0; i < _textPositions.Count; i++)
            {
                var text = _textPositions[i];
                if (y < text.Y)
                    continue;
                if (y > text.Y + text.Height)
                    continue;
                if (x < text.X && lastLine != text.Y)
                    return text.Run.Position;
                if (x >= text.X && x <= text.X + text.Run.Width)
                    return text.Run.GetCharPosition(x - text.X);
                lastLine = text.Y;
            }
            return _textPositions[i - 1].Run.Position + _textPositions[i - 1].Run.Length;
        }

        #endregion

        private struct TextPosition
        {
            public TextPosition(ITextHostRun run, float x, float y, float height)
            {
                Run = run;
                X = x;
                Y = y;
                Height = height;
            }

            public ITextHostRun Run { get; set; }

            public float X { get; set; }

            public float Y { get; set; }

            public float Height { get; set; }
        }
    }
}
