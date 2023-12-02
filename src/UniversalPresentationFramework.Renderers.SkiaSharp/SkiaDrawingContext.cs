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
            _canvas.DrawLine(point0.X, point0.Y, point1.X, point1.Y, paint);
        }

        public override void DrawEllipse(Brush brush, Pen pen, Point center, float radiusX, float radiusY)
        {
            CheckClosed();
            SKPaint paint = SkiaHelper.GetPaint(brush, pen);
            _canvas.DrawOval(center.X, center.Y, radiusX, radiusY, paint);
        }

        public override void DrawImage(ImageSource imageSource, Rect rectangle)
        {
            if (imageSource.Context == null)
                return;
            if (imageSource.Context is not SkiaImageContext context)
                throw new NotSupportedException("Only support skia image context.");
            CheckClosed();
            switch (context.Rotation)
            {
                case Media.Imaging.Rotation.Rotate0:
                    _canvas.DrawImage(context.Image, new SKRect(rectangle.X, rectangle.Y, rectangle.Right, rectangle.Bottom));
                    break;
                case Media.Imaging.Rotation.Rotate90:
                    _canvas.Save();
                    _canvas.Translate(rectangle.X + rectangle.Height, rectangle.Y);
                    _canvas.RotateDegrees(90);
                    _canvas.DrawImage(context.Image, new SKRect(0, 0, rectangle.Width, rectangle.Height));
                    _canvas.Restore();
                    break;
                case Media.Imaging.Rotation.Rotate180:
                    _canvas.Save();
                    _canvas.Translate(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height);
                    _canvas.RotateDegrees(180);
                    _canvas.DrawImage(context.Image, new SKRect(0, 0, rectangle.Width, rectangle.Height));
                    _canvas.Restore();
                    break;
                case Media.Imaging.Rotation.Rotate270:
                    _canvas.Save();
                    _canvas.Translate(rectangle.X, rectangle.Y + rectangle.Width);
                    _canvas.RotateDegrees(270);
                    _canvas.DrawImage(context.Image, new SKRect(0, 0, rectangle.Width, rectangle.Height));
                    _canvas.Restore();
                    break;
            }
        }

        public override void PushClip(Geometry clipGeometry)
        {
            CheckClosed();
            _canvas.Save();
            if (clipGeometry is RectangleGeometry rectangle)
            {
                var rect = new SKRect(rectangle.Rect.X, rectangle.Rect.Y, rectangle.Rect.Right, rectangle.Rect.Bottom);
                if (rectangle.RadiusX == 0 && rectangle.RadiusY == 0)
                    _canvas.ClipRect(rect);
                else
                    _canvas.ClipRoundRect(new SKRoundRect(rect, rectangle.RadiusX, rectangle.RadiusY));
            }
            else
            {
                //_canvas.ClipPath();
            }
        }

        public override void Pop()
        {
            _canvas.Restore();
        }
    }
}
