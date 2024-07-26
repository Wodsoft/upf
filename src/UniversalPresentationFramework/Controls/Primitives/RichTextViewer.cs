using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Documents;
using Wodsoft.UI.Input;
using Wodsoft.UI.Media.Animation;
using Wodsoft.UI.Media;
using Wodsoft.UI.Threading;

namespace Wodsoft.UI.Controls.Primitives
{
    public class RichTextViewer : FrameworkElement, IInputMethodSource
    {
        private IReadOnlyTextContainer? _textContainer;
        private FrameworkElement? _owner;
        private float _offsetX, _offsetY, _caretX, _caretY, _caretHeight, _measuredHeight;
        private Storyboard? _caretStoryboard;
        private bool _caretVisible, _caretMove, _caretUpdate;
        private IInputMethodContext? _inputMethodContext;
        private readonly List<TextPosition> _textPositions = new List<TextPosition>();
        private readonly RichTextViewerCaret _caret = new RichTextViewerCaret();
        private int _caretTextPosition;

        #region Properties

        public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty = ScrollViewer.HorizontalScrollBarVisibilityProperty.AddOwner(typeof(RichTextViewer),
            new FrameworkPropertyMetadata(ScrollBarVisibility.Disabled, FrameworkPropertyMetadataOptions.AffectsRender));
        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get { return (ScrollBarVisibility)GetValue(HorizontalScrollBarVisibilityProperty)!; }
            set { SetValue(HorizontalScrollBarVisibilityProperty, value); }
        }

        public static readonly DependencyProperty VerticalScrollBarVisibilityProperty = ScrollViewer.VerticalScrollBarVisibilityProperty.AddOwner(typeof(RichTextViewer),
            new FrameworkPropertyMetadata(ScrollBarVisibility.Disabled, FrameworkPropertyMetadataOptions.AffectsRender));
        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get { return (ScrollBarVisibility)GetValue(VerticalScrollBarVisibilityProperty)!; }
            set { SetValue(VerticalScrollBarVisibilityProperty, value); }
        }

        public static readonly DependencyProperty PaddingProperty = Control.PaddingProperty.AddOwner(typeof(RichTextViewer), new FrameworkPropertyMetadata(new Thickness(2, 0, 2, 0), FrameworkPropertyMetadataOptions.AffectsRender));
        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty)!; }
            set { SetValue(PaddingProperty, value); }
        }

        public static readonly DependencyProperty CaretBrushProperty = TextBoxBase.CaretBrushProperty.AddOwner(typeof(RichTextViewer), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
        public Brush? CaretBrush
        {
            get { return (Brush?)GetValue(CaretBrushProperty); }
            set { SetValue(CaretBrushProperty, value); }
        }

        private static readonly DependencyProperty _CaretVisibleProperty = DependencyProperty.Register("CaretVisible", typeof(bool), typeof(RichTextViewer), new FrameworkPropertyMetadata(false, OnCaretVisibleChanged));
        private static void OnCaretVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RichTextViewer)d)._caretVisible = (bool)e.NewValue!;
            if (d.Dispatcher is UIDispatcher uiDispatcher)
                uiDispatcher.UpdateRender();
        }

        //public static readonly DependencyProperty TextWrappingProperty = TextBlock.TextWrappingProperty.AddOwner(typeof(RichTextViewer));
        //public TextWrapping TextWrapping
        //{
        //    get => (TextWrapping)GetValue(TextWrappingProperty)!;
        //    set => SetValue(TextWrappingProperty, value);
        //}

        //public static readonly DependencyProperty TextTrimmingProperty = TextBlock.TextTrimmingProperty.AddOwner(typeof(RichTextViewer));
        //public TextTrimming TextTrimming
        //{
        //    get => (TextTrimming)GetValue(TextTrimmingProperty)!;
        //    set => SetValue(TextTrimmingProperty, value);
        //}

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

        public void InitializeViewer(IReadOnlyTextContainer textContainer, FrameworkElement owner)
        {
            _textContainer = textContainer;
            _owner = owner;
            textContainer.SelectionChanged += TextOwner_SelectionChanged;
            textContainer.TextChanged += TextOwner_TextChanged;
            //owner.PreviewTextInput += TextOwner_PreviewTextInput;
            //owner.GotKeyboardFocus += TextOwner_GotKeyboardFocus;
            //owner.LostKeyboardFocus += TextOwner_LostKeyboardFocus;
            //owner.KeyDown += TextOwner_KeyDown;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (_textContainer == null)
                return Size.Empty;
            if (availableSize != DesiredSize)
            {
                _textPositions.Clear();
                _measuredHeight = 0f;
                if (_textContainer.IsCaretVisible)
                {
                    _caretTextPosition = -1;
                    _caretUpdate = true;
                }
            }
            var padding = Padding;
            float width = padding.Left + padding.Right, height = padding.Top + padding.Bottom;
            float maxWidth = 0f, maxHeight = 0f;
            if (_textPositions.Count == 0)
            {
                float y = 0;
                TextTreeTextElementNode? node = _textContainer.Root.FirstChildNode as TextTreeTextElementNode;
                Size blockSize = new Size(availableSize.Width - width, float.PositiveInfinity);
                while (node != null)
                {
                    var block = node.TextElement as Block;
                    if (block != null)
                    {
                        var layout = block.Layout;
                        layout.Measure(blockSize);
                        _textPositions.Add(new TextPosition(block, 0, y, layout.Width, layout.Height));
                        y += layout.Height;
                    }
                    node = node.NextNode as TextTreeTextElementNode;
                }
            }
            var spans = CollectionsMarshal.AsSpan(_textPositions);
            for (int i = 0; i < spans.Length; i++)
            {
                ref var textPosition = ref spans[i];
                var right = textPosition.X + textPosition.Width;
                if (maxWidth < right)
                    maxWidth = right;
                var bottom = textPosition.Y + textPosition.Height;
                if (maxHeight < bottom)
                    maxHeight = bottom;
            }
            return new Size(MathF.Min(availableSize.Width, width + maxWidth), MathF.Min(availableSize.Height, height + maxHeight));
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (_textContainer == null)
                return;
            var renderSize = RenderSize;
            var padding = Padding;
            drawingContext.PushClip(new RectangleGeometry(new Rect(padding.Left, padding.Top, renderSize.Width - padding.Left - padding.Right, renderSize.Height - padding.Top - padding.Bottom)));
            if (_caretMove)
            {
                _caretMove = false;
                UpdateCaret(true);
            }
            float left = _offsetX;
            float top = _offsetY;
            float right = left + renderSize.Width - padding.Left - padding.Right;
            float bottom = top + renderSize.Height - padding.Top - padding.Bottom;
            var spans = CollectionsMarshal.AsSpan(_textPositions);
            for (int i = 0; i < spans.Length; i++)
            {
                ref var textPosition = ref spans[i];
                if ((textPosition.Y >= top && textPosition.Y < bottom) || (textPosition.Bottom > top && textPosition.Bottom <= bottom))
                {
                    if ((textPosition.X >= left && textPosition.X < right) || (textPosition.Right > left && textPosition.Right <= right) || (textPosition.X <= left && textPosition.Right >= right))
                    {
                        var clipLeft = MathF.Max(textPosition.X, left);
                        var clipTop = MathF.Max(textPosition.Y, top);
                        var clipRight = MathF.Min(textPosition.Right, right);
                        var clipBottom = MathF.Min(textPosition.Bottom, bottom);
                        var clip = new Rect(clipLeft - textPosition.X, clipTop - textPosition.Y, clipRight - clipLeft, clipBottom - clipTop);
                        textPosition.Block.Layout.Draw(drawingContext, new Point(textPosition.X - _offsetX + padding.Left, textPosition.Y - _offsetY + padding.Top), clip);
                    }
                }
                else
                    break;
            }
            drawingContext.Pop();
            if (_caretUpdate)
            {
                UpdateCaret(false);
                _caretUpdate = false;
            }
        }

        private void TextOwner_TextChanged(object? sender, TextChangedEventArgs e)
        {
            if (_caretStoryboard != null)
                _caretStoryboard.Seek(TimeSpan.Zero, TimeSeekOrigin.BeginTime);
            _caretUpdate = true;
            _textPositions.Clear();
            _caretTextPosition = -1;
            _measuredHeight = 0f;
            InvalidateVisual();
        }

        private void TextOwner_SelectionChanged(object? sender, RoutedEventArgs e)
        {
            if (_caretStoryboard != null)
                _caretStoryboard.Seek(TimeSpan.Zero, TimeSeekOrigin.BeginTime);
            _caretUpdate = true;
            InvalidateVisual();
        }

        private void UpdateCaret(bool moveView)
        {
            if (_textContainer!.SelectionStart == _textContainer.SelectionEnd)
            {
                var selectionStart = _textContainer.SelectionStart;
                var spans = CollectionsMarshal.AsSpan(_textPositions);
                for (int i = 0; i < spans.Length; i++)
                {
                    ref var textPosition = ref spans[i];
                    if (selectionStart < textPosition.Block.ElementStart)
                        continue;
                    else if (selectionStart > textPosition.Block.ElementEnd)
                        continue;
                    var rect = textPosition.Block.Layout.GetRectAtCharacter(selectionStart);
                    var padding = Padding;
                    rect.X += 0.5f + textPosition.X + padding.Left - _offsetX;
                    rect.Y += textPosition.Y + padding.Top - _offsetY;
                    rect.Width = 1f;
                    var clip = new Rect(padding.Left, padding.Top, RenderSize.Width - padding.Left - padding.Right, RenderSize.Height - padding.Top - padding.Bottom);
                    if (moveView)
                    {
                        if (rect.Top < clip.Top)
                        {
                            var value = clip.Top - rect.Top;
                            _offsetY -= value;
                            rect.Y += value;
                        }
                        else if (rect.Bottom > clip.Bottom)
                        {
                            var value = rect.Bottom - clip.Bottom;
                            _offsetY += value;
                            rect.Y -= value;
                        }
                        if (rect.Left < clip.Left)
                        {
                            var value = clip.Left - rect.Left;
                            _offsetX -= value;
                            rect.X += value;
                        }
                        else if (rect.Right > clip.Right)
                        {
                            var value = rect.Right - clip.Right;
                            _offsetX += value;
                            rect.X -= value;
                        }
                    }
                    _caretX = rect.X;
                    _caretY = rect.Y;
                    _caretHeight = textPosition.Height;
                    _caretTextPosition = i;
                    if (((rect.Top >= clip.Top && rect.Top < clip.Bottom) || (rect.Bottom >= clip.Top && rect.Bottom < clip.Bottom)) &&
                        (rect.Left >= clip.Left && rect.Left <= clip.Right))
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
                        clip = new Rect(0, 0, RenderSize.Width, RenderSize.Height);
                        _caret.BuildCaretRender(rect.Left, rect.Top, rect.Height, caretBrush, new RectangleGeometry(clip));
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
            if (_textContainer != null)
            {
                var renderSize = RenderSize;
                if (point.X >= 0 && point.X < renderSize.Width && point.Y >= 0 && point.Y < renderSize.Height)
                    return this;
            }
            return null;
        }

        private bool _mouseDown;
        private TextPointer? _mousePositionStart;
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (_textContainer != null && _textContainer.IsSelectable)
            {
                _mouseDown = true;
                CaptureMouse();
                var padding = Padding;
                var point = e.GetPosition(this);
                point.X -= padding.Left - _offsetX;
                point.Y -= padding.Top - _offsetY;
                var position = GetTextPosition(point);
                if (position != _textContainer.SelectionStart)
                    _textContainer.Select(position, position);
                _mousePositionStart = position;
                _owner!.Focus();
                InvalidateVisual();
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
                if (_mousePositionStart! > position)
                    _textContainer!.Select(position, _mousePositionStart!);
                else
                    _textContainer!.Select(_mousePositionStart!, position);
                InvalidateVisual();
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

        private TextPointer GetTextPosition(in Point point)
        {
            if (_textPositions.Count == 0)
                return _textContainer!.SelectionStart;
            var x = point.X;
            var y = point.Y;
            int i;
            //float lastLine = float.PositiveInfinity;
            for (i = 0; i < _textPositions.Count; i++)
            {
                var text = _textPositions[i];
                if (y < text.Y || y > text.Bottom)
                    continue;
                //if (text.X == 0 && lastLine != text.Y && x > text.X + text.Block.Width)
                //    return text.Block.Position + text.Block.Length;
                //if (x < text.X && lastLine != text.Y)
                //    return text.Block.Position;
                if (x >= text.X && x <= text.Right)
                    return text.Block.Layout.GetCharacterAtPoint(new Point(x - text.X, y - text.Y));
                //lastLine = text.Y;
            }
            return _textContainer!.DocumentEnd;
        }

        #endregion

        #region Keyboard Input

        //private void TextOwner_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        //{
        //    if (!_textContainer!.IsCaretVisible)
        //        return;
        //    if (_caretStoryboard == null)
        //    {
        //        _caretStoryboard = new Storyboard();
        //        _caretStoryboard.RepeatBehavior = RepeatBehavior.Forever;

        //        BooleanAnimationUsingKeyFrames blinkAnimation = new BooleanAnimationUsingKeyFrames();
        //        blinkAnimation.BeginTime = null;
        //        blinkAnimation.KeyFrames.Add(new DiscreteBooleanKeyFrame(true, KeyTime.FromPercent(0.0f)));
        //        blinkAnimation.KeyFrames.Add(new DiscreteBooleanKeyFrame(false, KeyTime.FromPercent(0.5f)));
        //        blinkAnimation.Duration = TimeSpan.FromSeconds(1);
        //        Storyboard.SetTarget(blinkAnimation, this);
        //        Storyboard.SetTargetProperty(blinkAnimation, new PropertyPath("CaretVisible"));
        //        _caretStoryboard.Children.Add(blinkAnimation);
        //        _caretStoryboard.Begin();

        //        UpdateCaret(false);
        //    }
        //    else
        //    {
        //        _caretStoryboard.Begin();
        //    }
        //    if (!_textContainer.IsReadOnly)
        //    {
        //        if (_inputMethodContext == null)
        //            _inputMethodContext = InputMethod.Current.CreateContext(this);
        //        if (_inputMethodContext != null)
        //            _inputMethodContext.Focus();
        //    }
        //}

        //private void TextOwner_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        //{
        //    if (_textContainer!.IsReadOnly && !_textContainer.IsReadOnlyCaretVisible)
        //        return;
        //    if (_caretStoryboard != null)
        //    {
        //        _caretStoryboard.Stop();
        //        _caretVisible = false;
        //    }
        //    if (_inputMethodContext != null)
        //        _inputMethodContext.Unfocus();
        //}

        //private void TextOwner_PreviewTextInput(object sender, TextCompositionEventArgs e)
        //{
        //    _caretMove = true;
        //    if (!_caret.IsVisible)
        //        UpdateCaret(true);
        //}

        //private void TextOwner_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Left)
        //    {
        //        if (_textContainer!.SelectionStart == _textContainer.FirstNode.Start)
        //            return;
        //        _caretMove = true;
        //        var position = _textContainer.SelectionStart.GetPositionAtOffset(1, LogicalDirection.Backward);
        //        if (position != null)
        //        {
        //            _textContainer!.Select(position, position);
        //            e.Handled = true;
        //        }
        //    }
        //    else if (e.Key == Key.Right)
        //    {
        //        if (_textContainer!.SelectionStart >= _textContainer.LastNode.End)
        //            return;
        //        _caretMove = true;
        //        var position = _textContainer.SelectionStart.GetPositionAtOffset(1, LogicalDirection.Forward);
        //        if (position != null)
        //        {
        //            _textContainer!.Select(position, position);
        //            e.Handled = true;
        //        }
        //    }
        //    else if (e.Key == Key.Up)
        //    {
        //        if (_caretTextPosition == -1)
        //            return;
        //        var spans = CollectionsMarshal.AsSpan(_textPositions);
        //        ref var textPosition = ref spans[_caretTextPosition];
        //        if (!textPosition.Block.GetCharacterRelateToCharacter(_textContainer!.SelectionStart, LogicalDirection.Backward, out var position))
        //        {
        //            if (_caretTextPosition == 0)
        //                return;
        //            var x = _caretX + _offsetX - Padding.Left - 0.5f;
        //            textPosition = ref spans[_caretTextPosition - 1];
        //            position = textPosition.Block.GetCharacterAtPoint(new Point(x, textPosition.Bottom));
        //        }
        //        _caretMove = true;
        //        _textContainer!.Select(position, position);
        //    }
        //    else if (e.Key == Key.Down)
        //    {
        //        if (_caretTextPosition == -1)
        //            return;
        //        var spans = CollectionsMarshal.AsSpan(_textPositions);
        //        ref var textPosition = ref spans[_caretTextPosition];
        //        if (!textPosition.Block.GetCharacterRelateToCharacter(_textContainer!.SelectionStart, LogicalDirection.Forward, out var position))
        //        {
        //            if (_caretTextPosition == spans.Length - 1)
        //                return;
        //            var x = _caretX + _offsetX - Padding.Left - textPosition.X - 0.5f;
        //            textPosition = ref spans[_caretTextPosition + 1];
        //            position = textPosition.Block.GetCharacterAtPoint(new Point(x, 0));
        //        }
        //        _caretMove = true;
        //        _textContainer!.Select(position, position);
        //    }
        //    else if (e.Key == Key.Home)
        //    {
        //        if (_caretTextPosition == -1)
        //            return;
        //        var spans = CollectionsMarshal.AsSpan(_textPositions);
        //        ref var textPosition = ref spans[_caretTextPosition];
        //        var y = _caretY + _offsetY - Padding.Top + _caretHeight / 2f - textPosition.Y;
        //        var position = textPosition.Block.GetCharacterAtPoint(new Point(0, y));
        //        if (_textContainer!.SelectionStart != position || _textContainer!.SelectionEnd != position)
        //            _textContainer.Select(position, position);
        //    }
        //    else if (e.Key == Key.End)
        //    {
        //        if (_caretTextPosition == -1)
        //            return; ;
        //        var spans = CollectionsMarshal.AsSpan(_textPositions);
        //        ref var textPosition = ref spans[_caretTextPosition];
        //        var y = _caretY + _offsetY - Padding.Top + _caretHeight / 2f - textPosition.Y;
        //        var position = textPosition.Block.GetCharacterAtPoint(new Point(textPosition.Width, y));
        //        if (_textContainer!.SelectionStart != position || _textContainer!.SelectionEnd != position)
        //            _textContainer.Select(position, position);
        //    }
        //}

        #endregion

        #region Input Method Source

        UIElement IInputMethodSource.UIScope => _owner ?? throw new InvalidOperationException("No ui scope found.");

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

        private class RichTextViewerCaret : Visual
        {
            private IDrawingContent? _drawingContent;

            public bool IsVisible { get; set; }

            public override bool HasRenderContent => IsVisible;

            public void BuildCaretRender(float x, float y, float height, Brush brush, RectangleGeometry clip)
            {
                if (FrameworkCoreProvider.RendererProvider != null)
                {
                    var drawingContext = FrameworkCoreProvider.RendererProvider.CreateDrawingContext(this);
                    drawingContext.PushClip(clip);
                    drawingContext.DrawLine(new Pen(brush, 1), new Point(x, y), new Point(x, y + height));
                    drawingContext.Pop();
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
