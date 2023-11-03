using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Renderers
{
    public class SkiaDrawingContext : DrawingContext, IDisposable
    {
        private readonly SKPictureRecorder _recorder;
        private readonly SKCanvas _canvas;
        private bool _disposed;

        internal SkiaDrawingContext(int width, int height)
        {
            _recorder = new SKPictureRecorder();
            _canvas = _recorder.BeginRecording(new SKRect(0, 0, width, height));
        }

        public override IDrawingContent Close()
        {            
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
    }
}
