using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI
{
    /// <summary>
    ///     Contains properties that are queries into the system's various settings.
    /// </summary>
    public static class SystemFonts
    {
        //Visual Studio batch replace
        //public static (\w+) (\w+)\r\n +{\r\n +get\r\n +{\r\n +throw new NotImplementedException\(\);\r\n +}\r\n +}
        //public static $1 $2 => FrameworkProvider.GetParameterProvider().$2;

        #region Fonts

        /// <summary>
        ///     Maps to SPI_GETICONTITLELOGFONT
        /// </summary>
        public static float IconFontSize
        {
            get
            {
                return FrameworkProvider.GetParameterProvider().IconFontSize;
            }
        }

        /// <summary>
        ///     Maps to SPI_GETICONTITLELOGFONT
        /// </summary>
        public static FontFamily IconFontFamily
        {
            get
            {
                return FrameworkProvider.GetParameterProvider().IconFontFamily;
            }
        }

        /// <summary>
        ///     Maps to SPI_GETICONTITLELOGFONT
        /// </summary>
        public static FontStyle IconFontStyle
        {
            get
            {
                return FrameworkProvider.GetParameterProvider().IconFontStyle;
            }
        }

        /// <summary>
        ///     Maps to SPI_GETICONTITLELOGFONT
        /// </summary>
        public static FontWeight IconFontWeight
        {
            get
            {
                return FrameworkProvider.GetParameterProvider().IconFontWeight;
            }
        }

        /// <summary>
        ///     Maps to SPI_GETICONTITLELOGFONT
        /// </summary>
        public static TextDecorationCollection IconFontTextDecorations
        {
            get
            {
                return FrameworkProvider.GetParameterProvider().IconFontTextDecorations;
            }
        }

        /// <summary>
        ///     Maps to SPI_NONCLIENTMETRICS
        /// </summary>
        public static float CaptionFontSize
        {
            get
            {
                return FrameworkProvider.GetParameterProvider().CaptionFontSize;
            }
        }


        /// <summary>
        ///     Maps to SPI_NONCLIENTMETRICS
        /// </summary>
        public static FontFamily CaptionFontFamily
        {
            get
            {
                return FrameworkProvider.GetParameterProvider().CaptionFontFamily;
            }
        }


        /// <summary>
        ///     Maps to SPI_NONCLIENTMETRICS
        /// </summary>
        public static FontStyle CaptionFontStyle
        {
            get
            {
                return FrameworkProvider.GetParameterProvider().CaptionFontStyle;
            }
        }


        /// <summary>
        ///     Maps to SPI_NONCLIENTMETRICS
        /// </summary>
        public static FontWeight CaptionFontWeight
        {
            get
            {
                return FrameworkProvider.GetParameterProvider().CaptionFontWeight;
            }
        }


        /// <summary>
        ///     Maps to SPI_NONCLIENTMETRICS
        /// </summary>
        public static TextDecorationCollection CaptionFontTextDecorations => FrameworkProvider.GetParameterProvider().CaptionFontTextDecorations;

        /// <summary>
        ///     Maps to SPI_NONCLIENTMETRICS
        /// </summary>
        public static float SmallCaptionFontSize => FrameworkProvider.GetParameterProvider().SmallCaptionFontSize;


        /// <summary>
        ///     Maps to SPI_NONCLIENTMETRICS
        /// </summary>
        public static FontFamily SmallCaptionFontFamily => FrameworkProvider.GetParameterProvider().SmallCaptionFontFamily;


        /// <summary>
        ///     Maps to SPI_NONCLIENTMETRICS
        /// </summary>
        public static FontStyle SmallCaptionFontStyle => FrameworkProvider.GetParameterProvider().SmallCaptionFontStyle;


        /// <summary>
        ///     Maps to SPI_NONCLIENTMETRICS
        /// </summary>
        public static FontWeight SmallCaptionFontWeight => FrameworkProvider.GetParameterProvider().SmallCaptionFontWeight;


        /// <summary>
        ///     Maps to SPI_NONCLIENTMETRICS
        /// </summary>
        public static TextDecorationCollection SmallCaptionFontTextDecorations => FrameworkProvider.GetParameterProvider().SmallCaptionFontTextDecorations;

        /// <summary>
        ///     Maps to SPI_NONCLIENTMETRICS
        /// </summary>
        public static float MenuFontSize => FrameworkProvider.GetParameterProvider().MenuFontSize;

        /// <summary>
        ///     Maps to SPI_NONCLIENTMETRICS
        /// </summary>
        public static FontFamily MenuFontFamily => FrameworkProvider.GetParameterProvider().MenuFontFamily;

        /// <summary>
        ///     Maps to SPI_NONCLIENTMETRICS
        /// </summary>
        public static FontStyle MenuFontStyle => FrameworkProvider.GetParameterProvider().MenuFontStyle;

        /// <summary>
        ///     Maps to SPI_NONCLIENTMETRICS
        /// </summary>
        public static FontWeight MenuFontWeight => FrameworkProvider.GetParameterProvider().MenuFontWeight;

        /// <summary>
        ///     Maps to SPI_NONCLIENTMETRICS
        /// </summary>
        public static TextDecorationCollection MenuFontTextDecorations => FrameworkProvider.GetParameterProvider().MenuFontTextDecorations;

        /// <summary>
        ///     Maps to SPI_NONCLIENTMETRICS
        /// </summary>
        public static float StatusFontSize => FrameworkProvider.GetParameterProvider().StatusFontSize;


        /// <summary>
        ///     Maps to SPI_NONCLIENTMETRICS
        /// </summary>
        public static FontFamily StatusFontFamily => FrameworkProvider.GetParameterProvider().StatusFontFamily;


        /// <summary>
        ///     Maps to SPI_NONCLIENTMETRICS
        /// </summary>
        public static FontStyle StatusFontStyle => FrameworkProvider.GetParameterProvider().StatusFontStyle;


        /// <summary>
        ///     Maps to SPI_NONCLIENTMETRICS
        /// </summary>
        public static FontWeight StatusFontWeight => FrameworkProvider.GetParameterProvider().StatusFontWeight;


        /// <summary>
        ///     Maps to SPI_NONCLIENTMETRICS
        /// </summary>
        public static TextDecorationCollection StatusFontTextDecorations => FrameworkProvider.GetParameterProvider().StatusFontTextDecorations;

        /// <summary>
        ///     Maps to SPI_NONCLIENTMETRICS
        /// </summary>
        public static float MessageFontSize => FrameworkProvider.GetParameterProvider().MessageFontSize;


        /// <summary>
        ///     Maps to SPI_NONCLIENTMETRICS
        /// </summary>
        public static FontFamily MessageFontFamily => FrameworkProvider.GetParameterProvider().MessageFontFamily;


        /// <summary>
        ///     Maps to SPI_NONCLIENTMETRICS
        /// </summary>
        public static FontStyle MessageFontStyle => FrameworkProvider.GetParameterProvider().MessageFontStyle;

        /// <summary>
        ///     Maps to SPI_NONCLIENTMETRICS
        /// </summary>
        public static FontWeight MessageFontWeight => FrameworkProvider.GetParameterProvider().MessageFontWeight;

        /// <summary>
        ///     Maps to SPI_NONCLIENTMETRICS
        /// </summary>
        public static TextDecorationCollection MessageFontTextDecorations => FrameworkProvider.GetParameterProvider().MessageFontTextDecorations;

        #endregion

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        private static SystemResourceKey CreateInstance(SystemResourceKeyID KeyId)
        {
            return new SystemResourceKey(KeyId);
        }

        #region Keys

        /// <summary>
        ///     IconFontSize System Resource Key
        /// </summary>
        public static ResourceKey IconFontSizeKey
        {
            get
            {
                if (_CacheIconFontSize == null)
                {
                    _CacheIconFontSize = CreateInstance(SystemResourceKeyID.IconFontSize);
                }

                return _CacheIconFontSize;
            }
        }

        /// <summary>
        ///     IconFontFamily System Resource Key
        /// </summary>
        public static ResourceKey IconFontFamilyKey
        {
            get
            {
                if (_CacheIconFontFamily == null)
                {
                    _CacheIconFontFamily = CreateInstance(SystemResourceKeyID.IconFontFamily);
                }

                return _CacheIconFontFamily;
            }
        }

        /// <summary>
        ///     IconFontStyle System Resource Key
        /// </summary>
        public static ResourceKey IconFontStyleKey
        {
            get
            {
                if (_CacheIconFontStyle == null)
                {
                    _CacheIconFontStyle = CreateInstance(SystemResourceKeyID.IconFontStyle);
                }

                return _CacheIconFontStyle;
            }
        }

        /// <summary>
        ///     IconFontWeight System Resource Key
        /// </summary>
        public static ResourceKey IconFontWeightKey
        {
            get
            {
                if (_CacheIconFontWeight == null)
                {
                    _CacheIconFontWeight = CreateInstance(SystemResourceKeyID.IconFontWeight);
                }

                return _CacheIconFontWeight;
            }
        }

        /// <summary>
        ///     IconFontTextDecorations System Resource Key
        /// </summary>
        public static ResourceKey IconFontTextDecorationsKey
        {
            get
            {
                if (_CacheIconFontTextDecorations == null)
                {
                    _CacheIconFontTextDecorations = CreateInstance(SystemResourceKeyID.IconFontTextDecorations);
                }

                return _CacheIconFontTextDecorations;
            }
        }




        /// <summary>
        ///     CaptionFontSize System Resource Key
        /// </summary>
        public static ResourceKey CaptionFontSizeKey
        {
            get
            {
                if (_CacheCaptionFontSize == null)
                {
                    _CacheCaptionFontSize = CreateInstance(SystemResourceKeyID.CaptionFontSize);
                }

                return _CacheCaptionFontSize;
            }
        }

        /// <summary>
        ///     CaptionFontFamily System Resource Key
        /// </summary>
        public static ResourceKey CaptionFontFamilyKey
        {
            get
            {
                if (_CacheCaptionFontFamily == null)
                {
                    _CacheCaptionFontFamily = CreateInstance(SystemResourceKeyID.CaptionFontFamily);
                }

                return _CacheCaptionFontFamily;
            }
        }

        /// <summary>
        ///     CaptionFontStyle System Resource Key
        /// </summary>
        public static ResourceKey CaptionFontStyleKey
        {
            get
            {
                if (_CacheCaptionFontStyle == null)
                {
                    _CacheCaptionFontStyle = CreateInstance(SystemResourceKeyID.CaptionFontStyle);
                }

                return _CacheCaptionFontStyle;
            }
        }

        /// <summary>
        ///     CaptionFontWeight System Resource Key
        /// </summary>
        public static ResourceKey CaptionFontWeightKey
        {
            get
            {
                if (_CacheCaptionFontWeight == null)
                {
                    _CacheCaptionFontWeight = CreateInstance(SystemResourceKeyID.CaptionFontWeight);
                }

                return _CacheCaptionFontWeight;
            }
        }

        /// <summary>
        ///     CaptionFontTextDecorations System Resource Key
        /// </summary>
        public static ResourceKey CaptionFontTextDecorationsKey
        {
            get
            {
                if (_CacheCaptionFontTextDecorations == null)
                {
                    _CacheCaptionFontTextDecorations = CreateInstance(SystemResourceKeyID.CaptionFontTextDecorations);
                }

                return _CacheCaptionFontTextDecorations;
            }
        }

        /// <summary>
        ///     SmallCaptionFontSize System Resource Key
        /// </summary>
        public static ResourceKey SmallCaptionFontSizeKey
        {
            get
            {
                if (_CacheSmallCaptionFontSize == null)
                {
                    _CacheSmallCaptionFontSize = CreateInstance(SystemResourceKeyID.SmallCaptionFontSize);
                }

                return _CacheSmallCaptionFontSize;
            }
        }

        /// <summary>
        ///     SmallCaptionFontFamily System Resource Key
        /// </summary>
        public static ResourceKey SmallCaptionFontFamilyKey
        {
            get
            {
                if (_CacheSmallCaptionFontFamily == null)
                {
                    _CacheSmallCaptionFontFamily = CreateInstance(SystemResourceKeyID.SmallCaptionFontFamily);
                }

                return _CacheSmallCaptionFontFamily;
            }
        }

        /// <summary>
        ///     SmallCaptionFontStyle System Resource Key
        /// </summary>
        public static ResourceKey SmallCaptionFontStyleKey
        {
            get
            {
                if (_CacheSmallCaptionFontStyle == null)
                {
                    _CacheSmallCaptionFontStyle = CreateInstance(SystemResourceKeyID.SmallCaptionFontStyle);
                }

                return _CacheSmallCaptionFontStyle;
            }
        }

        /// <summary>
        ///     SmallCaptionFontWeight System Resource Key
        /// </summary>
        public static ResourceKey SmallCaptionFontWeightKey
        {
            get
            {
                if (_CacheSmallCaptionFontWeight == null)
                {
                    _CacheSmallCaptionFontWeight = CreateInstance(SystemResourceKeyID.SmallCaptionFontWeight);
                }

                return _CacheSmallCaptionFontWeight;
            }
        }

        /// <summary>
        ///     SmallCaptionFontTextDecorations System Resource Key
        /// </summary>
        public static ResourceKey SmallCaptionFontTextDecorationsKey
        {
            get
            {
                if (_CacheSmallCaptionFontTextDecorations == null)
                {
                    _CacheSmallCaptionFontTextDecorations = CreateInstance(SystemResourceKeyID.SmallCaptionFontTextDecorations);
                }

                return _CacheSmallCaptionFontTextDecorations;
            }
        }

        /// <summary>
        ///     MenuFontSize System Resource Key
        /// </summary>
        public static ResourceKey MenuFontSizeKey
        {
            get
            {
                if (_CacheMenuFontSize == null)
                {
                    _CacheMenuFontSize = CreateInstance(SystemResourceKeyID.MenuFontSize);
                }

                return _CacheMenuFontSize;
            }
        }

        /// <summary>
        ///     MenuFontFamily System Resource Key
        /// </summary>
        public static ResourceKey MenuFontFamilyKey
        {
            get
            {
                if (_CacheMenuFontFamily == null)
                {
                    _CacheMenuFontFamily = CreateInstance(SystemResourceKeyID.MenuFontFamily);
                }

                return _CacheMenuFontFamily;
            }
        }

        /// <summary>
        ///     MenuFontStyle System Resource Key
        /// </summary>
        public static ResourceKey MenuFontStyleKey
        {
            get
            {
                if (_CacheMenuFontStyle == null)
                {
                    _CacheMenuFontStyle = CreateInstance(SystemResourceKeyID.MenuFontStyle);
                }

                return _CacheMenuFontStyle;
            }
        }

        /// <summary>
        ///     MenuFontWeight System Resource Key
        /// </summary>
        public static ResourceKey MenuFontWeightKey
        {
            get
            {
                if (_CacheMenuFontWeight == null)
                {
                    _CacheMenuFontWeight = CreateInstance(SystemResourceKeyID.MenuFontWeight);
                }

                return _CacheMenuFontWeight;
            }
        }

        /// <summary>
        ///     MenuFontTextDecorations System Resource Key
        /// </summary>
        public static ResourceKey MenuFontTextDecorationsKey
        {
            get
            {
                if (_CacheMenuFontTextDecorations == null)
                {
                    _CacheMenuFontTextDecorations = CreateInstance(SystemResourceKeyID.MenuFontTextDecorations);
                }

                return _CacheMenuFontTextDecorations;
            }
        }

        /// <summary>
        ///     StatusFontSize System Resource Key
        /// </summary>
        public static ResourceKey StatusFontSizeKey
        {
            get
            {
                if (_CacheStatusFontSize == null)
                {
                    _CacheStatusFontSize = CreateInstance(SystemResourceKeyID.StatusFontSize);
                }

                return _CacheStatusFontSize;
            }
        }

        /// <summary>
        ///     StatusFontFamily System Resource Key
        /// </summary>
        public static ResourceKey StatusFontFamilyKey
        {
            get
            {
                if (_CacheStatusFontFamily == null)
                {
                    _CacheStatusFontFamily = CreateInstance(SystemResourceKeyID.StatusFontFamily);
                }

                return _CacheStatusFontFamily;
            }
        }

        /// <summary>
        ///     StatusFontStyle System Resource Key
        /// </summary>
        public static ResourceKey StatusFontStyleKey
        {
            get
            {
                if (_CacheStatusFontStyle == null)
                {
                    _CacheStatusFontStyle = CreateInstance(SystemResourceKeyID.StatusFontStyle);
                }

                return _CacheStatusFontStyle;
            }
        }

        /// <summary>
        ///     StatusFontWeight System Resource Key
        /// </summary>
        public static ResourceKey StatusFontWeightKey
        {
            get
            {
                if (_CacheStatusFontWeight == null)
                {
                    _CacheStatusFontWeight = CreateInstance(SystemResourceKeyID.StatusFontWeight);
                }

                return _CacheStatusFontWeight;
            }
        }

        /// <summary>
        ///     StatusFontTextDecorations System Resource Key
        /// </summary>
        public static ResourceKey StatusFontTextDecorationsKey
        {
            get
            {
                if (_CacheStatusFontTextDecorations == null)
                {
                    _CacheStatusFontTextDecorations = CreateInstance(SystemResourceKeyID.StatusFontTextDecorations);
                }

                return _CacheStatusFontTextDecorations;
            }
        }

        /// <summary>
        ///     MessageFontSize System Resource Key
        /// </summary>
        public static ResourceKey MessageFontSizeKey
        {
            get
            {
                if (_CacheMessageFontSize == null)
                {
                    _CacheMessageFontSize = CreateInstance(SystemResourceKeyID.MessageFontSize);
                }

                return _CacheMessageFontSize;
            }
        }

        /// <summary>
        ///     MessageFontFamily System Resource Key
        /// </summary>
        public static ResourceKey MessageFontFamilyKey
        {
            get
            {
                if (_CacheMessageFontFamily == null)
                {
                    _CacheMessageFontFamily = CreateInstance(SystemResourceKeyID.MessageFontFamily);
                }

                return _CacheMessageFontFamily;
            }
        }

        /// <summary>
        ///     MessageFontStyle System Resource Key
        /// </summary>
        public static ResourceKey MessageFontStyleKey
        {
            get
            {
                if (_CacheMessageFontStyle == null)
                {
                    _CacheMessageFontStyle = CreateInstance(SystemResourceKeyID.MessageFontStyle);
                }

                return _CacheMessageFontStyle;
            }
        }

        /// <summary>
        ///     MessageFontWeight System Resource Key
        /// </summary>
        public static ResourceKey MessageFontWeightKey
        {
            get
            {
                if (_CacheMessageFontWeight == null)
                {
                    _CacheMessageFontWeight = CreateInstance(SystemResourceKeyID.MessageFontWeight);
                }

                return _CacheMessageFontWeight;
            }
        }

        /// <summary>
        ///     MessageFontTextDecorations System Resource Key
        /// </summary>
        public static ResourceKey MessageFontTextDecorationsKey
        {
            get
            {
                if (_CacheMessageFontTextDecorations == null)
                {
                    _CacheMessageFontTextDecorations = CreateInstance(SystemResourceKeyID.MessageFontTextDecorations);
                }

                return _CacheMessageFontTextDecorations;
            }
        }

        #endregion

        #region Implementation

        private static float ConvertFontHeight(int height)
        {
            return Math.Abs(height);
            //int dpi = SystemParameters.Dpi;

            //if (dpi != 0)
            //{
            //    return Math.Abs(height) * 96f / dpi;
            //}
            //else
            //{
            //    // Could not get the DPI to convert the size, using the hardcoded fallback value
            //    return _FallbackFontSize;
            //}
        }

        private const float _FallbackFontSize = 11.0f;   // To use if unable to get the system size

        internal static void InvalidateIconMetrics()
        {
            //_iconFontTextDecorations = null;
            _IconFontFamily = null;
        }

        internal static void InvalidateNonClientMetrics()
        {
            //_messageFontTextDecorations = null;
            //_statusFontTextDecorations = null;
            //_menuFontTextDecorations = null;
            //_smallCaptionFontTextDecorations = null;
            //_captionFontTextDecorations = null;

            _MessageFontFamily = null;
            _StatusFontFamily = null;
            _MenuFontFamily = null;
            _SmallCaptionFontFamily = null;
            _CaptionFontFamily = null;
        }

        //private static TextDecorationCollection? _iconFontTextDecorations;
        //private static TextDecorationCollection? _messageFontTextDecorations;
        //private static TextDecorationCollection? _statusFontTextDecorations;
        //private static TextDecorationCollection? _menuFontTextDecorations;
        //private static TextDecorationCollection? _smallCaptionFontTextDecorations;
        //private static TextDecorationCollection? _captionFontTextDecorations;

        private static FontFamily? _IconFontFamily;
        private static FontFamily? _MessageFontFamily;
        private static FontFamily? _StatusFontFamily;
        private static FontFamily? _MenuFontFamily;
        private static FontFamily? _SmallCaptionFontFamily;
        private static FontFamily? _CaptionFontFamily;

        private static SystemResourceKey? _CacheIconFontSize;
        private static SystemResourceKey? _CacheIconFontFamily;
        private static SystemResourceKey? _CacheIconFontStyle;
        private static SystemResourceKey? _CacheIconFontWeight;
        private static SystemResourceKey? _CacheIconFontTextDecorations;
        private static SystemResourceKey? _CacheCaptionFontSize;
        private static SystemResourceKey? _CacheCaptionFontFamily;
        private static SystemResourceKey? _CacheCaptionFontStyle;
        private static SystemResourceKey? _CacheCaptionFontWeight;
        private static SystemResourceKey? _CacheCaptionFontTextDecorations;
        private static SystemResourceKey? _CacheSmallCaptionFontSize;
        private static SystemResourceKey? _CacheSmallCaptionFontFamily;
        private static SystemResourceKey? _CacheSmallCaptionFontStyle;
        private static SystemResourceKey? _CacheSmallCaptionFontWeight;
        private static SystemResourceKey? _CacheSmallCaptionFontTextDecorations;
        private static SystemResourceKey? _CacheMenuFontSize;
        private static SystemResourceKey? _CacheMenuFontFamily;
        private static SystemResourceKey? _CacheMenuFontStyle;
        private static SystemResourceKey? _CacheMenuFontWeight;
        private static SystemResourceKey? _CacheMenuFontTextDecorations;
        private static SystemResourceKey? _CacheStatusFontSize;
        private static SystemResourceKey? _CacheStatusFontFamily;
        private static SystemResourceKey? _CacheStatusFontStyle;
        private static SystemResourceKey? _CacheStatusFontWeight;
        private static SystemResourceKey? _CacheStatusFontTextDecorations;
        private static SystemResourceKey? _CacheMessageFontSize;
        private static SystemResourceKey? _CacheMessageFontFamily;
        private static SystemResourceKey? _CacheMessageFontStyle;
        private static SystemResourceKey? _CacheMessageFontWeight;
        private static SystemResourceKey? _CacheMessageFontTextDecorations;

        #endregion
    }
}
