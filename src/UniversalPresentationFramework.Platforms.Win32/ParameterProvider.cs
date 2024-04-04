using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Providers;
using Windows.Win32;
using Windows.Win32.UI.WindowsAndMessaging;
using Wodsoft.UI.Controls;
using Wodsoft.UI.Media;

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
                    PInvoke.SystemParametersInfo(SYSTEM_PARAMETERS_INFO_ACTION.SPI_GETNONCLIENTMETRICS, clientMetrics.cbSize, &clientMetrics, 0);
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

        public FontWeight FontWeight => throw new NotImplementedException();

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

        public float MessageFontSize => throw new NotImplementedException();

        public FontFamily MessageFontFamily => throw new NotImplementedException();

        public FontStyle MessageFontStyle => throw new NotImplementedException();

        public FontWeight MessageFontWeight => throw new NotImplementedException();

        public TextDecorationCollection MessageFontTextDecorations => throw new NotImplementedException();

        #endregion
    }
}
