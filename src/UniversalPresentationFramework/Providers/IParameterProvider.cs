using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Providers
{
    public interface IParameterProvider
    {
        #region Window Parameters

        bool MinimizeAnimation { get; }
        int Border { get; }
        float CaretWidth { get; }
        bool DragFullWindows { get; }
        int ForegroundFlashCount { get; }
        float BorderWidth { get; }
        float ScrollWidth { get; }
        float ScrollHeight { get; }
        float CaptionWidth { get; }
        float CaptionHeight { get; }
        float SmallCaptionWidth { get; }
        float SmallCaptionHeight { get; }
        float MenuWidth { get; }
        float MenuHeight { get; }

        #endregion

        #region Font Parameters

        float IconFontSize { get; }
        FontFamily IconFontFamily { get; }
        FontStyle IconFontStyle { get; }
        FontWeight FontWeight { get; }
        TextDecorationCollection IconFontTextDecorations { get; }
        float CaptionFontSize { get; }
        FontFamily CaptionFontFamily { get; }
        FontStyle CaptionFontStyle { get; }
        FontWeight CaptionFontWeight { get; }
        TextDecorationCollection CaptionFontTextDecorations { get; }
        float SmallCaptionFontSize { get; set; }
        FontFamily SmallCaptionFontFamily { get; }
        FontStyle SmallCaptionFontStyle { get; }
        FontWeight SmallCaptionFontWeight { get; }
        TextDecorationCollection SmallCaptionFontTextDecorations { get; }
        float MenuFontSize { get; }
        FontFamily MenuFontFamily { get; }
        FontStyle MenuFontStyle { get; }
        FontWeight MenuFontWeight { get; }
        TextDecorationCollection MenuFontTextDecorations { get; }
        float StatusFontSize { get; }
        FontFamily StatusFontFamily { get; }
        FontStyle StatusFontStyle { get; }
        FontWeight StatusFontWeight { get; }
        TextDecorationCollection StatusFontTextDecorations { get; }
        float MessageFontSize { get; }
        FontFamily MessageFontFamily { get; }
        FontStyle MessageFontStyle { get; }
        FontWeight MessageFontWeight { get; }
        TextDecorationCollection MessageFontTextDecorations { get; }

        #endregion
    }
}
