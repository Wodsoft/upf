using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
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

        //https://learn.microsoft.com/zh-cn/windows/win32/wic/-wic-codec-native-pixel-formats?redirectedfrom=MSDN
        public static SKColorType GetColorType(PixelFormat pixelFormat)
        {
            if (pixelFormat == PixelFormats.Default)
                return SKImageInfo.PlatformColorType;
            switch (pixelFormat.ChannelOrder)
            {
                case PixelFormatChannelOrder.ChannelOrderBGRA:
                    if (pixelFormat.ChannelBits.SequenceEqual(new byte[] { 8, 8, 8, 8 }))
                        return SKColorType.Bgra8888;
                    else if (pixelFormat.ChannelBits.SequenceEqual(new byte[] { 10, 10, 10, 2 }))
                        return SKColorType.Bgra1010102;
                    else
                        throw new NotSupportedException($"Skia renderer provider do not support BGRA with \"{string.Join('-', pixelFormat.ChannelBits)}\" channels.");
                case PixelFormatChannelOrder.ChannelOrderBGR:
                    if (pixelFormat.ChannelBits.SequenceEqual(new byte[] { 10, 10, 10 }))
                        return SKColorType.Bgr101010x;
                    else
                        throw new NotSupportedException($"Skia renderer provider do not support BGR with \"{string.Join('-', pixelFormat.ChannelBits)}\" channels.");
                case PixelFormatChannelOrder.ChannelOrderARGB:
                    if (pixelFormat.ChannelBits.SequenceEqual(new byte[] { 4, 4, 4, 4 }))
                        return SKColorType.Argb4444;
                    else
                        throw new NotSupportedException($"Skia renderer provider do not support ARGB with \"{string.Join('-', pixelFormat.ChannelBits)}\" channels.");
                case PixelFormatChannelOrder.ChannelOrderABGR:
                    throw new NotSupportedException($"Skia renderer provider do not support ABGR.");
                case PixelFormatChannelOrder.ChannelOrderRGB:
                    if (pixelFormat.ChannelBits.SequenceEqual(new byte[] { 5, 6, 5 }))
                        return SKColorType.Rgb565;
                    else if (pixelFormat.ChannelBits.SequenceEqual(new byte[] { 8, 8, 8 }))
                        return SKColorType.Rgb888x;
                    else if (pixelFormat.ChannelBits.SequenceEqual(new byte[] { 10, 10, 10 }))
                        return SKColorType.Rgb101010x;
                    else
                        throw new NotSupportedException($"Skia renderer provider do not support RGB with \"{string.Join('-', pixelFormat.ChannelBits)}\" channels.");
                case PixelFormatChannelOrder.ChannelOrderRGBA:
                    if (pixelFormat.ChannelBits.SequenceEqual(new byte[] { 8, 8, 8, 8 }))
                        return SKColorType.Rgba8888;
                    else if (pixelFormat.ChannelBits.SequenceEqual(new byte[] { 10, 10, 10, 2 }))
                        return SKColorType.Rgba1010102;
                    else if (pixelFormat.ChannelBits.SequenceEqual(new byte[] { 16, 16, 16, 16 }))
                        if (pixelFormat.IsFloat)
                            return SKColorType.RgbaF16;
                        else
                            return SKColorType.Rgba16161616;
                    else if (pixelFormat.ChannelBits.SequenceEqual(new byte[] { 32, 32, 32, 32 }))
                        return SKColorType.RgbaF32;
                    else
                        throw new NotSupportedException($"Skia renderer provider do not support RGB with \"{string.Join('-', pixelFormat.ChannelBits)}\" channels.");
            }
            if (pixelFormat.ColorSpace == PixelFormatColorSpace.IsGray)
            {
                switch (pixelFormat.BitsPerPixel)
                {
                    case 8:
                        return SKColorType.Gray8;
                    default:
                        throw new NotSupportedException($"Skia renderer provider do not support Gray with \"{pixelFormat.BitsPerPixel}\" bits per pixel.");
                }
            }
            throw new NotSupportedException($"Skia renderer provider do not support pixel format. Color type \"{pixelFormat.ColorSpace}\". \"{string.Join('-', pixelFormat.ChannelBits)}\" Channels.");
        }

        public static PixelFormat GetPixelFormat(SKColorType colorType, SKAlphaType alphaType, SKColorSpace? colorSpace)
        {
            byte[] channels;
            PixelFormatColorSpace space;
            if (colorSpace == null)
                space = PixelFormatColorSpace.Unkonwn;
            else
                space = colorSpace.GammaIsLinear ? PixelFormatColorSpace.IsScRGB : PixelFormatColorSpace.IsSRGB;
            PixelFormatChannelOrder channelOrder;
            bool isFloat = false;
            bool isPremutilplied = alphaType == SKAlphaType.Premul;
            switch (colorType)
            {
                case SKColorType.Argb4444:
                    channelOrder = PixelFormatChannelOrder.ChannelOrderARGB;
                    channels = [4, 4, 4, 4];
                    break;
                case SKColorType.Bgr101010x:
                    channelOrder = PixelFormatChannelOrder.ChannelOrderBGR;
                    channels = [10, 10, 10];
                    break;
                case SKColorType.Bgra8888:
                    channelOrder = PixelFormatChannelOrder.ChannelOrderBGRA;
                    channels = [8, 8, 8, 8];
                    break;
                case SKColorType.Bgra1010102:
                    channelOrder = PixelFormatChannelOrder.ChannelOrderBGRA;
                    channels = [10, 10, 10, 2];
                    break;
                case SKColorType.Rgb565:
                    channelOrder = PixelFormatChannelOrder.ChannelOrderRGB;
                    channels = [5, 6, 5];
                    break;
                case SKColorType.Rgb888x:
                    channelOrder = PixelFormatChannelOrder.ChannelOrderRGB;
                    channels = [8, 8, 8, 8];
                    break;
                case SKColorType.Rgb101010x:
                    channelOrder = PixelFormatChannelOrder.ChannelOrderRGB;
                    channels = [10, 10, 10];
                    break;
                case SKColorType.Rgba8888:
                    channelOrder = PixelFormatChannelOrder.ChannelOrderRGBA;
                    channels = [8, 8, 8, 8];
                    break;
                case SKColorType.Rgba1010102:
                    channelOrder = PixelFormatChannelOrder.ChannelOrderRGBA;
                    channels = [10, 10, 10, 2];
                    break;
                case SKColorType.Rgba16161616:
                    channelOrder = PixelFormatChannelOrder.ChannelOrderRGBA;
                    channels = [16, 16, 16, 16];
                    break;
                case SKColorType.RgbaF16:
                    channelOrder = PixelFormatChannelOrder.ChannelOrderRGBA;
                    channels = [16, 16, 16, 16];
                    isFloat = true;
                    break;
                case SKColorType.RgbaF16Clamped:
                    channelOrder = PixelFormatChannelOrder.ChannelOrderRGBA;
                    channels = [16, 16, 16, 16];
                    isFloat = true;
                    break;
                case SKColorType.RgbaF32:
                    channelOrder = PixelFormatChannelOrder.ChannelOrderRGBA;
                    channels = [32, 32, 32, 32];
                    isFloat = true;
                    break;
                case SKColorType.Gray8:
                    channelOrder = PixelFormatChannelOrder.Unkonwn;
                    channels = [8];
                    space = PixelFormatColorSpace.IsGray;
                    break;
                case SKColorType.Alpha8:
                case SKColorType.Alpha16:
                case SKColorType.AlphaF16:
                    channelOrder = PixelFormatChannelOrder.Unkonwn;
                    space = PixelFormatColorSpace.Unkonwn;
                    channels = [16];
                    break;
                case SKColorType.Rg88:
                    channelOrder = PixelFormatChannelOrder.Unkonwn;
                    channels = [8, 8];
                    break;
                case SKColorType.Rg1616:
                    channelOrder = PixelFormatChannelOrder.Unkonwn;
                    channels = [16, 16];
                    break;
                case SKColorType.RgF16:
                    channelOrder = PixelFormatChannelOrder.Unkonwn;
                    channels = [16, 16];
                    isFloat = true;
                    break;
                default:
                    return new PixelFormat();
            }
            var bitsPerPixel = (byte)channels.Sum(t => t);
            bitsPerPixel += (byte)(8 - (bitsPerPixel % 8));
            return new PixelFormat((byte)channels.Sum(t => t), space, isPremutilplied, channelOrder, channels, isFloat);
        }
    }
}
