using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Renderers
{
    public class SkiaDrawingContext : VisualDrawingContext, IDisposable
    {
        private readonly SKPictureRecorder _recorder;
        private readonly SKCanvas _canvas;
        private bool _disposed, _closed, _hasContent;

        internal SkiaDrawingContext(int width, int height)
        {
            _recorder = new SKPictureRecorder();
            _canvas = _recorder.BeginRecording(new SKRect(0, 0, width, height));
        }

        private void CheckClosed()
        {
            if (_closed)
                throw new InvalidOperationException("Could not use a closed drawing context.");
            _hasContent = true;
        }

        public override IDrawingContent? Close()
        {
            if (!_hasContent)
                return null;
            return new SkiaDrawingContent(_recorder.EndRecordingAsDrawable(), new SKPoint());
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _canvas.Dispose();
                    _recorder.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public override void DrawRoundedRectangle(Brush brush, Pen pen, Rect rectangle, float radiusX, float radiusY)
        {
            CheckClosed();
            SKPaint paint = SkiaHelper.GetPaint(brush, pen);
            _canvas.DrawRoundRect(new SKRect(rectangle.X, rectangle.Y, rectangle.Right, rectangle.Bottom), radiusX, radiusY, paint);
        }

        public override void DrawRectangle(Brush brush, Pen pen, Rect rectangle)
        {
            CheckClosed();
            SKPaint paint = SkiaHelper.GetPaint(brush, pen);
            _canvas.DrawRect(new SKRect(rectangle.X, rectangle.Y, rectangle.Right, rectangle.Bottom), paint);
        }

        public override void DrawLine(Pen pen, Point point0, Point point1)
        {
            CheckClosed();
            SKPaint paint = SkiaHelper.GetPaint(null, pen);
            ref var skPoint0 = ref Unsafe.As<Point, SKPoint>(ref point0);
            ref var skPoint1 = ref Unsafe.As<Point, SKPoint>(ref point1);
            _canvas.DrawLine(skPoint0, skPoint1, paint);
        }

        public override void DrawEllipse(Brush brush, Pen pen, Point center, float radiusX, float radiusY)
        {
            CheckClosed();
            SKPaint paint = SkiaHelper.GetPaint(brush, pen);
            _canvas.DrawOval(center.X, center.Y, radiusX, radiusY, paint);
            
        }
    }
}
