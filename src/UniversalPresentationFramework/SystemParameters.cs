using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Controls;

namespace Wodsoft.UI
{
    public static class SystemParameters
    {
        private static SystemResourceKey? _ThinHorizontalBorderHeight;
        private static SystemResourceKey? _ThinVerticalBorderWidth;
        private static SystemResourceKey? _CursorWidth;
        private static SystemResourceKey? _CursorHeight;
        private static SystemResourceKey? _ThickHorizontalBorderHeight;
        private static SystemResourceKey? _ThickVerticalBorderWidth;
        private static SystemResourceKey? _FixedFrameHorizontalBorderHeight;
        private static SystemResourceKey? _FixedFrameVerticalBorderWidth;
        private static SystemResourceKey? _FocusHorizontalBorderHeight;
        private static SystemResourceKey? _FocusVerticalBorderWidth;
        private static SystemResourceKey? _FullPrimaryScreenWidth;
        private static SystemResourceKey? _FullPrimaryScreenHeight;
        private static SystemResourceKey? _HorizontalScrollBarButtonWidth;
        private static SystemResourceKey? _HorizontalScrollBarHeight;
        private static SystemResourceKey? _HorizontalScrollBarThumbWidth;
        private static SystemResourceKey? _IconWidth;
        private static SystemResourceKey? _IconHeight;
        private static SystemResourceKey? _IconGridWidth;
        private static SystemResourceKey? _IconGridHeight;
        private static SystemResourceKey? _MaximizedPrimaryScreenWidth;
        private static SystemResourceKey? _MaximizedPrimaryScreenHeight;
        private static SystemResourceKey? _MaximumWindowTrackWidth;
        private static SystemResourceKey? _MaximumWindowTrackHeight;
        private static SystemResourceKey? _MenuCheckmarkWidth;
        private static SystemResourceKey? _MenuCheckmarkHeight;
        private static SystemResourceKey? _MenuButtonWidth;
        private static SystemResourceKey? _MenuButtonHeight;
        private static SystemResourceKey? _MinimumWindowWidth;
        private static SystemResourceKey? _MinimumWindowHeight;
        private static SystemResourceKey? _MinimizedWindowWidth;
        private static SystemResourceKey? _MinimizedWindowHeight;
        private static SystemResourceKey? _MinimizedGridWidth;
        private static SystemResourceKey? _MinimizedGridHeight;
        private static SystemResourceKey? _MinimumWindowTrackWidth;
        private static SystemResourceKey? _MinimumWindowTrackHeight;
        private static SystemResourceKey? _PrimaryScreenWidth;
        private static SystemResourceKey? _PrimaryScreenHeight;
        private static SystemResourceKey? _WindowCaptionButtonWidth;
        private static SystemResourceKey? _WindowCaptionButtonHeight;
        private static SystemResourceKey? _ResizeFrameHorizontalBorderHeight;
        private static SystemResourceKey? _ResizeFrameVerticalBorderWidth;
        private static SystemResourceKey? _SmallIconWidth;
        private static SystemResourceKey? _SmallIconHeight;
        private static SystemResourceKey? _SmallWindowCaptionButtonWidth;
        private static SystemResourceKey? _SmallWindowCaptionButtonHeight;
        private static SystemResourceKey? _VirtualScreenWidth;
        private static SystemResourceKey? _VirtualScreenHeight;
        private static SystemResourceKey? _VerticalScrollBarWidth;
        private static SystemResourceKey? _VerticalScrollBarButtonHeight;
        private static SystemResourceKey? _WindowCaptionHeight;
        private static SystemResourceKey? _KanjiWindowHeight;
        private static SystemResourceKey? _MenuBarHeight;
        private static SystemResourceKey? _SmallCaptionHeight;
        private static SystemResourceKey? _VerticalScrollBarThumbHeight;
        private static SystemResourceKey? _IsImmEnabled;
        private static SystemResourceKey? _IsMediaCenter;
        private static SystemResourceKey? _IsMenuDropRightAligned;
        private static SystemResourceKey? _IsMiddleEastEnabled;
        private static SystemResourceKey? _IsMousePresent;
        private static SystemResourceKey? _IsMouseWheelPresent;
        private static SystemResourceKey? _IsPenWindows;
        private static SystemResourceKey? _IsRemotelyControlled;
        private static SystemResourceKey? _IsRemoteSession;
        private static SystemResourceKey? _ShowSounds;
        private static SystemResourceKey? _IsSlowMachine;
        private static SystemResourceKey? _SwapButtons;
        private static SystemResourceKey? _IsTabletPC;
        private static SystemResourceKey? _VirtualScreenLeft;
        private static SystemResourceKey? _VirtualScreenTop;
        private static SystemResourceKey? _FocusBorderWidth;
        private static SystemResourceKey? _FocusBorderHeight;
        private static SystemResourceKey? _HighContrast;
        private static SystemResourceKey? _DropShadow;
        private static SystemResourceKey? _FlatMenu;
        private static SystemResourceKey? _WorkArea;
        private static SystemResourceKey? _IconHorizontalSpacing;
        private static SystemResourceKey? _IconVerticalSpacing;
        private static SystemResourceKey? _IconTitleWrap;
        private static SystemResourceKey? _KeyboardCues;
        private static SystemResourceKey? _KeyboardDelay;
        private static SystemResourceKey? _KeyboardPreference;
        private static SystemResourceKey? _KeyboardSpeed;
        private static SystemResourceKey? _SnapToDefaultButton;
        private static SystemResourceKey? _WheelScrollLines;
        private static SystemResourceKey? _MouseHoverTime;
        private static SystemResourceKey? _MouseHoverHeight;
        private static SystemResourceKey? _MouseHoverWidth;
        private static SystemResourceKey? _MenuDropAlignment;
        private static SystemResourceKey? _MenuFade;
        private static SystemResourceKey? _MenuShowDelay;
        private static SystemResourceKey? _ComboBoxAnimation;
        private static SystemResourceKey? _ClientAreaAnimation;
        private static SystemResourceKey? _CursorShadow;
        private static SystemResourceKey? _GradientCaptions;
        private static SystemResourceKey? _HotTracking;
        private static SystemResourceKey? _ListBoxSmoothScrolling;
        private static SystemResourceKey? _MenuAnimation;
        private static SystemResourceKey? _SelectionFade;
        private static SystemResourceKey? _StylusHotTracking;
        private static SystemResourceKey? _ToolTipAnimation;
        private static SystemResourceKey? _ToolTipFade;
        private static SystemResourceKey? _UIEffects;
        private static SystemResourceKey? _MinimizeAnimation;
        private static SystemResourceKey? _Border;
        private static SystemResourceKey? _CaretWidth;
        private static SystemResourceKey? _ForegroundFlashCount;
        private static SystemResourceKey? _DragFullWindows;
        private static SystemResourceKey? _BorderWidth;
        private static SystemResourceKey? _ScrollWidth;
        private static SystemResourceKey? _ScrollHeight;
        private static SystemResourceKey? _CaptionWidth;
        private static SystemResourceKey? _CaptionHeight;
        private static SystemResourceKey? _SmallCaptionWidth;
        private static SystemResourceKey? _MenuWidth;
        private static SystemResourceKey? _MenuHeight;
        private static SystemResourceKey? _ComboBoxPopupAnimation;
        private static SystemResourceKey? _MenuPopupAnimation;
        private static SystemResourceKey? _ToolTipPopupAnimation;
        private static SystemResourceKey? _PowerLineStatus;

        //Visual Studio batch replace
        //public static (\w+) (\w+)\r\n +{\r\n +get\r\n +{\r\n +throw new NotImplementedException\(\);\r\n +}\r\n +}
        //public static $1 $2 => FrameworkProvider.GetParameterProvider().$2;


        #region Accessibility Parameters

        /// <summary>
        ///     Maps to SPI_GETFOCUSBORDERWIDTH
        /// </summary>
        public static float FocusBorderWidth => FrameworkProvider.GetParameterProvider().FocusBorderWidth;

        /// <summary>
        ///     Maps to SPI_GETFOCUSBORDERHEIGHT
        /// </summary>
        public static float FocusBorderHeight => FrameworkProvider.GetParameterProvider().FocusBorderHeight;

        /// <summary>
        ///     Maps to SPI_GETHIGHCONTRAST -> HCF_HIGHCONTRASTON
        /// </summary>
        public static bool HighContrast => FrameworkProvider.GetParameterProvider().HighContrast;

        #endregion

        #region Accessibility Keys

        /// <summary>
        ///     FocusBorderWidth System Resource Key
        /// </summary>
        public static ResourceKey FocusBorderWidthKey
        {
            get
            {
                if (_FocusBorderWidth == null)
                {
                    _FocusBorderWidth = CreateInstance(SystemResourceKeyID.FocusBorderWidth);
                }

                return _FocusBorderWidth;
            }
        }

        /// <summary>
        ///     FocusBorderHeight System Resource Key
        /// </summary>
        public static ResourceKey FocusBorderHeightKey
        {
            get
            {
                if (_FocusBorderHeight == null)
                {
                    _FocusBorderHeight = CreateInstance(SystemResourceKeyID.FocusBorderHeight);
                }

                return _FocusBorderHeight;
            }
        }

        /// <summary>
        ///     HighContrast System Resource Key
        /// </summary>
        public static ResourceKey HighContrastKey
        {
            get
            {
                if (_HighContrast == null)
                {
                    _HighContrast = CreateInstance(SystemResourceKeyID.HighContrast);
                }

                return _HighContrast;
            }
        }

        #endregion

        #region Desktop Keys

        /// <summary>
        ///     DropShadow System Resource Key
        /// </summary>
        public static ResourceKey DropShadowKey
        {
            get
            {
                if (_DropShadow == null)
                {
                    _DropShadow = CreateInstance(SystemResourceKeyID.DropShadow);
                }

                return _DropShadow;
            }
        }

        /// <summary>
        ///     FlatMenu System Resource Key
        /// </summary>
        public static ResourceKey FlatMenuKey
        {
            get
            {
                if (_FlatMenu == null)
                {
                    _FlatMenu = CreateInstance(SystemResourceKeyID.FlatMenu);
                }

                return _FlatMenu;
            }
        }

        /// <summary>
        ///     WorkArea System Resource Key
        /// </summary>
        public static ResourceKey WorkAreaKey
        {
            get
            {
                if (_WorkArea == null)
                {
                    _WorkArea = CreateInstance(SystemResourceKeyID.WorkArea);
                }

                return _WorkArea;
            }
        }

        #endregion

        #region Icon Keys

        /// <summary>
        ///     IconHorizontalSpacing System Resource Key
        /// </summary>
        public static ResourceKey IconHorizontalSpacingKey
        {
            get
            {
                if (_IconHorizontalSpacing == null)
                {
                    _IconHorizontalSpacing = CreateInstance(SystemResourceKeyID.IconHorizontalSpacing);
                }

                return _IconHorizontalSpacing;
            }
        }

        /// <summary>
        ///     IconVerticalSpacing System Resource Key
        /// </summary>
        public static ResourceKey IconVerticalSpacingKey
        {
            get
            {
                if (_IconVerticalSpacing == null)
                {
                    _IconVerticalSpacing = CreateInstance(SystemResourceKeyID.IconVerticalSpacing);
                }

                return _IconVerticalSpacing;
            }
        }

        /// <summary>
        ///     IconTitleWrap System Resource Key
        /// </summary>
        public static ResourceKey IconTitleWrapKey
        {
            get
            {
                if (_IconTitleWrap == null)
                {
                    _IconTitleWrap = CreateInstance(SystemResourceKeyID.IconTitleWrap);
                }

                return _IconTitleWrap;
            }
        }

        #endregion

        #region Input Keys

        /// <summary>
        ///     KeyboardCues System Resource Key
        /// </summary>
        public static ResourceKey KeyboardCuesKey
        {
            get
            {
                if (_KeyboardCues == null)
                {
                    _KeyboardCues = CreateInstance(SystemResourceKeyID.KeyboardCues);
                }

                return _KeyboardCues;
            }
        }

        /// <summary>
        ///     KeyboardDelay System Resource Key
        /// </summary>
        public static ResourceKey KeyboardDelayKey
        {
            get
            {
                if (_KeyboardDelay == null)
                {
                    _KeyboardDelay = CreateInstance(SystemResourceKeyID.KeyboardDelay);
                }

                return _KeyboardDelay;
            }
        }

        /// <summary>
        ///     KeyboardPreference System Resource Key
        /// </summary>
        public static ResourceKey KeyboardPreferenceKey
        {
            get
            {
                if (_KeyboardPreference == null)
                {
                    _KeyboardPreference = CreateInstance(SystemResourceKeyID.KeyboardPreference);
                }

                return _KeyboardPreference;
            }
        }

        /// <summary>
        ///     KeyboardSpeed System Resource Key
        /// </summary>
        public static ResourceKey KeyboardSpeedKey
        {
            get
            {
                if (_KeyboardSpeed == null)
                {
                    _KeyboardSpeed = CreateInstance(SystemResourceKeyID.KeyboardSpeed);
                }

                return _KeyboardSpeed;
            }
        }

        /// <summary>
        ///     SnapToDefaultButton System Resource Key
        /// </summary>
        public static ResourceKey SnapToDefaultButtonKey
        {
            get
            {
                if (_SnapToDefaultButton == null)
                {
                    _SnapToDefaultButton = CreateInstance(SystemResourceKeyID.SnapToDefaultButton);
                }

                return _SnapToDefaultButton;
            }
        }

        /// <summary>
        ///     WheelScrollLines System Resource Key
        /// </summary>
        public static ResourceKey WheelScrollLinesKey
        {
            get
            {
                if (_WheelScrollLines == null)
                {
                    _WheelScrollLines = CreateInstance(SystemResourceKeyID.WheelScrollLines);
                }

                return _WheelScrollLines;
            }
        }

        /// <summary>
        ///     MouseHoverTime System Resource Key
        /// </summary>
        public static ResourceKey MouseHoverTimeKey
        {
            get
            {
                if (_MouseHoverTime == null)
                {
                    _MouseHoverTime = CreateInstance(SystemResourceKeyID.MouseHoverTime);
                }

                return _MouseHoverTime;
            }
        }

        /// <summary>
        ///     MouseHoverHeight System Resource Key
        /// </summary>
        public static ResourceKey MouseHoverHeightKey
        {
            get
            {
                if (_MouseHoverHeight == null)
                {
                    _MouseHoverHeight = CreateInstance(SystemResourceKeyID.MouseHoverHeight);
                }

                return _MouseHoverHeight;
            }
        }

        /// <summary>
        ///     MouseHoverWidth System Resource Key
        /// </summary>
        public static ResourceKey MouseHoverWidthKey
        {
            get
            {
                if (_MouseHoverWidth == null)
                {
                    _MouseHoverWidth = CreateInstance(SystemResourceKeyID.MouseHoverWidth);
                }

                return _MouseHoverWidth;
            }
        }

        #endregion

        #region Menu Keys

        /// <summary>
        ///     MenuDropAlignment System Resource Key
        /// </summary>
        public static ResourceKey MenuDropAlignmentKey
        {
            get
            {
                if (_MenuDropAlignment == null)
                {
                    _MenuDropAlignment = CreateInstance(SystemResourceKeyID.MenuDropAlignment);
                }

                return _MenuDropAlignment;
            }
        }

        /// <summary>
        ///     MenuFade System Resource Key
        /// </summary>
        public static ResourceKey MenuFadeKey
        {
            get
            {
                if (_MenuFade == null)
                {
                    _MenuFade = CreateInstance(SystemResourceKeyID.MenuFade);
                }

                return _MenuFade;
            }
        }

        /// <summary>
        ///     MenuShowDelay System Resource Key
        /// </summary>
        public static ResourceKey MenuShowDelayKey
        {
            get
            {
                if (_MenuShowDelay == null)
                {
                    _MenuShowDelay = CreateInstance(SystemResourceKeyID.MenuShowDelay);
                }

                return _MenuShowDelay;
            }
        }

        #endregion

        #region UI Effects Keys

        /// <summary>
        ///     ComboBoxAnimation System Resource Key
        /// </summary>
        public static ResourceKey ComboBoxAnimationKey
        {
            get
            {
                if (_ComboBoxAnimation == null)
                {
                    _ComboBoxAnimation = CreateInstance(SystemResourceKeyID.ComboBoxAnimation);
                }

                return _ComboBoxAnimation;
            }
        }

        /// <summary>
        ///     ClientAreaAnimation System Resource Key
        /// </summary>
        public static ResourceKey ClientAreaAnimationKey
        {
            get
            {
                if (_ClientAreaAnimation == null)
                {
                    _ClientAreaAnimation = CreateInstance(SystemResourceKeyID.ClientAreaAnimation);
                }

                return _ClientAreaAnimation;
            }
        }

        /// <summary>
        ///     CursorShadow System Resource Key
        /// </summary>
        public static ResourceKey CursorShadowKey
        {
            get
            {
                if (_CursorShadow == null)
                {
                    _CursorShadow = CreateInstance(SystemResourceKeyID.CursorShadow);
                }

                return _CursorShadow;
            }
        }

        /// <summary>
        ///     GradientCaptions System Resource Key
        /// </summary>
        public static ResourceKey GradientCaptionsKey
        {
            get
            {
                if (_GradientCaptions == null)
                {
                    _GradientCaptions = CreateInstance(SystemResourceKeyID.GradientCaptions);
                }

                return _GradientCaptions;
            }
        }

        /// <summary>
        ///     HotTracking System Resource Key
        /// </summary>
        public static ResourceKey HotTrackingKey
        {
            get
            {
                if (_HotTracking == null)
                {
                    _HotTracking = CreateInstance(SystemResourceKeyID.HotTracking);
                }

                return _HotTracking;
            }
        }

        /// <summary>
        ///     ListBoxSmoothScrolling System Resource Key
        /// </summary>
        public static ResourceKey ListBoxSmoothScrollingKey
        {
            get
            {
                if (_ListBoxSmoothScrolling == null)
                {
                    _ListBoxSmoothScrolling = CreateInstance(SystemResourceKeyID.ListBoxSmoothScrolling);
                }

                return _ListBoxSmoothScrolling;
            }
        }

        /// <summary>
        ///     MenuAnimation System Resource Key
        /// </summary>
        public static ResourceKey MenuAnimationKey
        {
            get
            {
                if (_MenuAnimation == null)
                {
                    _MenuAnimation = CreateInstance(SystemResourceKeyID.MenuAnimation);
                }

                return _MenuAnimation;
            }
        }

        /// <summary>
        ///     SelectionFade System Resource Key
        /// </summary>
        public static ResourceKey SelectionFadeKey
        {
            get
            {
                if (_SelectionFade == null)
                {
                    _SelectionFade = CreateInstance(SystemResourceKeyID.SelectionFade);
                }

                return _SelectionFade;
            }
        }

        /// <summary>
        ///     StylusHotTracking System Resource Key
        /// </summary>
        public static ResourceKey StylusHotTrackingKey
        {
            get
            {
                if (_StylusHotTracking == null)
                {
                    _StylusHotTracking = CreateInstance(SystemResourceKeyID.StylusHotTracking);
                }

                return _StylusHotTracking;
            }
        }

        /// <summary>
        ///     ToolTipAnimation System Resource Key
        /// </summary>
        public static ResourceKey ToolTipAnimationKey
        {
            get
            {
                if (_ToolTipAnimation == null)
                {
                    _ToolTipAnimation = CreateInstance(SystemResourceKeyID.ToolTipAnimation);
                }

                return _ToolTipAnimation;
            }
        }

        /// <summary>
        ///     ToolTipFade System Resource Key
        /// </summary>
        public static ResourceKey ToolTipFadeKey
        {
            get
            {
                if (_ToolTipFade == null)
                {
                    _ToolTipFade = CreateInstance(SystemResourceKeyID.ToolTipFade);
                }

                return _ToolTipFade;
            }
        }

        /// <summary>
        ///     UIEffects System Resource Key
        /// </summary>
        public static ResourceKey UIEffectsKey
        {
            get
            {
                if (_UIEffects == null)
                {
                    _UIEffects = CreateInstance(SystemResourceKeyID.UIEffects);
                }

                return _UIEffects;
            }
        }

        /// <summary>
        ///     ComboBoxPopupAnimation System Resource Key
        /// </summary>
        public static ResourceKey ComboBoxPopupAnimationKey
        {
            get
            {
                if (_ComboBoxPopupAnimation == null)
                {
                    _ComboBoxPopupAnimation = CreateInstance(SystemResourceKeyID.ComboBoxPopupAnimation);
                }

                return _ComboBoxPopupAnimation;
            }
        }

        /// <summary>
        ///     MenuPopupAnimation System Resource Key
        /// </summary>
        public static ResourceKey MenuPopupAnimationKey
        {
            get
            {
                if (_MenuPopupAnimation == null)
                {
                    _MenuPopupAnimation = CreateInstance(SystemResourceKeyID.MenuPopupAnimation);
                }

                return _MenuPopupAnimation;
            }
        }

        /// <summary>
        ///     ToolTipPopupAnimation System Resource Key
        /// </summary>
        public static ResourceKey ToolTipPopupAnimationKey
        {
            get
            {
                if (_ToolTipPopupAnimation == null)
                {
                    _ToolTipPopupAnimation = CreateInstance(SystemResourceKeyID.ToolTipPopupAnimation);
                }

                return _ToolTipPopupAnimation;
            }
        }

        #endregion

        #region Window Parameters

        /// <summary>
        ///     Maps to SPI_GETANIMATION
        /// </summary>
        public static bool MinimizeAnimation => FrameworkProvider.GetParameterProvider().MinimizeAnimation;

        /// <summary>
        ///     Maps to SPI_GETBORDER
        /// </summary>
        public static int Border => FrameworkProvider.GetParameterProvider().Border;

        /// <summary>
        ///     Maps to SPI_GETCARETWIDTH
        /// </summary>
        public static float CaretWidth => FrameworkProvider.GetParameterProvider().CaretWidth;

        /// <summary>
        ///     Maps to SPI_GETDRAGFULLWINDOWS
        /// </summary>
        public static bool DragFullWindows => FrameworkProvider.GetParameterProvider().DragFullWindows;

        /// <summary>
        ///     Maps to SPI_GETFOREGROUNDFLASHCOUNT
        /// </summary>

        public static int ForegroundFlashCount => FrameworkProvider.GetParameterProvider().ForegroundFlashCount;

        /// <summary>
        ///     From SPI_GETNONCLIENTMETRICS
        /// </summary>
        public static float BorderWidth => FrameworkProvider.GetParameterProvider().BorderWidth;

        /// <summary>
        ///     From SPI_GETNONCLIENTMETRICS
        /// </summary>
        public static float ScrollWidth => FrameworkProvider.GetParameterProvider().ScrollWidth;

        /// <summary>
        ///     From SPI_GETNONCLIENTMETRICS
        /// </summary>
        public static float ScrollHeight => FrameworkProvider.GetParameterProvider().ScrollHeight;

        /// <summary>
        ///     From SPI_GETNONCLIENTMETRICS
        /// </summary>
        public static float CaptionWidth => FrameworkProvider.GetParameterProvider().CaptionWidth;

        /// <summary>
        ///     From SPI_GETNONCLIENTMETRICS
        /// </summary>
        public static float CaptionHeight => FrameworkProvider.GetParameterProvider().CaptionHeight;

        /// <summary>
        ///     From SPI_GETNONCLIENTMETRICS
        /// </summary>
        public static float SmallCaptionWidth => FrameworkProvider.GetParameterProvider().SmallCaptionWidth;

        /// <summary>
        ///     From SPI_NONCLIENTMETRICS
        /// </summary>
        public static float SmallCaptionHeight => FrameworkProvider.GetParameterProvider().SmallCaptionHeight;

        /// <summary>
        ///     From SPI_NONCLIENTMETRICS
        /// </summary>
        public static float MenuWidth => FrameworkProvider.GetParameterProvider().MenuWidth;

        /// <summary>
        ///     From SPI_NONCLIENTMETRICS
        /// </summary>
        public static float MenuHeight => FrameworkProvider.GetParameterProvider().MenuHeight;

        #endregion

        #region Window Parameters Keys

        /// <summary>
        ///     MinimizeAnimation System Resource Key
        /// </summary>
        public static ResourceKey MinimizeAnimationKey
        {
            get
            {
                if (_MinimizeAnimation == null)
                {
                    _MinimizeAnimation = CreateInstance(SystemResourceKeyID.MinimizeAnimation);
                }

                return _MinimizeAnimation;
            }
        }

        /// <summary>
        ///     Border System Resource Key
        /// </summary>
        public static ResourceKey BorderKey
        {
            get
            {
                if (_Border == null)
                {
                    _Border = CreateInstance(SystemResourceKeyID.Border);
                }

                return _Border;
            }
        }

        /// <summary>
        ///     CaretWidth System Resource Key
        /// </summary>
        public static ResourceKey CaretWidthKey
        {
            get
            {
                if (_CaretWidth == null)
                {
                    _CaretWidth = CreateInstance(SystemResourceKeyID.CaretWidth);
                }

                return _CaretWidth;
            }
        }

        /// <summary>
        ///     ForegroundFlashCount System Resource Key
        /// </summary>
        public static ResourceKey ForegroundFlashCountKey
        {
            get
            {
                if (_ForegroundFlashCount == null)
                {
                    _ForegroundFlashCount = CreateInstance(SystemResourceKeyID.ForegroundFlashCount);
                }

                return _ForegroundFlashCount;
            }
        }

        /// <summary>
        ///     DragFullWindows System Resource Key
        /// </summary>
        public static ResourceKey DragFullWindowsKey
        {
            get
            {
                if (_DragFullWindows == null)
                {
                    _DragFullWindows = CreateInstance(SystemResourceKeyID.DragFullWindows);
                }

                return _DragFullWindows;
            }
        }

        /// <summary>
        ///     BorderWidth System Resource Key
        /// </summary>
        public static ResourceKey BorderWidthKey
        {
            get
            {
                if (_BorderWidth == null)
                {
                    _BorderWidth = CreateInstance(SystemResourceKeyID.BorderWidth);
                }

                return _BorderWidth;
            }
        }

        /// <summary>
        ///     ScrollWidth System Resource Key
        /// </summary>
        public static ResourceKey ScrollWidthKey
        {
            get
            {
                if (_ScrollWidth == null)
                {
                    _ScrollWidth = CreateInstance(SystemResourceKeyID.ScrollWidth);
                }

                return _ScrollWidth;
            }
        }

        /// <summary>
        ///     ScrollHeight System Resource Key
        /// </summary>
        public static ResourceKey ScrollHeightKey
        {
            get
            {
                if (_ScrollHeight == null)
                {
                    _ScrollHeight = CreateInstance(SystemResourceKeyID.ScrollHeight);
                }

                return _ScrollHeight;
            }
        }

        /// <summary>
        ///     CaptionWidth System Resource Key
        /// </summary>
        public static ResourceKey CaptionWidthKey
        {
            get
            {
                if (_CaptionWidth == null)
                {
                    _CaptionWidth = CreateInstance(SystemResourceKeyID.CaptionWidth);
                }

                return _CaptionWidth;
            }
        }

        /// <summary>
        ///     CaptionHeight System Resource Key
        /// </summary>
        public static ResourceKey CaptionHeightKey
        {
            get
            {
                if (_CaptionHeight == null)
                {
                    _CaptionHeight = CreateInstance(SystemResourceKeyID.CaptionHeight);
                }

                return _CaptionHeight;
            }
        }

        /// <summary>
        ///     SmallCaptionWidth System Resource Key
        /// </summary>
        public static ResourceKey SmallCaptionWidthKey
        {
            get
            {
                if (_SmallCaptionWidth == null)
                {
                    _SmallCaptionWidth = CreateInstance(SystemResourceKeyID.SmallCaptionWidth);
                }

                return _SmallCaptionWidth;
            }
        }

        /// <summary>
        ///     MenuWidth System Resource Key
        /// </summary>
        public static ResourceKey MenuWidthKey
        {
            get
            {
                if (_MenuWidth == null)
                {
                    _MenuWidth = CreateInstance(SystemResourceKeyID.MenuWidth);
                }

                return _MenuWidth;
            }
        }

        /// <summary>
        ///     MenuHeight System Resource Key
        /// </summary>
        public static ResourceKey MenuHeightKey
        {
            get
            {
                if (_MenuHeight == null)
                {
                    _MenuHeight = CreateInstance(SystemResourceKeyID.MenuHeight);
                }

                return _MenuHeight;
            }
        }

        #endregion

        #region Metrics Keys

        /// <summary>
        ///     ThinHorizontalBorderHeight System Resource Key
        /// </summary>
        public static ResourceKey ThinHorizontalBorderHeightKey
        {
            get
            {
                if (_ThinHorizontalBorderHeight == null)
                {
                    _ThinHorizontalBorderHeight = CreateInstance(SystemResourceKeyID.ThinHorizontalBorderHeight);
                }

                return _ThinHorizontalBorderHeight;
            }
        }

        /// <summary>
        ///     ThinVerticalBorderWidth System Resource Key
        /// </summary>
        public static ResourceKey ThinVerticalBorderWidthKey
        {
            get
            {
                if (_ThinVerticalBorderWidth == null)
                {
                    _ThinVerticalBorderWidth = CreateInstance(SystemResourceKeyID.ThinVerticalBorderWidth);
                }

                return _ThinVerticalBorderWidth;
            }
        }

        /// <summary>
        ///     CursorWidth System Resource Key
        /// </summary>
        public static ResourceKey CursorWidthKey
        {
            get
            {
                if (_CursorWidth == null)
                {
                    _CursorWidth = CreateInstance(SystemResourceKeyID.CursorWidth);
                }

                return _CursorWidth;
            }
        }

        /// <summary>
        ///     CursorHeight System Resource Key
        /// </summary>
        public static ResourceKey CursorHeightKey
        {
            get
            {
                if (_CursorHeight == null)
                {
                    _CursorHeight = CreateInstance(SystemResourceKeyID.CursorHeight);
                }

                return _CursorHeight;
            }
        }

        /// <summary>
        ///     ThickHorizontalBorderHeight System Resource Key
        /// </summary>
        public static ResourceKey ThickHorizontalBorderHeightKey
        {
            get
            {
                if (_ThickHorizontalBorderHeight == null)
                {
                    _ThickHorizontalBorderHeight = CreateInstance(SystemResourceKeyID.ThickHorizontalBorderHeight);
                }

                return _ThickHorizontalBorderHeight;
            }
        }

        /// <summary>
        ///     ThickVerticalBorderWidth System Resource Key
        /// </summary>
        public static ResourceKey ThickVerticalBorderWidthKey
        {
            get
            {
                if (_ThickVerticalBorderWidth == null)
                {
                    _ThickVerticalBorderWidth = CreateInstance(SystemResourceKeyID.ThickVerticalBorderWidth);
                }

                return _ThickVerticalBorderWidth;
            }
        }

        /// <summary>
        ///     FixedFrameHorizontalBorderHeight System Resource Key
        /// </summary>
        public static ResourceKey FixedFrameHorizontalBorderHeightKey
        {
            get
            {
                if (_FixedFrameHorizontalBorderHeight == null)
                {
                    _FixedFrameHorizontalBorderHeight = CreateInstance(SystemResourceKeyID.FixedFrameHorizontalBorderHeight);
                }

                return _FixedFrameHorizontalBorderHeight;
            }
        }

        /// <summary>
        ///     FixedFrameVerticalBorderWidth System Resource Key
        /// </summary>
        public static ResourceKey FixedFrameVerticalBorderWidthKey
        {
            get
            {
                if (_FixedFrameVerticalBorderWidth == null)
                {
                    _FixedFrameVerticalBorderWidth = CreateInstance(SystemResourceKeyID.FixedFrameVerticalBorderWidth);
                }

                return _FixedFrameVerticalBorderWidth;
            }
        }

        /// <summary>
        ///     FocusHorizontalBorderHeight System Resource Key
        /// </summary>
        public static ResourceKey FocusHorizontalBorderHeightKey
        {
            get
            {
                if (_FocusHorizontalBorderHeight == null)
                {
                    _FocusHorizontalBorderHeight = CreateInstance(SystemResourceKeyID.FocusHorizontalBorderHeight);
                }

                return _FocusHorizontalBorderHeight;
            }
        }

        /// <summary>
        ///     FocusVerticalBorderWidth System Resource Key
        /// </summary>
        public static ResourceKey FocusVerticalBorderWidthKey
        {
            get
            {
                if (_FocusVerticalBorderWidth == null)
                {
                    _FocusVerticalBorderWidth = CreateInstance(SystemResourceKeyID.FocusVerticalBorderWidth);
                }

                return _FocusVerticalBorderWidth;
            }
        }

        /// <summary>
        ///     FullPrimaryScreenWidth System Resource Key
        /// </summary>
        public static ResourceKey FullPrimaryScreenWidthKey
        {
            get
            {
                if (_FullPrimaryScreenWidth == null)
                {
                    _FullPrimaryScreenWidth = CreateInstance(SystemResourceKeyID.FullPrimaryScreenWidth);
                }

                return _FullPrimaryScreenWidth;
            }
        }

        /// <summary>
        ///     FullPrimaryScreenHeight System Resource Key
        /// </summary>
        public static ResourceKey FullPrimaryScreenHeightKey
        {
            get
            {
                if (_FullPrimaryScreenHeight == null)
                {
                    _FullPrimaryScreenHeight = CreateInstance(SystemResourceKeyID.FullPrimaryScreenHeight);
                }

                return _FullPrimaryScreenHeight;
            }
        }

        /// <summary>
        ///     HorizontalScrollBarButtonWidth System Resource Key
        /// </summary>
        public static ResourceKey HorizontalScrollBarButtonWidthKey
        {
            get
            {
                if (_HorizontalScrollBarButtonWidth == null)
                {
                    _HorizontalScrollBarButtonWidth = CreateInstance(SystemResourceKeyID.HorizontalScrollBarButtonWidth);
                }

                return _HorizontalScrollBarButtonWidth;
            }
        }

        /// <summary>
        ///     HorizontalScrollBarHeight System Resource Key
        /// </summary>
        public static ResourceKey HorizontalScrollBarHeightKey
        {
            get
            {
                if (_HorizontalScrollBarHeight == null)
                {
                    _HorizontalScrollBarHeight = CreateInstance(SystemResourceKeyID.HorizontalScrollBarHeight);
                }

                return _HorizontalScrollBarHeight;
            }
        }

        /// <summary>
        ///     HorizontalScrollBarThumbWidth System Resource Key
        /// </summary>
        public static ResourceKey HorizontalScrollBarThumbWidthKey
        {
            get
            {
                if (_HorizontalScrollBarThumbWidth == null)
                {
                    _HorizontalScrollBarThumbWidth = CreateInstance(SystemResourceKeyID.HorizontalScrollBarThumbWidth);
                }

                return _HorizontalScrollBarThumbWidth;
            }
        }

        /// <summary>
        ///     IconWidth System Resource Key
        /// </summary>
        public static ResourceKey IconWidthKey
        {
            get
            {
                if (_IconWidth == null)
                {
                    _IconWidth = CreateInstance(SystemResourceKeyID.IconWidth);
                }

                return _IconWidth;
            }
        }

        /// <summary>
        ///     IconHeight System Resource Key
        /// </summary>
        public static ResourceKey IconHeightKey
        {
            get
            {
                if (_IconHeight == null)
                {
                    _IconHeight = CreateInstance(SystemResourceKeyID.IconHeight);
                }

                return _IconHeight;
            }
        }

        /// <summary>
        ///     IconGridWidth System Resource Key
        /// </summary>
        public static ResourceKey IconGridWidthKey
        {
            get
            {
                if (_IconGridWidth == null)
                {
                    _IconGridWidth = CreateInstance(SystemResourceKeyID.IconGridWidth);
                }

                return _IconGridWidth;
            }
        }

        /// <summary>
        ///     IconGridHeight System Resource Key
        /// </summary>
        public static ResourceKey IconGridHeightKey
        {
            get
            {
                if (_IconGridHeight == null)
                {
                    _IconGridHeight = CreateInstance(SystemResourceKeyID.IconGridHeight);
                }

                return _IconGridHeight;
            }
        }

        /// <summary>
        ///     MaximizedPrimaryScreenWidth System Resource Key
        /// </summary>
        public static ResourceKey MaximizedPrimaryScreenWidthKey
        {
            get
            {
                if (_MaximizedPrimaryScreenWidth == null)
                {
                    _MaximizedPrimaryScreenWidth = CreateInstance(SystemResourceKeyID.MaximizedPrimaryScreenWidth);
                }

                return _MaximizedPrimaryScreenWidth;
            }
        }

        /// <summary>
        ///     MaximizedPrimaryScreenHeight System Resource Key
        /// </summary>
        public static ResourceKey MaximizedPrimaryScreenHeightKey
        {
            get
            {
                if (_MaximizedPrimaryScreenHeight == null)
                {
                    _MaximizedPrimaryScreenHeight = CreateInstance(SystemResourceKeyID.MaximizedPrimaryScreenHeight);
                }

                return _MaximizedPrimaryScreenHeight;
            }
        }

        /// <summary>
        ///     MaximumWindowTrackWidth System Resource Key
        /// </summary>
        public static ResourceKey MaximumWindowTrackWidthKey
        {
            get
            {
                if (_MaximumWindowTrackWidth == null)
                {
                    _MaximumWindowTrackWidth = CreateInstance(SystemResourceKeyID.MaximumWindowTrackWidth);
                }

                return _MaximumWindowTrackWidth;
            }
        }

        /// <summary>
        ///     MaximumWindowTrackHeight System Resource Key
        /// </summary>
        public static ResourceKey MaximumWindowTrackHeightKey
        {
            get
            {
                if (_MaximumWindowTrackHeight == null)
                {
                    _MaximumWindowTrackHeight = CreateInstance(SystemResourceKeyID.MaximumWindowTrackHeight);
                }

                return _MaximumWindowTrackHeight;
            }
        }

        /// <summary>
        ///     MenuCheckmarkWidth System Resource Key
        /// </summary>
        public static ResourceKey MenuCheckmarkWidthKey
        {
            get
            {
                if (_MenuCheckmarkWidth == null)
                {
                    _MenuCheckmarkWidth = CreateInstance(SystemResourceKeyID.MenuCheckmarkWidth);
                }

                return _MenuCheckmarkWidth;
            }
        }

        /// <summary>
        ///     MenuCheckmarkHeight System Resource Key
        /// </summary>
        public static ResourceKey MenuCheckmarkHeightKey
        {
            get
            {
                if (_MenuCheckmarkHeight == null)
                {
                    _MenuCheckmarkHeight = CreateInstance(SystemResourceKeyID.MenuCheckmarkHeight);
                }

                return _MenuCheckmarkHeight;
            }
        }

        /// <summary>
        ///     MenuButtonWidth System Resource Key
        /// </summary>
        public static ResourceKey MenuButtonWidthKey
        {
            get
            {
                if (_MenuButtonWidth == null)
                {
                    _MenuButtonWidth = CreateInstance(SystemResourceKeyID.MenuButtonWidth);
                }

                return _MenuButtonWidth;
            }
        }

        /// <summary>
        ///     MenuButtonHeight System Resource Key
        /// </summary>
        public static ResourceKey MenuButtonHeightKey
        {
            get
            {
                if (_MenuButtonHeight == null)
                {
                    _MenuButtonHeight = CreateInstance(SystemResourceKeyID.MenuButtonHeight);
                }

                return _MenuButtonHeight;
            }
        }

        /// <summary>
        ///     MinimumWindowWidth System Resource Key
        /// </summary>
        public static ResourceKey MinimumWindowWidthKey
        {
            get
            {
                if (_MinimumWindowWidth == null)
                {
                    _MinimumWindowWidth = CreateInstance(SystemResourceKeyID.MinimumWindowWidth);
                }

                return _MinimumWindowWidth;
            }
        }

        /// <summary>
        ///     MinimumWindowHeight System Resource Key
        /// </summary>
        public static ResourceKey MinimumWindowHeightKey
        {
            get
            {
                if (_MinimumWindowHeight == null)
                {
                    _MinimumWindowHeight = CreateInstance(SystemResourceKeyID.MinimumWindowHeight);
                }

                return _MinimumWindowHeight;
            }
        }

        /// <summary>
        ///     MinimizedWindowWidth System Resource Key
        /// </summary>
        public static ResourceKey MinimizedWindowWidthKey
        {
            get
            {
                if (_MinimizedWindowWidth == null)
                {
                    _MinimizedWindowWidth = CreateInstance(SystemResourceKeyID.MinimizedWindowWidth);
                }

                return _MinimizedWindowWidth;
            }
        }

        /// <summary>
        ///     MinimizedWindowHeight System Resource Key
        /// </summary>
        public static ResourceKey MinimizedWindowHeightKey
        {
            get
            {
                if (_MinimizedWindowHeight == null)
                {
                    _MinimizedWindowHeight = CreateInstance(SystemResourceKeyID.MinimizedWindowHeight);
                }

                return _MinimizedWindowHeight;
            }
        }

        /// <summary>
        ///     MinimizedGridWidth System Resource Key
        /// </summary>
        public static ResourceKey MinimizedGridWidthKey
        {
            get
            {
                if (_MinimizedGridWidth == null)
                {
                    _MinimizedGridWidth = CreateInstance(SystemResourceKeyID.MinimizedGridWidth);
                }

                return _MinimizedGridWidth;
            }
        }

        /// <summary>
        ///     MinimizedGridHeight System Resource Key
        /// </summary>
        public static ResourceKey MinimizedGridHeightKey
        {
            get
            {
                if (_MinimizedGridHeight == null)
                {
                    _MinimizedGridHeight = CreateInstance(SystemResourceKeyID.MinimizedGridHeight);
                }

                return _MinimizedGridHeight;
            }
        }

        /// <summary>
        ///     MinimumWindowTrackWidth System Resource Key
        /// </summary>
        public static ResourceKey MinimumWindowTrackWidthKey
        {
            get
            {
                if (_MinimumWindowTrackWidth == null)
                {
                    _MinimumWindowTrackWidth = CreateInstance(SystemResourceKeyID.MinimumWindowTrackWidth);
                }

                return _MinimumWindowTrackWidth;
            }
        }

        /// <summary>
        ///     MinimumWindowTrackHeight System Resource Key
        /// </summary>
        public static ResourceKey MinimumWindowTrackHeightKey
        {
            get
            {
                if (_MinimumWindowTrackHeight == null)
                {
                    _MinimumWindowTrackHeight = CreateInstance(SystemResourceKeyID.MinimumWindowTrackHeight);
                }

                return _MinimumWindowTrackHeight;
            }
        }

        /// <summary>
        ///     PrimaryScreenWidth System Resource Key
        /// </summary>
        public static ResourceKey PrimaryScreenWidthKey
        {
            get
            {
                if (_PrimaryScreenWidth == null)
                {
                    _PrimaryScreenWidth = CreateInstance(SystemResourceKeyID.PrimaryScreenWidth);
                }

                return _PrimaryScreenWidth;
            }
        }

        /// <summary>
        ///     PrimaryScreenHeight System Resource Key
        /// </summary>
        public static ResourceKey PrimaryScreenHeightKey
        {
            get
            {
                if (_PrimaryScreenHeight == null)
                {
                    _PrimaryScreenHeight = CreateInstance(SystemResourceKeyID.PrimaryScreenHeight);
                }

                return _PrimaryScreenHeight;
            }
        }

        /// <summary>
        ///     WindowCaptionButtonWidth System Resource Key
        /// </summary>
        public static ResourceKey WindowCaptionButtonWidthKey
        {
            get
            {
                if (_WindowCaptionButtonWidth == null)
                {
                    _WindowCaptionButtonWidth = CreateInstance(SystemResourceKeyID.WindowCaptionButtonWidth);
                }

                return _WindowCaptionButtonWidth;
            }
        }

        /// <summary>
        ///     WindowCaptionButtonHeight System Resource Key
        /// </summary>
        public static ResourceKey WindowCaptionButtonHeightKey
        {
            get
            {
                if (_WindowCaptionButtonHeight == null)
                {
                    _WindowCaptionButtonHeight = CreateInstance(SystemResourceKeyID.WindowCaptionButtonHeight);
                }

                return _WindowCaptionButtonHeight;
            }
        }

        /// <summary>
        ///     ResizeFrameHorizontalBorderHeight System Resource Key
        /// </summary>
        public static ResourceKey ResizeFrameHorizontalBorderHeightKey
        {
            get
            {
                if (_ResizeFrameHorizontalBorderHeight == null)
                {
                    _ResizeFrameHorizontalBorderHeight = CreateInstance(SystemResourceKeyID.ResizeFrameHorizontalBorderHeight);
                }

                return _ResizeFrameHorizontalBorderHeight;
            }
        }

        /// <summary>
        ///     ResizeFrameVerticalBorderWidth System Resource Key
        /// </summary>
        public static ResourceKey ResizeFrameVerticalBorderWidthKey
        {
            get
            {
                if (_ResizeFrameVerticalBorderWidth == null)
                {
                    _ResizeFrameVerticalBorderWidth = CreateInstance(SystemResourceKeyID.ResizeFrameVerticalBorderWidth);
                }

                return _ResizeFrameVerticalBorderWidth;
            }
        }

        /// <summary>
        ///     SmallIconWidth System Resource Key
        /// </summary>
        public static ResourceKey SmallIconWidthKey
        {
            get
            {
                if (_SmallIconWidth == null)
                {
                    _SmallIconWidth = CreateInstance(SystemResourceKeyID.SmallIconWidth);
                }

                return _SmallIconWidth;
            }
        }

        /// <summary>
        ///     SmallIconHeight System Resource Key
        /// </summary>
        public static ResourceKey SmallIconHeightKey
        {
            get
            {
                if (_SmallIconHeight == null)
                {
                    _SmallIconHeight = CreateInstance(SystemResourceKeyID.SmallIconHeight);
                }

                return _SmallIconHeight;
            }
        }

        /// <summary>
        ///     SmallWindowCaptionButtonWidth System Resource Key
        /// </summary>
        public static ResourceKey SmallWindowCaptionButtonWidthKey
        {
            get
            {
                if (_SmallWindowCaptionButtonWidth == null)
                {
                    _SmallWindowCaptionButtonWidth = CreateInstance(SystemResourceKeyID.SmallWindowCaptionButtonWidth);
                }

                return _SmallWindowCaptionButtonWidth;
            }
        }

        /// <summary>
        ///     SmallWindowCaptionButtonHeight System Resource Key
        /// </summary>
        public static ResourceKey SmallWindowCaptionButtonHeightKey
        {
            get
            {
                if (_SmallWindowCaptionButtonHeight == null)
                {
                    _SmallWindowCaptionButtonHeight = CreateInstance(SystemResourceKeyID.SmallWindowCaptionButtonHeight);
                }

                return _SmallWindowCaptionButtonHeight;
            }
        }

        /// <summary>
        ///     VirtualScreenWidth System Resource Key
        /// </summary>
        public static ResourceKey VirtualScreenWidthKey
        {
            get
            {
                if (_VirtualScreenWidth == null)
                {
                    _VirtualScreenWidth = CreateInstance(SystemResourceKeyID.VirtualScreenWidth);
                }

                return _VirtualScreenWidth;
            }
        }

        /// <summary>
        ///     VirtualScreenHeight System Resource Key
        /// </summary>
        public static ResourceKey VirtualScreenHeightKey
        {
            get
            {
                if (_VirtualScreenHeight == null)
                {
                    _VirtualScreenHeight = CreateInstance(SystemResourceKeyID.VirtualScreenHeight);
                }

                return _VirtualScreenHeight;
            }
        }

        /// <summary>
        ///     VerticalScrollBarWidth System Resource Key
        /// </summary>
        public static ResourceKey VerticalScrollBarWidthKey
        {
            get
            {
                if (_VerticalScrollBarWidth == null)
                {
                    _VerticalScrollBarWidth = CreateInstance(SystemResourceKeyID.VerticalScrollBarWidth);
                }

                return _VerticalScrollBarWidth;
            }
        }

        /// <summary>
        ///     VerticalScrollBarButtonHeight System Resource Key
        /// </summary>
        public static ResourceKey VerticalScrollBarButtonHeightKey
        {
            get
            {
                if (_VerticalScrollBarButtonHeight == null)
                {
                    _VerticalScrollBarButtonHeight = CreateInstance(SystemResourceKeyID.VerticalScrollBarButtonHeight);
                }

                return _VerticalScrollBarButtonHeight;
            }
        }

        /// <summary>
        ///     WindowCaptionHeight System Resource Key
        /// </summary>
        public static ResourceKey WindowCaptionHeightKey
        {
            get
            {
                if (_WindowCaptionHeight == null)
                {
                    _WindowCaptionHeight = CreateInstance(SystemResourceKeyID.WindowCaptionHeight);
                }

                return _WindowCaptionHeight;
            }
        }

        /// <summary>
        ///     KanjiWindowHeight System Resource Key
        /// </summary>
        public static ResourceKey KanjiWindowHeightKey
        {
            get
            {
                if (_KanjiWindowHeight == null)
                {
                    _KanjiWindowHeight = CreateInstance(SystemResourceKeyID.KanjiWindowHeight);
                }

                return _KanjiWindowHeight;
            }
        }

        /// <summary>
        ///     MenuBarHeight System Resource Key
        /// </summary>
        public static ResourceKey MenuBarHeightKey
        {
            get
            {
                if (_MenuBarHeight == null)
                {
                    _MenuBarHeight = CreateInstance(SystemResourceKeyID.MenuBarHeight);
                }

                return _MenuBarHeight;
            }
        }

        /// <summary>
        ///     SmallCaptionHeight System Resource Key
        /// </summary>
        public static ResourceKey SmallCaptionHeightKey
        {
            get
            {
                if (_SmallCaptionHeight == null)
                {
                    _SmallCaptionHeight = CreateInstance(SystemResourceKeyID.SmallCaptionHeight);
                }

                return _SmallCaptionHeight;
            }
        }

        /// <summary>
        ///     VerticalScrollBarThumbHeight System Resource Key
        /// </summary>
        public static ResourceKey VerticalScrollBarThumbHeightKey
        {
            get
            {
                if (_VerticalScrollBarThumbHeight == null)
                {
                    _VerticalScrollBarThumbHeight = CreateInstance(SystemResourceKeyID.VerticalScrollBarThumbHeight);
                }

                return _VerticalScrollBarThumbHeight;
            }
        }

        /// <summary>
        ///     IsImmEnabled System Resource Key
        /// </summary>
        public static ResourceKey IsImmEnabledKey
        {
            get
            {
                if (_IsImmEnabled == null)
                {
                    _IsImmEnabled = CreateInstance(SystemResourceKeyID.IsImmEnabled);
                }

                return _IsImmEnabled;
            }
        }

        /// <summary>
        ///     IsMediaCenter System Resource Key
        /// </summary>
        public static ResourceKey IsMediaCenterKey
        {
            get
            {
                if (_IsMediaCenter == null)
                {
                    _IsMediaCenter = CreateInstance(SystemResourceKeyID.IsMediaCenter);
                }

                return _IsMediaCenter;
            }
        }

        /// <summary>
        ///     IsMenuDropRightAligned System Resource Key
        /// </summary>
        public static ResourceKey IsMenuDropRightAlignedKey
        {
            get
            {
                if (_IsMenuDropRightAligned == null)
                {
                    _IsMenuDropRightAligned = CreateInstance(SystemResourceKeyID.IsMenuDropRightAligned);
                }

                return _IsMenuDropRightAligned;
            }
        }

        /// <summary>
        ///     IsMiddleEastEnabled System Resource Key
        /// </summary>
        public static ResourceKey IsMiddleEastEnabledKey
        {
            get
            {
                if (_IsMiddleEastEnabled == null)
                {
                    _IsMiddleEastEnabled = CreateInstance(SystemResourceKeyID.IsMiddleEastEnabled);
                }

                return _IsMiddleEastEnabled;
            }
        }

        /// <summary>
        ///     IsMousePresent System Resource Key
        /// </summary>
        public static ResourceKey IsMousePresentKey
        {
            get
            {
                if (_IsMousePresent == null)
                {
                    _IsMousePresent = CreateInstance(SystemResourceKeyID.IsMousePresent);
                }

                return _IsMousePresent;
            }
        }

        /// <summary>
        ///     IsMouseWheelPresent System Resource Key
        /// </summary>
        public static ResourceKey IsMouseWheelPresentKey
        {
            get
            {
                if (_IsMouseWheelPresent == null)
                {
                    _IsMouseWheelPresent = CreateInstance(SystemResourceKeyID.IsMouseWheelPresent);
                }

                return _IsMouseWheelPresent;
            }
        }

        /// <summary>
        ///     IsPenWindows System Resource Key
        /// </summary>
        public static ResourceKey IsPenWindowsKey
        {
            get
            {
                if (_IsPenWindows == null)
                {
                    _IsPenWindows = CreateInstance(SystemResourceKeyID.IsPenWindows);
                }

                return _IsPenWindows;
            }
        }

        /// <summary>
        ///     IsRemotelyControlled System Resource Key
        /// </summary>
        public static ResourceKey IsRemotelyControlledKey
        {
            get
            {
                if (_IsRemotelyControlled == null)
                {
                    _IsRemotelyControlled = CreateInstance(SystemResourceKeyID.IsRemotelyControlled);
                }

                return _IsRemotelyControlled;
            }
        }

        /// <summary>
        ///     IsRemoteSession System Resource Key
        /// </summary>
        public static ResourceKey IsRemoteSessionKey
        {
            get
            {
                if (_IsRemoteSession == null)
                {
                    _IsRemoteSession = CreateInstance(SystemResourceKeyID.IsRemoteSession);
                }

                return _IsRemoteSession;
            }
        }

        /// <summary>
        ///     ShowSounds System Resource Key
        /// </summary>
        public static ResourceKey ShowSoundsKey
        {
            get
            {
                if (_ShowSounds == null)
                {
                    _ShowSounds = CreateInstance(SystemResourceKeyID.ShowSounds);
                }

                return _ShowSounds;
            }
        }

        /// <summary>
        ///     IsSlowMachine System Resource Key
        /// </summary>
        public static ResourceKey IsSlowMachineKey
        {
            get
            {
                if (_IsSlowMachine == null)
                {
                    _IsSlowMachine = CreateInstance(SystemResourceKeyID.IsSlowMachine);
                }

                return _IsSlowMachine;
            }
        }

        /// <summary>
        ///     SwapButtons System Resource Key
        /// </summary>
        public static ResourceKey SwapButtonsKey
        {
            get
            {
                if (_SwapButtons == null)
                {
                    _SwapButtons = CreateInstance(SystemResourceKeyID.SwapButtons);
                }

                return _SwapButtons;
            }
        }

        /// <summary>
        ///     IsTabletPC System Resource Key
        /// </summary>
        public static ResourceKey IsTabletPCKey
        {
            get
            {
                if (_IsTabletPC == null)
                {
                    _IsTabletPC = CreateInstance(SystemResourceKeyID.IsTabletPC);
                }

                return _IsTabletPC;
            }
        }

        /// <summary>
        ///     VirtualScreenLeft System Resource Key
        /// </summary>
        public static ResourceKey VirtualScreenLeftKey
        {
            get
            {
                if (_VirtualScreenLeft == null)
                {
                    _VirtualScreenLeft = CreateInstance(SystemResourceKeyID.VirtualScreenLeft);
                }

                return _VirtualScreenLeft;
            }
        }

        /// <summary>
        ///     VirtualScreenTop System Resource Key
        /// </summary>
        public static ResourceKey VirtualScreenTopKey
        {
            get
            {
                if (_VirtualScreenTop == null)
                {
                    _VirtualScreenTop = CreateInstance(SystemResourceKeyID.VirtualScreenTop);
                }

                return _VirtualScreenTop;
            }
        }

        #endregion

        //#region Theme Style Keys

        ///// <summary>
        /////     Resource Key for the FocusVisualStyle
        ///// </summary>
        //public static ResourceKey FocusVisualStyleKey
        //{
        //    get
        //    {
        //        if (_FocusVisualStyle == null)
        //        {
        //            _FocusVisualStyle = new SystemThemeKey(SystemResourceKeyID.FocusVisualStyle);
        //        }

        //        return _FocusVisualStyle;
        //    }
        //}

        ///// <summary>
        ///// Resource Key for the browser window style
        ///// </summary>
        ///// <value></value>
        //public static ResourceKey NavigationChromeStyleKey
        //{
        //    get
        //    {
        //        if (_NavigationChromeStyle == null)
        //        {
        //            _NavigationChromeStyle = new SystemThemeKey(SystemResourceKeyID.NavigationChromeStyle);
        //        }

        //        return _NavigationChromeStyle;
        //    }
        //}

        ///// <summary>
        ///// Resource Key for the down level browser window style
        ///// </summary>
        ///// <value></value>
        //public static ResourceKey NavigationChromeDownLevelStyleKey
        //{
        //    get
        //    {
        //        if (_NavigationChromeDownLevelStyle == null)
        //        {
        //            _NavigationChromeDownLevelStyle = new SystemThemeKey(SystemResourceKeyID.NavigationChromeDownLevelStyle);
        //        }

        //        return _NavigationChromeDownLevelStyle;
        //    }
        //}

        //#endregion

        #region PowerKeys
        /// <summary>
        /// Resource Key for the PowerLineStatus property
        /// </summary>
        /// <value></value>
        public static ResourceKey PowerLineStatusKey
        {
            get
            {
                if (_PowerLineStatus == null)
                {
                    _PowerLineStatus = CreateInstance(SystemResourceKeyID.PowerLineStatus);
                }

                return _PowerLineStatus;
            }
        }
        #endregion

        private static SystemResourceKey CreateInstance(SystemResourceKeyID KeyId)
        {
            return new SystemResourceKey(KeyId);
        }
    }
}
