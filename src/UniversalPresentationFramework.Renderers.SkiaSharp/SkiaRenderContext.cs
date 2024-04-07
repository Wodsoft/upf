using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Renderers
{
    public class SkiaRenderContext : RenderContext
    {
        private readonly SKCanvas _canvas;

        public SkiaRenderContext(SKCanvas canvas)
        {
            _canvas = canvas;
        }

        public SKCanvas Canvas => _canvas;

        public override void Render(IDrawingContent drawingContent)
        {
            if (drawingContent is SkiaDrawingContent skiaDrawingContent)
                _canvas.DrawDrawable(skiaDrawingContent.Drawable, skiaDrawingContent.OriginalPoint);
            else
                throw new NotSupportedException("Only support skia drawing content.");
        }
    }
}
