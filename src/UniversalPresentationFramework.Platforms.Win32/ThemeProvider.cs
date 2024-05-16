using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using Wodsoft.UI.Providers;


namespace Wodsoft.UI.Platforms.Win32
{
    public partial class ThemeProvider : IThemeProvider
    {
        private readonly object _themeLock = new object();
        private ThemeInfo? _themeInfo;

        public ThemeProvider()
        {
            _themeInfo = GetCurrentTheme();
        }

        public string Name => _themeInfo?.Name ?? "Areo";

        public string Color => _themeInfo?.Color ?? "NormalColor";

        public event EventHandler? ThemeChanged;

        public object? GetResourceValue(SystemResourceKeyID key)
        {
            switch (key)
            {
                case SystemResourceKeyID.ActiveBorderBrush:
                    return ActiveBorderBrush;
                case SystemResourceKeyID.ActiveBorderColor:
                    return ActiveBorderColor;
                case SystemResourceKeyID.ActiveCaptionBrush:
                    return ActiveCaptionBrush;
                case SystemResourceKeyID.ActiveCaptionColor:
                    return ActiveCaptionColor;
                case SystemResourceKeyID.ActiveCaptionTextBrush:
                    return ActiveCaptionTextBrush;
                case SystemResourceKeyID.ActiveCaptionTextColor:
                    return ActiveCaptionTextColor;
                case SystemResourceKeyID.AppWorkspaceBrush:
                    return AppWorkspaceBrush;
                case SystemResourceKeyID.AppWorkspaceColor:
                    return AppWorkspaceColor;
                case SystemResourceKeyID.Border:
                    return Border;
                case SystemResourceKeyID.BorderWidth:
                    return BorderWidth;
                case SystemResourceKeyID.CaptionFontFamily:
                    return CaptionFontFamily;
                case SystemResourceKeyID.CaptionFontSize:
                    return CaptionFontSize;
                case SystemResourceKeyID.CaptionFontStyle:
                    return CaptionFontStyle;
                case SystemResourceKeyID.CaptionFontTextDecorations:
                    return CaptionFontTextDecorations;
                case SystemResourceKeyID.CaptionFontWeight:
                    return CaptionFontWeight;
                case SystemResourceKeyID.CaptionHeight:
                    return CaptionHeight;
                case SystemResourceKeyID.CaptionWidth:
                    return CaptionWidth;
                case SystemResourceKeyID.CaretWidth:
                    return CaretWidth;
                //case SystemResourceKeyID.ClientAreaAnimation:
                //case SystemResourceKeyID.ComboBoxAnimation:
                //case SystemResourceKeyID.ComboBoxPopupAnimation:
                case SystemResourceKeyID.ControlBrush:
                    return ControlBrush;
                case SystemResourceKeyID.ControlColor:
                    return ControlColor;
                case SystemResourceKeyID.ControlDarkBrush:
                    return ControlDarkBrush;
                case SystemResourceKeyID.ControlDarkColor:
                    return ControlDarkColor;
                case SystemResourceKeyID.ControlDarkDarkBrush:
                    return ControlDarkDarkBrush;
                case SystemResourceKeyID.ControlDarkDarkColor:
                    return ControlDarkDarkColor;
                case SystemResourceKeyID.ControlLightBrush:
                    return ControlLightBrush;
                case SystemResourceKeyID.ControlLightColor:
                    return ControlLightColor;
                case SystemResourceKeyID.ControlLightLightBrush:
                    return ControlLightLightBrush;
                case SystemResourceKeyID.ControlLightLightColor:
                    return ControlLightLightColor;
                case SystemResourceKeyID.ControlTextBrush:
                    return ControlTextBrush;
                case SystemResourceKeyID.ControlTextColor:
                    return ControlTextColor;
                //case SystemResourceKeyID.CursorHeight:
                //case SystemResourceKeyID.CursorShadow:
                //case SystemResourceKeyID.CursorWidth:
                case SystemResourceKeyID.DesktopBrush:
                    return DesktopBrush;
                case SystemResourceKeyID.DesktopColor:
                    return DesktopColor;
                //case SystemResourceKeyID.DragFullWindows:
                //case SystemResourceKeyID.DropShadow:
                //case SystemResourceKeyID.FixedFrameHorizontalBorderHeight:
                //case SystemResourceKeyID.FixedFrameVerticalBorderWidth:
                //case SystemResourceKeyID.FlatMenu:
                //case SystemResourceKeyID.FocusBorderHeight:
                //case SystemResourceKeyID.FocusBorderWidth:
                //case SystemResourceKeyID.FocusHorizontalBorderHeight:
                //case SystemResourceKeyID.FocusVerticalBorderWidth:
                //case SystemResourceKeyID.FocusVisualStyle:
                //case SystemResourceKeyID.ForegroundFlashCount:
                //case SystemResourceKeyID.FullPrimaryScreenHeight:
                //case SystemResourceKeyID.FullPrimaryScreenWidth:
                case SystemResourceKeyID.GradientActiveCaptionBrush:
                    return GradientActiveCaptionBrush;
                case SystemResourceKeyID.GradientActiveCaptionColor:
                    return GradientActiveCaptionColor;
                //case SystemResourceKeyID.GradientCaptions:
                case SystemResourceKeyID.GradientInactiveCaptionBrush:
                    return GradientInactiveCaptionBrush;
                case SystemResourceKeyID.GradientInactiveCaptionColor:
                    return GradientInactiveCaptionColor;
                case SystemResourceKeyID.GrayTextBrush:
                    return GrayTextBrush;
                case SystemResourceKeyID.GrayTextColor:
                    return GrayTextColor;
                //case SystemResourceKeyID.GridViewItemContainerStyle:
                //case SystemResourceKeyID.GridViewScrollViewerStyle:
                //case SystemResourceKeyID.GridViewStyle:
                case SystemResourceKeyID.HighContrast:
                    return HighContrast;
                case SystemResourceKeyID.HighlightBrush:
                    return HighlightBrush;
                case SystemResourceKeyID.HighlightColor:
                    return HighlightColor;
                case SystemResourceKeyID.HighlightTextBrush:
                    return HighlightTextBrush;
                case SystemResourceKeyID.HighlightTextColor:
                    return HighlightTextColor;
                //case SystemResourceKeyID.HorizontalScrollBarButtonWidth:
                //case SystemResourceKeyID.HorizontalScrollBarHeight:
                //case SystemResourceKeyID.HorizontalScrollBarThumbWidth:
                case SystemResourceKeyID.HotTrackBrush:
                    return HotTrackBrush;
                case SystemResourceKeyID.HotTrackColor:
                    return HotTrackColor;
                //case SystemResourceKeyID.HotTracking:
                case SystemResourceKeyID.IconFontFamily:
                    return IconFontFamily;
                case SystemResourceKeyID.IconFontSize:
                    return IconFontSize;
                case SystemResourceKeyID.IconFontStyle:
                    return IconFontStyle;
                case SystemResourceKeyID.IconFontTextDecorations:
                    return IconFontTextDecorations;
                case SystemResourceKeyID.IconFontWeight:
                    return IconFontWeight;
                //case SystemResourceKeyID.IconGridHeight:
                //case SystemResourceKeyID.IconGridWidth:
                //case SystemResourceKeyID.IconHeight:
                //case SystemResourceKeyID.IconHorizontalSpacing:
                //case SystemResourceKeyID.IconTitleWrap:
                //case SystemResourceKeyID.IconVerticalSpacing:
                //case SystemResourceKeyID.IconWidth:
                case SystemResourceKeyID.InactiveBorderBrush:
                    return InactiveBorderBrush;
                case SystemResourceKeyID.InactiveBorderColor:
                    return InactiveBorderColor;
                case SystemResourceKeyID.InactiveCaptionBrush:
                    return InactiveCaptionBrush;
                case SystemResourceKeyID.InactiveCaptionColor:
                    return InactiveCaptionColor;
                case SystemResourceKeyID.InactiveCaptionTextBrush:
                    return InactiveCaptionTextBrush;
                case SystemResourceKeyID.InactiveCaptionTextColor:
                    return InactiveCaptionTextColor;
                case SystemResourceKeyID.InactiveSelectionHighlightBrush:
                    return InactiveSelectionHighlightBrush;
                case SystemResourceKeyID.InactiveSelectionHighlightTextBrush:
                    return InactiveSelectionHighlightTextBrush;
                case SystemResourceKeyID.InfoBrush:
                    return InfoBrush;
                case SystemResourceKeyID.InfoColor:
                    return InfoColor;
                case SystemResourceKeyID.InfoTextBrush:
                    return InfoTextBrush;
                case SystemResourceKeyID.InfoTextColor:
                    return InfoTextColor;
                //case SystemResourceKeyID.InternalSystemColorsEnd:
                //case SystemResourceKeyID.InternalSystemColorsExtendedEnd:
                //case SystemResourceKeyID.InternalSystemColorsExtendedStart:
                //case SystemResourceKeyID.InternalSystemColorsStart:
                //case SystemResourceKeyID.InternalSystemFontsEnd:
                //case SystemResourceKeyID.InternalSystemFontsStart:
                //case SystemResourceKeyID.InternalSystemParametersEnd:
                //case SystemResourceKeyID.InternalSystemParametersStart:
                //case SystemResourceKeyID.InternalSystemThemeStylesEnd:
                //case SystemResourceKeyID.InternalSystemThemeStylesStart:
                //case SystemResourceKeyID.IsImmEnabled:
                //case SystemResourceKeyID.IsMediaCenter:
                //case SystemResourceKeyID.IsMenuDropRightAligned:
                //case SystemResourceKeyID.IsMiddleEastEnabled:
                //case SystemResourceKeyID.IsMousePresent:
                //case SystemResourceKeyID.IsMouseWheelPresent:
                //case SystemResourceKeyID.IsPenWindows:
                //case SystemResourceKeyID.IsRemotelyControlled:
                //case SystemResourceKeyID.IsRemoteSession:
                //case SystemResourceKeyID.IsSlowMachine:
                //case SystemResourceKeyID.IsTabletPC:
                //case SystemResourceKeyID.KanjiWindowHeight:
                //case SystemResourceKeyID.KeyboardCues:
                //case SystemResourceKeyID.KeyboardDelay:
                //case SystemResourceKeyID.KeyboardPreference:
                //case SystemResourceKeyID.KeyboardSpeed:
                //case SystemResourceKeyID.ListBoxSmoothScrolling:
                //case SystemResourceKeyID.MaximizedPrimaryScreenHeight:
                //case SystemResourceKeyID.MaximizedPrimaryScreenWidth:
                //case SystemResourceKeyID.MaximumWindowTrackHeight:
                //case SystemResourceKeyID.MaximumWindowTrackWidth:
                //case SystemResourceKeyID.MenuAnimation:
                case SystemResourceKeyID.MenuBarBrush:
                    return MenuBarBrush;
                case SystemResourceKeyID.MenuBarColor:
                    return MenuBarColor;
                //case SystemResourceKeyID.MenuBarHeight:
                case SystemResourceKeyID.MenuBrush:
                    return MenuBrush;
                //case SystemResourceKeyID.MenuButtonHeight:
                //case SystemResourceKeyID.MenuButtonWidth:
                //case SystemResourceKeyID.MenuCheckmarkHeight:
                //case SystemResourceKeyID.MenuCheckmarkWidth:
                case SystemResourceKeyID.MenuColor:
                    return MenuColor;
                //case SystemResourceKeyID.MenuDropAlignment:
                //case SystemResourceKeyID.MenuFade:
                case SystemResourceKeyID.MenuFontFamily:
                    return MenuFontFamily;
                case SystemResourceKeyID.MenuFontSize:
                    return MenuFontSize;
                case SystemResourceKeyID.MenuFontStyle:
                    return MenuFontStyle;
                case SystemResourceKeyID.MenuFontTextDecorations:
                    return MenuFontTextDecorations;
                case SystemResourceKeyID.MenuFontWeight:
                    return MenuFontWeight;
                case SystemResourceKeyID.MenuHeight:
                    return MenuHeight;
                case SystemResourceKeyID.MenuHighlightBrush:
                    return MenuHighlightBrush;
                case SystemResourceKeyID.MenuHighlightColor:
                    return MenuHighlightColor;
                //case SystemResourceKeyID.MenuItemSeparatorStyle:
                //case SystemResourceKeyID.MenuPopupAnimation:
                //case SystemResourceKeyID.MenuShowDelay:
                case SystemResourceKeyID.MenuTextBrush:
                    return MenuTextBrush;
                case SystemResourceKeyID.MenuTextColor:
                    return MenuTextColor;
                case SystemResourceKeyID.MenuWidth:
                    return MenuWidth;
                case SystemResourceKeyID.MessageFontFamily:
                    return MessageFontFamily;
                case SystemResourceKeyID.MessageFontSize:
                    return MessageFontSize;
                case SystemResourceKeyID.MessageFontStyle:
                    return MessageFontStyle;
                case SystemResourceKeyID.MessageFontTextDecorations:
                    return MessageFontTextDecorations;
                case SystemResourceKeyID.MessageFontWeight:
                    return MessageFontWeight;
                //case SystemResourceKeyID.MinimizeAnimation:
                //case SystemResourceKeyID.MinimizedGridHeight:
                //case SystemResourceKeyID.MinimizedGridWidth:
                //case SystemResourceKeyID.MinimizedWindowHeight:
                //case SystemResourceKeyID.MinimizedWindowWidth:
                //case SystemResourceKeyID.MinimumWindowHeight:
                //case SystemResourceKeyID.MinimumWindowTrackHeight:
                //case SystemResourceKeyID.MinimumWindowTrackWidth:
                //case SystemResourceKeyID.MinimumWindowWidth:
                //case SystemResourceKeyID.MouseHoverHeight:
                //case SystemResourceKeyID.MouseHoverTime:
                //case SystemResourceKeyID.MouseHoverWidth:
                //case SystemResourceKeyID.NavigationChromeDownLevelStyle:
                //case SystemResourceKeyID.NavigationChromeStyle:
                //case SystemResourceKeyID.PowerLineStatus:
                //case SystemResourceKeyID.PrimaryScreenHeight:
                //case SystemResourceKeyID.PrimaryScreenWidth:
                //case SystemResourceKeyID.ResizeFrameHorizontalBorderHeight:
                //case SystemResourceKeyID.ResizeFrameVerticalBorderWidth:
                case SystemResourceKeyID.ScrollBarBrush:
                    return ScrollBarBrush;
                case SystemResourceKeyID.ScrollBarColor:
                    return ScrollBarColor;
                case SystemResourceKeyID.ScrollHeight:
                    return ScrollHeight;
                case SystemResourceKeyID.ScrollWidth:
                    return ScrollWidth;
                //case SystemResourceKeyID.SelectionFade:
                //case SystemResourceKeyID.ShowSounds:
                case SystemResourceKeyID.SmallCaptionFontFamily:
                    return SmallCaptionFontFamily;
                case SystemResourceKeyID.SmallCaptionFontSize:
                    return SmallCaptionFontSize;
                case SystemResourceKeyID.SmallCaptionFontStyle:
                    return SmallCaptionFontStyle;
                case SystemResourceKeyID.SmallCaptionFontTextDecorations:
                    return SmallCaptionFontTextDecorations;
                case SystemResourceKeyID.SmallCaptionFontWeight:
                    return SmallCaptionFontWeight;
                case SystemResourceKeyID.SmallCaptionHeight:
                    return SmallCaptionHeight;
                case SystemResourceKeyID.SmallCaptionWidth:
                    return SmallCaptionWidth;
                //case SystemResourceKeyID.SmallIconHeight:
                //case SystemResourceKeyID.SmallIconWidth:
                //case SystemResourceKeyID.SmallWindowCaptionButtonHeight:
                //case SystemResourceKeyID.SmallWindowCaptionButtonWidth:
                //case SystemResourceKeyID.SnapToDefaultButton:
                //case SystemResourceKeyID.StatusBarSeparatorStyle:
                case SystemResourceKeyID.StatusFontFamily:
                    return StatusFontFamily;
                case SystemResourceKeyID.StatusFontSize:
                    return StatusFontSize;
                case SystemResourceKeyID.StatusFontStyle:
                    return StatusFontStyle;
                case SystemResourceKeyID.StatusFontTextDecorations:
                    return StatusFontTextDecorations;
                case SystemResourceKeyID.StatusFontWeight:
                    return StatusFontWeight;
                //case SystemResourceKeyID.StylusHotTracking:
                //case SystemResourceKeyID.SwapButtons:
                //case SystemResourceKeyID.ThickHorizontalBorderHeight:
                //case SystemResourceKeyID.ThickVerticalBorderWidth:
                //case SystemResourceKeyID.ThinHorizontalBorderHeight:
                //case SystemResourceKeyID.ThinVerticalBorderWidth:
                //case SystemResourceKeyID.ToolBarButtonStyle:
                //case SystemResourceKeyID.ToolBarCheckBoxStyle:
                //case SystemResourceKeyID.ToolBarComboBoxStyle:
                //case SystemResourceKeyID.ToolBarMenuStyle:
                //case SystemResourceKeyID.ToolBarRadioButtonStyle:
                //case SystemResourceKeyID.ToolBarSeparatorStyle:
                //case SystemResourceKeyID.ToolBarTextBoxStyle:
                //case SystemResourceKeyID.ToolBarToggleButtonStyle:
                //case SystemResourceKeyID.ToolTipAnimation:
                //case SystemResourceKeyID.ToolTipFade:
                //case SystemResourceKeyID.ToolTipPopupAnimation:
                //case SystemResourceKeyID.UIEffects:
                //case SystemResourceKeyID.VerticalScrollBarButtonHeight:
                //case SystemResourceKeyID.VerticalScrollBarThumbHeight:
                //case SystemResourceKeyID.VerticalScrollBarWidth:
                //case SystemResourceKeyID.VirtualScreenHeight:
                //case SystemResourceKeyID.VirtualScreenLeft:
                //case SystemResourceKeyID.VirtualScreenTop:
                //case SystemResourceKeyID.VirtualScreenWidth:
                //case SystemResourceKeyID.WheelScrollLines:
                case SystemResourceKeyID.WindowBrush:
                    return WindowBrush;
                //case SystemResourceKeyID.WindowCaptionButtonHeight:
                //case SystemResourceKeyID.WindowCaptionButtonWidth:
                //case SystemResourceKeyID.WindowCaptionHeight:
                case SystemResourceKeyID.WindowColor:
                    return WindowColor;
                case SystemResourceKeyID.WindowFrameBrush:
                    return WindowFrameBrush;
                case SystemResourceKeyID.WindowFrameColor:
                    return WindowFrameColor;
                case SystemResourceKeyID.WindowTextBrush:
                    return WindowTextBrush;
                case SystemResourceKeyID.WindowTextColor:
                    return WindowTextColor;
                //case SystemResourceKeyID.WorkArea:                    
                default:
                    return null;
            }
        }

        internal void OnThemeChanged()
        {
            lock (_themeLock)
            {
                var themeInfo = GetCurrentTheme();
                if (themeInfo != _themeInfo)
                {
                    _themeInfo = themeInfo;
                    ThemeChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private unsafe ThemeInfo? GetCurrentTheme()
        {
            char[] themeFileName = new char[260];
            char[] themeColor = new char[260];
            char[] themeSize = new char[260];
            fixed (char* c1 = &themeFileName[0])
            {
                fixed (char* c2 = &themeColor[0])
                {
                    fixed (char* c3 = &themeSize[0])
                    {
                        PWSTR themeFileNamePtr = new PWSTR(c1);
                        PWSTR themeColorPtr = new PWSTR(c2);
                        PWSTR themeSizePtr = new PWSTR(c3);
                        var result = PInvoke.GetCurrentThemeName(themeFileNamePtr, 260, themeColorPtr, 260, themeSizePtr, 260);
                        if (result.Succeeded)
                        {
                            return new ThemeInfo(Path.GetFileNameWithoutExtension(themeFileNamePtr.ToString()), themeColorPtr.ToString(), themeSizePtr.ToString());
                        }
                        return null;
                    }
                }
            }
        }

        private record class ThemeInfo
        {
            public ThemeInfo(string name, string color, string size)
            {
                Name = name;
                Color = color;
                Size = size;
            }

            public string Name;

            public string Color;

            public string Size;
        }
    }
}
