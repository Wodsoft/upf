using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Documents;
using Wodsoft.UI.Input;
using Wodsoft.UI.Media;
using Wodsoft.UI.Media.Animation;
using Wodsoft.UI.Threading;

namespace Wodsoft.UI.Controls.Primitives
{
    public class TextViewer : FrameworkElement, IInputMethodSource
    {
        private ITextHost? _textHost;
        private float _offsetX, _offsetY, _caretX, _caretY, _caretHeight, _measuredHeight;
        private Storyboard? _caretStoryboard;
        private bool _caretVisible, _caretMove, _caretUpdate;
        private IInputMethodContext? _inputMethodContext;
        private List<TextPosition> _textPositions = new List<TextPosition>();
        private TextViewerCaret _caret = new TextViewerCaret();
        private int _caretTextPosition;

        #region Properties

        public static readonly DependencyProperty CanContentScrollProperty = ScrollViewer.CanContentScrollProperty.AddOwner(typeof(TextViewer),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));
        public bool CanContentScroll
        {
            get { return (bool)GetValue(CanContentScrollProperty)!; }
            set { SetValue(CanContentScrollProperty, value); }
        }

        public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty = ScrollViewer.HorizontalScrollBarVisibilityProperty.AddOwner(typeof(TextViewer),
            new FrameworkPropertyMetadata(ScrollBarVisibility.Disabled, FrameworkPropertyMetadataOptions.AffectsRender));
        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get { return (ScrollBarVisibility)GetValue(HorizontalScrollBarVisibilityProperty)!; }
            set { SetValue(HorizontalScrollBarVisibilityProperty, value); }
        }

        public static readonly DependencyProperty VerticalScrollBarVisibilityProperty = ScrollViewer.VerticalScrollBarVisibilityProperty.AddOwner(typeof(TextViewer),
            new FrameworkPropertyMetadata(ScrollBarVisibility.Disabled, FrameworkPropertyMetadataOptions.AffectsRender));
        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get { return (ScrollBarVisibility)GetValue(VerticalScrollBarVisibilityProperty)!; }
            set { SetValue(VerticalScrollBarVisibilityProperty, value); }
        }

        public static readonly DependencyProperty TextWrappingProperty =
                TextBlock.TextWrappingProperty.AddOwner(
                        typeof(TextViewer),
                        new FrameworkPropertyMetadata(TextWrapping.NoWrap));
        public TextWrapping TextWrapping
        {
            get => (TextWrapping)GetValue(TextWrappingProperty)!;
            set => SetValue(TextWrappingProperty, value);
        }

        public static readonly DependencyProperty TextAlignmentProperty = Block.TextAlignmentProperty.AddOwner(typeof(TextViewer));
        public TextAlignment TextAlignment
        {
            get => (TextAlignment)GetValue(TextAlignmentProperty)!;
            set => SetValue(TextAlignmentProperty, value);
        }

        public static readonly DependencyProperty TextTrimmingProperty =
                TextBlock.TextTrimmingProperty.AddOwner(
                        typeof(TextViewer),
                        new FrameworkPropertyMetadata(TextTrimming.CharacterEllipsis));
        public TextTrimming TextTrimming
        {
            get => (TextTrimming)GetValue(TextTrimmingProperty)!;
            set => SetValue(TextWrappingProperty, value);
        }

        public static readonly DependencyProperty PaddingProperty = Control.PaddingProperty.AddOwner(typeof(TextViewer), new FrameworkPropertyMetadata(new Thickness(2, 0, 2, 0), FrameworkPropertyMetadataOptions.AffectsRender));
        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty)!; }
            set { SetValue(PaddingProperty, value); }
        }

        public static readonly DependencyProperty CaretBrushProperty = TextBoxBase.CaretBrushProperty.AddOwner(typeof(TextViewer), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
        public Brush? CaretBrush
        {
            get { return (Brush?)GetValue(CaretBrushProperty); }
            set { SetValue(CaretBrushProperty, value); }
        }

        private static readonly DependencyProperty _CaretVisibleProperty = DependencyProperty.Register("CaretVisible", typeof(bool), typeof(TextViewer), new FrameworkPropertyMetadata(false, OnCaretVisibleChanged));
        private static void OnCaretVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TextViewer)d)._caretVisible = (bool)e.NewValue!;
            if (d.Dispatcher is UIDispatcher uiDispatcher)
                uiDispatcher.UpdateRender();
        }


        #endregion

        #region Visual

        protected internal override int VisualChildrenCount => _caretVisible ? 1 : 0;

        protected internal override Visual GetVisualChild(int index)
        {
            if (index == 0)
                return _caret;
            throw new ArgumentOutOfRangeException("index");
        }

        #endregion

        #region Layout

        protected override Size MeasureOverride(Size availableSize)
        {
            if (_textHost == null)
            {
                var root = LogicalRoot as FrameworkElement;
                if (root == null || root.TemplatedParent is not ITextHost textHost)
                    return Size.Empty;
                root.TemplatedParent.PreviewTextInput += TextHost_PreviewTextInput;
                _textHost = textHost;
                _textHost.SelectionChanged += TextHost_SelectionChanged;
                _textHost.TextChanged += TextHost_TextChanged;
                _textHost.GotKeyboardFocus += TextHost_GotKeyboardFocus;
                _textHost.LostKeyboardFocus += TextHost_LostKeyboardFocus;
                _textHost.KeyDown += TextHost_KeyDown;
            }

            var padding = Padding;
            var textWrapping = TextWrapping;
            var textTrimming = TextTrimming;
            float width = padding.Left + padding.Right, height = padding.Top + padding.Bottom;
            int i = 0, l = 0;
            float maxWidth = 0f, maxHeight = 0f;
            bool fetchWidth = true, fetchHeight = true;
            var lines = _textHost.Lines;
            while (fetchWidth || fetchHeight)
            {
                if (i == _textPositions.Count)
                {
                    ITextHostLine? line = null;
                    if (l == 0 && _textPositions.Count != 0)
                    {
                        var lastRun = _textPositions[_textPositions.Count - 1].Run;
                        var position = lastRun.Position + lastRun.Length;
                        for (int j = 0; j < lines.Count; j++)
                        {
                            if (lines[j].Position < position)
                                continue;
                            line = lines[j];
                            l = j + 1;
                        }
                    }
                    else
                    {
                        if (l < lines.Count)
                        {
                            line = lines[l];
                            l++;
                        }
                    }
                    if (line == null)
                        break;
                    MeasureLine(line, availableSize.Width - padding.Left - padding.Right, textWrapping, textTrimming);
                }
                var spans = CollectionsMarshal.AsSpan(_textPositions);
                for (; i < spans.Length; i++)
                {
                    ref var textPosition = ref spans[i];
                    if (fetchWidth)
                    {
                        var textRight = textPosition.X + textPosition.Run.Width;
                        if (textRight > maxWidth)
                            maxWidth = textRight;
                        //if found 
                        if (maxWidth + width >= availableSize.Width)
                            fetchWidth = false;
                    }
                    if (fetchHeight)
                    {
                        var textHeight = textPosition.Y + textPosition.Height;
                        if (textHeight > maxHeight)
                            maxHeight = textHeight;
                        if (maxHeight + height >= availableSize.Height)
                            fetchHeight = false;
                    }
                }
            }
            return new Size(width + maxWidth, height + maxHeight);
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
            drawingContext.PushClip(new RectangleGeometry(new Rect(padding.Left, padding.Top, renderSize.Width - padding.Left - padding.Right, renderSize.Height - padding.Top - padding.Bottom)));
            bool fetch = true;
            int i = 0, l = 0;
            var lines = _textHost.Lines;
            float availableWidth = _textHost.AcceptsReturn ? RenderSize.Width - padding.Left - padding.Right : float.PositiveInfinity;
            if (_caretMove)
            {
                var selectionStart = _textHost!.SelectionStart;
                int lastRunEnd;
                if (_textPositions.Count == 0)
                    lastRunEnd = 0;
                else
                {
                    var lastRun = _textPositions[_textPositions.Count - 1].Run;
                    lastRunEnd = lastRun.Position + lastRun.Length;
                }
                //measure to selection start of line
                if (selectionStart > lastRunEnd)
                {
                    for (; l < lines.Count; l++)
                    {
                        var line = lines[l];
                        if (line.Position < lastRunEnd)
                            continue;
                        if (line.Position > selectionStart)
                            break;
                        MeasureLine(line, availableWidth, textWrapping, textTrimming);
                    }
                }
                _caretMove = false;
                UpdateCaret(true);
            }
            float left = _offsetX;
            float top = _offsetY;
            float right = left + renderSize.Width - padding.Left - padding.Right;
            float bottom = top + renderSize.Height - padding.Top - padding.Bottom;
            if (flowDirection == FlowDirection.LeftToRight)
            {
                while (fetch)
                {
                    if (i == _textPositions.Count)
                    {
                        ITextHostLine? line = null;
                        if (l == 0 && _textPositions.Count != 0)
                        {
                            var lastRun = _textPositions[_textPositions.Count - 1].Run;
                            var position = lastRun.Position + lastRun.Length;
                            for (int j = 0; j < lines.Count; j++)
                            {
                                if (lines[j].Position < position)
                                    continue;
                                line = lines[j];
                                l = j + 1;
                            }
                        }
                        else
                        {
                            if (l < lines.Count)
                            {
                                line = lines[l];
                                l++;
                            }
                        }
                        if (line == null)
                            break;
                        MeasureLine(line, availableWidth, textWrapping, textTrimming);
                    }
                    var spans = CollectionsMarshal.AsSpan(_textPositions);
                    for (; i < spans.Length; i++)
                    {
                        ref var textPosition = ref spans[i];
                        if (textPosition.Run.Length == 0)
                            continue;
                        if (((textPosition.Y >= top && textPosition.Y < bottom) || (textPosition.Y + textPosition.Height > top && textPosition.Y + textPosition.Height <= bottom)) &&
                            ((textPosition.X >= left && textPosition.X < right) || (textPosition.X + textPosition.Run.Width > left && textPosition.Run.Width <= right) || (textPosition.X <= left && textPosition.X + textPosition.Run.Width >= right)))
                        {
                            textPosition.Run.Draw(drawingContext, new Point(textPosition.X - _offsetX + padding.Left, textPosition.Y - _offsetY + textPosition.Baseline + padding.Top));
                        }
                    }
                }
            }
            drawingContext.Pop();
            if (_caretUpdate)
            {
                UpdateCaret(false);
                _caretUpdate = false;
            }
        }

        private void MeasureLine(ITextHostLine line, float availableWidth, TextWrapping textWrapping, TextTrimming textTrimming)
        {
            int currentPositions = _textPositions.Count;
            float lineHeight, baseline, x = 0f, currentAvailableWidth = availableWidth;
            if (float.IsNaN(line.LineHeight))
            {
                baseline = lineHeight = 0f;
            }
            else
            {
                baseline = line.Baseline;
                lineHeight = line.LineHeight;
            }
            var adjustLineHeight = () =>
            {
                var spans = CollectionsMarshal.AsSpan(_textPositions);
                for (int i = currentPositions; i < spans.Length; i++)
                {
                    spans[i].Height = lineHeight;
                    spans[i].Baseline = baseline;
                }
            };
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
                if (run.Width > currentAvailableWidth)
                {
                    adjustLineHeight();
                    if (textWrapping == TextWrapping.NoWrap || textTrimming == TextTrimming.None)
                    {
                        TextPosition position = new TextPosition(run, x, _measuredHeight, lineHeight, baseline);
                        _textPositions.Add(position);
                        _measuredHeight += lineHeight;
                        if (textWrapping == TextWrapping.NoWrap)
                            //no wrap, go to next line
                            break;
                        //no trimming, go to next run
                        continue;
                    }
                    run.Wrap(textTrimming, currentAvailableWidth, textWrapping == TextWrapping.WrapWithOverflow, out var leftRun, out var rightRun);
                    if (leftRun != null)
                    {
                        TextPosition position = new TextPosition(leftRun, x, _measuredHeight, lineHeight, baseline);
                        _textPositions.Add(position);
                    }
                    currentPositions = _textPositions.Count;
                    _measuredHeight += lineHeight;
                    currentAvailableWidth = availableWidth;
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
                        TextPosition position = new TextPosition(rightRun, x, _measuredHeight, lineHeight, baseline);
                        _textPositions.Add(position);
                        currentAvailableWidth -= rightRun.Width;
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
                    currentAvailableWidth -= run.Width;
                    TextPosition position = new TextPosition(run, x, _measuredHeight, lineHeight, baseline);
                    _textPositions.Add(position);
                    x += run.Width;
                }
                _measuredHeight += lineHeight;
            }
            adjustLineHeight();
        }

        private void TextHost_TextChanged(object? sender, TextChangedEventArgs e)
        {
            if (_caretStoryboard != null)
                _caretStoryboard.Seek(TimeSpan.Zero, TimeSeekOrigin.BeginTime);
            _caretUpdate = true;
            _textPositions.Clear();
            _caretTextPosition = -1;
            _measuredHeight = 0f;
            InvalidateVisual();
        }

        private void TextHost_SelectionChanged(object? sender, RoutedEventArgs e)
        {
            if (_caretStoryboard != null)
                _caretStoryboard.Seek(TimeSpan.Zero, TimeSeekOrigin.BeginTime);
            _caretUpdate = true;
            InvalidateVisual();
        }

        private void UpdateCaret(bool moveView)
        {
            if (_textHost!.SelectionLength == 0)
            {
                var selectionStart = _textHost.SelectionStart;
                var spans = CollectionsMarshal.AsSpan(_textPositions);
                for (int i = 0; i < spans.Length; i++)
                {
                    ref var textPosition = ref spans[i];
                    if (selectionStart < textPosition.Run.Position)
                        continue;
                    else if (selectionStart > textPosition.Run.Position + textPosition.Run.Length)
                        continue;
                    var padding = Padding;
                    var x = textPosition.X + 0.5f + padding.Left - _offsetX;
                    if (selectionStart == textPosition.Run.Position + textPosition.Run.Length)
                        x += textPosition.Run.Width;
                    else if (selectionStart > textPosition.Run.Position)
                        x += textPosition.Run.Widths.Slice(0, selectionStart - textPosition.Run.Position).Sum();
                    var y = textPosition.Y - _offsetY + padding.Top;
                    var clip = new Rect(padding.Left, padding.Top, RenderSize.Width - padding.Left - padding.Right, RenderSize.Height - padding.Top - padding.Bottom);
                    if (moveView)
                    {
                        if (y < clip.Top)
                        {
                            var value = clip.Top - y;
                            _offsetY -= value;
                            y += value;
                        }
                        else if (y + textPosition.Height > clip.Bottom)
                        {
                            var value = y + textPosition.Height - clip.Bottom;
                            _offsetY += value;
                            y -= value;
                        }
                        if (x < clip.Left)
                        {
                            var value = clip.Left - x;
                            _offsetX -= value;
                            x += value;
                        }
                        else if (x > clip.Right)
                        {
                            var value = x - clip.Right;
                            _offsetX += value;
                            x -= value;
                        }
                    }
                    _caretX = x;
                    _caretY = y;
                    _caretHeight = textPosition.Height;
                    _caretTextPosition = i;
                    if (((y >= clip.Top && y < clip.Bottom) || (y + textPosition.Height >= clip.Top && y + textPosition.Height < clip.Bottom)) &&
                        (x >= clip.Left && x <= clip.Right))
                    {
                        var caretBrush = CaretBrush;
                        if (caretBrush == null)
                        {
                            var background = ((FrameworkElement)LogicalRoot).TemplatedParent!.GetValue(Panel.BackgroundProperty);
                            Color backgroundColor;
                            if (background != null && background != DependencyProperty.UnsetValue &&
                                background is SolidColorBrush)
                            {
                                backgroundColor = ((SolidColorBrush)background).Color;
                            }
                            else
                            {
                                backgroundColor = SystemColors.WindowColor;
                            }
                            byte r = (byte)~(backgroundColor.R);
                            byte g = (byte)~(backgroundColor.G);
                            byte b = (byte)~(backgroundColor.B);
                            caretBrush = new SolidColorBrush(Color.FromRgb(r, g, b));
                        }
                        _caret.IsVisible = true;
                        _caret.BuildCaretRender(x, y, textPosition.Height, caretBrush, new RectangleGeometry(clip));
                    }
                    else
                    {
                        _caret.IsVisible = false;
                    }
                    break;
                }
            }
            else
            {
                _caret.IsVisible = false;
            }
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
                point.X -= padding.Left - _offsetX;
                point.Y -= padding.Top - _offsetY;
                var position = GetTextPosition(point);
                if (position != _textHost.SelectionStart)
                    _textHost.Select(position, 0);
                _mousePositionStart = position;
                _textHost.Focus();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_mouseDown)
            {
                var padding = Padding;
                var point = e.GetPosition(this);
                point.X -= padding.Left - _offsetX;
                point.Y -= padding.Top - _offsetY;
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
                if (text.X == 0 && lastLine != text.Y && x > text.X + text.Run.Width)
                    return text.Run.Position + text.Run.Length;
                if (x < text.X && lastLine != text.Y)
                    return text.Run.Position;
                if (x >= text.X && x <= text.X + text.Run.Width)
                    return text.Run.GetCharPosition(x - text.X);
                lastLine = text.Y;
            }
            return _textPositions[i - 1].Run.Position + _textPositions[i - 1].Run.Length;
        }

        #endregion

        #region Keyboard Input

        private void TextHost_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (_textHost!.IsReadOnly && !_textHost.IsReadOnlyCaretVisible)
                return;
            if (_caretStoryboard == null)
            {
                _caretStoryboard = new Storyboard();
                _caretStoryboard.RepeatBehavior = RepeatBehavior.Forever;

                BooleanAnimationUsingKeyFrames blinkAnimation = new BooleanAnimationUsingKeyFrames();
                blinkAnimation.BeginTime = null;
                blinkAnimation.KeyFrames.Add(new DiscreteBooleanKeyFrame(true, KeyTime.FromPercent(0.0f)));
                blinkAnimation.KeyFrames.Add(new DiscreteBooleanKeyFrame(false, KeyTime.FromPercent(0.5f)));
                blinkAnimation.Duration = TimeSpan.FromSeconds(1);
                Storyboard.SetTarget(blinkAnimation, this);
                Storyboard.SetTargetProperty(blinkAnimation, new PropertyPath("CaretVisible"));
                _caretStoryboard.Children.Add(blinkAnimation);
                _caretStoryboard.Begin();

                UpdateCaret(false);
            }
            else
            {
                _caretStoryboard.Begin();
            }
            if (!_textHost.IsReadOnly)
            {
                if (_inputMethodContext == null)
                    _inputMethodContext = InputMethod.Current.CreateContext(this);
                if (_inputMethodContext != null)
                    _inputMethodContext.Focus();
            }
        }

        private void TextHost_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (_textHost!.IsReadOnly && !_textHost.IsReadOnlyCaretVisible)
                return;
            if (_caretStoryboard != null)
            {
                _caretStoryboard.Stop();
                _caretVisible = false;
            }
            if (_inputMethodContext != null)
                _inputMethodContext.Unfocus();
        }

        private void TextHost_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            _caretMove = true;
            if (!_caret.IsVisible)
                UpdateCaret(true);
        }

        private void TextHost_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                if (_textHost!.SelectionStart == 0)
                    return;
                _caretMove = true;
                _textHost!.Select(_textHost.SelectionStart - 1, 0);
                e.Handled = true;
            }
            else if (e.Key == Key.Right)
            {
                if (_textHost!.SelectionStart >= _textHost.TextLength)
                    return;
                _caretMove = true;
                _textHost!.Select(_textHost.SelectionStart + 1, 0);
                e.Handled = true;
            }
            else if (e.Key == Key.Up)
            {
                if (_caretTextPosition == -1)
                    return;
                var padding = Padding;
                var spans = CollectionsMarshal.AsSpan(_textPositions);
                ref var textPosition = ref spans[_caretTextPosition];
                bool reachLine = false;
                float lineY = 0f;
                for (int i = _caretTextPosition - 1; i >= 0; i--)
                {
                    ref var p = ref spans[i];
                    if (!reachLine && p.Y == textPosition.Y)
                    {
                        textPosition = ref p;
                        continue;
                    }
                    else if (reachLine && p.Y != lineY)
                        break;
                    if (!reachLine)
                    {
                        reachLine = true;
                        lineY = p.Y;
                    }
                    textPosition = ref p;
                    if (p.X + padding.Left - _offsetX <= _caretX)
                        break;
                }
                if (textPosition.Run != spans[_caretTextPosition].Run)
                {
                    var l = _caretX - (textPosition.X + padding.Left - _offsetX);
                    _caretMove = true;
                    _textHost!.Select(textPosition.Run.GetCharPosition(l), 0);
                }
            }
            else if (e.Key == Key.Down)
            {
                if (_caretTextPosition == -1)
                    return;
                var padding = Padding;
                var spans = CollectionsMarshal.AsSpan(_textPositions);
                ref var textPosition = ref spans[_caretTextPosition];
                bool reachLine = false;
                float lineY = 0f;
                for (int i = _caretTextPosition + 1; i < spans.Length; i++)
                {
                    ref var p = ref spans[i];
                    if (!reachLine && p.Y == textPosition.Y)
                    {
                        textPosition = ref p;
                        continue;
                    }
                    else if (reachLine && p.Y != lineY)
                        break;
                    if (!reachLine)
                    {
                        reachLine = true;
                        lineY = p.Y;
                    }
                    textPosition = ref p;
                    if (p.X + p.Run.Width + padding.Left - _offsetX >= _caretX)
                        break;
                }
                if (textPosition.Run != spans[_caretTextPosition].Run)
                {
                    var l = _caretX - (textPosition.X + padding.Left - _offsetX);
                    _caretMove = true;
                    _textHost!.Select(textPosition.Run.GetCharPosition(l), 0);
                }
            }
            else if (e.Key == Key.Home)
            {
                if (_caretTextPosition == -1)
                    return;
                var spans = CollectionsMarshal.AsSpan(_textPositions);
                ref var textPosition = ref spans[_caretTextPosition];
                for (int i = _caretTextPosition - 1; i >= 0; i--)
                {
                    ref var p = ref spans[i];
                    if (p.Y != textPosition.Y)
                        break;
                    textPosition = ref p;
                }
                if (_textHost!.SelectionStart != textPosition.Run.Position)
                {
                    _caretMove = true;
                    _textHost.Select(textPosition.Run.Position, 0);
                }
            }
            else if (e.Key == Key.End)
            {
                if (_caretTextPosition == -1)
                    return; ;
                var spans = CollectionsMarshal.AsSpan(_textPositions);
                ref var textPosition = ref spans[_caretTextPosition];
                for (int i = _caretTextPosition + 1; i < spans.Length; i++)
                {
                    ref var p = ref spans[i];
                    if (p.Y != textPosition.Y)
                        break;
                    textPosition = ref p;
                }
                if (_textHost!.SelectionStart != textPosition.Run.Position + textPosition.Run.Length)
                {
                    _caretMove = true;
                    _textHost.Select(textPosition.Run.Position + textPosition.Run.Length, 0);
                }
            }
        }

        #endregion

        #region Input Method Source

        UIElement IInputMethodSource.UIScope => (UIElement?)_textHost ?? throw new InvalidOperationException("No ui scope found.");

        Vector2 IInputMethodSource.CaretPosition
        {
            get
            {
                var offset = Vector2.Zero;
                Visual current = this;
                while (true)
                {
                    offset += current.VisualOffset;
                    if (current.VisualParent == null)
                        break;
                    current = current.VisualParent;
                }
                offset.X += _caretX;
                offset.Y += _caretY + _caretHeight;
                return offset;
            }
        }

        #endregion

        private struct TextPosition
        {
            public TextPosition(ITextHostRun run, float x, float y, float height, float baseline)
            {
                Run = run;
                X = x;
                Y = y;
                Height = height;
                Baseline = baseline;
            }

            public ITextHostRun Run;

            public float X;

            public float Y;

            public float Height;

            public float Baseline;
        }

        private class TextViewerCaret : Visual
        {
            private IDrawingContent? _drawingContent;

            public bool IsVisible { get; set; }

            public override bool HasRenderContent => IsVisible;

            public void BuildCaretRender(float x, float y, float height, Brush brush, RectangleGeometry clip)
            {
                if (FrameworkCoreProvider.RendererProvider != null)
                {
                    var drawingContext = FrameworkCoreProvider.RendererProvider.CreateDrawingContext(this);
                    drawingContext.DrawLine(new Pen(brush, 1), new Point(x, y), new Point(x, y + height));
                    _drawingContent = drawingContext.Close();
                }
            }

            public override void RenderContext(RenderContext renderContext)
            {
                if (_drawingContent != null)
                    renderContext.Render(_drawingContent);
            }
        }
    }
}
