using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    /// <summary>
    /// PixelFormats - The collection of supported Pixel Formats
    /// </summary>
    public static class PixelFormats
    {
        /// <summary>
        /// Default: for situations when the pixel format may not be important
        /// </summary>
        public static readonly PixelFormat Default = new PixelFormat(PixelFormatEnum.Default);

        /// <summary>
        /// Indexed1: Paletted image with 2 colors.
        /// </summary>
        public static PixelFormat Indexed1 = new PixelFormat(PixelFormatEnum.Indexed1);

        /// <summary>
        /// Indexed2: Paletted image with 4 colors.
        /// </summary>
        public static PixelFormat Indexed2 = new PixelFormat(PixelFormatEnum.Indexed2);

        /// <summary>
        /// Indexed4: Paletted image with 16 colors.
        /// </summary>
        public static PixelFormat Indexed4 = new PixelFormat(PixelFormatEnum.Indexed4);

        /// <summary>
        /// Indexed8: Paletted image with 256 colors.
        /// </summary>
        public static PixelFormat Indexed8 = new PixelFormat(PixelFormatEnum.Indexed8);

        /// <summary>
        /// BlackWhite: Monochrome, 2-color image, black and white only.
        /// </summary>
        public static PixelFormat BlackWhite = new PixelFormat(PixelFormatEnum.BlackWhite);

        /// <summary>
        /// Gray2: Image with 4 shades of gray
        /// </summary>
        public static PixelFormat Gray2 = new PixelFormat(PixelFormatEnum.Gray2);

        /// <summary>
        /// Gray4: Image with 16 shades of gray
        /// </summary>
        public static PixelFormat Gray4 = new PixelFormat(PixelFormatEnum.Gray4);

        /// <summary>
        /// Gray8: Image with 256 shades of gray
        /// </summary>
        public static PixelFormat Gray8 = new PixelFormat(PixelFormatEnum.Gray8);

        /// <summary>
        /// Bgr555: 16 bpp SRGB format
        /// </summary>
        public static PixelFormat Bgr555 = new PixelFormat(PixelFormatEnum.Bgr555);

        /// <summary>
        /// Bgr565: 16 bpp SRGB format
        /// </summary>
        public static PixelFormat Bgr565 = new PixelFormat(PixelFormatEnum.Bgr565);

        /// <summary>
        /// Rgb128Float: 128 bpp extended format; Gamma is 1.0
        /// </summary>
        public static PixelFormat Rgb128Float = new PixelFormat(PixelFormatEnum.Rgb128Float);

        /// <summary>
        /// Bgr24: 24 bpp SRGB format
        /// </summary>
        public static PixelFormat Bgr24 = new PixelFormat(PixelFormatEnum.Bgr24);

        /// <summary>
        /// Rgb24: 24 bpp SRGB format
        /// </summary>
        public static PixelFormat Rgb24 = new PixelFormat(PixelFormatEnum.Rgb24);

        /// <summary>
        /// Bgr101010: 32 bpp SRGB format
        /// </summary>
        public static PixelFormat Bgr101010 = new PixelFormat(PixelFormatEnum.Bgr101010);

        /// <summary>
        /// Bgr32: 32 bpp SRGB format
        /// </summary>
        public static PixelFormat Bgr32 = new PixelFormat(PixelFormatEnum.Bgr32);

        /// <summary>
        /// Bgra32: 32 bpp SRGB format
        /// </summary>
        public static PixelFormat Bgra32 = new PixelFormat(PixelFormatEnum.Bgra32);

        /// <summary>
        /// Pbgra32: 32 bpp SRGB format
        /// </summary>
        public static PixelFormat Pbgra32 = new PixelFormat(PixelFormatEnum.Pbgra32);

        /// <summary>
        /// Rgb48: 48 bpp extended format; Gamma is 1.0
        /// </summary>
        public static PixelFormat Rgb48 = new PixelFormat(PixelFormatEnum.Rgb48);

        /// <summary>
        /// Rgba64: 64 bpp extended format; Gamma is 1.0
        /// </summary>
        public static PixelFormat Rgba64 = new PixelFormat(PixelFormatEnum.Rgba64);

        /// <summary>
        /// Prgba64: 64 bpp extended format; Gamma is 1.0
        /// </summary>
        public static PixelFormat Prgba64 = new PixelFormat(PixelFormatEnum.Prgba64);

        /// <summary>
        /// Gray16: 16 bpp Gray-scale format; Gamma is 1.0
        /// </summary>
        public static PixelFormat Gray16 = new PixelFormat(PixelFormatEnum.Gray16);

        /// <summary>
        /// Gray32Float: 32 bpp Gray-scale format; Gamma is 1.0
        /// </summary>
        public static PixelFormat Gray32Float = new PixelFormat(PixelFormatEnum.Gray32Float);

        /// <summary>
        /// Rgba128Float: 128 bpp extended format; Gamma is 1.0
        /// </summary>
        public static PixelFormat Rgba128Float = new PixelFormat(PixelFormatEnum.Rgba128Float);

        /// <summary>
        /// Prgba128Float: 128 bpp extended format; Gamma is 1.0
        /// </summary>
        public static PixelFormat Prgba128Float = new PixelFormat(PixelFormatEnum.Prgba128Float);

        /// <summary>
        /// Cmyk32: 32 bpp format
        /// </summary>
        public static PixelFormat Cmyk32 = new PixelFormat(PixelFormatEnum.Cmyk32);
    }
}
