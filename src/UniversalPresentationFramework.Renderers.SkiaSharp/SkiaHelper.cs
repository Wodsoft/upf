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
        private readonly static SKRuntimeEffect _ImageTileShader;

        static SkiaHelper()
        {
            const string imageTileShader = @"
uniform float2 tileSize;
uniform int alignmentX;//0:Left, 1: Center, 2: Right
uniform int alignmentY;//0:Top, 1: Center, 2: Bottom
uniform int tileModeX;//1: Repeat, 2:Mirror, 3: None
uniform int tileModeY;//1: Repeat, 2:Mirror, 3: None
uniform int stretch;//0:None, 2: Uniform, 3:UniformToFill
uniform int r;//Tile Aspect > Image Aspect
uniform shader image;
uniform float2 imageSize;
uniform float4 imageSrc;//Image left, top, right, bottom relative position

half4 main(float2 coord) {
  float cellX = mod(coord.x, tileSize.x);
  float cellY = mod(coord.y, tileSize.y);
  if ((tileModeX == 3 && coord.x > tileSize.x) || (tileModeY == 3 && coord.y > tileSize.y))
    return vec4(.0,.0,.0,.0);
  if (tileModeX == 2 && mod(floor(coord.x / tileSize.x), 2) == 1)
    cellX = tileSize.x - cellX;
  if (tileModeY == 2 && mod(floor(coord.y / tileSize.y), 2) == 1)
    cellY = tileSize.y - cellY;
  if (stretch == 0)
  {
    float imageWidth = (imageSrc.z - imageSrc.x) * imageSize.x;
    float imageHeight = (imageSrc.w - imageSrc.y) * imageSize.y;
    cellX = cellX / imageWidth;
    cellY = cellY / imageHeight;
    if (alignmentX == 0 && cellX > 1)
      return vec4(.0,.0,.0,.0);
    else if (alignmentX == 1)
    {
      cellX += (1 - tileSize.x / imageWidth)  / 2;
      if (cellX < 0 || cellX > 1)
        return vec4(.0,.0,.0,.0);
    }
    else if (alignmentX == 2)
    {
      cellX += 1 - tileSize.x / imageWidth;
      if (cellX < 0)
        return vec4(.0,.0,.0,.0);
    } 
    if (alignmentY == 0 && cellY > 1)
      return vec4(.0,.0,.0,.0);
    else if (alignmentY == 1)
    {
      cellY += (1 - tileSize.y / imageHeight) / 2;
      if (cellY < 0 || cellY > 1)
        return vec4(.0,.0,.0,.0); 
    }
    else if (alignmentY == 2)
    {
      cellY += 1 - tileSize.y / imageHeight;
      if (cellY < 0)
        return vec4(.0,.0,.0,.0);
    }
  }
  else if (stretch == 1)
  {
      cellX = cellX / tileSize.x;
      cellY = cellY / tileSize.y;
  }
  else if (stretch == 2)
  {
    if (r == 1)
    {
      cellX = cellX / tileSize.y;
      cellY = cellY / tileSize.y;
      if (alignmentX == 0 && cellX > 1)
        return vec4(.0,.0,.0,.0);
      else if (alignmentX == 1)
      {
        cellX -= (tileSize.x / tileSize.y - 1) / 2;
        if (cellX < 0 || cellX > 1)
          return vec4(.0,.0,.0,.0); 
      }
      else if (alignmentX == 2)
      {
        cellX -= tileSize.x / tileSize.y - 1;
        if (cellX < 0)
          return vec4(.0,.0,.0,.0);
      }
    }
    else
    {
      cellX = cellX / tileSize.x;
      cellY = cellY / tileSize.x;
      if (alignmentY == 0 && cellY > 1)
        return vec4(.0,.0,.0,.0);
      else if (alignmentY == 1)
      {
        cellY -= (tileSize.y / tileSize.x - 1) / 2;
        if (cellY < 0 || cellY > 1)
          return vec4(.0,.0,.0,.0); 
      }
      else if (alignmentY == 2)
      {
        cellY -= tileSize.y / tileSize.x - 1;
        if (cellY < 0)
          return vec4(.0,.0,.0,.0);        
      }
    }
  }
  else if (stretch == 3)
  {
    if (r == 1)
    {
      cellX = cellX / tileSize.x;
      cellY = cellY / tileSize.x;
      if (alignmentY == 1)
        cellY += (1 - tileSize.y / tileSize.x) / 2;
      else if (alignmentY == 2)
        cellY += (1 - tileSize.y / tileSize.x);
    }
    else
    {
      cellX = cellX / tileSize.y;
      cellY = cellY / tileSize.y;
      if (alignmentX == 1)
        cellX += (1 - tileSize.x / tileSize.y) / 2;
      else if (alignmentX == 2)
        cellX += (1 - tileSize.x / tileSize.y);
    }
  }
  else
    return vec4(.0,.0,.0,.0);
  cellX = (imageSrc.x + cellX * (imageSrc.z - imageSrc.x)) * imageSize.x;
  cellY = (imageSrc.y + cellY * (imageSrc.w - imageSrc.y)) * imageSize.y;
  return image.eval(vec2(cellX,cellY));
}";
            _ImageTileShader = SKRuntimeEffect.CreateShader(imageTileShader, out var errors);
            if (errors != null)
                throw new NotSupportedException("Tile shader create failed. " + errors);
        }

        public static SKPaint? GetFillPaint(Brush brush, in Rect rect)
        {
            SKPaint? paint = GetPaint(brush, rect);
            if (paint == null)
                return null;
            paint.Style = SKPaintStyle.Fill;
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
            var newRect = new Rect(rect.Left - halfStrokeThickness, rect.Top - halfStrokeThickness, rect.Width + strokeThickness, rect.Height + strokeThickness);
            var paint = GetPaint(brush, newRect);
            if (paint == null)
                return null;
            paint.StrokeCap = GetStrokeCap(pen.DashCap);
            paint.StrokeJoin = GetStrokeJoin(pen.LineJoin);
            paint.StrokeMiter = pen.MiterLimit;
            paint.StrokeWidth = strokeThickness;
            paint.Style = SKPaintStyle.Stroke;
            return paint;
        }

        private static SKPaint? GetPaint(Brush brush, in Rect rect)
        {
            var paint = new SKPaint();
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
                    Matrix3x2 matrix = Matrix3x2.Identity;
                    Transform(ref matrix, rect, brush);
                    paint.Shader = SKShader.CreateLinearGradient(GetPoint(startPoint), GetPoint(endPoint), colors, SKColorSpace.CreateSrgbLinear(), offsets, GetTitlMode(linearGradientBrush.SpreadMethod), GetMatrix(matrix));
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
                    Matrix3x2 matrix;
                    if (radiusX == radiusY)
                        matrix = Matrix3x2.Identity;
                    else
                    {
                        var offset = new Vector2(rect.X + center.X, rect.Y + center.Y);
                        matrix = Matrix3x2.CreateScale(1f, radiusY / radiusX, offset);
                    }
                    Transform(ref matrix, rect, brush);
                    paint.Shader = SKShader.CreateRadialGradient(GetPoint(center), radiusX, colors, SKColorSpace.CreateSrgbLinear(), offsets, GetTitlMode(radialGradientBrush.SpreadMethod), GetMatrix(matrix));
                }
                else
                    return null;
            }
            else if (brush is TileBrush tileBrush)
            {
                //calc tile mode
                SKShaderTileMode tileX, tileY;
                switch (tileBrush.TileMode)
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
                //cale tile size
                float tileWidth, tileHeight, tileOffsetX, tileOffsetY;
                var viewport = tileBrush.Viewport;
                if (tileBrush.ViewportUnits == BrushMappingMode.RelativeToBoundingBox)
                {
                    tileWidth = viewport.Width * rect.Width;
                    tileHeight = viewport.Height * rect.Height;
                    tileOffsetX = viewport.X * rect.Width;
                    tileOffsetY = viewport.Y * rect.Height;
                }
                else
                {
                    tileWidth = viewport.Width;
                    tileHeight = viewport.Height;
                    tileOffsetX = viewport.X;
                    tileOffsetY = viewport.Y;
                }
                Matrix3x2 matrix = Matrix3x2.CreateTranslation(tileOffsetX + rect.X, tileOffsetY + rect.Y);
                Transform(ref matrix, in rect, brush);
                if (tileBrush is ImageBrush imageBrush)
                {
                    var imageSource = imageBrush.ImageSource;
                    if (imageSource != null)
                    {
                        if (imageSource.Context is not ISkiaImageContext skiaImageContext)
                            throw new NotSupportedException("ImageSource context invalid.");
                        var image = skiaImageContext.Image;
                        var viewbox = imageBrush.Viewbox;
                        float[] imageSrc;
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
                                imageSrc = [viewbox.X, viewbox.Y, viewbox.X + viewbox.Width, viewbox.Y + viewbox.Height];
                            }
                            else
                                imageSrc = [0, 0, 1, 1];
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
                                imageSrc = [viewbox.X / image.Width, viewbox.Y / image.Height, (viewbox.X + viewbox.Width) / image.Width, (viewbox.Y + viewbox.Height) / image.Height];
                            }
                            else
                                imageSrc = [0, 0, 1, 1];
                        }
                        var uniforms = new SKRuntimeEffectUniforms(_ImageTileShader)
                        {
                            { "tileSize", new SKSize(tileWidth, tileHeight) },
                            { "alignmentX", (int)tileBrush.AlignmentX },
                            { "alignmentY", (int)tileBrush.AlignmentY },
                            { "tileModeX", (int)tileX },
                            { "tileModeY", (int)tileY },
                            { "stretch", (int) tileBrush.Stretch },
                            { "r", tileWidth / tileHeight > image.Width / (float)image.Height ? 1 : 0 },
                            { "imageSize", new SKSize(image.Width, image.Height) },
                            { "imageSrc", imageSrc }
                        };
                        var children = new SKRuntimeEffectChildren(_ImageTileShader)
                        {
                            { "image", image.ToRawShader() }
                        };
                        paint.Shader = _ImageTileShader.ToShader(uniforms, children, GetMatrix(matrix));
                    }
                    else
                        return null;
                }
            }
            else
                throw new NotSupportedException($"Skia does not support brush type \"{brush.GetType().FullName}\".");
            return paint;
        }

        private static void Transform(ref Matrix3x2 source, in Rect rect, Brush brush)
        {
            Matrix3x2 transformMatrix;
            var absoluteTransform = brush.Transform;
            if (absoluteTransform != null)
            {
                transformMatrix = absoluteTransform.Value;
                if (!transformMatrix.IsIdentity)
                    source *= transformMatrix;
            }
            var relativeTransform = brush.RelativeTransform;
            if (relativeTransform != null)
            {
                transformMatrix = relativeTransform.Value;
                if (!transformMatrix.IsIdentity)
                {
                    transformMatrix.M31 *= rect.Width;
                    transformMatrix.M32 *= rect.Height;
                    source *= transformMatrix;
                }
            }
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
