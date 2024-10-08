﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;
using Wodsoft.UI.Media.Imaging;

namespace Wodsoft.UI.Providers
{
    /// <summary>
    /// Framework Renderer Provider.
    /// </summary>
    public interface IRendererProvider
    {
        VisualDrawingContext CreateDrawingContext(Visual visual);

        IImageContext CreateImageContext(Stream stream, int newWidth, int newHeight, Rotation rotation);

        IBitmapContext CreateBitmapContext(int pixelWidth, int pixelHeight, float dpiX, float dpiY, PixelFormat pixelFormat, BitmapPalette? palette);

        IBitmapContext CreateBitmapContext(IImageContext context);

        IRenderBitmapContext CreateRenderBitmapContext(int pixelWidth, int pixelHeight, float dpiX, float dpiY, PixelFormat pixelFormat);

        StreamGeometryContext CreateGeometryContext();

        GlyphTypeface? CreateGlyphTypeface(string familyName, FontStyle style, FontWeight weight, FontStretch stretch);
    }
}
