using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Renderers
{
    public class SkiaDrawingContent : IDrawingContent, IDisposable
    {
        private readonly SKDrawable _drawable;
        private readonly SKPoint _originalPoint;
        private bool _disposed;

        public SkiaDrawingContent(SKDrawable drawable, SKPoint originalPoint)
        {
            _drawable = drawable;
            _originalPoint = originalPoint;
        }

        public SKDrawable Drawable => _drawable;

        public SKPoint OriginalPoint => _originalPoint;

        public bool HitTest(in Point point)
        {
            return _drawable.Bounds.Contains(point.X, point.Y);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _drawable.Dispose();
                }
                _disposed = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
