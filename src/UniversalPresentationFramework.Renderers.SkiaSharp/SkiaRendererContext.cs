using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Controls;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Renderers
{
    public abstract class SkiaRendererContext : IRendererContext, IDisposable
    {
        private GRContext? _grContext;
        private SKSurface? _surface;
        private bool _disposed;
        private int _width, _height;
        private Stopwatch _stopwatch;

        private static SKPoint _FpsPoint;
        private static SKPaint _FpsPaint = new SKPaint(new SKFont(SKTypeface.Default, 24, 1, 0));
        static SkiaRendererContext()
        {
            _FpsPaint.SetColor(new SKColorF(1f, 1f, 0), null);
            _FpsPaint.Style = SKPaintStyle.Fill;
            _FpsPaint.TextAlign = SKTextAlign.Left;
            var rect = new SKRect();
            var measure = _FpsPaint.MeasureText("0", ref rect);
            _FpsPoint = new SKPoint(10 - rect.Left, 10 - rect.Top);
        }

        public SkiaRendererContext(GRContext? grContext)
        {
            _grContext = grContext;
            _stopwatch = new Stopwatch();
        }

        protected GRContext? GRContext => _grContext;

        public void Render(Visual visual)
        {
            if (_grContext == null)
                throw new InvalidOperationException("Skia renderer context not initialized.");

            var size = visual.GetVisualSize();
            var dpi = visual.GetDpi();
            int width = (int)(size.Width * dpi.DpiScaleX);
            int height = (int)(size.Height * dpi.DpiScaleY);
            if (_surface == null || _width != width || _height != height)
            {
                if (_surface != null)
                    _surface.Dispose();
                _surface = CreateSurface(width, height);
                if (_surface == null)
                    throw new NotSupportedException("Failed to create surface.");
                _width = width;
                _height = height;
            }
            BeforeRender();
            var canvas = _surface.Canvas;
            canvas.Clear(new SKColor(255, 255, 255, 0));
            canvas.ResetMatrix();
            canvas.Scale(dpi.DpiScaleX, dpi.DpiScaleY);
            RenderCore(visual);
            if (Debugger.IsAttached)
            {                
                var fps = (int)MathF.Round(1000f / _stopwatch.ElapsedMilliseconds);
                fps = Math.Max(1, fps);
                canvas.DrawText(fps.ToString(), _FpsPoint, _FpsPaint);
            }
            canvas.Flush();
            AfterRender();
            _surface.Flush();
            _stopwatch.Restart();
        }

        private void RenderCore(Visual visual)
        {
            var currentMatrix = _surface!.Canvas.TotalMatrix;
            SkiaRenderContext renderContext = new SkiaRenderContext(_surface!);
            visual.RenderContext(renderContext);
            int childrenCount = VisualTreeHelper.GetChildrenCount(visual);
            for (int i = 0; i < childrenCount; i++)
            {
                RenderCore(VisualTreeHelper.GetVisualChild(visual, i));
            }
            _surface!.Canvas.SetMatrix(currentMatrix);
        }

        protected virtual void BeforeRender()
        {

        }

        protected virtual void AfterRender()
        {

        }

        protected abstract SKSurface CreateSurface(int width, int height);

        #region Dispose

        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                DisposeCore(disposing);
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                    if (_grContext != null)
                    {
                        _grContext.Dispose();
                        _grContext = null;
                    }
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                _disposed = true;
            }
        }

        protected virtual void DisposeCore(bool disposing)
        {

        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
