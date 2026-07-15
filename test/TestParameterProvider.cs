using System;
using Wodsoft.UI.Media;
using Wodsoft.UI.Providers;

namespace Wodsoft.UI.Test
{
    /// <summary>
    /// Fixed Windows (96 DPI) system parameter defaults for deterministic tests.
    /// Values mirror stock Win10/11 light theme SPI / NONCLIENTMETRICS / GetSysColor.
    /// </summary>
    public class TestParameterProvider : IParameterProvider
    {
        private float _smallCaptionFontSize = 12f;

        #region Window Parameters

        public bool MinimizeAnimation => true;

        public int Border => 1;

        public float CaretWidth => 1f;

        public bool DragFullWindows => true;

        public int ForegroundFlashCount => 7;

        public float BorderWidth => 1f;

        public float ScrollWidth => 17f;

        public float ScrollHeight => 17f;

        public float CaptionWidth => 36f;

        public float CaptionHeight => 22f;

        public float SmallCaptionWidth => 22f;

        public float SmallCaptionHeight => 22f;

        public float MenuWidth => 19f;

        public float MenuHeight => 19f;

        #endregion

        #region Font Parameters

        private static readonly FontFamily _DefaultFontFamily = new FontFamily("Segoe UI");

        public float IconFontSize => 12f;

        public FontFamily IconFontFamily => _DefaultFontFamily;

        public FontStyle IconFontStyle => FontStyles.Normal;

        public FontWeight IconFontWeight => FontWeights.Normal;

        public TextDecorationCollection IconFontTextDecorations => TextDecorationCollection.Empty;

        public float CaptionFontSize => 12f;

        public FontFamily CaptionFontFamily => _DefaultFontFamily;

        public FontStyle CaptionFontStyle => FontStyles.Normal;

        public FontWeight CaptionFontWeight => FontWeights.Normal;

        public TextDecorationCollection CaptionFontTextDecorations => TextDecorationCollection.Empty;

        public float SmallCaptionFontSize
        {
            get => _smallCaptionFontSize;
            set => _smallCaptionFontSize = value;
        }

        public FontFamily SmallCaptionFontFamily => _DefaultFontFamily;

        public FontStyle SmallCaptionFontStyle => FontStyles.Normal;

        public FontWeight SmallCaptionFontWeight => FontWeights.Normal;

        public TextDecorationCollection SmallCaptionFontTextDecorations => TextDecorationCollection.Empty;

        public float MenuFontSize => 12f;

        public FontFamily MenuFontFamily => _DefaultFontFamily;

        public FontStyle MenuFontStyle => FontStyles.Normal;

        public FontWeight MenuFontWeight => FontWeights.Normal;

        public TextDecorationCollection MenuFontTextDecorations => TextDecorationCollection.Empty;

        public float StatusFontSize => 12f;

        public FontFamily StatusFontFamily => _DefaultFontFamily;

        public FontStyle StatusFontStyle => FontStyles.Normal;

        public FontWeight StatusFontWeight => FontWeights.Normal;

        public TextDecorationCollection StatusFontTextDecorations => TextDecorationCollection.Empty;

        public float MessageFontSize => 12f;

        public FontFamily MessageFontFamily => _DefaultFontFamily;

        public FontStyle MessageFontStyle => FontStyles.Normal;

        public FontWeight MessageFontWeight => FontWeights.Normal;

        public TextDecorationCollection MessageFontTextDecorations => TextDecorationCollection.Empty;

        #endregion

        #region Accessibility Parameters

        public float FocusBorderWidth => 1f;

        public float FocusBorderHeight => 1f;

        public bool HighContrast => false;

        #endregion

        #region System Colors

        public Color ActiveBorderColor => Color.FromRgb(0xB4, 0xB4, 0xB4);

        public Color ActiveCaptionColor => Color.FromRgb(0x99, 0xB4, 0xD1);

        public Color ActiveCaptionTextColor => Colors.Black;

        public Color AppWorkspaceColor => Color.FromRgb(0xAB, 0xAB, 0xAB);

        public Color ControlColor => Color.FromRgb(0xF0, 0xF0, 0xF0);

        public Color ControlDarkColor => Color.FromRgb(0xA0, 0xA0, 0xA0);

        public Color ControlDarkDarkColor => Color.FromRgb(0x69, 0x69, 0x69);

        public Color ControlLightColor => Color.FromRgb(0xE3, 0xE3, 0xE3);

        public Color ControlLightLightColor => Colors.White;

        public Color ControlTextColor => Colors.Black;

        public Color DesktopColor => Colors.Black;

        public Color GradientActiveCaptionColor => Color.FromRgb(0xB9, 0xD1, 0xEA);

        public Color GradientInactiveCaptionColor => Color.FromRgb(0xD7, 0xE4, 0xF2);

        public Color GrayTextColor => Color.FromRgb(0x6D, 0x6D, 0x6D);

        public Color HighlightColor => Color.FromRgb(0x00, 0x78, 0xD7);

        public Color HighlightTextColor => Colors.White;

        public Color HotTrackColor => Color.FromRgb(0x00, 0x66, 0xCC);

        public Color InactiveBorderColor => Color.FromRgb(0xF4, 0xF7, 0xFC);

        public Color InactiveCaptionColor => Color.FromRgb(0xBF, 0xCD, 0xDB);

        public Color InactiveCaptionTextColor => Colors.Black;

        public Color InfoColor => Color.FromRgb(0xFF, 0xFF, 0xE1);

        public Color InfoTextColor => Colors.Black;

        public Color MenuColor => Color.FromRgb(0xF0, 0xF0, 0xF0);

        public Color MenuBarColor => Color.FromRgb(0xF0, 0xF0, 0xF0);

        public Color MenuHighlightColor => Color.FromRgb(0x00, 0x78, 0xD7);

        public Color MenuTextColor => Colors.Black;

        public Color ScrollBarColor => Color.FromRgb(0xC8, 0xC8, 0xC8);

        public Color WindowColor => Colors.White;

        public Color WindowFrameColor => Color.FromRgb(0x64, 0x64, 0x64);

        public Color WindowTextColor => Colors.Black;

        public SolidColorBrush ActiveBorderBrush => _ActiveBorderBrush;

        public SolidColorBrush ActiveCaptionBrush => _ActiveCaptionBrush;

        public SolidColorBrush ActiveCaptionTextBrush => _ActiveCaptionTextBrush;

        public SolidColorBrush AppWorkspaceBrush => _AppWorkspaceBrush;

        public SolidColorBrush ControlBrush => _ControlBrush;

        public SolidColorBrush ControlDarkBrush => _ControlDarkBrush;

        public SolidColorBrush ControlDarkDarkBrush => _ControlDarkDarkBrush;

        public SolidColorBrush ControlLightBrush => _ControlLightBrush;

        public SolidColorBrush ControlLightLightBrush => _ControlLightLightBrush;

        public SolidColorBrush ControlTextBrush => _ControlTextBrush;

        public SolidColorBrush DesktopBrush => _DesktopBrush;

        public SolidColorBrush GradientActiveCaptionBrush => _GradientActiveCaptionBrush;

        public SolidColorBrush GradientInactiveCaptionBrush => _GradientInactiveCaptionBrush;

        public SolidColorBrush GrayTextBrush => _GrayTextBrush;

        public SolidColorBrush HighlightBrush => _HighlightBrush;

        public SolidColorBrush HighlightTextBrush => _HighlightTextBrush;

        public SolidColorBrush HotTrackBrush => _HotTrackBrush;

        public SolidColorBrush InactiveBorderBrush => _InactiveBorderBrush;

        public SolidColorBrush InactiveCaptionBrush => _InactiveCaptionBrush;

        public SolidColorBrush InactiveCaptionTextBrush => _InactiveCaptionTextBrush;

        public SolidColorBrush InfoBrush => _InfoBrush;

        public SolidColorBrush InfoTextBrush => _InfoTextBrush;

        public SolidColorBrush MenuBrush => _MenuBrush;

        public SolidColorBrush MenuBarBrush => _MenuBarBrush;

        public SolidColorBrush MenuHighlightBrush => _MenuHighlightBrush;

        public SolidColorBrush MenuTextBrush => _MenuTextBrush;

        public SolidColorBrush ScrollBarBrush => _ScrollBarBrush;

        public SolidColorBrush WindowBrush => _WindowBrush;

        public SolidColorBrush WindowFrameBrush => _WindowFrameBrush;

        public SolidColorBrush WindowTextBrush => _WindowTextBrush;

        public SolidColorBrush InactiveSelectionHighlightBrush => HighContrast ? HighlightBrush : ControlBrush;

        public SolidColorBrush InactiveSelectionHighlightTextBrush => HighContrast ? HighlightTextBrush : ControlTextBrush;

        private static readonly SolidColorBrush _ActiveBorderBrush = CreateBrush(0xB4, 0xB4, 0xB4);
        private static readonly SolidColorBrush _ActiveCaptionBrush = CreateBrush(0x99, 0xB4, 0xD1);
        private static readonly SolidColorBrush _ActiveCaptionTextBrush = CreateBrush(Colors.Black);
        private static readonly SolidColorBrush _AppWorkspaceBrush = CreateBrush(0xAB, 0xAB, 0xAB);
        private static readonly SolidColorBrush _ControlBrush = CreateBrush(0xF0, 0xF0, 0xF0);
        private static readonly SolidColorBrush _ControlDarkBrush = CreateBrush(0xA0, 0xA0, 0xA0);
        private static readonly SolidColorBrush _ControlDarkDarkBrush = CreateBrush(0x69, 0x69, 0x69);
        private static readonly SolidColorBrush _ControlLightBrush = CreateBrush(0xE3, 0xE3, 0xE3);
        private static readonly SolidColorBrush _ControlLightLightBrush = CreateBrush(Colors.White);
        private static readonly SolidColorBrush _ControlTextBrush = CreateBrush(Colors.Black);
        private static readonly SolidColorBrush _DesktopBrush = CreateBrush(Colors.Black);
        private static readonly SolidColorBrush _GradientActiveCaptionBrush = CreateBrush(0xB9, 0xD1, 0xEA);
        private static readonly SolidColorBrush _GradientInactiveCaptionBrush = CreateBrush(0xD7, 0xE4, 0xF2);
        private static readonly SolidColorBrush _GrayTextBrush = CreateBrush(0x6D, 0x6D, 0x6D);
        private static readonly SolidColorBrush _HighlightBrush = CreateBrush(0x00, 0x78, 0xD7);
        private static readonly SolidColorBrush _HighlightTextBrush = CreateBrush(Colors.White);
        private static readonly SolidColorBrush _HotTrackBrush = CreateBrush(0x00, 0x66, 0xCC);
        private static readonly SolidColorBrush _InactiveBorderBrush = CreateBrush(0xF4, 0xF7, 0xFC);
        private static readonly SolidColorBrush _InactiveCaptionBrush = CreateBrush(0xBF, 0xCD, 0xDB);
        private static readonly SolidColorBrush _InactiveCaptionTextBrush = CreateBrush(Colors.Black);
        private static readonly SolidColorBrush _InfoBrush = CreateBrush(0xFF, 0xFF, 0xE1);
        private static readonly SolidColorBrush _InfoTextBrush = CreateBrush(Colors.Black);
        private static readonly SolidColorBrush _MenuBrush = CreateBrush(0xF0, 0xF0, 0xF0);
        private static readonly SolidColorBrush _MenuBarBrush = CreateBrush(0xF0, 0xF0, 0xF0);
        private static readonly SolidColorBrush _MenuHighlightBrush = CreateBrush(0x00, 0x78, 0xD7);
        private static readonly SolidColorBrush _MenuTextBrush = CreateBrush(Colors.Black);
        private static readonly SolidColorBrush _ScrollBarBrush = CreateBrush(0xC8, 0xC8, 0xC8);
        private static readonly SolidColorBrush _WindowBrush = CreateBrush(Colors.White);
        private static readonly SolidColorBrush _WindowFrameBrush = CreateBrush(0x64, 0x64, 0x64);
        private static readonly SolidColorBrush _WindowTextBrush = CreateBrush(Colors.Black);

        private static SolidColorBrush CreateBrush(byte r, byte g, byte b) => CreateBrush(Color.FromRgb(r, g, b));

        private static SolidColorBrush CreateBrush(Color color)
        {
            var brush = new SolidColorBrush(color);
            brush.Freeze();
            return brush;
        }

        #endregion

        #region Input Parameters

        public float MouseHoverWidth => 4f;

        public float MouseHoverHeight => 4f;

        public TimeSpan MouseHoverTime => TimeSpan.FromMilliseconds(400);

        public int WheelScrollLines => 3;

        public bool SnapToDefaultButton => false;

        public int KeyboardSpeed => 31;

        public bool KeyboardPreference => false;

        public int KeyboardDelay => 1;

        public bool KeyboardCues => false;

        #endregion
    }
}
