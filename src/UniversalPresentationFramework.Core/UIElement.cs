using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI
{
    public class UIElement : Visual
    {
        #region Layout

        private Size _previousAvailableSize;
        private bool _isMeasuring, _isMeasuringDuringArrange;
        public void Measure(Size availableSize)
        {
            if (float.IsNaN(availableSize.Width) || float.IsNaN(availableSize.Height))
                throw new InvalidOperationException("Width and height can not be NaN.");

            if (Visibility == Visibility.Collapsed)
                return;

            bool isCloseToPreviousMeasure = FloatUtil.AreClose(availableSize, _previousAvailableSize);
            if (isCloseToPreviousMeasure)
                return;

            _isMeasuring = true;
            var previousSize = _desiredSize;
            Size desiredSize;
            try
            {
                desiredSize = MeasureCore(availableSize);
            }
            finally
            {
                _isMeasuring = false;
            }
            if (float.IsPositiveInfinity(desiredSize.Width) || float.IsPositiveInfinity(desiredSize.Height))
                throw new InvalidOperationException("Measure size can not be positive infinity.");
            if (float.IsNaN(desiredSize.Width) || float.IsNaN(desiredSize.Height))
                throw new InvalidOperationException("Measure size can not be NaN.");
            _previousAvailableSize = availableSize;
            _desiredSize = desiredSize;
            IsMeasureValid = true;

            if (!_isMeasuringDuringArrange && !FloatUtil.AreClose(_desiredSize, previousSize))
            {
                var parent = VisualParent as UIElement;
                if (parent != null && !parent._isMeasuring)
                    parent.OnChildDesiredSizeChanged(this);
            }
        }

        private Rect _previousFinalRect;
        private bool _isArranging, _isRenderValid;
        public void Arrange(Rect finalRect)
        {
            if (float.IsPositiveInfinity(finalRect.Width)
                || float.IsPositiveInfinity(finalRect.Height)
                || float.IsNaN(finalRect.Width)
                || float.IsNaN(finalRect.Height))
                throw new InvalidOperationException("Width and height can not be positive infinity and NaN.");

            if (Visibility == Visibility.Collapsed)
                return;

            if (!IsMeasureValid)
            {
                _isMeasuringDuringArrange = true;
                try
                {
                    Measure(finalRect.Size);
                }
                finally
                {
                    _isMeasuringDuringArrange = false;
                }
            }

            if (!IsArrangeValid || !FloatUtil.AreClose(finalRect, _previousFinalRect))
            {
                _isArranging = true;
                var oldRenderSize = RenderSize;
                try
                {
                    ArrangeCore(finalRect);
                }
                finally
                {
                    _isArranging = false;
                }
                _previousFinalRect = finalRect;
                IsArrangeValid = true;

                if (IsArrangeValid && IsMeasureValid && (!FloatUtil.AreClose(oldRenderSize, _renderSize)|| !_isRenderValid) )
                {
                    if (FrameworkProvider.RendererProvider != null)
                    {
                        var drawingContext = FrameworkProvider.RendererProvider.CreateDrawingContext(this);
                        try
                        {
                            OnRender(drawingContext);
                        }
                        finally
                        {
                            _drawingContent = drawingContext.Close();
                            _isRenderValid = true;
                        }
                    }
                }
            }
        }

        protected virtual Size MeasureCore(Size availableSize)
        {
            return new Size();
        }

        protected virtual void ArrangeCore(Rect finalRect)
        {
            RenderSize = finalRect.Size;
        }

        public void InvalidateMeasure()
        {
            IsMeasureValid = false;
        }

        public void InvalidateArrange()
        {
            IsArrangeValid = false;
        }

        public void InvalidateVisual()
        {
            _isRenderValid = false;
        }

        protected virtual void OnChildDesiredSizeChanged(UIElement child)
        {
            if (IsMeasureValid)
            {
                InvalidateMeasure();
            }
        }

        private Size _desiredSize;
        public Size DesiredSize
        {
            get
            {
                if (Visibility == Visibility.Collapsed)
                    return new Size(0, 0);
                else
                    return _desiredSize;
            }
        }

        private Size _renderSize;
        public Size RenderSize
        {
            get
            {
                if (this.Visibility == Visibility.Collapsed)
                    return new Size();
                else
                    return _renderSize;
            }
            set
            {
                _renderSize = value;
            }
        }

        public override Size GetVisualSize()
        {
            return _renderSize;
        }

        public bool IsMeasureValid { get; private set; }

        public bool IsArrangeValid { get; private set; }

        #endregion

        #region Render

        private IDrawingContent? _drawingContent;
        protected virtual void OnRender(DrawingContext drawingContext)
        {

        }

        public sealed override void RenderContext(RenderContext renderContext)
        {
            if (_drawingContent != null)
                renderContext.Render(_drawingContent);
        }

        #endregion


        public static readonly DependencyProperty VisibilityProperty =
        DependencyProperty.Register(
                "Visibility",
                typeof(Visibility),
                typeof(UIElement),
                new PropertyMetadata(
                        Visibility.Visible,
                        new PropertyChangedCallback(OnVisibilityChanged)),
                new ValidateValueCallback(ValidateVisibility));

        private static void OnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement uie = (UIElement)d;
            Visibility newVisibility = (Visibility)e.NewValue!;
            uie._visibility = newVisibility;
        }

        private static bool ValidateVisibility(object? o)
        {
            if (o is Visibility value)
                return (value == Visibility.Visible) || (value == Visibility.Hidden) || (value == Visibility.Collapsed);
            return false;
        }

        private Visibility _visibility;
        public Visibility Visibility
        {
            get { return _visibility; }
            set { SetValue(VisibilityProperty, value); }
        }
    }
}
