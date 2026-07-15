using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Test
{
    public class DrawingVisual : Visual
    {
        private VisualDrawingContext? _drawingContext;
        private IDrawingContent? _drawingContent;

        public override bool HasRenderContent => _drawingContent != null;

        public VisualDrawingContext? DrawingContext { get => _drawingContext; private set { _drawingContext = value; } }

        public Size Size { get; set; }

        public override Size GetVisualSize()
        {
            return Size;
        }

        [MemberNotNull("DrawingContext")]
        public void Open()
        {
            if (DrawingContext == null)
                DrawingContext = FrameworkCoreProvider.GetRendererProvider().CreateDrawingContext(this);
        }

        public void Close()
        {
            if (_drawingContext != null)
            {
                _drawingContent = _drawingContext.Close();
                _drawingContext = null;
            }
        }

        public override void RenderContext(RenderContext renderContext)
        {
            if (_drawingContent != null)
                renderContext.Render(_drawingContent);
        }
    }
}
