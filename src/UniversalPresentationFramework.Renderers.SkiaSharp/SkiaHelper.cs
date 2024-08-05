using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Renderers
{
    public static class SkiaHelper
    {
        public static SKPaint? GetFillPaint(Brush brush, Pen? pen, in Rect rect)
        {
            var paint = new SKPaint();
            paint.Style = SKPaintStyle.Fill;
            float strokeThickness, halfStrokeThickness;
            if (pen == null || pen.Brush == null)
            {
                strokeThickness = 0f;
                halfStrokeThickness = 0f;
            }
            else
            {
                strokeThickness = pen.Thickness;
                halfStrokeThickness = strokeThickness / 2f;
            }
            if (brush is SolidColorBrush solidColorBrush)
            {
                paint.ColorF = GetColor(solidColorBrush);
            }
            else if (brush is LinearGradientBrush linearGradientBrush)
            {
                var stops = linearGradientBrush.GradientStops;
                if (stops != null && stops.Count != 0)
                {
                    SKColorF[] colors = new SKColorF[stops.Count];
                    float[] offsets = new float[stops.Count];
                    for (int i = 0; i < stops.Count; i++)
                    {
                        var stop = stops[i];
                        colors[i] = GetColor(stop.Color ?? Colors.Transparent);
                        offsets[i] = stop.Offset;
                    }
                    var startPoint = linearGradientBrush.StartPoint;
                    var endPoint = linearGradientBrush.EndPoint;
                    if (linearGradientBrush.MappingMode == BrushMappingMode.RelativeToBoundingBox)
                    {
                        startPoint.X = rect.Left + startPoint.X * rect.Width;
                        startPoint.Y = rect.Top + startPoint.Y * rect.Height;
                        endPoint.X = rect.Left + endPoint.X * rect.Width;
                        endPoint.Y = rect.Top + endPoint.Y * rect.Height;
                    }
                    if (strokeThickness != 0f)
                    {
                        startPoint.X -= halfStrokeThickness;
                        startPoint.Y -= halfStrokeThickness;
                        endPoint.X += halfStrokeThickness;
                        endPoint.Y += halfStrokeThickness;
                    }
                    paint.Shader = SKShader.CreateLinearGradient(GetPoint(startPoint), GetPoint(endPoint), colors, SKColorSpace.CreateSrgbLinear(), offsets, GetTitlMode(linearGradientBrush.SpreadMethod));
                }
                else
                    return null;
            }
            else if (brush is RadialGradientBrush radialGradientBrush)
            {
                var stops = radialGradientBrush.GradientStops;
                if (stops != null && stops.Count != 0)
                {
                    SKColorF[] colors = new SKColorF[stops.Count];
                    float[] offsets = new float[stops.Count];
                    for (int i = 0; i < stops.Count; i++)
                    {
                        var stop = stops[i];
                        colors[i] = GetColor(stop.Color ?? Colors.Transparent);
                        offsets[i] = stop.Offset;
                    }
                    var center = radialGradientBrush.Center;
                    var radiusX = radialGradientBrush.RadiusX;
                    var radiusY = radialGradientBrush.RadiusY;
                    if (radialGradientBrush.MappingMode == BrushMappingMode.RelativeToBoundingBox)
                    {
                        center.X = rect.X + rect.Width * center.X;
                        center.Y = rect.Y + rect.Height * center.Y;
                        radiusX *= rect.Width;
                        radiusY *= rect.Height;
                    }
                    if (strokeThickness != 0f)
                    {
                        radiusX += halfStrokeThickness;
                        radiusY += halfStrokeThickness;
                    }
                    if (radiusX == radiusY)
                        paint.Shader = SKShader.CreateRadialGradient(GetPoint(center), radiusX, colors, SKColorSpace.CreateSrgbLinear(), offsets, GetTitlMode(radialGradientBrush.SpreadMethod));
                    {
                        var offset = new Vector2(rect.X + center.X, rect.Y + center.Y);
                        var matrix = Matrix3x2.CreateTranslation(-offset) * Matrix3x2.CreateScale(1f, radiusY / radiusX) * Matrix3x2.CreateTranslation(offset);
                        paint.Shader = SKShader.CreateRadialGradient(GetPoint(center), radiusX, colors, SKColorSpace.CreateSrgbLinear(), offsets, GetTitlMode(radialGradientBrush.SpreadMethod), GetMatrix(matrix));
                    }
                }
                else
                    return null;
            }
            else if (brush is ImageBrush imageBrush)
            {
                var imageSource = imageBrush.ImageSource;
                if (imageSource != null)
                {
                    if (imageSource.Context is not ISkiaImageContext skiaImageContext)
                        throw new NotSupportedException("ImageSource context invalid.");
                    //skiaImageContext.Image
                    SKShaderTileMode tileX, tileY;
                    switch (imageBrush.TileMode)
                    {
                        case TileMode.None:
                            tileX = SKShaderTileMode.Decal;
                            tileY = SKShaderTileMode.Decal;
                            break;
                        case TileMode.Tile:
                            tileX = SKShaderTileMode.Repeat;
                            tileY = SKShaderTileMode.Repeat;
                            break;
                        case TileMode.FlipX:
                            tileX = SKShaderTileMode.Mirror;
                            tileY = SKShaderTileMode.Repeat;
                            break;
                        case TileMode.FlipY:
                            tileX = SKShaderTileMode.Repeat;
                            tileY = SKShaderTileMode.Mirror;
                            break;
                        case TileMode.FlipXY:
                            tileX = SKShaderTileMode.Mirror;
                            tileY = SKShaderTileMode.Mirror;
                            break;
                        default:
                            throw new NotSupportedException("Invalid tile mode.");
                    }
                    var image = skiaImageContext.Image;
                    var viewbox = imageBrush.Viewbox;
                    if (imageBrush.ViewboxUnits == BrushMappingMode.RelativeToBoundingBox)
                    {
                        if (viewbox.X != 0 || viewbox.Y != 0 || viewbox.Width != 1f || viewbox.Height != 1f)
                        {
                            if (viewbox.X < 0 || viewbox.X >= 1f)
                                throw new ArgumentException("X of Viewbox out of range.");
                            if (viewbox.Y < 0 || viewbox.Y >= 1f)
                                throw new ArgumentException("Y of Viewbox out of range.");
                            if (viewbox.Width < 0 || viewbox.X + viewbox.Width > 1f)
                                throw new ArgumentException("Width of Viewbox out of range.");
                            if (viewbox.Height < 0 || viewbox.Y + viewbox.Height > 1f)
                                throw new ArgumentException("Width of Viewbox out of range.");
                            image = image.Subset(new SKRectI((int)(viewbox.X * image.Width), (int)(viewbox.Y * image.Height), (int)(viewbox.Right * image.Width), (int)(viewbox.Bottom * image.Height)));
                        }
                    }
                    else
                    {
                        if (viewbox.X != 0 || viewbox.Y != 0 || viewbox.Width != image.Width || viewbox.Height != image.Height)
                        {
                            if (viewbox.X < 0 || viewbox.X >= image.Width)
                                throw new ArgumentException("X of Viewbox out of range.");
                            if (viewbox.Y < 0 || viewbox.Y >= image.Height)
                                throw new ArgumentException("Y of Viewbox out of range.");
                            if (viewbox.Width < 0 || viewbox.X + viewbox.Width > image.Width)
                                throw new ArgumentException("Width of Viewbox out of range.");
                            if (viewbox.Height < 0 || viewbox.Y + viewbox.Height > image.Height)
                                throw new ArgumentException("Width of Viewbox out of range.");
                            image = image.Subset(new SKRectI((int)viewbox.X, (int)viewbox.Y, (int)viewbox.Right, (int)viewbox.Bottom));
                        }
                    }
                    Matrix3x2 matrix = Matrix3x2.CreateTranslation(rect.X - halfStrokeThickness, rect.Y - halfStrokeThickness);
                    var viewport = imageBrush.Viewport;
                    float scaleX = (rect.Width + strokeThickness) / image.Width, scaleY = (rect.Height + strokeThickness) / image.Height;
                    if (imageBrush.ViewportUnits == BrushMappingMode.RelativeToBoundingBox)
                    {
                        if (viewport.X != 0 || viewport.Y != 0)
                            matrix *= Matrix3x2.CreateTranslation(viewport.X * rect.Width, viewport.Y * rect.Height);
                        matrix *= Matrix3x2.CreateScale(viewport.Width * scaleX, viewport.Height * scaleY);
                    }
                    else
                    {
                        if (viewport.X != 0 || viewport.Y != 0)
                            matrix *= Matrix3x2.CreateTranslation(viewport.X, viewport.Y);
                        matrix *= Matrix3x2.CreateScale(viewport.Width / rect.Width * scaleX, viewport.Height / rect.Height * scaleY);
                    }
                    paint.Shader = SKShader.CreateImage(image, tileX, tileY, GetMatrix(matrix));
                }
                else
                    return null;
            }
            else
                throw new NotSupportedException($"Skia does not support brush type \"{brush.GetType().FullName}\".");
            return paint;
        }

        public static SKPaint? GetStrokePaint(Pen pen, in Rect rect)
        {
            var brush = pen.Brush;
            if (brush == null)
                return null;
            var strokeThickness = pen.Thickness;
            if (strokeThickness == 0f)
                return null;
            var halfStrokeThickness = strokeThickness / 2f;
            var paint = new SKPaint();
            paint.StrokeCap = GetStrokeCap(pen.DashCap);
            paint.StrokeJoin = GetStrokeJoin(pen.LineJoin);
            paint.StrokeMiter = pen.MiterLimit;
            paint.StrokeWidth = strokeThickness;
            paint.Style = SKPaintStyle.Stroke;
            if (brush is SolidColorBrush solidColorBrush)
            {
                paint.ColorF = GetColor(solidColorBrush);
            }
            else if (brush is LinearGradientBrush linearGradientBrush)
            {
                var stops = linearGradientBrush.GradientStops;
                if (stops != null && stops.Count != 0)
                {
                    SKColorF[] colors = new SKColorF[stops.Count];
                    float[] offsets = new float[stops.Count];
                    for (int i = 0; i < stops.Count; i++)
                    {
                        var stop = stops[i];
                        colors[i] = GetColor(stop.Color ?? Colors.Transparent);
                        offsets[i] = stop.Offset;
                    }
                    var startPoint = linearGradientBrush.StartPoint;
                    var endPoint = linearGradientBrush.EndPoint;
                    if (linearGradientBrush.MappingMode == BrushMappingMode.RelativeToBoundingBox)
                    {
                        startPoint.X = rect.Left + startPoint.X * rect.Width;
                        startPoint.Y = rect.Top + startPoint.Y * rect.Height;
                        endPoint.X = rect.Left + endPoint.X * rect.Width;
                        endPoint.Y = rect.Top + endPoint.Y * rect.Height;
                    }
                    if (strokeThickness != 0f)
                    {
                        startPoint.X -= halfStrokeThickness;
                        startPoint.Y -= halfStrokeThickness;
                        endPoint.X += halfStrokeThickness;
                        endPoint.Y += halfStrokeThickness;
                    }
                    paint.Shader = SKShader.CreateLinearGradient(GetPoint(startPoint), GetPoint(endPoint), colors, SKColorSpace.CreateSrgbLinear(), offsets, GetTitlMode(linearGradientBrush.SpreadMethod));
                }
            }
            else if (brush is RadialGradientBrush radialGradientBrush)
            {
                var stops = radialGradientBrush.GradientStops;
                if (stops != null && stops.Count != 0)
                {
                    SKColorF[] colors = new SKColorF[stops.Count];
                    float[] offsets = new float[stops.Count];
                    for (int i = 0; i < stops.Count; i++)
                    {
                        var stop = stops[i];
                        colors[i] = GetColor(stop.Color ?? Colors.Transparent);
                        offsets[i] = stop.Offset;
                    }
                    var center = radialGradientBrush.Center;
                    var radiusX = radialGradientBrush.RadiusX;
                    var radiusY = radialGradientBrush.RadiusY;
                    if (radialGradientBrush.MappingMode == BrushMappingMode.RelativeToBoundingBox)
                    {
                        center.X = rect.X + rect.Width * center.X;
                        center.Y = rect.Y + rect.Height * center.Y;
                        radiusX *= rect.Width;
                        radiusY *= rect.Height;
                    }
                    if (strokeThickness != 0f)
                    {
                        radiusX += halfStrokeThickness;
                        radiusY += halfStrokeThickness;
                    }
                    if (radiusX == radiusY)
                        paint.Shader = SKShader.CreateRadialGradient(GetPoint(center), radiusX, colors, SKColorSpace.CreateSrgbLinear(), offsets, GetTitlMode(radialGradientBrush.SpreadMethod));
                    else
                    {
                        var offset = new Vector2(rect.X + center.X, rect.Y + center.Y);
                        var matrix = Matrix3x2.CreateTranslation(-offset) * Matrix3x2.CreateScale(1f, radiusY / radiusX) * Matrix3x2.CreateTranslation(offset);
                        paint.Shader = SKShader.CreateRadialGradient(GetPoint(center), radiusX, colors, SKColorSpace.CreateSrgbLinear(), offsets, GetTitlMode(radialGradientBrush.SpreadMethod), GetMatrix(matrix));
                    }
                }
            }
            else if (brush is ImageBrush imageBrush)
            {
                var imageSource = imageBrush.ImageSource;
                if (imageSource != null)
                {
                    if (imageSource.Context is not ISkiaImageContext skiaImageContext)
                        throw new NotSupportedException("ImageSource context invalid.");
                    //skiaImageContext.Image
                    SKShaderTileMode tileX, tileY;
                    switch (imageBrush.TileMode)
                    {
                        case TileMode.None:
                            tileX = SKShaderTileMode.Decal;
                            tileY = SKShaderTileMode.Decal;
                            break;
                        case TileMode.Tile:
                            tileX = SKShaderTileMode.Repeat;
                            tileY = SKShaderTileMode.Repeat;
                            break;
                        case TileMode.FlipX:
                            tileX = SKShaderTileMode.Mirror;
                            tileY = SKShaderTileMode.Repeat;
                            break;
                        case TileMode.FlipY:
                            tileX = SKShaderTileMode.Repeat;
                            tileY = SKShaderTileMode.Mirror;
                            break;
                        case TileMode.FlipXY:
                            tileX = SKShaderTileMode.Mirror;
                            tileY = SKShaderTileMode.Mirror;
                            break;
                        default:
                            throw new NotSupportedException("Invalid tile mode.");
                    }
                    var image = skiaImageContext.Image;
                    var viewbox = imageBrush.Viewbox;
                    if (imageBrush.ViewboxUnits == BrushMappingMode.RelativeToBoundingBox)
                    {
                        if (viewbox.X != 0 || viewbox.Y != 0 || viewbox.Width != 1f || viewbox.Height != 1f)
                        {
                            if (viewbox.X < 0 || viewbox.X >= 1f)
                                throw new ArgumentException("X of Viewbox out of range.");
                            if (viewbox.Y < 0 || viewbox.Y >= 1f)
                                throw new ArgumentException("Y of Viewbox out of range.");
                            if (viewbox.Width < 0 || viewbox.X + viewbox.Width > 1f)
                                throw new ArgumentException("Width of Viewbox out of range.");
                            if (viewbox.Height < 0 || viewbox.Y + viewbox.Height > 1f)
                                throw new ArgumentException("Width of Viewbox out of range.");
                            image = image.Subset(new SKRectI((int)(viewbox.X * image.Width), (int)(viewbox.Y * image.Height), (int)(viewbox.Right * image.Width), (int)(viewbox.Bottom * image.Height)));
                        }
                    }
                    else
                    {
                        if (viewbox.X != 0 || viewbox.Y != 0 || viewbox.Width != image.Width || viewbox.Height != image.Height)
                        {
                            if (viewbox.X < 0 || viewbox.X >= image.Width)
                                throw new ArgumentException("X of Viewbox out of range.");
                            if (viewbox.Y < 0 || viewbox.Y >= image.Height)
                                throw new ArgumentException("Y of Viewbox out of range.");
                            if (viewbox.Width < 0 || viewbox.X + viewbox.Width > image.Width)
                                throw new ArgumentException("Width of Viewbox out of range.");
                            if (viewbox.Height < 0 || viewbox.Y + viewbox.Height > image.Height)
                                throw new ArgumentException("Width of Viewbox out of range.");
                            image = image.Subset(new SKRectI((int)viewbox.X, (int)viewbox.Y, (int)viewbox.Right, (int)viewbox.Bottom));
                        }
                    }
                    Matrix3x2 matrix = Matrix3x2.CreateTranslation(rect.X - halfStrokeThickness, rect.Y - halfStrokeThickness);
                    float scaleX = (rect.Width + strokeThickness) / image.Width, scaleY = (rect.Height + strokeThickness) / image.Height;
                    var viewport = imageBrush.Viewport;
                    if (imageBrush.ViewportUnits == BrushMappingMode.RelativeToBoundingBox)
                    {
                        if (viewport.X != 0 || viewport.Y != 0)
                            matrix *= Matrix3x2.CreateTranslation(viewport.X * rect.Width, viewport.Y * rect.Height);
                        matrix *= Matrix3x2.CreateScale(viewport.Width * scaleX, viewport.Height * scaleY);
                    }
                    else
                    {
                        if (viewport.X != 0 || viewport.Y != 0)
                            matrix *= Matrix3x2.CreateTranslation(viewport.X, viewport.Y);
                        matrix *= Matrix3x2.CreateScale(viewport.Width / rect.Width * scaleX, viewport.Height / rect.Height * scaleY);
                    }
                    paint.Shader = SKShader.CreateImage(image, tileX, tileY, GetMatrix(matrix));
                }
            }
            else
                throw new NotSupportedException($"Skia does not support brush type \"{brush.GetType().FullName}\".");
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
                        return SKColorType.Rgba8888;
                    else if (pixelFormat.ChannelBits.SequenceEqual(new byte[] { 10, 10, 10, 2 }))
                        return SKColorType.Rgba1010102;
                    else if (pixelFormat.ChannelBits.SequenceEqual(new byte[] { 16, 16, 16, 16 }))
                        if (pixelFormat.IsFloat)
                            return SKColorType.RgbaF16;
                        else
                            return SKColorType.Rgba16161616;
                    else
                        throw new NotSupportedException($"Skia renderer provider do not support BGRA with \"{string.Join('-', pixelFormat.ChannelBits)}\" channels.");
                case PixelFormatChannelOrder.ChannelOrderBGR:
                    if (pixelFormat.ChannelBits.SequenceEqual(new byte[] { 5, 6, 5 }))
                        return SKColorType.Rgb565;
                    else if (pixelFormat.ChannelBits.SequenceEqual(new byte[] { 8, 8, 8 }))
                        return SKColorType.Rgb888x;
                    else if (pixelFormat.ChannelBits.SequenceEqual(new byte[] { 10, 10, 10 }))
                        return SKColorType.Rgb101010x;
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
                    if (pixelFormat.ChannelBits.SequenceEqual(new byte[] { 10, 10, 10 }))
                        return SKColorType.Bgr101010x;
                    else
                        throw new NotSupportedException($"Skia renderer provider do not support RGB with \"{string.Join('-', pixelFormat.ChannelBits)}\" channels.");
                case PixelFormatChannelOrder.ChannelOrderRGBA:
                    if (pixelFormat.ChannelBits.SequenceEqual(new byte[] { 8, 8, 8, 8 }))
                        return SKColorType.Bgra8888;
                    else if (pixelFormat.ChannelBits.SequenceEqual(new byte[] { 10, 10, 10, 2 }))
                        return SKColorType.Bgra1010102;
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

        public static SKFontStyleSlant GetFontStyle(FontStyle style)
        {
            if (style == FontStyles.Italic)
                return SKFontStyleSlant.Italic;
            else if (style == FontStyles.Oblique)
                return SKFontStyleSlant.Oblique;
            else
                return SKFontStyleSlant.Upright;
        }

        public static SKPoint GetPoint(in Point point)
        {
            return new SKPoint(point.X, point.Y);
        }

        public static SKShaderTileMode GetTitlMode(in GradientSpreadMethod method)
        {
            switch (method)
            {
                case GradientSpreadMethod.Pad:
                    return SKShaderTileMode.Clamp;
                case GradientSpreadMethod.Repeat:
                    return SKShaderTileMode.Repeat;
                case GradientSpreadMethod.Reflect:
                    return SKShaderTileMode.Mirror;
                default:
                    throw new NotSupportedException("Unknown GradientSpreadMethod.");
            }
        }

        public static SKMatrix GetMatrix(in Matrix3x2 matrix)
        {
            return new SKMatrix(matrix.M11, matrix.M12, matrix.M31, matrix.M21, matrix.M22, matrix.M32, 0f, 0f, 1f);
        }
    }
}
