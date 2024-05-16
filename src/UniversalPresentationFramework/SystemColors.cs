using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI
{
    /// <summary>
    ///     Contains properties that are queries into the system's various colors.
    /// </summary>
    public static class SystemColors
    {
        #region Colors

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color ActiveBorderColor => FrameworkProvider.GetParameterProvider().ActiveBorderColor;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color ActiveCaptionColor => FrameworkProvider.GetParameterProvider().ActiveCaptionColor;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color ActiveCaptionTextColor => FrameworkProvider.GetParameterProvider().ActiveCaptionTextColor;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color AppWorkspaceColor => FrameworkProvider.GetParameterProvider().AppWorkspaceColor;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color ControlColor => FrameworkProvider.GetParameterProvider().ControlColor;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color ControlDarkColor => FrameworkProvider.GetParameterProvider().ControlDarkColor;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color ControlDarkDarkColor => FrameworkProvider.GetParameterProvider().ControlDarkDarkColor;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color ControlLightColor => FrameworkProvider.GetParameterProvider().ControlLightColor;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color ControlLightLightColor => FrameworkProvider.GetParameterProvider().ControlLightLightColor;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color ControlTextColor => FrameworkProvider.GetParameterProvider().ControlTextColor;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color DesktopColor => FrameworkProvider.GetParameterProvider().DesktopColor;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color GradientActiveCaptionColor => FrameworkProvider.GetParameterProvider().GradientActiveCaptionColor;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color GradientInactiveCaptionColor => FrameworkProvider.GetParameterProvider().GradientInactiveCaptionColor;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color GrayTextColor => FrameworkProvider.GetParameterProvider().GrayTextColor;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color HighlightColor => FrameworkProvider.GetParameterProvider().HighlightColor;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color HighlightTextColor => FrameworkProvider.GetParameterProvider().HighlightTextColor;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color HotTrackColor => FrameworkProvider.GetParameterProvider().HotTrackColor;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color InactiveBorderColor => FrameworkProvider.GetParameterProvider().InactiveBorderColor;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color InactiveCaptionColor => FrameworkProvider.GetParameterProvider().InactiveCaptionColor;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color InactiveCaptionTextColor => FrameworkProvider.GetParameterProvider().InactiveCaptionTextColor;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color InfoColor => FrameworkProvider.GetParameterProvider().InfoColor;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color InfoTextColor => FrameworkProvider.GetParameterProvider().InfoTextColor;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color MenuColor => FrameworkProvider.GetParameterProvider().MenuColor;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color MenuBarColor => FrameworkProvider.GetParameterProvider().MenuBarColor;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color MenuHighlightColor => FrameworkProvider.GetParameterProvider().MenuHighlightColor;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color MenuTextColor => FrameworkProvider.GetParameterProvider().MenuTextColor;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color ScrollBarColor => FrameworkProvider.GetParameterProvider().ScrollBarColor;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color WindowColor => FrameworkProvider.GetParameterProvider().WindowColor;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color WindowFrameColor => FrameworkProvider.GetParameterProvider().WindowFrameColor;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static Color WindowTextColor => FrameworkProvider.GetParameterProvider().WindowTextColor;

        #endregion

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        private static SystemResourceKey CreateInstance(SystemResourceKeyID KeyId)
        {
            return new SystemResourceKey(KeyId);
        }

        #region Color Keys

        /// <summary>
        ///     ActiveBorderColor System Resource Key
        /// </summary>
        public static ResourceKey ActiveBorderColorKey
        {
            get
            {
                if (_CacheActiveBorderColor == null)
                {
                    _CacheActiveBorderColor = CreateInstance(SystemResourceKeyID.ActiveBorderColor);
                }

                return _CacheActiveBorderColor;
            }
        }

        /// <summary>
        ///     ActiveCaptionColor System Resource Key
        /// </summary>
        public static ResourceKey ActiveCaptionColorKey
        {
            get
            {
                if (_CacheActiveCaptionColor == null)
                {
                    _CacheActiveCaptionColor = CreateInstance(SystemResourceKeyID.ActiveCaptionColor);
                }

                return _CacheActiveCaptionColor;
            }
        }

        /// <summary>
        ///     ActiveCaptionTextColor System Resource Key
        /// </summary>
        public static ResourceKey ActiveCaptionTextColorKey
        {
            get
            {
                if (_CacheActiveCaptionTextColor == null)
                {
                    _CacheActiveCaptionTextColor = CreateInstance(SystemResourceKeyID.ActiveCaptionTextColor);
                }

                return _CacheActiveCaptionTextColor;
            }
        }

        /// <summary>
        ///     AppWorkspaceColor System Resource Key
        /// </summary>
        public static ResourceKey AppWorkspaceColorKey
        {
            get
            {
                if (_CacheAppWorkspaceColor == null)
                {
                    _CacheAppWorkspaceColor = CreateInstance(SystemResourceKeyID.AppWorkspaceColor);
                }

                return _CacheAppWorkspaceColor;
            }
        }

        /// <summary>
        ///     ControlColor System Resource Key
        /// </summary>
        public static ResourceKey ControlColorKey
        {
            get
            {
                if (_CacheControlColor == null)
                {
                    _CacheControlColor = CreateInstance(SystemResourceKeyID.ControlColor);
                }

                return _CacheControlColor;
            }
        }

        /// <summary>
        ///     ControlDarkColor System Resource Key
        /// </summary>
        public static ResourceKey ControlDarkColorKey
        {
            get
            {
                if (_CacheControlDarkColor == null)
                {
                    _CacheControlDarkColor = CreateInstance(SystemResourceKeyID.ControlDarkColor);
                }

                return _CacheControlDarkColor;
            }
        }

        /// <summary>
        ///     ControlDarkDarkColor System Resource Key
        /// </summary>
        public static ResourceKey ControlDarkDarkColorKey
        {
            get
            {
                if (_CacheControlDarkDarkColor == null)
                {
                    _CacheControlDarkDarkColor = CreateInstance(SystemResourceKeyID.ControlDarkDarkColor);
                }

                return _CacheControlDarkDarkColor;
            }
        }

        /// <summary>
        ///     ControlLightColor System Resource Key
        /// </summary>
        public static ResourceKey ControlLightColorKey
        {
            get
            {
                if (_CacheControlLightColor == null)
                {
                    _CacheControlLightColor = CreateInstance(SystemResourceKeyID.ControlLightColor);
                }

                return _CacheControlLightColor;
            }
        }

        /// <summary>
        ///     ControlLightLightColor System Resource Key
        /// </summary>
        public static ResourceKey ControlLightLightColorKey
        {
            get
            {
                if (_CacheControlLightLightColor == null)
                {
                    _CacheControlLightLightColor = CreateInstance(SystemResourceKeyID.ControlLightLightColor);
                }

                return _CacheControlLightLightColor;
            }
        }

        /// <summary>
        ///     ControlTextColor System Resource Key
        /// </summary>
        public static ResourceKey ControlTextColorKey
        {
            get
            {
                if (_CacheControlTextColor == null)
                {
                    _CacheControlTextColor = CreateInstance(SystemResourceKeyID.ControlTextColor);
                }

                return _CacheControlTextColor;
            }
        }

        /// <summary>
        ///     DesktopColor System Resource Key
        /// </summary>
        public static ResourceKey DesktopColorKey
        {
            get
            {
                if (_CacheDesktopColor == null)
                {
                    _CacheDesktopColor = CreateInstance(SystemResourceKeyID.DesktopColor);
                }

                return _CacheDesktopColor;
            }
        }

        /// <summary>
        ///     GradientActiveCaptionColor System Resource Key
        /// </summary>
        public static ResourceKey GradientActiveCaptionColorKey
        {
            get
            {
                if (_CacheGradientActiveCaptionColor == null)
                {
                    _CacheGradientActiveCaptionColor = CreateInstance(SystemResourceKeyID.GradientActiveCaptionColor);
                }

                return _CacheGradientActiveCaptionColor;
            }
        }

        /// <summary>
        ///     GradientInactiveCaptionColor System Resource Key
        /// </summary>
        public static ResourceKey GradientInactiveCaptionColorKey
        {
            get
            {
                if (_CacheGradientInactiveCaptionColor == null)
                {
                    _CacheGradientInactiveCaptionColor = CreateInstance(SystemResourceKeyID.GradientInactiveCaptionColor);
                }

                return _CacheGradientInactiveCaptionColor;
            }
        }

        /// <summary>
        ///     GrayTextColor System Resource Key
        /// </summary>
        public static ResourceKey GrayTextColorKey
        {
            get
            {
                if (_CacheGrayTextColor == null)
                {
                    _CacheGrayTextColor = CreateInstance(SystemResourceKeyID.GrayTextColor);
                }

                return _CacheGrayTextColor;
            }
        }

        /// <summary>
        ///     HighlightColor System Resource Key
        /// </summary>
        public static ResourceKey HighlightColorKey
        {
            get
            {
                if (_CacheHighlightColor == null)
                {
                    _CacheHighlightColor = CreateInstance(SystemResourceKeyID.HighlightColor);
                }

                return _CacheHighlightColor;
            }
        }

        /// <summary>
        ///     HighlightTextColor System Resource Key
        /// </summary>
        public static ResourceKey HighlightTextColorKey
        {
            get
            {
                if (_CacheHighlightTextColor == null)
                {
                    _CacheHighlightTextColor = CreateInstance(SystemResourceKeyID.HighlightTextColor);
                }

                return _CacheHighlightTextColor;
            }
        }

        /// <summary>
        ///     HotTrackColor System Resource Key
        /// </summary>
        public static ResourceKey HotTrackColorKey
        {
            get
            {
                if (_CacheHotTrackColor == null)
                {
                    _CacheHotTrackColor = CreateInstance(SystemResourceKeyID.HotTrackColor);
                }

                return _CacheHotTrackColor;
            }
        }

        /// <summary>
        ///     InactiveBorderColor System Resource Key
        /// </summary>
        public static ResourceKey InactiveBorderColorKey
        {
            get
            {
                if (_CacheInactiveBorderColor == null)
                {
                    _CacheInactiveBorderColor = CreateInstance(SystemResourceKeyID.InactiveBorderColor);
                }

                return _CacheInactiveBorderColor;
            }
        }

        /// <summary>
        ///     InactiveCaptionColor System Resource Key
        /// </summary>
        public static ResourceKey InactiveCaptionColorKey
        {
            get
            {
                if (_CacheInactiveCaptionColor == null)
                {
                    _CacheInactiveCaptionColor = CreateInstance(SystemResourceKeyID.InactiveCaptionColor);
                }

                return _CacheInactiveCaptionColor;
            }
        }

        /// <summary>
        ///     InactiveCaptionTextColor System Resource Key
        /// </summary>
        public static ResourceKey InactiveCaptionTextColorKey
        {
            get
            {
                if (_CacheInactiveCaptionTextColor == null)
                {
                    _CacheInactiveCaptionTextColor = CreateInstance(SystemResourceKeyID.InactiveCaptionTextColor);
                }

                return _CacheInactiveCaptionTextColor;
            }
        }

        /// <summary>
        ///     InfoColor System Resource Key
        /// </summary>
        public static ResourceKey InfoColorKey
        {
            get
            {
                if (_CacheInfoColor == null)
                {
                    _CacheInfoColor = CreateInstance(SystemResourceKeyID.InfoColor);
                }

                return _CacheInfoColor;
            }
        }

        /// <summary>
        ///     InfoTextColor System Resource Key
        /// </summary>
        public static ResourceKey InfoTextColorKey
        {
            get
            {
                if (_CacheInfoTextColor == null)
                {
                    _CacheInfoTextColor = CreateInstance(SystemResourceKeyID.InfoTextColor);
                }

                return _CacheInfoTextColor;
            }
        }

        /// <summary>
        ///     MenuColor System Resource Key
        /// </summary>
        public static ResourceKey MenuColorKey
        {
            get
            {
                if (_CacheMenuColor == null)
                {
                    _CacheMenuColor = CreateInstance(SystemResourceKeyID.MenuColor);
                }

                return _CacheMenuColor;
            }
        }

        /// <summary>
        ///     MenuBarColor System Resource Key
        /// </summary>
        public static ResourceKey MenuBarColorKey
        {
            get
            {
                if (_CacheMenuBarColor == null)
                {
                    _CacheMenuBarColor = CreateInstance(SystemResourceKeyID.MenuBarColor);
                }

                return _CacheMenuBarColor;
            }
        }

        /// <summary>
        ///     MenuHighlightColor System Resource Key
        /// </summary>
        public static ResourceKey MenuHighlightColorKey
        {
            get
            {
                if (_CacheMenuHighlightColor == null)
                {
                    _CacheMenuHighlightColor = CreateInstance(SystemResourceKeyID.MenuHighlightColor);
                }

                return _CacheMenuHighlightColor;
            }
        }

        /// <summary>
        ///     MenuTextColor System Resource Key
        /// </summary>
        public static ResourceKey MenuTextColorKey
        {
            get
            {
                if (_CacheMenuTextColor == null)
                {
                    _CacheMenuTextColor = CreateInstance(SystemResourceKeyID.MenuTextColor);
                }

                return _CacheMenuTextColor;
            }
        }

        /// <summary>
        ///     ScrollBarColor System Resource Key
        /// </summary>
        public static ResourceKey ScrollBarColorKey
        {
            get
            {
                if (_CacheScrollBarColor == null)
                {
                    _CacheScrollBarColor = CreateInstance(SystemResourceKeyID.ScrollBarColor);
                }

                return _CacheScrollBarColor;
            }
        }

        /// <summary>
        ///     WindowColor System Resource Key
        /// </summary>
        public static ResourceKey WindowColorKey
        {
            get
            {
                if (_CacheWindowColor == null)
                {
                    _CacheWindowColor = CreateInstance(SystemResourceKeyID.WindowColor);
                }

                return _CacheWindowColor;
            }
        }

        /// <summary>
        ///     WindowFrameColor System Resource Key
        /// </summary>
        public static ResourceKey WindowFrameColorKey
        {
            get
            {
                if (_CacheWindowFrameColor == null)
                {
                    _CacheWindowFrameColor = CreateInstance(SystemResourceKeyID.WindowFrameColor);
                }

                return _CacheWindowFrameColor;
            }
        }

        /// <summary>
        ///     WindowTextColor System Resource Key
        /// </summary>
        public static ResourceKey WindowTextColorKey
        {
            get
            {
                if (_CacheWindowTextColor == null)
                {
                    _CacheWindowTextColor = CreateInstance(SystemResourceKeyID.WindowTextColor);
                }

                return _CacheWindowTextColor;
            }
        }

        #endregion

        #region Brushes

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush ActiveBorderBrush => FrameworkProvider.GetParameterProvider().ActiveBorderBrush;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush ActiveCaptionBrush => FrameworkProvider.GetParameterProvider().ActiveCaptionBrush;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush ActiveCaptionTextBrush => FrameworkProvider.GetParameterProvider().ActiveCaptionTextBrush;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush AppWorkspaceBrush => FrameworkProvider.GetParameterProvider().AppWorkspaceBrush;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush ControlBrush => FrameworkProvider.GetParameterProvider().ControlBrush;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush ControlDarkBrush => FrameworkProvider.GetParameterProvider().ControlDarkBrush;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush ControlDarkDarkBrush => FrameworkProvider.GetParameterProvider().ControlDarkDarkBrush;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush ControlLightBrush => FrameworkProvider.GetParameterProvider().ControlLightBrush;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush ControlLightLightBrush => FrameworkProvider.GetParameterProvider().ControlLightLightBrush;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush ControlTextBrush => FrameworkProvider.GetParameterProvider().ControlTextBrush;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush DesktopBrush => FrameworkProvider.GetParameterProvider().DesktopBrush;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush GradientActiveCaptionBrush => FrameworkProvider.GetParameterProvider().GradientActiveCaptionBrush;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush GradientInactiveCaptionBrush => FrameworkProvider.GetParameterProvider().GradientInactiveCaptionBrush;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush GrayTextBrush => FrameworkProvider.GetParameterProvider().GrayTextBrush;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush HighlightBrush => FrameworkProvider.GetParameterProvider().HighlightBrush;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush HighlightTextBrush => FrameworkProvider.GetParameterProvider().HighlightTextBrush;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush HotTrackBrush => FrameworkProvider.GetParameterProvider().HotTrackBrush;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush InactiveBorderBrush => FrameworkProvider.GetParameterProvider().InactiveBorderBrush;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush InactiveCaptionBrush => FrameworkProvider.GetParameterProvider().InactiveCaptionBrush;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush InactiveCaptionTextBrush => FrameworkProvider.GetParameterProvider().InactiveCaptionTextBrush;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush InfoBrush => FrameworkProvider.GetParameterProvider().InfoBrush;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush InfoTextBrush => FrameworkProvider.GetParameterProvider().InfoTextBrush;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush MenuBrush => FrameworkProvider.GetParameterProvider().MenuBrush;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush MenuBarBrush => FrameworkProvider.GetParameterProvider().MenuBarBrush;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush MenuHighlightBrush => FrameworkProvider.GetParameterProvider().MenuHighlightBrush;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush MenuTextBrush => FrameworkProvider.GetParameterProvider().MenuTextBrush;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush ScrollBarBrush => FrameworkProvider.GetParameterProvider().ScrollBarBrush;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush WindowBrush => FrameworkProvider.GetParameterProvider().WindowBrush;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush WindowFrameBrush => FrameworkProvider.GetParameterProvider().WindowFrameBrush;

        /// <summary>
        ///     System color of the same name.
        /// </summary>
        public static SolidColorBrush WindowTextBrush => FrameworkProvider.GetParameterProvider().WindowTextBrush;

        /// <summary>
        ///     Inactive selection highlight brush.
        /// </summary>
        /// <remarks>
        ///     Please note that this property does not have an equivalent system color.
        /// </remarks>
        public static SolidColorBrush InactiveSelectionHighlightBrush => FrameworkProvider.GetParameterProvider().InactiveSelectionHighlightBrush;

        /// <summary>
        ///     Inactive selection highlight text brush.
        /// </summary>
        /// <remarks>
        ///     Please note that this property does not have an equivalent system color.
        /// </remarks>
        public static SolidColorBrush InactiveSelectionHighlightTextBrush => FrameworkProvider.GetParameterProvider().InactiveSelectionHighlightTextBrush;

        #endregion

        #region Brush Keys

        /// <summary>
        ///     ActiveBorderBrush System Resource Key
        /// </summary>
        public static ResourceKey ActiveBorderBrushKey
        {
            get
            {
                if (_CacheActiveBorderBrush == null)
                {
                    _CacheActiveBorderBrush = CreateInstance(SystemResourceKeyID.ActiveBorderBrush);
                }

                return _CacheActiveBorderBrush;
            }
        }

        /// <summary>
        ///     ActiveCaptionBrush System Resource Key
        /// </summary>
        public static ResourceKey ActiveCaptionBrushKey
        {
            get
            {
                if (_CacheActiveCaptionBrush == null)
                {
                    _CacheActiveCaptionBrush = CreateInstance(SystemResourceKeyID.ActiveCaptionBrush);
                }

                return _CacheActiveCaptionBrush;
            }
        }

        /// <summary>
        ///     ActiveCaptionTextBrush System Resource Key
        /// </summary>
        public static ResourceKey ActiveCaptionTextBrushKey
        {
            get
            {
                if (_CacheActiveCaptionTextBrush == null)
                {
                    _CacheActiveCaptionTextBrush = CreateInstance(SystemResourceKeyID.ActiveCaptionTextBrush);
                }

                return _CacheActiveCaptionTextBrush;
            }
        }

        /// <summary>
        ///     AppWorkspaceBrush System Resource Key
        /// </summary>
        public static ResourceKey AppWorkspaceBrushKey
        {
            get
            {
                if (_CacheAppWorkspaceBrush == null)
                {
                    _CacheAppWorkspaceBrush = CreateInstance(SystemResourceKeyID.AppWorkspaceBrush);
                }

                return _CacheAppWorkspaceBrush;
            }
        }

        /// <summary>
        ///     ControlBrush System Resource Key
        /// </summary>
        public static ResourceKey ControlBrushKey
        {
            get
            {
                if (_CacheControlBrush == null)
                {
                    _CacheControlBrush = CreateInstance(SystemResourceKeyID.ControlBrush);
                }

                return _CacheControlBrush;
            }
        }

        /// <summary>
        ///     ControlDarkBrush System Resource Key
        /// </summary>
        public static ResourceKey ControlDarkBrushKey
        {
            get
            {
                if (_CacheControlDarkBrush == null)
                {
                    _CacheControlDarkBrush = CreateInstance(SystemResourceKeyID.ControlDarkBrush);
                }

                return _CacheControlDarkBrush;
            }
        }

        /// <summary>
        ///     ControlDarkDarkBrush System Resource Key
        /// </summary>
        public static ResourceKey ControlDarkDarkBrushKey
        {
            get
            {
                if (_CacheControlDarkDarkBrush == null)
                {
                    _CacheControlDarkDarkBrush = CreateInstance(SystemResourceKeyID.ControlDarkDarkBrush);
                }

                return _CacheControlDarkDarkBrush;
            }
        }

        /// <summary>
        ///     ControlLightBrush System Resource Key
        /// </summary>
        public static ResourceKey ControlLightBrushKey
        {
            get
            {
                if (_CacheControlLightBrush == null)
                {
                    _CacheControlLightBrush = CreateInstance(SystemResourceKeyID.ControlLightBrush);
                }

                return _CacheControlLightBrush;
            }
        }

        /// <summary>
        ///     ControlLightLightBrush System Resource Key
        /// </summary>
        public static ResourceKey ControlLightLightBrushKey
        {
            get
            {
                if (_CacheControlLightLightBrush == null)
                {
                    _CacheControlLightLightBrush = CreateInstance(SystemResourceKeyID.ControlLightLightBrush);
                }

                return _CacheControlLightLightBrush;
            }
        }

        /// <summary>
        ///     ControlTextBrush System Resource Key
        /// </summary>
        public static ResourceKey ControlTextBrushKey
        {
            get
            {
                if (_CacheControlTextBrush == null)
                {
                    _CacheControlTextBrush = CreateInstance(SystemResourceKeyID.ControlTextBrush);
                }

                return _CacheControlTextBrush;
            }
        }

        /// <summary>
        ///     DesktopBrush System Resource Key
        /// </summary>
        public static ResourceKey DesktopBrushKey
        {
            get
            {
                if (_CacheDesktopBrush == null)
                {
                    _CacheDesktopBrush = CreateInstance(SystemResourceKeyID.DesktopBrush);
                }

                return _CacheDesktopBrush;
            }
        }

        /// <summary>
        ///     GradientActiveCaptionBrush System Resource Key
        /// </summary>
        public static ResourceKey GradientActiveCaptionBrushKey
        {
            get
            {
                if (_CacheGradientActiveCaptionBrush == null)
                {
                    _CacheGradientActiveCaptionBrush = CreateInstance(SystemResourceKeyID.GradientActiveCaptionBrush);
                }

                return _CacheGradientActiveCaptionBrush;
            }
        }

        /// <summary>
        ///     GradientInactiveCaptionBrush System Resource Key
        /// </summary>
        public static ResourceKey GradientInactiveCaptionBrushKey
        {
            get
            {
                if (_CacheGradientInactiveCaptionBrush == null)
                {
                    _CacheGradientInactiveCaptionBrush = CreateInstance(SystemResourceKeyID.GradientInactiveCaptionBrush);
                }

                return _CacheGradientInactiveCaptionBrush;
            }
        }

        /// <summary>
        ///     GrayTextBrush System Resource Key
        /// </summary>
        public static ResourceKey GrayTextBrushKey
        {
            get
            {
                if (_CacheGrayTextBrush == null)
                {
                    _CacheGrayTextBrush = CreateInstance(SystemResourceKeyID.GrayTextBrush);
                }

                return _CacheGrayTextBrush;
            }
        }

        /// <summary>
        ///     HighlightBrush System Resource Key
        /// </summary>
        public static ResourceKey HighlightBrushKey
        {
            get
            {
                if (_CacheHighlightBrush == null)
                {
                    _CacheHighlightBrush = CreateInstance(SystemResourceKeyID.HighlightBrush);
                }

                return _CacheHighlightBrush;
            }
        }

        /// <summary>
        ///     HighlightTextBrush System Resource Key
        /// </summary>
        public static ResourceKey HighlightTextBrushKey
        {
            get
            {
                if (_CacheHighlightTextBrush == null)
                {
                    _CacheHighlightTextBrush = CreateInstance(SystemResourceKeyID.HighlightTextBrush);
                }

                return _CacheHighlightTextBrush;
            }
        }

        /// <summary>
        ///     HotTrackBrush System Resource Key
        /// </summary>
        public static ResourceKey HotTrackBrushKey
        {
            get
            {
                if (_CacheHotTrackBrush == null)
                {
                    _CacheHotTrackBrush = CreateInstance(SystemResourceKeyID.HotTrackBrush);
                }

                return _CacheHotTrackBrush;
            }
        }

        /// <summary>
        ///     InactiveBorderBrush System Resource Key
        /// </summary>
        public static ResourceKey InactiveBorderBrushKey
        {
            get
            {
                if (_CacheInactiveBorderBrush == null)
                {
                    _CacheInactiveBorderBrush = CreateInstance(SystemResourceKeyID.InactiveBorderBrush);
                }

                return _CacheInactiveBorderBrush;
            }
        }

        /// <summary>
        ///     InactiveCaptionBrush System Resource Key
        /// </summary>
        public static ResourceKey InactiveCaptionBrushKey
        {
            get
            {
                if (_CacheInactiveCaptionBrush == null)
                {
                    _CacheInactiveCaptionBrush = CreateInstance(SystemResourceKeyID.InactiveCaptionBrush);
                }

                return _CacheInactiveCaptionBrush;
            }
        }

        /// <summary>
        ///     InactiveCaptionTextBrush System Resource Key
        /// </summary>
        public static ResourceKey InactiveCaptionTextBrushKey
        {
            get
            {
                if (_CacheInactiveCaptionTextBrush == null)
                {
                    _CacheInactiveCaptionTextBrush = CreateInstance(SystemResourceKeyID.InactiveCaptionTextBrush);
                }

                return _CacheInactiveCaptionTextBrush;
            }
        }

        /// <summary>
        ///     InfoBrush System Resource Key
        /// </summary>
        public static ResourceKey InfoBrushKey
        {
            get
            {
                if (_CacheInfoBrush == null)
                {
                    _CacheInfoBrush = CreateInstance(SystemResourceKeyID.InfoBrush);
                }

                return _CacheInfoBrush;
            }
        }

        /// <summary>
        ///     InfoTextBrush System Resource Key
        /// </summary>
        public static ResourceKey InfoTextBrushKey
        {
            get
            {
                if (_CacheInfoTextBrush == null)
                {
                    _CacheInfoTextBrush = CreateInstance(SystemResourceKeyID.InfoTextBrush);
                }

                return _CacheInfoTextBrush;
            }
        }

        /// <summary>
        ///     MenuBrush System Resource Key
        /// </summary>
        public static ResourceKey MenuBrushKey
        {
            get
            {
                if (_CacheMenuBrush == null)
                {
                    _CacheMenuBrush = CreateInstance(SystemResourceKeyID.MenuBrush);
                }

                return _CacheMenuBrush;
            }
        }

        /// <summary>
        ///     MenuBarBrush System Resource Key
        /// </summary>
        public static ResourceKey MenuBarBrushKey
        {
            get
            {
                if (_CacheMenuBarBrush == null)
                {
                    _CacheMenuBarBrush = CreateInstance(SystemResourceKeyID.MenuBarBrush);
                }

                return _CacheMenuBarBrush;
            }
        }

        /// <summary>
        ///     MenuHighlightBrush System Resource Key
        /// </summary>
        public static ResourceKey MenuHighlightBrushKey
        {
            get
            {
                if (_CacheMenuHighlightBrush == null)
                {
                    _CacheMenuHighlightBrush = CreateInstance(SystemResourceKeyID.MenuHighlightBrush);
                }

                return _CacheMenuHighlightBrush;
            }
        }

        /// <summary>
        ///     MenuTextBrush System Resource Key
        /// </summary>
        public static ResourceKey MenuTextBrushKey
        {
            get
            {
                if (_CacheMenuTextBrush == null)
                {
                    _CacheMenuTextBrush = CreateInstance(SystemResourceKeyID.MenuTextBrush);
                }

                return _CacheMenuTextBrush;
            }
        }

        /// <summary>
        ///     ScrollBarBrush System Resource Key
        /// </summary>
        public static ResourceKey ScrollBarBrushKey
        {
            get
            {
                if (_CacheScrollBarBrush == null)
                {
                    _CacheScrollBarBrush = CreateInstance(SystemResourceKeyID.ScrollBarBrush);
                }

                return _CacheScrollBarBrush;
            }
        }

        /// <summary>
        ///     WindowBrush System Resource Key
        /// </summary>
        public static ResourceKey WindowBrushKey
        {
            get
            {
                if (_CacheWindowBrush == null)
                {
                    _CacheWindowBrush = CreateInstance(SystemResourceKeyID.WindowBrush);
                }

                return _CacheWindowBrush;
            }
        }

        /// <summary>
        ///     WindowFrameBrush System Resource Key
        /// </summary>
        public static ResourceKey WindowFrameBrushKey
        {
            get
            {
                if (_CacheWindowFrameBrush == null)
                {
                    _CacheWindowFrameBrush = CreateInstance(SystemResourceKeyID.WindowFrameBrush);
                }

                return _CacheWindowFrameBrush;
            }
        }

        /// <summary>
        ///     WindowTextBrush System Resource Key
        /// </summary>
        public static ResourceKey WindowTextBrushKey
        {
            get
            {
                if (_CacheWindowTextBrush == null)
                {
                    _CacheWindowTextBrush = CreateInstance(SystemResourceKeyID.WindowTextBrush);
                }

                return _CacheWindowTextBrush;
            }
        }

        /// <summary>
        ///     InactiveSelectionHighlightBrush System Resource Key
        /// </summary>
        public static ResourceKey InactiveSelectionHighlightBrushKey
        {
            get
            {
                if (_CacheInactiveSelectionHighlightBrush == null)
                {
                    _CacheInactiveSelectionHighlightBrush = CreateInstance(SystemResourceKeyID.InactiveSelectionHighlightBrush);
                }
                return _CacheInactiveSelectionHighlightBrush;
            }
        }

        /// <summary>
        ///     InactiveSelectionHighlightTextBrush System Resource Key
        /// </summary>
        public static ResourceKey InactiveSelectionHighlightTextBrushKey
        {
            get
            {
                if (_CacheInactiveSelectionHighlightTextBrush == null)
                {
                    _CacheInactiveSelectionHighlightTextBrush = CreateInstance(SystemResourceKeyID.InactiveSelectionHighlightTextBrush);
                }
                return _CacheInactiveSelectionHighlightTextBrush;
            }
        }

        #endregion

        #region Implementation

        //internal static bool InvalidateCache()
        //{
        //    bool color = SystemResources.ClearBitArray(_ColorCacheValid);
        //    bool brush = SystemResources.ClearBitArray(_BrushCacheValid);
        //    return color || brush;
        //}

        //// Shift count and bit mask for A, R, G, B components
        //private const int _AlphaShift = 24;
        //private const int _RedShift = 16;
        //private const int _GreenShift = 8;
        //private const int _BlueShift = 0;

        //private const int _Win32RedShift = 0;
        //private const int _Win32GreenShift = 8;
        //private const int _Win32BlueShift = 16;

        //private static int Encode(int alpha, int red, int green, int blue)
        //{
        //    return red << _RedShift | green << _GreenShift | blue << _BlueShift | alpha << _AlphaShift;
        //}

        //private static int FromWin32Value(int value)
        //{
        //    return Encode(255,
        //        (value >> _Win32RedShift) & 0xFF,
        //        (value >> _Win32GreenShift) & 0xFF,
        //        (value >> _Win32BlueShift) & 0xFF);
        //}

        ///// <summary>
        /////     Query for system colors.
        ///// </summary>
        ///// <param name="slot">The color slot.</param>
        ///// <returns>The system color.</returns>
        //private static Color GetSystemColor(CacheSlot slot)
        //{
        //    Color color;

        //    lock (_ColorCacheValid)
        //    {
        //        // the loop protects against a race condition - see SystemParameters
        //        while (!_ColorCacheValid[(int)slot])
        //        {
        //            _ColorCacheValid[(int)slot] = true;

        //            uint argb;
        //            int sysColor = SafeNativeMethods.GetSysColor(SlotToFlag(slot));

        //            argb = (uint)FromWin32Value(sysColor);
        //            color = Color.FromArgb((byte)((argb & 0xff000000) >> 24), (byte)((argb & 0x00ff0000) >> 16), (byte)((argb & 0x0000ff00) >> 8), (byte)(argb & 0x000000ff));

        //            _ColorCache[(int)slot] = color;
        //        }

        //        color = _ColorCache[(int)slot];
        //    }

        //    return color;
        //}

        //private static SolidColorBrush MakeBrush(CacheSlot slot)
        //{
        //    SolidColorBrush brush;

        //    lock (_BrushCacheValid)
        //    {
        //        // the loop protects against a race condition - see SystemParameters
        //        while (!_BrushCacheValid[(int)slot])
        //        {
        //            _BrushCacheValid[(int)slot] = true;

        //            brush = new SolidColorBrush(GetSystemColor(slot));
        //            brush.Freeze();

        //            _BrushCache[(int)slot] = brush;
        //        }

        //        brush = _BrushCache[(int)slot];
        //    }

        //    return brush;
        //}

        //private static int SlotToFlag(CacheSlot slot)
        //{
        //    // FxCop: Hashtable would be overkill, using switch instead

        //    switch (slot)
        //    {
        //        case CacheSlot.ActiveBorder:
        //            return (int)NativeMethods.Win32SystemColors.ActiveBorder;
        //        case CacheSlot.ActiveCaption:
        //            return (int)NativeMethods.Win32SystemColors.ActiveCaption;
        //        case CacheSlot.ActiveCaptionText:
        //            return (int)NativeMethods.Win32SystemColors.ActiveCaptionText;
        //        case CacheSlot.AppWorkspace:
        //            return (int)NativeMethods.Win32SystemColors.AppWorkspace;
        //        case CacheSlot.Control:
        //            return (int)NativeMethods.Win32SystemColors.Control;
        //        case CacheSlot.ControlDark:
        //            return (int)NativeMethods.Win32SystemColors.ControlDark;
        //        case CacheSlot.ControlDarkDark:
        //            return (int)NativeMethods.Win32SystemColors.ControlDarkDark;
        //        case CacheSlot.ControlLight:
        //            return (int)NativeMethods.Win32SystemColors.ControlLight;
        //        case CacheSlot.ControlLightLight:
        //            return (int)NativeMethods.Win32SystemColors.ControlLightLight;
        //        case CacheSlot.ControlText:
        //            return (int)NativeMethods.Win32SystemColors.ControlText;
        //        case CacheSlot.Desktop:
        //            return (int)NativeMethods.Win32SystemColors.Desktop;
        //        case CacheSlot.GradientActiveCaption:
        //            return (int)NativeMethods.Win32SystemColors.GradientActiveCaption;
        //        case CacheSlot.GradientInactiveCaption:
        //            return (int)NativeMethods.Win32SystemColors.GradientInactiveCaption;
        //        case CacheSlot.GrayText:
        //            return (int)NativeMethods.Win32SystemColors.GrayText;
        //        case CacheSlot.Highlight:
        //            return (int)NativeMethods.Win32SystemColors.Highlight;
        //        case CacheSlot.HighlightText:
        //            return (int)NativeMethods.Win32SystemColors.HighlightText;
        //        case CacheSlot.HotTrack:
        //            return (int)NativeMethods.Win32SystemColors.HotTrack;
        //        case CacheSlot.InactiveBorder:
        //            return (int)NativeMethods.Win32SystemColors.InactiveBorder;
        //        case CacheSlot.InactiveCaption:
        //            return (int)NativeMethods.Win32SystemColors.InactiveCaption;
        //        case CacheSlot.InactiveCaptionText:
        //            return (int)NativeMethods.Win32SystemColors.InactiveCaptionText;
        //        case CacheSlot.Info:
        //            return (int)NativeMethods.Win32SystemColors.Info;
        //        case CacheSlot.InfoText:
        //            return (int)NativeMethods.Win32SystemColors.InfoText;
        //        case CacheSlot.Menu:
        //            return (int)NativeMethods.Win32SystemColors.Menu;
        //        case CacheSlot.MenuBar:
        //            return (int)NativeMethods.Win32SystemColors.MenuBar;
        //        case CacheSlot.MenuHighlight:
        //            return (int)NativeMethods.Win32SystemColors.MenuHighlight;
        //        case CacheSlot.MenuText:
        //            return (int)NativeMethods.Win32SystemColors.MenuText;
        //        case CacheSlot.ScrollBar:
        //            return (int)NativeMethods.Win32SystemColors.ScrollBar;
        //        case CacheSlot.Window:
        //            return (int)NativeMethods.Win32SystemColors.Window;
        //        case CacheSlot.WindowFrame:
        //            return (int)NativeMethods.Win32SystemColors.WindowFrame;
        //        case CacheSlot.WindowText:
        //            return (int)NativeMethods.Win32SystemColors.WindowText;
        //    }

        //    return 0;
        //}

        //private enum CacheSlot : int
        //{
        //    ActiveBorder,
        //    ActiveCaption,
        //    ActiveCaptionText,
        //    AppWorkspace,
        //    Control,
        //    ControlDark,
        //    ControlDarkDark,
        //    ControlLight,
        //    ControlLightLight,
        //    ControlText,
        //    Desktop,
        //    GradientActiveCaption,
        //    GradientInactiveCaption,
        //    GrayText,
        //    Highlight,
        //    HighlightText,
        //    HotTrack,
        //    InactiveBorder,
        //    InactiveCaption,
        //    InactiveCaptionText,
        //    Info,
        //    InfoText,
        //    Menu,
        //    MenuBar,
        //    MenuHighlight,
        //    MenuText,
        //    ScrollBar,
        //    Window,
        //    WindowFrame,
        //    WindowText,

        //    NumSlots
        //}

        //private static BitArray _ColorCacheValid = new BitArray((int)CacheSlot.NumSlots);
        //private static Color[] _ColorCache = new Color[(int)CacheSlot.NumSlots];
        //private static BitArray _BrushCacheValid = new BitArray((int)CacheSlot.NumSlots);
        //private static SolidColorBrush[] _BrushCache = new SolidColorBrush[(int)CacheSlot.NumSlots];

        private static SystemResourceKey? _CacheActiveBorderBrush;
        private static SystemResourceKey? _CacheActiveCaptionBrush;
        private static SystemResourceKey? _CacheActiveCaptionTextBrush;
        private static SystemResourceKey? _CacheAppWorkspaceBrush;
        private static SystemResourceKey? _CacheControlBrush;
        private static SystemResourceKey? _CacheControlDarkBrush;
        private static SystemResourceKey? _CacheControlDarkDarkBrush;
        private static SystemResourceKey? _CacheControlLightBrush;
        private static SystemResourceKey? _CacheControlLightLightBrush;
        private static SystemResourceKey? _CacheControlTextBrush;
        private static SystemResourceKey? _CacheDesktopBrush;
        private static SystemResourceKey? _CacheGradientActiveCaptionBrush;
        private static SystemResourceKey? _CacheGradientInactiveCaptionBrush;
        private static SystemResourceKey? _CacheGrayTextBrush;
        private static SystemResourceKey? _CacheHighlightBrush;
        private static SystemResourceKey? _CacheHighlightTextBrush;
        private static SystemResourceKey? _CacheHotTrackBrush;
        private static SystemResourceKey? _CacheInactiveBorderBrush;
        private static SystemResourceKey? _CacheInactiveCaptionBrush;
        private static SystemResourceKey? _CacheInactiveCaptionTextBrush;
        private static SystemResourceKey? _CacheInfoBrush;
        private static SystemResourceKey? _CacheInfoTextBrush;
        private static SystemResourceKey? _CacheMenuBrush;
        private static SystemResourceKey? _CacheMenuBarBrush;
        private static SystemResourceKey? _CacheMenuHighlightBrush;
        private static SystemResourceKey? _CacheMenuTextBrush;
        private static SystemResourceKey? _CacheScrollBarBrush;
        private static SystemResourceKey? _CacheWindowBrush;
        private static SystemResourceKey? _CacheWindowFrameBrush;
        private static SystemResourceKey? _CacheWindowTextBrush;
        private static SystemResourceKey? _CacheInactiveSelectionHighlightBrush;
        private static SystemResourceKey? _CacheInactiveSelectionHighlightTextBrush;
        private static SystemResourceKey? _CacheActiveBorderColor;
        private static SystemResourceKey? _CacheActiveCaptionColor;
        private static SystemResourceKey? _CacheActiveCaptionTextColor;
        private static SystemResourceKey? _CacheAppWorkspaceColor;
        private static SystemResourceKey? _CacheControlColor;
        private static SystemResourceKey? _CacheControlDarkColor;
        private static SystemResourceKey? _CacheControlDarkDarkColor;
        private static SystemResourceKey? _CacheControlLightColor;
        private static SystemResourceKey? _CacheControlLightLightColor;
        private static SystemResourceKey? _CacheControlTextColor;
        private static SystemResourceKey? _CacheDesktopColor;
        private static SystemResourceKey? _CacheGradientActiveCaptionColor;
        private static SystemResourceKey? _CacheGradientInactiveCaptionColor;
        private static SystemResourceKey? _CacheGrayTextColor;
        private static SystemResourceKey? _CacheHighlightColor;
        private static SystemResourceKey? _CacheHighlightTextColor;
        private static SystemResourceKey? _CacheHotTrackColor;
        private static SystemResourceKey? _CacheInactiveBorderColor;
        private static SystemResourceKey? _CacheInactiveCaptionColor;
        private static SystemResourceKey? _CacheInactiveCaptionTextColor;
        private static SystemResourceKey? _CacheInfoColor;
        private static SystemResourceKey? _CacheInfoTextColor;
        private static SystemResourceKey? _CacheMenuColor;
        private static SystemResourceKey? _CacheMenuBarColor;
        private static SystemResourceKey? _CacheMenuHighlightColor;
        private static SystemResourceKey? _CacheMenuTextColor;
        private static SystemResourceKey? _CacheScrollBarColor;
        private static SystemResourceKey? _CacheWindowColor;
        private static SystemResourceKey? _CacheWindowFrameColor;
        private static SystemResourceKey? _CacheWindowTextColor;

        #endregion
    }
}
