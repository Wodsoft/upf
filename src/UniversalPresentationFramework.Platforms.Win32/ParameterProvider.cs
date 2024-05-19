using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Providers;
using Windows.Win32;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.WindowsAndMessaging;
using Wodsoft.UI.Controls;
using Wodsoft.UI.Media;
using System.Collections;

namespace Wodsoft.UI.Platforms.Win32
{
    public partial class ThemeProvider : IParameterProvider
    {
        #region Window Parameters

        public bool MinimizeAnimation => throw new NotImplementedException();

        private int? _border;
        public unsafe int Border
        {
            get
            {
                if (_border == null)
                {
                    int border = 0;
                    PInvoke.SystemParametersInfo(SYSTEM_PARAMETERS_INFO_ACTION.SPI_GETBORDER, 0, &border, 0);
                    _border = border;
                }
                return _border.Value;
            }
        }

        public float CaretWidth => throw new NotImplementedException();

        public bool DragFullWindows => throw new NotImplementedException();

        public int ForegroundFlashCount => throw new NotImplementedException();

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
                    PInvoke.SystemParametersInfoForDpi((uint)SYSTEM_PARAMETERS_INFO_ACTION.SPI_GETNONCLIENTMETRICS, clientMetrics.cbSize, &clientMetrics, 0, 96);
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

        public float IconFontSize => throw new NotImplementedException();

        public FontFamily IconFontFamily => throw new NotImplementedException();

        public FontStyle IconFontStyle => throw new NotImplementedException();

        public FontWeight IconFontWeight => throw new NotImplementedException();

        public TextDecorationCollection IconFontTextDecorations => throw new NotImplementedException();

        public float CaptionFontSize => throw new NotImplementedException();

        public FontFamily CaptionFontFamily => throw new NotImplementedException();

        public FontStyle CaptionFontStyle => throw new NotImplementedException();

        public FontWeight CaptionFontWeight => throw new NotImplementedException();

        public TextDecorationCollection CaptionFontTextDecorations => throw new NotImplementedException();

        public float SmallCaptionFontSize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public FontFamily SmallCaptionFontFamily => throw new NotImplementedException();

        public FontStyle SmallCaptionFontStyle => throw new NotImplementedException();

        public FontWeight SmallCaptionFontWeight => throw new NotImplementedException();

        public TextDecorationCollection SmallCaptionFontTextDecorations => throw new NotImplementedException();

        public float MenuFontSize => throw new NotImplementedException();

        public FontFamily MenuFontFamily => throw new NotImplementedException();

        public FontStyle MenuFontStyle => throw new NotImplementedException();

        public FontWeight MenuFontWeight => throw new NotImplementedException();

        public TextDecorationCollection MenuFontTextDecorations => throw new NotImplementedException();

        public float StatusFontSize => throw new NotImplementedException();

        public FontFamily StatusFontFamily => throw new NotImplementedException();

        public FontStyle StatusFontStyle => throw new NotImplementedException();

        public FontWeight StatusFontWeight => throw new NotImplementedException();

        public TextDecorationCollection StatusFontTextDecorations => throw new NotImplementedException();

        public float MessageFontSize => -ClientMetrics.lfMessageFont.lfHeight;

        public FontFamily MessageFontFamily => new FontFamily(ClientMetrics.lfMessageFont.lfFaceName.ToString());

        public FontStyle MessageFontStyle => ClientMetrics.lfMessageFont.lfItalic != 0 ? FontStyles.Italic : FontStyles.Normal;

        public FontWeight MessageFontWeight => FontWeight.FromOpenTypeWeight(ClientMetrics.lfMessageFont.lfWeight);

        public TextDecorationCollection MessageFontTextDecorations => throw new NotImplementedException();

        #endregion

        #region Accessibility Parameters

        public float FocusBorderWidth => throw new NotImplementedException();

        public float FocusBorderHeight => throw new NotImplementedException();

        public bool HighContrast => throw new NotImplementedException();

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
    }
}
