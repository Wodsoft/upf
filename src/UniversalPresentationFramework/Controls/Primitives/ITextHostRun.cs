using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Controls.Primitives
{
    public interface ITextHostRun
    {
        bool IsMeasured { get; }

        void Measure();

        float Width { get; }

        float Height { get; }

        float Baseline { get; }

        int Position { get; }

        int Length { get; }

        ReadOnlySpan<float> Widths { get; }

        void Wrap(TextTrimming trimming, float width, bool overflow, out ITextHostRun? left, out ITextHostRun? right);

        void Draw(DrawingContext drawingContext, in Point origin);

        int GetCharPosition(in float x);
    }
}
