using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Renderers
{
    internal static class SkiaHelper
    {
        public static SKPaint GetPaint(Brush? brush, Pen? pen)
        {
            var paint = new SKPaint();
            if (brush != null)
            {
                if (brush is SolidColorBrush solidColorBrush)
                {
                    paint.ColorF = GetColor(solidColorBrush);
                }
                else
                    throw new NotSupportedException($"Skia does not support brush type \"{brush.GetType().FullName}\".");
            }
            if (pen != null)
            {
                paint.StrokeCap = GetStrokeCap(pen.DashCap);
                paint.StrokeJoin = GetStrokeJoin(pen.LineJoin);
                paint.StrokeMiter = pen.MiterLimit;
                paint.StrokeWidth = pen.Thickness;
            }
            if (brush == null && pen != null)
                paint.Style = SKPaintStyle.Stroke;
            else if (brush != null && pen == null)
                paint.Style = SKPaintStyle.Fill;
            else
                paint.Style = SKPaintStyle.StrokeAndFill;
            return paint;
        }

        public static SKColorF GetColor(SolidColorBrush brush)
        {
            var color = brush.Color;
            return new SKColorF(color.ScR, color.ScG, color.ScB, color.ScA * brush.Opacity);
        }

        public static SKColorF GetColor(Color color)
        {
            return new SKColorF(color.ScR, color.ScG, color.ScB, color.ScA);
        }

        public static SKStrokeCap GetStrokeCap(PenLineCap penLineCap)
        {
            switch (penLineCap)
            {
                case PenLineCap.Square:
                    return SKStrokeCap.Square;
                case PenLineCap.Round:
                    return SKStrokeCap.Round;
                default:
                    return SKStrokeCap.Butt;
            }
        }

        public static SKStrokeJoin GetStrokeJoin(PenLineJoin penLineJoin)
        {
            switch (penLineJoin)
            {
                case PenLineJoin.Round:
                    return SKStrokeJoin.Round;
                case PenLineJoin.Miter:
                    return SKStrokeJoin.Miter;
                case PenLineJoin.Bevel:                    
                    return SKStrokeJoin.Bevel;
                default:
                    return SKStrokeJoin.Miter;
            }
        }
    }
}
