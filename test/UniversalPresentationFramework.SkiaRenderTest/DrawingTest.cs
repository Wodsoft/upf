using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;
using UPF = Wodsoft.UI;
using WPF = System.Windows;

namespace Wodsoft.UI.Test
{
    public class DrawingTest : RenderTest
    {
        [Fact]
        public void DrawFormattedText()
        {
            DrawingVisual visual = new DrawingVisual();
            visual.Open();
            var formattedText = new FormattedText("Hello World. This is UPF formatted text test result.", CultureInfo.InstalledUICulture, FlowDirection.LeftToRight, new Typeface("微软雅黑"), 12f, Brushes.Black, 1);
            formattedText.MaxTextWidth = 100;
            formattedText.MaxTextHeight = 100;
            visual.DrawingContext.DrawRectangle(Brushes.White, null, new Rect(0, 0, 200, 200));
            visual.DrawingContext.DrawText(formattedText, new Point(50, 50));
            visual.DrawingContext.DrawRectangle(null, new Pen(Brushes.Black, 1), new Rect(50, 50, 100, 100));
            visual.Close();
            visual.Size = new Size(100, 100);
            RenderToBitmap(visual, 200, 200);
        }

        [Fact]
        public void WPFDrawFormattedTest()
        {
            var typeface = new WPF.Media.Typeface("微软雅黑");
            typeface.TryGetGlyphTypeface(out var glyphTypeface);
            WPF.Media.DrawingVisual drawingVisual = new WPF.Media.DrawingVisual();
            var drawingContext = drawingVisual.RenderOpen();
            var formattedText = new WPF.Media.FormattedText("Hello World. This is UPF formatted text test result.", CultureInfo.InstalledUICulture, WPF.FlowDirection.LeftToRight, new WPF.Media.Typeface("微软雅黑"), 12f, WPF.Media.Brushes.Black, 1);
            formattedText.MaxTextWidth = 100;
            formattedText.MaxTextHeight = 100;
            drawingContext.DrawRectangle(WPF.Media.Brushes.White, null, new WPF.Rect(0, 0, 200, 200));
            drawingContext.DrawText(formattedText, new WPF.Point(50, 50));
            drawingContext.DrawRectangle(null, new WPF.Media.Pen(WPF.Media.Brushes.Black, 1), new WPF.Rect(50, 50, 100, 100));
            drawingContext.Close();
            RenderToBitmap(drawingVisual, 200, 200);
        }
    }
}
