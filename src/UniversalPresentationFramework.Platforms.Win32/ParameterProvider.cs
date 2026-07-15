using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.WindowsAndMessaging;
using Wodsoft.UI.Media;
using Wodsoft.UI.Providers;

namespace Wodsoft.UI.Platforms.Win32
{
    public partial class ThemeProvider : IParameterProvider
    {
        #region Window Parameters

        private bool? _minimizeAnimation;
        public unsafe bool MinimizeAnimation
        {
            get
            {
                if (_minimizeAnimation == null)
                {
                    var animInfo = new ANIMATIONINFO();
                    animInfo.cbSize = (uint)sizeof(ANIMATIONINFO);
                    if (!PInvoke.SystemParametersInfo(SYSTEM_PARAMETERS_INFO_ACTION.SPI_GETANIMATION, animInfo.cbSize, &animInfo, 0))
                        throw new Win32Exception();
                    _minimizeAnimation = animInfo.iMinAnimate != 0;
                }
                return _minimizeAnimation.Value;
            }
        }

        private int? _border;
        public unsafe int Border
        {
            get
            {
                if (_border == null)
                {
                    int border = 0;
                    if (!PInvoke.SystemParametersInfo(SYSTEM_PARAMETERS_INFO_ACTION.SPI_GETBORDER, 0, &border, 0))
                        throw new Win32Exception();
                    _border = border;
                }
                return _border.Value;
            }
        }

        private float? _caretWidth;
        public unsafe float CaretWidth
        {
            get
            {
                if (_caretWidth == null)
                {
                    // OS does not scale SPI_GETCARETWIDTH to the primary monitor's DPI.
                    int caretWidth = 0;
                    if (!PInvoke.SystemParametersInfo(SYSTEM_PARAMETERS_INFO_ACTION.SPI_GETCARETWIDTH, 0, &caretWidth, 0))
                        throw new Win32Exception();
                    _caretWidth = caretWidth;
                }
                return _caretWidth.Value;
            }
        }

        private bool? _dragFullWindows;
        public bool DragFullWindows => GetSystemParameterBool(SYSTEM_PARAMETERS_INFO_ACTION.SPI_GETDRAGFULLWINDOWS, ref _dragFullWindows);

        private int? _foregroundFlashCount;
        public int ForegroundFlashCount => GetSystemParameterInt(SYSTEM_PARAMETERS_INFO_ACTION.SPI_GETFOREGROUNDFLASHCOUNT, ref _foregroundFlashCount);

        public float BorderWidth => ClientMetrics.iBorderWidth;

        public float ScrollWidth => ClientMetrics.iScrollWidth;

        public float ScrollHeight => ClientMetrics.iScrollHeight;

        private NONCLIENTMETRICSW? _clientMetrics;
        private unsafe NONCLIENTMETRICSW ClientMetrics
        {
            get
            {
                if (_clientMetrics == null)
                {
                    var clientMetrics = new NONCLIENTMETRICSW();
                    clientMetrics.cbSize = (uint)sizeof(NONCLIENTMETRICSW);
#pragma warning disable CA1416 // 验证平台兼容性
                    if (!PInvoke.SystemParametersInfoForDpi((uint)SYSTEM_PARAMETERS_INFO_ACTION.SPI_GETNONCLIENTMETRICS, clientMetrics.cbSize, &clientMetrics, 0, 96))
                        throw new Win32Exception();
#pragma warning restore CA1416 // 验证平台兼容性
                    _clientMetrics = clientMetrics;
                }
                return _clientMetrics.Value;
            }
        }

        public float CaptionWidth => ClientMetrics.iCaptionWidth;

        public float CaptionHeight => ClientMetrics.iCaptionHeight;

        public float SmallCaptionWidth => ClientMetrics.iSmCaptionWidth;

        public float SmallCaptionHeight => ClientMetrics.iSmCaptionHeight;

        public float MenuWidth => ClientMetrics.iMenuWidth;

        public float MenuHeight => ClientMetrics.iMenuHeight;

        #endregion

        #region Font Parameters

        private ICONMETRICSW? _iconMetrics;
        private unsafe ICONMETRICSW IconMetrics
        {
            get
            {
                if (_iconMetrics == null)
                {
                    var iconMetrics = new ICONMETRICSW();
                    iconMetrics.cbSize = (uint)sizeof(ICONMETRICSW);
#pragma warning disable CA1416 // 验证平台兼容性
                    if (!PInvoke.SystemParametersInfoForDpi((uint)SYSTEM_PARAMETERS_INFO_ACTION.SPI_GETICONMETRICS, iconMetrics.cbSize, &iconMetrics, 0, 96))
                        throw new Win32Exception();
#pragma warning restore CA1416 // 验证平台兼容性
                    _iconMetrics = iconMetrics;
                }
                return _iconMetrics.Value;
            }
        }

        public float IconFontSize => GetFontSize(IconMetrics.lfFont);

        public FontFamily IconFontFamily => GetFontFamily(IconMetrics.lfFont);

        public FontStyle IconFontStyle => GetFontStyle(IconMetrics.lfFont);

        public FontWeight IconFontWeight => GetFontWeight(IconMetrics.lfFont);

        public TextDecorationCollection IconFontTextDecorations => GetFontTextDecorations(IconMetrics.lfFont);

        public float CaptionFontSize => GetFontSize(ClientMetrics.lfCaptionFont);

        public FontFamily CaptionFontFamily => GetFontFamily(ClientMetrics.lfCaptionFont);

        public FontStyle CaptionFontStyle => GetFontStyle(ClientMetrics.lfCaptionFont);

        public FontWeight CaptionFontWeight => GetFontWeight(ClientMetrics.lfCaptionFont);

        public TextDecorationCollection CaptionFontTextDecorations => GetFontTextDecorations(ClientMetrics.lfCaptionFont);

        public float SmallCaptionFontSize
        {
            get => GetFontSize(ClientMetrics.lfSmCaptionFont);
            set => throw new NotSupportedException("SmallCaptionFontSize is read-only system parameter.");
        }

        public FontFamily SmallCaptionFontFamily => GetFontFamily(ClientMetrics.lfSmCaptionFont);

        public FontStyle SmallCaptionFontStyle => GetFontStyle(ClientMetrics.lfSmCaptionFont);

        public FontWeight SmallCaptionFontWeight => GetFontWeight(ClientMetrics.lfSmCaptionFont);

        public TextDecorationCollection SmallCaptionFontTextDecorations => GetFontTextDecorations(ClientMetrics.lfSmCaptionFont);

        public float MenuFontSize => GetFontSize(ClientMetrics.lfMenuFont);

        public FontFamily MenuFontFamily => GetFontFamily(ClientMetrics.lfMenuFont);

        public FontStyle MenuFontStyle => GetFontStyle(ClientMetrics.lfMenuFont);

        public FontWeight MenuFontWeight => GetFontWeight(ClientMetrics.lfMenuFont);

        public TextDecorationCollection MenuFontTextDecorations => GetFontTextDecorations(ClientMetrics.lfMenuFont);

        public float StatusFontSize => GetFontSize(ClientMetrics.lfStatusFont);

        public FontFamily StatusFontFamily => GetFontFamily(ClientMetrics.lfStatusFont);

        public FontStyle StatusFontStyle => GetFontStyle(ClientMetrics.lfStatusFont);

        public FontWeight StatusFontWeight => GetFontWeight(ClientMetrics.lfStatusFont);

        public TextDecorationCollection StatusFontTextDecorations => GetFontTextDecorations(ClientMetrics.lfStatusFont);

        public float MessageFontSize => GetFontSize(ClientMetrics.lfMessageFont);

        public FontFamily MessageFontFamily => GetFontFamily(ClientMetrics.lfMessageFont);

        public FontStyle MessageFontStyle => GetFontStyle(ClientMetrics.lfMessageFont);

        public FontWeight MessageFontWeight => GetFontWeight(ClientMetrics.lfMessageFont);

        public TextDecorationCollection MessageFontTextDecorations => GetFontTextDecorations(ClientMetrics.lfMessageFont);

        private static float GetFontSize(in LOGFONTW logFont) => Math.Abs(logFont.lfHeight);

        private static FontFamily GetFontFamily(in LOGFONTW logFont) => new FontFamily(logFont.lfFaceName.ToString());

        private static FontStyle GetFontStyle(in LOGFONTW logFont) => logFont.lfItalic != 0 ? FontStyles.Italic : FontStyles.Normal;

        private static FontWeight GetFontWeight(in LOGFONTW logFont) => FontWeight.FromOpenTypeWeight(logFont.lfWeight);

        private static TextDecorationCollection GetFontTextDecorations(in LOGFONTW logFont)
        {
            bool underline = logFont.lfUnderline != 0;
            bool strikeOut = logFont.lfStrikeOut != 0;
            if (!underline && !strikeOut)
                return TextDecorationCollection.Empty;

            var collection = new TextDecorationCollection();
            if (underline)
            {
                foreach (TextDecoration decoration in TextDecorations.Underline)
                    collection.Add((TextDecoration)decoration.Clone());
            }
            if (strikeOut)
            {
                foreach (TextDecoration decoration in TextDecorations.Strikethrough)
                    collection.Add((TextDecoration)decoration.Clone());
            }
            collection.Freeze();
            return collection;
        }

        #endregion

        #region Accessibility Parameters

        private float? _focusBorderWidth;
        public unsafe float FocusBorderWidth
        {
            get
            {
                if (_focusBorderWidth == null)
                {
                    int focusBorderWidth = 0;
                    if (!PInvoke.SystemParametersInfo(SYSTEM_PARAMETERS_INFO_ACTION.SPI_GETFOCUSBORDERWIDTH, 0, &focusBorderWidth, 0))
                        throw new Win32Exception();
                    _focusBorderWidth = ConvertPixel(focusBorderWidth);
                }
                return _focusBorderWidth.Value;
            }
        }

        private float? _focusBorderHeight;
        public unsafe float FocusBorderHeight
        {
            get
            {
                if (_focusBorderHeight == null)
                {
                    int focusBorderHeight = 0;
                    if (!PInvoke.SystemParametersInfo(SYSTEM_PARAMETERS_INFO_ACTION.SPI_GETFOCUSBORDERHEIGHT, 0, &focusBorderHeight, 0))
                        throw new Win32Exception();
                    _focusBorderHeight = ConvertPixel(focusBorderHeight);
                }
                return _focusBorderHeight.Value;
            }
        }

        private bool? _highContrast;
        public unsafe bool HighContrast
        {
            get
            {
                if (_highContrast == null)
                {
                    var highContrast = new HIGHCONTRAST_I();
                    highContrast.cbSize = (uint)sizeof(HIGHCONTRAST_I);
                    if (!PInvoke.SystemParametersInfo(SYSTEM_PARAMETERS_INFO_ACTION.SPI_GETHIGHCONTRAST, highContrast.cbSize, &highContrast, 0))
                        throw new Win32Exception();
                    _highContrast = (highContrast.dwFlags & HCF_HIGHCONTRASTON) == HCF_HIGHCONTRASTON;
                }
                return _highContrast.Value;
            }
        }

        #endregion

        #region System Colors

        public Color ActiveBorderColor => GetColor(SYS_COLOR_INDEX.COLOR_ACTIVEBORDER);

        public Color ActiveCaptionColor => GetColor(SYS_COLOR_INDEX.COLOR_ACTIVECAPTION);

        public Color ActiveCaptionTextColor => GetColor(SYS_COLOR_INDEX.COLOR_CAPTIONTEXT);

        public Color AppWorkspaceColor => GetColor(SYS_COLOR_INDEX.COLOR_APPWORKSPACE);

        public Color ControlColor => GetColor(SYS_COLOR_INDEX.COLOR_BTNFACE);

        public Color ControlDarkColor => GetColor(SYS_COLOR_INDEX.COLOR_BTNSHADOW);

        public Color ControlDarkDarkColor => GetColor(SYS_COLOR_INDEX.COLOR_3DDKSHADOW);

        public Color ControlLightColor => GetColor(SYS_COLOR_INDEX.COLOR_3DLIGHT);

        public Color ControlLightLightColor => GetColor(SYS_COLOR_INDEX.COLOR_BTNHIGHLIGHT);

        public Color ControlTextColor => GetColor(SYS_COLOR_INDEX.COLOR_BTNTEXT);

        public Color DesktopColor => GetColor(SYS_COLOR_INDEX.COLOR_DESKTOP);

        public Color GradientActiveCaptionColor => GetColor(SYS_COLOR_INDEX.COLOR_GRADIENTACTIVECAPTION);

        public Color GradientInactiveCaptionColor => GetColor(SYS_COLOR_INDEX.COLOR_GRADIENTINACTIVECAPTION);

        public Color GrayTextColor => GetColor(SYS_COLOR_INDEX.COLOR_GRAYTEXT);

        public Color HighlightColor => GetColor(SYS_COLOR_INDEX.COLOR_HIGHLIGHT);

        public Color HighlightTextColor => GetColor(SYS_COLOR_INDEX.COLOR_HIGHLIGHTTEXT);

        public Color HotTrackColor => GetColor(SYS_COLOR_INDEX.COLOR_HOTLIGHT);

        public Color InactiveBorderColor => GetColor(SYS_COLOR_INDEX.COLOR_INACTIVEBORDER);

        public Color InactiveCaptionColor => GetColor(SYS_COLOR_INDEX.COLOR_INACTIVECAPTION);

        public Color InactiveCaptionTextColor => GetColor(SYS_COLOR_INDEX.COLOR_INACTIVECAPTIONTEXT);

        public Color InfoColor => GetColor(SYS_COLOR_INDEX.COLOR_INFOBK);

        public Color InfoTextColor => GetColor(SYS_COLOR_INDEX.COLOR_INFOTEXT);

        public Color MenuColor => GetColor(SYS_COLOR_INDEX.COLOR_MENU);

        public Color MenuBarColor => GetColor(SYS_COLOR_INDEX.COLOR_MENUBAR);

        public Color MenuHighlightColor => GetColor(SYS_COLOR_INDEX.COLOR_MENUHILIGHT);

        public Color MenuTextColor => GetColor(SYS_COLOR_INDEX.COLOR_MENUTEXT);

        public Color ScrollBarColor => GetColor(SYS_COLOR_INDEX.COLOR_SCROLLBAR);

        public Color WindowColor => GetColor(SYS_COLOR_INDEX.COLOR_WINDOW);

        public Color WindowFrameColor => GetColor(SYS_COLOR_INDEX.COLOR_WINDOWFRAME);

        public Color WindowTextColor => GetColor(SYS_COLOR_INDEX.COLOR_WINDOWTEXT);

        public SolidColorBrush ActiveBorderBrush => GetBrush(SYS_COLOR_INDEX.COLOR_ACTIVEBORDER);

        public SolidColorBrush ActiveCaptionBrush => GetBrush(SYS_COLOR_INDEX.COLOR_ACTIVECAPTION);

        public SolidColorBrush ActiveCaptionTextBrush => GetBrush(SYS_COLOR_INDEX.COLOR_CAPTIONTEXT);

        public SolidColorBrush AppWorkspaceBrush => GetBrush(SYS_COLOR_INDEX.COLOR_APPWORKSPACE);

        public SolidColorBrush ControlBrush => GetBrush(SYS_COLOR_INDEX.COLOR_BTNFACE);

        public SolidColorBrush ControlDarkBrush => GetBrush(SYS_COLOR_INDEX.COLOR_BTNSHADOW);

        public SolidColorBrush ControlDarkDarkBrush => GetBrush(SYS_COLOR_INDEX.COLOR_3DDKSHADOW);

        public SolidColorBrush ControlLightBrush => GetBrush(SYS_COLOR_INDEX.COLOR_3DLIGHT);

        public SolidColorBrush ControlLightLightBrush => GetBrush(SYS_COLOR_INDEX.COLOR_BTNHIGHLIGHT);

        public SolidColorBrush ControlTextBrush => GetBrush(SYS_COLOR_INDEX.COLOR_BTNTEXT);

        public SolidColorBrush DesktopBrush => GetBrush(SYS_COLOR_INDEX.COLOR_DESKTOP);

        public SolidColorBrush GradientActiveCaptionBrush => GetBrush(SYS_COLOR_INDEX.COLOR_GRADIENTACTIVECAPTION);

        public SolidColorBrush GradientInactiveCaptionBrush => GetBrush(SYS_COLOR_INDEX.COLOR_GRADIENTINACTIVECAPTION);

        public SolidColorBrush GrayTextBrush => GetBrush(SYS_COLOR_INDEX.COLOR_GRAYTEXT);

        public SolidColorBrush HighlightBrush => GetBrush(SYS_COLOR_INDEX.COLOR_HIGHLIGHT);

        public SolidColorBrush HighlightTextBrush => GetBrush(SYS_COLOR_INDEX.COLOR_HIGHLIGHTTEXT);

        public SolidColorBrush HotTrackBrush => GetBrush(SYS_COLOR_INDEX.COLOR_HOTLIGHT);

        public SolidColorBrush InactiveBorderBrush => GetBrush(SYS_COLOR_INDEX.COLOR_INACTIVEBORDER);

        public SolidColorBrush InactiveCaptionBrush => GetBrush(SYS_COLOR_INDEX.COLOR_INACTIVECAPTION);

        public SolidColorBrush InactiveCaptionTextBrush => GetBrush(SYS_COLOR_INDEX.COLOR_INACTIVECAPTIONTEXT);

        public SolidColorBrush InfoBrush => GetBrush(SYS_COLOR_INDEX.COLOR_INFOBK);

        public SolidColorBrush InfoTextBrush => GetBrush(SYS_COLOR_INDEX.COLOR_INFOTEXT);

        public SolidColorBrush MenuBrush => GetBrush(SYS_COLOR_INDEX.COLOR_MENU);

        public SolidColorBrush MenuBarBrush => GetBrush(SYS_COLOR_INDEX.COLOR_MENUBAR);

        public SolidColorBrush MenuHighlightBrush => GetBrush(SYS_COLOR_INDEX.COLOR_MENUHILIGHT);

        public SolidColorBrush MenuTextBrush => GetBrush(SYS_COLOR_INDEX.COLOR_MENUTEXT);

        public SolidColorBrush ScrollBarBrush => GetBrush(SYS_COLOR_INDEX.COLOR_SCROLLBAR);

        public SolidColorBrush WindowBrush => GetBrush(SYS_COLOR_INDEX.COLOR_WINDOW);

        public SolidColorBrush WindowFrameBrush => GetBrush(SYS_COLOR_INDEX.COLOR_WINDOWFRAME);

        public SolidColorBrush WindowTextBrush => GetBrush(SYS_COLOR_INDEX.COLOR_WINDOWTEXT);

        public SolidColorBrush InactiveSelectionHighlightBrush => HighContrast ? HighlightBrush : ControlBrush;

        public SolidColorBrush InactiveSelectionHighlightTextBrush => HighContrast ? HighlightTextBrush : ControlTextBrush;

        private Color GetSystemColor(SYS_COLOR_INDEX index)
        {
            var color = PInvoke.GetSysColor(index);
            return new Color(255, (byte)(color & 0xff), (byte)((color >> 8) & 0xff), (byte)((color >> 16) & 0xff));
        }

        private BitArray _colorCacheValid = new BitArray((int)SYS_COLOR_INDEX.COLOR_MENUHILIGHT + 1);
        private Color[] _colorCache = new Color[(int)SYS_COLOR_INDEX.COLOR_MENUHILIGHT + 1];
        private BitArray _brushCacheValid = new BitArray((int)SYS_COLOR_INDEX.COLOR_MENUHILIGHT + 1);
        private SolidColorBrush[] _brushCache = new SolidColorBrush[(int)SYS_COLOR_INDEX.COLOR_MENUHILIGHT + 1];
        private Color GetColor(SYS_COLOR_INDEX index)
        {
            lock (_colorCacheValid)
            {
                var slot = (int)index;
                if (!_colorCacheValid[slot])
                {
                    _colorCacheValid[slot] = true;
                    var color = GetSystemColor(index);
                    _colorCache[slot] = color;
                    return color;
                }
                return _colorCache[slot];
            }
        }
        private SolidColorBrush GetBrush(SYS_COLOR_INDEX index)
        {
            lock (_brushCacheValid)
            {
                var slot = (int)index;
                if (!_brushCacheValid[slot])
                {
                    _brushCacheValid[slot] = true;
                    Color color;
                    if (_colorCacheValid[slot])
                        color = _colorCache[slot];
                    else
                        color = GetSystemColor(index);
                    var bursh = new SolidColorBrush(color);
                    _brushCache[slot] = bursh;
                    return bursh;
                }
                return _brushCache[slot];
            }
        }

        #endregion

        #region Input Parameters

        private bool? _keyboardCues;
        public bool KeyboardCues => GetSystemParameterBool(SYSTEM_PARAMETERS_INFO_ACTION.SPI_GETKEYBOARDCUES, ref _keyboardCues);

        private int? _keyboardDelay;
        public int KeyboardDelay => GetSystemParameterInt(SYSTEM_PARAMETERS_INFO_ACTION.SPI_GETKEYBOARDDELAY, ref _keyboardDelay);

        private bool? _keyboardPreference;
        public bool KeyboardPreference => GetSystemParameterBool(SYSTEM_PARAMETERS_INFO_ACTION.SPI_GETKEYBOARDPREF, ref _keyboardPreference);

        private int? _keyboardSpeed;
        public int KeyboardSpeed => GetSystemParameterInt(SYSTEM_PARAMETERS_INFO_ACTION.SPI_GETKEYBOARDSPEED, ref _keyboardSpeed);

        private bool? _snapToDefaultButton;
        public bool SnapToDefaultButton => GetSystemParameterBool(SYSTEM_PARAMETERS_INFO_ACTION.SPI_GETSNAPTODEFBUTTON, ref _snapToDefaultButton);

        private int? _wheelScrollLines;
        public int WheelScrollLines => GetSystemParameterInt(SYSTEM_PARAMETERS_INFO_ACTION.SPI_GETWHEELSCROLLLINES, ref _wheelScrollLines);

        public TimeSpan MouseHoverTime => TimeSpan.FromMilliseconds(MouseHoverTimeMilliseconds);

        private int? _mouseHoverTimeMilliseconds;
        internal int MouseHoverTimeMilliseconds => GetSystemParameterInt(SYSTEM_PARAMETERS_INFO_ACTION.SPI_GETMOUSEHOVERTIME, ref _mouseHoverTimeMilliseconds);

        private float? _mouseHoverHeight;
        public unsafe float MouseHoverHeight
        {
            get
            {
                if (_mouseHoverHeight == null)
                {
                    int mouseHoverHeight = 0;
                    if (!PInvoke.SystemParametersInfo(SYSTEM_PARAMETERS_INFO_ACTION.SPI_GETMOUSEHOVERHEIGHT, 0, &mouseHoverHeight, 0))
                        throw new Win32Exception();
                    _mouseHoverHeight = ConvertPixel(mouseHoverHeight);
                }
                return _mouseHoverHeight.Value;
            }
        }

        private float? _mouseHoverWidth;
        public unsafe float MouseHoverWidth
        {
            get
            {
                if (_mouseHoverWidth == null)
                {
                    int mouseHoverWidth = 0;
                    if (!PInvoke.SystemParametersInfo(SYSTEM_PARAMETERS_INFO_ACTION.SPI_GETMOUSEHOVERWIDTH, 0, &mouseHoverWidth, 0))
                        throw new Win32Exception();
                    _mouseHoverWidth = ConvertPixel(mouseHoverWidth);
                }
                return _mouseHoverWidth.Value;
            }
        }

        #endregion

        #region Helpers

        private unsafe bool GetSystemParameterBool(SYSTEM_PARAMETERS_INFO_ACTION action, ref bool? cache)
        {
            if (cache == null)
            {
                BOOL value = false;
                if (!PInvoke.SystemParametersInfo(action, 0, &value, 0))
                    throw new Win32Exception();
                cache = value;
            }
            return cache.Value;
        }

        private unsafe int GetSystemParameterInt(SYSTEM_PARAMETERS_INFO_ACTION action, ref int? cache)
        {
            if (cache == null)
            {
                int value = 0;
                if (!PInvoke.SystemParametersInfo(action, 0, &value, 0))
                    throw new Win32Exception();
                cache = value;
            }
            return cache.Value;
        }

        private unsafe float ConvertPixel(int pixel)
        {
            var dc = PInvoke.GetDC(HWND.Null);
            try
            {
                int dpi = PInvoke.GetDeviceCaps(dc, GET_DEVICE_CAPS_INDEX.LOGPIXELSX);
                if (dpi != 0)
                    return pixel * 96f / dpi;
                return pixel;
            }
            finally
            {
                PInvoke.ReleaseDC(HWND.Null, dc);
            }
        }

        // Mirrors WPF's HIGHCONTRAST_I layout for SPI_GETHIGHCONTRAST.
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct HIGHCONTRAST_I
        {
            public uint cbSize;
            public uint dwFlags;
            public IntPtr lpszDefaultScheme;
        }

        private const uint HCF_HIGHCONTRASTON = 0x00000001;

        #endregion
    }
}
