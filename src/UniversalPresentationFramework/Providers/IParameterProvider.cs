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
        FontWeight IconFontWeight { get; }
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

        #region Accessibility Parameters

        float FocusBorderWidth { get; }
        float FocusBorderHeight { get; }
        bool HighContrast { get; }

        #endregion

        #region System Colors

        Color ActiveBorderColor { get; }
        Color ActiveCaptionColor { get; }
        Color ActiveCaptionTextColor { get; }
        Color AppWorkspaceColor { get; }
        Color ControlColor { get; }
        Color ControlDarkColor { get; }
        Color ControlDarkDarkColor { get; }
        Color ControlLightColor { get; }
        Color ControlLightLightColor { get; }
        Color ControlTextColor { get; }
        Color DesktopColor { get; }
        Color GradientActiveCaptionColor { get; }
        Color GradientInactiveCaptionColor { get; }
        Color GrayTextColor { get; }
        Color HighlightColor { get; }
        Color HighlightTextColor { get; }
        Color HotTrackColor { get; }
        Color InactiveBorderColor { get; }
        Color InactiveCaptionColor { get; }
        Color InactiveCaptionTextColor { get; }
        Color InfoColor { get; }
        Color InfoTextColor { get; }
        Color MenuColor { get; }
        Color MenuBarColor { get; }
        Color MenuHighlightColor { get; }
        Color MenuTextColor { get; }
        Color ScrollBarColor { get; }
        Color WindowColor { get; }
        Color WindowFrameColor { get; }
        Color WindowTextColor { get; }
        SolidColorBrush ActiveBorderBrush { get; }
        SolidColorBrush ActiveCaptionBrush { get; }
        SolidColorBrush ActiveCaptionTextBrush { get; }
        SolidColorBrush AppWorkspaceBrush { get; }
        SolidColorBrush ControlBrush { get; }
        SolidColorBrush ControlDarkBrush { get; }
        SolidColorBrush ControlDarkDarkBrush { get; }
        SolidColorBrush ControlLightBrush { get; }
        SolidColorBrush ControlLightLightBrush { get; }
        SolidColorBrush ControlTextBrush { get; }
        SolidColorBrush DesktopBrush { get; }
        SolidColorBrush GradientActiveCaptionBrush { get; }
        SolidColorBrush GradientInactiveCaptionBrush { get; }
        SolidColorBrush GrayTextBrush { get; }
        SolidColorBrush HighlightBrush { get; }
        SolidColorBrush HighlightTextBrush { get; }
        SolidColorBrush HotTrackBrush { get; }
        SolidColorBrush InactiveBorderBrush { get; }
        SolidColorBrush InactiveCaptionBrush { get; }
        SolidColorBrush InactiveCaptionTextBrush { get; }
        SolidColorBrush InfoBrush { get; }
        SolidColorBrush InfoTextBrush { get; }
        SolidColorBrush MenuBrush { get; }
        SolidColorBrush MenuBarBrush { get; }
        SolidColorBrush MenuHighlightBrush { get; }
        SolidColorBrush MenuTextBrush { get; }
        SolidColorBrush ScrollBarBrush { get; }
        SolidColorBrush WindowBrush { get; }
        SolidColorBrush WindowFrameBrush { get; }
        SolidColorBrush WindowTextBrush { get; }
        SolidColorBrush InactiveSelectionHighlightBrush { get; }
        SolidColorBrush InactiveSelectionHighlightTextBrush { get; }

        #endregion
    }
}
