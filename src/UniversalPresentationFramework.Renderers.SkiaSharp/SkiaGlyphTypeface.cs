using SkiaSharp;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;
using static System.Net.Mime.MediaTypeNames;

namespace Wodsoft.UI.Renderers
{
    public class SkiaGlyphTypeface : GlyphTypeface
    {
        private readonly SKTypeface _typeface;
        private readonly SKFont _font;
        private SKFontMetrics _metrics;

        private SkiaGlyphTypeface(SKTypeface typeface, FontStyle style, FontWeight weight, FontStretch stretch)
        {
            _typeface = typeface;
            _font = _typeface.ToFont(1f);
            Style = style;
            Stretch = stretch;
            Weight = weight;
            _font.GetFontMetrics(out _metrics);
        }

        public static SkiaGlyphTypeface? Create(string familyName, FontStyle style, FontWeight weight, FontStretch stretch)
        {
            var typeface = SKTypeface.FromFamilyName(familyName, weight.ToOpenTypeWeight(), stretch.ToOpenTypeStretch(), SkiaHelper.GetFontStyle(style));
            if (typeface == null)
                return null;
            return new SkiaGlyphTypeface(typeface, style, weight, stretch);
        }

        public override int BreakText(ReadOnlySpan<char> text, float emSize, float maxWidth, out float measuredWidth)
        {
            var position = _font.BreakText(text, maxWidth / emSize, out measuredWidth);
            measuredWidth *= emSize;
            return position;
        }

        public override float MeasureText(ReadOnlySpan<char> text, float emSize)
        {
            return _font.MeasureText(text) * emSize;
        }

        public override ReadOnlyMemory<float> GetTextWidths(ReadOnlySpan<char> text, float emSize)
        {
            var widths = _font.GetGlyphWidths(text);
            if (emSize == 1f)
                return widths;
            if (Vector.IsHardwareAccelerated)
            {
                var span = new Span<float>(widths);
                var vectorSize = Vector<float>.Count;
                var emVector = new Vector<float>(emSize);
                int i = 0;
                for (; i < widths.Length - vectorSize; i += vectorSize)
                {
                    ref var vector = ref Unsafe.As<float, Vector<float>>(ref span[i]);
                    vector *= emVector;
                }
                for (; i < widths.Length; i++)
                    span[i] *= emSize;
            }
            else
            {
                for (int i = 0; i < widths.Length; i++)
                    widths[i] *= emSize;
            }
            return widths;
        }

        public override float Baseline => -_metrics.Ascent;

        public override float Height => -_metrics.Ascent + _metrics.Descent;

        public override FontStyle Style { get; }

        public override FontStretch Stretch { get; }

        public override FontWeight Weight { get; }

        public override float CapsHeight => _metrics.CapHeight;

        public override float XHeight => _metrics.XHeight;

        public override float UnderlinePosition => -_metrics.UnderlinePosition ?? 0f;

        public override float UnderlineThickness => _metrics.UnderlineThickness ?? 0f;

        public override float StrikethroughPosition => -_metrics.StrikeoutPosition ?? 0f;

        public override float StrikethroughThickness => _metrics.StrikeoutThickness ?? 0f;

        public override float Descent => _metrics.Descent;

        public override float Ascent => -_metrics.Ascent;

        internal SKTypeface SKTypeface => _typeface;

        internal SKFont SKFont => _font;
    }
}
