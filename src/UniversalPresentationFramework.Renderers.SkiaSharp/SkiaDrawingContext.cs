using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Renderers
{
    public class SkiaDrawingContext : VisualDrawingContext, IDisposable
    {
        private readonly SKPictureRecorder _recorder;
        private readonly SKCanvas _canvas;
        private readonly List<OpacityState> _opacityStates;
        private bool _disposed, _closed, _hasContent;

        internal SkiaDrawingContext(int width, int height)
        {
            _recorder = new SKPictureRecorder();
            _canvas = _recorder.BeginRecording(new SKRect(0, 0, width, height));
            _opacityStates = new List<OpacityState>();
        }

        private void CheckClosed()
        {
            if (_closed)
                throw new InvalidOperationException("Could not use a closed drawing context.");
            _hasContent = true;
        }

        private void ApplyPaint(SKPaint skPaint)
        {
            if (_opacityStates.Count != 0)
            {
                float opacity = 1f;
                var states = CollectionsMarshal.AsSpan(_opacityStates);
                for (int i = 0; i < states.Length; i++)
                {
                    ref var state = ref states[i];
                    opacity *= state.Opacity;
                    skPaint.ColorFilter = SKColorFilter.CreateColorMatrix([
                        1, 0, 0, 0, 0,//R
                    0, 1, 0, 0, 0,//G
                    0, 0, 1, 0, 0,//B
                    0, 0, 0, opacity, 0//A
                    ]);
                }
            }
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

        public override void DrawRoundedRectangle(Brush? brush, Pen? pen, Rect rectangle, float radiusX, float radiusY)
        {
            CheckClosed();
            if (brush == null && pen == null)
                return;
            var rect = new SKRect(rectangle.X, rectangle.Y, rectangle.Right, rectangle.Bottom);
            if (brush != null)
            {
                SKPaint? paint = SkiaHelper.GetFillPaint(brush, pen, rectangle);
                if (paint != null)
                {
                    ApplyPaint(paint);
                    _canvas.DrawRoundRect(rect, radiusX, radiusY, paint);
                }
            }
            if (pen != null)
            {
                SKPaint? paint = SkiaHelper.GetStrokePaint(pen, rectangle);
                if (paint != null)
                {
                    ApplyPaint(paint);
                    _canvas.DrawRoundRect(rect, radiusX, radiusY, paint);
                }
            }
        }

        public override void DrawRectangle(Brush? brush, Pen? pen, Rect rectangle)
        {
            CheckClosed();
            if (brush == null && pen == null)
                return;
            var rect = new SKRect(rectangle.X, rectangle.Y, rectangle.Right, rectangle.Bottom);
            if (brush != null)
            {
                SKPaint? paint = SkiaHelper.GetFillPaint(brush, pen, rectangle);
                if (paint != null)
                {
                    ApplyPaint(paint);
                    _canvas.DrawRect(rect, paint);
                }
            }
            if (pen != null)
            {
                SKPaint? paint = SkiaHelper.GetStrokePaint(pen, rectangle);
                if (paint != null)
                {
                    ApplyPaint(paint);
                    _canvas.DrawRect(rect, paint);
                }
            }
        }

        public override void DrawLine(Pen? pen, Point point0, Point point1)
        {
            CheckClosed();
            if (pen != null)
            {
                SKPaint? paint = SkiaHelper.GetStrokePaint(pen, new Rect(point0, point1));
                if (paint != null)
                {
                    ApplyPaint(paint);
                    _canvas.DrawLine(point0.X, point0.Y, point1.X, point1.Y, paint);
                }
            }
        }

        public override void DrawEllipse(Brush? brush, Pen? pen, Point center, float radiusX, float radiusY)
        {
            CheckClosed();
            var rect = new Rect(center.X - radiusX, center.Y - radiusY, radiusX * 2, radiusY * 2);
            if (brush != null)
            {
                SKPaint? paint = SkiaHelper.GetFillPaint(brush, pen, rect);
                if (paint != null)
                {
                    ApplyPaint(paint);
                    _canvas.DrawOval(center.X, center.Y, radiusX, radiusY, paint);
                }
            }
            if (pen != null)
            {
                SKPaint? paint = SkiaHelper.GetStrokePaint(pen, rect);
                if (paint != null)
                {
                    ApplyPaint(paint);
                    _canvas.DrawOval(center.X, center.Y, radiusX, radiusY, paint);
                }
            }
        }

        public override void DrawImage(ImageSource imageSource, Rect rectangle)
        {
            if (imageSource.Context == null)
                return;
            if (imageSource.Context is not ISkiaImageContext context)
                throw new NotSupportedException("Only support skia image context.");
            CheckClosed();
            SKPaint? paint;
            if (_opacityStates.Count == 0)
                paint = null;
            else
            {
                paint = new SKPaint();
                ApplyPaint(paint);
            }
            switch (context.Rotation)
            {
                case Media.Imaging.Rotation.Rotate0:
                    _canvas.DrawImage(context.Image, new SKRect(rectangle.X, rectangle.Y, rectangle.Right, rectangle.Bottom), paint);
                    break;
                case Media.Imaging.Rotation.Rotate90:
                    _canvas.Save();
                    _canvas.Translate(rectangle.X + rectangle.Height, rectangle.Y);
                    _canvas.RotateDegrees(90);
                    _canvas.DrawImage(context.Image, new SKRect(0, 0, rectangle.Width, rectangle.Height), paint);
                    _canvas.Restore();
                    break;
                case Media.Imaging.Rotation.Rotate180:
                    _canvas.Save();
                    _canvas.Translate(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height);
                    _canvas.RotateDegrees(180);
                    _canvas.DrawImage(context.Image, new SKRect(0, 0, rectangle.Width, rectangle.Height), paint);
                    _canvas.Restore();
                    break;
                case Media.Imaging.Rotation.Rotate270:
                    _canvas.Save();
                    _canvas.Translate(rectangle.X, rectangle.Y + rectangle.Width);
                    _canvas.RotateDegrees(270);
                    _canvas.DrawImage(context.Image, new SKRect(0, 0, rectangle.Width, rectangle.Height), paint);
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

        public override void PushOpacity(float opacity)
        {
            CheckClosed();
            _opacityStates.Add(new OpacityState(_canvas.SaveCount, opacity));
        }

        public override void PushTransform(Transform transform)
        {
            CheckClosed();
            _canvas.Save();
            var matrix = transform.Value;
            _canvas.Concat(SkiaHelper.GetMatrix(matrix));
        }

        public override void Pop()
        {
            CheckClosed();
            if (_opacityStates.Count != 0)
            {
                ref var state = ref CollectionsMarshal.AsSpan(_opacityStates)[_opacityStates.Count - 1];
                if (state.SaveCount == _canvas.SaveCount)
                {
                    _opacityStates.RemoveAt(_opacityStates.Count - 1);
                    return;
                }
            }
            _canvas.Restore();
        }

        public override void DrawGeometry(Brush? brush, Pen? pen, Geometry geometry)
        {
            CheckClosed();
            if (geometry is RectangleGeometry rectangle)
            {
                float rx = rectangle.RadiusX, ry = rectangle.RadiusY;
                if (rx == 0 && ry == 0)
                    DrawRectangle(brush, pen, rectangle.Rect);
                else
                    DrawRoundedRectangle(brush, pen, rectangle.Rect, rx, ry);
            }
            else
            {
                var data = geometry.GetPathGeometryData();
                if (data == null)
                    return;
                var rect = geometry.Bounds;
                if (brush != null)
                {
                    SKPaint? paint = SkiaHelper.GetFillPaint(brush, pen, rect);
                    if (paint != null)
                    {
                        ApplyPaint(paint);
                        if (data is SkiaGeometryData skData)
                        {
                            _canvas.DrawPath(skData.Path, paint);
                        }
                        else
                        {
                            var path = SKPath.ParseSvgPathData(data.ToPathString());
                            _canvas.DrawPath(path, paint);
                        }
                    }
                }
                if (pen != null)
                {
                    SKPaint? paint = SkiaHelper.GetStrokePaint(pen, rect);
                    if (paint != null)
                    {
                        ApplyPaint(paint);
                        if (data is SkiaGeometryData skData)
                        {
                            _canvas.DrawPath(skData.Path, paint);
                        }
                        else
                        {
                            var path = SKPath.ParseSvgPathData(data.ToPathString());
                            _canvas.DrawPath(path, paint);
                        }
                    }
                }
            }
        }

        public override void DrawText(ReadOnlySpan<char> text, GlyphTypeface glyphTypeface, float fontSize, Brush foreground, Point origin)
        {
            if (glyphTypeface is not SkiaGlyphTypeface skiaGlyphTypeface)
                throw new NotSupportedException("Only support SkiaGlyphTypeface.");
            CheckClosed();
            var font = skiaGlyphTypeface.SKTypeface.ToFont(fontSize);
            var blob = SKTextBlob.Create(text, font)!;
            var textRect = blob.Bounds;            
            var paint = SkiaHelper.GetFillPaint(foreground, null, new Rect(origin, new Size(textRect.Width, textRect.Height)));
            if (paint != null)
            {
                ApplyPaint(paint);
                _canvas.DrawText(blob, origin.X, origin.Y, paint);
            }
        }

        public override void DrawText(ReadOnlySpan<char> text, GlyphTypeface glyphTypeface, float fontSize, Brush foreground, Point origin, Size size)
        {
            if (glyphTypeface is not SkiaGlyphTypeface skiaGlyphTypeface)
                throw new NotSupportedException("Only support SkiaGlyphTypeface.");
            CheckClosed();
            var font = skiaGlyphTypeface.SKTypeface.ToFont(fontSize);
            var paint = SkiaHelper.GetFillPaint(foreground, null, new Rect(origin, size));
            if (paint != null)
            {
                ApplyPaint(paint);
                _canvas.DrawText(SKTextBlob.Create(text, font), origin.X, origin.Y, paint);
            }
        }

        private struct OpacityState
        {
            public int SaveCount;
            public float Opacity;

            public OpacityState(int saveCount, float opacity)
            {
                SaveCount = saveCount;
                Opacity = opacity;
            }
        }
    }
}
