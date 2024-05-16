using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Controls;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Renderers
{
    public abstract class SkiaRendererContext : IRendererContext, IDisposable
    {
        private bool _disposed;
        private Stopwatch _stopwatch;

        private static SKPoint _FpsPoint;
        private static SKFont _FpsFont = new SKFont(SKTypeface.Default, 24, 1, 0);
        private static SKPaint _FpsPaint = new SKPaint();
        static SkiaRendererContext()
        {
            _FpsPaint.SetColor(new SKColorF(1f, 1f, 0), null);
            _FpsPaint.Style = SKPaintStyle.Fill;
            _FpsFont.MeasureText("0", out var rect);
            _FpsPoint = new SKPoint(10 - rect.Left, 10 - rect.Top);
        }

        public SkiaRendererContext()
        {
            _stopwatch = new Stopwatch();
        }

        public bool IsShowFPS { get; set; } = true;

        public virtual void Render(Visual visual)
        {
            var size = visual.GetVisualSize();
            if (size.IsEmpty)
                return;
            var dpi = visual.GetDpi();
            int width = (int)(size.Width * dpi.DpiScaleX);
            int height = (int)(size.Height * dpi.DpiScaleY);
            if (width == 0 || height == 0)
                return;
            SKSurface surface = GetSurface();
            BeforeRender();
            var canvas = surface.Canvas;
            canvas.Clear(new SKColor(255, 255, 255, 0));
            canvas.ResetMatrix();
            canvas.Scale(dpi.DpiScaleX, dpi.DpiScaleY);
            SkiaRenderContext renderContext = new SkiaRenderContext(canvas);
            RenderCore(visual, renderContext);
            if (Debugger.IsAttached && IsShowFPS)
            {
                var elapsedTime = _stopwatch.ElapsedMilliseconds;
                var fps = (int)MathF.Round(1000f / elapsedTime);
                fps = Math.Max(1, fps);
                canvas.DrawText(fps.ToString(), _FpsPoint, _FpsFont, _FpsPaint);
            }
            //canvas.Flush();
            surface.Flush();
            AfterRender();
            _stopwatch.Restart();
        }

        private void RenderCore(Visual visual, SkiaRenderContext renderContext)
        {
            var offset = visual.VisualOffset;
            var canvas = renderContext.Canvas;
            var saveCount = canvas.SaveCount;
            if (offset != Vector2.Zero)
            {
                canvas.Save();
                canvas.Translate(visual.VisualOffset.X, visual.VisualOffset.Y);
            }
            if (visual.HasRenderContent)
                visual.RenderContext(renderContext);
            int childrenCount = VisualTreeHelper.GetChildrenCount(visual);
            for (int i = 0; i < childrenCount; i++)
            {
                RenderCore(VisualTreeHelper.GetChild(visual, i), renderContext);
            }
            if (offset != Vector2.Zero)
                canvas.RestoreToCount(saveCount - 1);
        }

        protected virtual void BeforeRender()
        {

        }

        protected virtual void AfterRender()
        {

        }

        protected abstract SKSurface GetSurface();

        #region Dispose

        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                DisposeCore(disposing);

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
