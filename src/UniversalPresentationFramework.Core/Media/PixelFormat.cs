using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public record struct PixelFormat
    {
        private readonly PixelFormatFlags _flags;
        private readonly byte[] _channelBits;

        internal PixelFormat(PixelFormatEnum format)
        {
            _flags = GetPixelFormatFlagsFromEnum(format, out _channelBits);
        }

        public PixelFormat(byte bitsPerPixel, PixelFormatColorSpace colorSpace, bool isPremultiplied, PixelFormatChannelOrder channelOrder, byte[] channelBits, bool isFloat = false)
        {
            if (channelBits == null)
                throw new ArgumentNullException(nameof(channelBits));
            _flags = (PixelFormatFlags)(bitsPerPixel | (int)colorSpace | (int)channelOrder);
            if (isPremultiplied)
                _flags |= PixelFormatFlags.Premultiplied;
            if (channelBits.Sum(t => t) != bitsPerPixel)
                throw new ArgumentException("Bits per pixel do not equal channel bits sum result.");
            _channelBits = channelBits;
            if (isFloat)
                _flags |= PixelFormatFlags.IsFloat;
        }

        public PixelFormat(byte bitsPerPixel)
        {
            _flags = (PixelFormatFlags)(bitsPerPixel) | PixelFormatFlags.Palettized;
            _channelBits = Array.Empty<byte>();
        }

        public byte BitsPerPixel => (byte)((int)_flags & 0xff);

        public PixelFormatColorSpace ColorSpace => (PixelFormatColorSpace)((int)_flags & 0x700);

        public bool IsPalettized => _flags.HasFlag(PixelFormatFlags.Palettized);

        public bool IsPremultiplied => _flags.HasFlag(PixelFormatFlags.Premultiplied);

        public PixelFormatChannelOrder ChannelOrder => (PixelFormatChannelOrder)((int)_flags & 0xf000);

        public byte[] ChannelBits => _channelBits;

        public bool IsFloat => _flags.HasFlag(PixelFormatFlags.IsFloat);

        static private PixelFormatFlags GetPixelFormatFlagsFromEnum(PixelFormatEnum pixelFormatEnum, out byte[] channelBits)
        {
            switch (pixelFormatEnum)
            {
                case PixelFormatEnum.Default:
                    channelBits = Array.Empty<byte>();
                    return PixelFormatFlags.BitsPerPixelUndefined;
                case PixelFormatEnum.Indexed1:
                    channelBits = Array.Empty<byte>();
                    return PixelFormatFlags.BitsPerPixel1 | PixelFormatFlags.Palettized;
                case PixelFormatEnum.Indexed2:
                    channelBits = Array.Empty<byte>();
                    return PixelFormatFlags.BitsPerPixel2 | PixelFormatFlags.Palettized;
                case PixelFormatEnum.Indexed4:
                    channelBits = Array.Empty<byte>();
                    return PixelFormatFlags.BitsPerPixel4 | PixelFormatFlags.Palettized;
                case PixelFormatEnum.Indexed8:
                    channelBits = Array.Empty<byte>();
                    return PixelFormatFlags.BitsPerPixel8 | PixelFormatFlags.Palettized;
                case PixelFormatEnum.BlackWhite:
                    channelBits = [1];
                    return PixelFormatFlags.BitsPerPixel1 | PixelFormatFlags.IsGray;
                case PixelFormatEnum.Gray2:
                    channelBits = [2];
                    return PixelFormatFlags.BitsPerPixel2 | PixelFormatFlags.IsGray;
                case PixelFormatEnum.Gray4:
                    channelBits = [4];
                    return PixelFormatFlags.BitsPerPixel4 | PixelFormatFlags.IsGray;
                case PixelFormatEnum.Gray8:
                    channelBits = [8];
                    return PixelFormatFlags.BitsPerPixel8 | PixelFormatFlags.IsGray;
                case PixelFormatEnum.Bgr555:
                    channelBits = [5, 5, 5];
                    return PixelFormatFlags.BitsPerPixel16 | PixelFormatFlags.IsSRGB | PixelFormatFlags.ChannelOrderBGR;
                case PixelFormatEnum.Bgr565:
                    channelBits = [5, 6, 5];
                    return PixelFormatFlags.BitsPerPixel16 | PixelFormatFlags.IsSRGB | PixelFormatFlags.ChannelOrderBGR;
                case PixelFormatEnum.Bgr101010:
                    channelBits = [10, 10, 10];
                    return PixelFormatFlags.BitsPerPixel32 | PixelFormatFlags.IsSRGB | PixelFormatFlags.ChannelOrderBGR;
                case PixelFormatEnum.Bgr24:
                    channelBits = [8, 8, 8];
                    return PixelFormatFlags.BitsPerPixel24 | PixelFormatFlags.IsSRGB | PixelFormatFlags.ChannelOrderBGR;
                case PixelFormatEnum.Rgb24:
                    channelBits = [8, 8, 8];
                    return PixelFormatFlags.BitsPerPixel24 | PixelFormatFlags.IsSRGB | PixelFormatFlags.ChannelOrderRGB;
                case PixelFormatEnum.Bgr32:
                    channelBits = [8, 8, 8];
                    return PixelFormatFlags.BitsPerPixel32 | PixelFormatFlags.IsSRGB | PixelFormatFlags.ChannelOrderBGR;
                case PixelFormatEnum.Bgra32:
                    channelBits = [8, 8, 8, 8];
                    return PixelFormatFlags.BitsPerPixel32 | PixelFormatFlags.IsSRGB | PixelFormatFlags.ChannelOrderBGRA;
                case PixelFormatEnum.Pbgra32:
                    channelBits = [8, 8, 8, 8];
                    return PixelFormatFlags.BitsPerPixel32 | PixelFormatFlags.IsSRGB | PixelFormatFlags.Premultiplied | PixelFormatFlags.ChannelOrderBGRA;
                case PixelFormatEnum.Rgb48:
                    channelBits = [16, 16, 16];
                    return PixelFormatFlags.BitsPerPixel48 | PixelFormatFlags.IsSRGB | PixelFormatFlags.ChannelOrderRGB;
                case PixelFormatEnum.Rgba64:
                    channelBits = [16, 16, 16, 16];
                    return PixelFormatFlags.BitsPerPixel64 | PixelFormatFlags.IsSRGB | PixelFormatFlags.ChannelOrderRGBA;
                case PixelFormatEnum.Prgba64:
                    channelBits = [16, 16, 16, 16];
                    return PixelFormatFlags.BitsPerPixel64 | PixelFormatFlags.IsSRGB | PixelFormatFlags.Premultiplied | PixelFormatFlags.ChannelOrderRGBA;
                case PixelFormatEnum.Gray16:
                    channelBits = [16];
                    return PixelFormatFlags.BitsPerPixel16 | PixelFormatFlags.IsSRGB | PixelFormatFlags.IsGray;
                case PixelFormatEnum.Gray32Float:
                    channelBits = [32];
                    return PixelFormatFlags.BitsPerPixel32 | PixelFormatFlags.IsScRGB | PixelFormatFlags.IsGray;
                case PixelFormatEnum.Rgb128Float:
                    channelBits = [32, 32, 32, 32];
                    return PixelFormatFlags.BitsPerPixel128 | PixelFormatFlags.IsScRGB | PixelFormatFlags.ChannelOrderRGB;
                case PixelFormatEnum.Rgba128Float:
                    channelBits = [32, 32, 32, 32];
                    return PixelFormatFlags.BitsPerPixel128 | PixelFormatFlags.IsScRGB | PixelFormatFlags.ChannelOrderRGBA;
                case PixelFormatEnum.Prgba128Float:
                    channelBits = [32, 32, 32, 32];
                    return PixelFormatFlags.BitsPerPixel128 | PixelFormatFlags.IsScRGB | PixelFormatFlags.Premultiplied | PixelFormatFlags.ChannelOrderRGBA;
                case PixelFormatEnum.Cmyk32:
                    channelBits = [8, 8, 8, 8];
                    return PixelFormatFlags.BitsPerPixel32 | PixelFormatFlags.IsCMYK;
            }
            // 3rd party pixel format -- we don't expose anything about it.
            channelBits = Array.Empty<byte>();
            return PixelFormatFlags.BitsPerPixelUndefined;
        }
    }

    [Flags]
    internal enum PixelFormatFlags
    {
        BitsPerPixelMask = 0x00FF,
        BitsPerPixelUndefined = 0,
        BitsPerPixel1 = 1,
        BitsPerPixel2 = 2,
        BitsPerPixel4 = 4,
        BitsPerPixel8 = 8,
        BitsPerPixel16 = 16,
        BitsPerPixel24 = 24,
        BitsPerPixel32 = 32,
        BitsPerPixel48 = 48,
        BitsPerPixel64 = 64,
        BitsPerPixel96 = 96,
        BitsPerPixel128 = 128,
        IsGray = 0x00000100,   // Grayscale only
        IsCMYK = 0x00000200,   // CMYK, not ARGB
        IsSRGB = 0x00000300,   // Gamma is approximately 2.2
        IsScRGB = 0x00000400,   // Gamma is 1.0
        Premultiplied = 0x00000800,   // Premultiplied Alpha
        ChannelOrderRGB = 0x00001000,
        ChannelOrderBGR = 0x00002000,
        ChannelOrderARGB = 0x00003000,
        ChannelOrderABGR = 0x00004000,
        ChannelOrderRGBA = 0x00005000,
        ChannelOrderBGRA = 0x00006000,
        Palettized = 0x00010000,   // Pixels are indexes into a palette
        IsFloat = 0x0002000
    }

    public enum PixelFormatColorSpace
    {
        Unkonwn = 0,
        IsGray = 0x00000100,   // Grayscale only
        IsCMYK = 0x00000200,   // CMYK, not ARGB
        IsSRGB = 0x00000300,   // Gamma is approximately 2.2
        IsScRGB = 0x00000400,   // Gamma is 1.0
    }

    public enum PixelFormatChannelOrder
    {
        Unkonwn = 0,
        ChannelOrderRGB = 0x00001000,
        ChannelOrderBGR = 0x00002000,
        ChannelOrderARGB = 0x00003000,
        ChannelOrderABGR = 0x00004000,
        ChannelOrderRGBA = 0x00005000,
        ChannelOrderBGRA = 0x00006000
    }

    internal enum PixelFormatEnum
    {
        /// <summary>
        /// Default: (DontCare) the format is not important
        /// </summary>
        Default = 0,

        /// <summary>
        /// Extended: the pixel format is 3rd party - we don't know anything about it.
        /// </summary>
        Extended = Default,

        /// <summary>
        /// Indexed1: Paletted image with 2 colors.
        /// </summary>
        Indexed1 = 0x1,

        /// <summary>
        /// Indexed2: Paletted image with 4 colors.
        /// </summary>
        Indexed2 = 0x2,

        /// <summary>
        /// Indexed4: Paletted image with 16 colors.
        /// </summary>
        Indexed4 = 0x3,

        /// <summary>
        /// Indexed8: Paletted image with 256 colors.
        /// </summary>
        Indexed8 = 0x4,

        /// <summary>
        /// BlackWhite: Monochrome, 2-color image, black and white only.
        /// </summary>
        BlackWhite = 0x5,

        /// <summary>
        /// Gray2: Image with 4 shades of gray
        /// </summary>
        Gray2 = 0x6,

        /// <summary>
        /// Gray4: Image with 16 shades of gray
        /// </summary>
        Gray4 = 0x7,

        /// <summary>
        /// Gray8: Image with 256 shades of gray
        /// </summary>
        Gray8 = 0x8,

        /// <summary>
        /// Bgr555: 16 bpp SRGB format
        /// </summary>
        Bgr555 = 0x9,

        /// <summary>
        /// Bgr565: 16 bpp SRGB format
        /// </summary>
        Bgr565 = 0xA,

        /// <summary>
        /// Gray16: 16 bpp Gray format
        /// </summary>
        Gray16 = 0xB,

        /// <summary>
        /// Bgr24: 24 bpp SRGB format
        /// </summary>
        Bgr24 = 0xC,

        /// <summary>
        /// BGR24: 24 bpp SRGB format
        /// </summary>
        Rgb24 = 0xD,

        /// <summary>
        /// Bgr32: 32 bpp SRGB format
        /// </summary>
        Bgr32 = 0xE,

        /// <summary>
        /// Bgra32: 32 bpp SRGB format
        /// </summary>
        Bgra32 = 0xF,

        /// <summary>
        /// Pbgra32: 32 bpp SRGB format
        /// </summary>
        Pbgra32 = 0x10,

        /// <summary>
        /// Gray32Float: 32 bpp Gray format, gamma is 1.0
        /// </summary>
        Gray32Float = 0x11,

        /// <summary>
        /// Bgr101010: 32 bpp Gray fixed point format
        /// </summary>
        Bgr101010 = 0x14,

        /// <summary>
        /// Rgb48: 48 bpp RGB format
        /// </summary>
        Rgb48 = 0x15,

        /// <summary>
        /// Rgba64: 64 bpp extended format; Gamma is 1.0
        /// </summary>
        Rgba64 = 0x16,

        /// <summary>
        /// Prgba64: 64 bpp extended format; Gamma is 1.0
        /// </summary>
        Prgba64 = 0x17,

        /// <summary>
        /// Rgba128Float: 128 bpp extended format; Gamma is 1.0
        /// </summary>
        Rgba128Float = 0x19,

        /// <summary>
        /// Prgba128Float: 128 bpp extended format; Gamma is 1.0
        /// </summary>
        Prgba128Float = 0x1A,

        /// <summary>
        /// PABGR128Float: 128 bpp extended format; Gamma is 1.0
        /// </summary>
        Rgb128Float = 0x1B,

        /// <summary>
        /// CMYK32: 32 bpp CMYK format.
        /// </summary>
        Cmyk32 = 0x1C
    }
}
