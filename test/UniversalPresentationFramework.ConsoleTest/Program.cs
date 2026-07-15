// See https://aka.ms/new-console-template for more information

using System.Globalization;
using Wodsoft.UI;
using Wodsoft.UI.Media;
using Wodsoft.UI.Media.Imaging;
using Wodsoft.UI.Platforms.Win32;
using Wodsoft.UI.Renderers;
using Wodsoft.UI.Test;

Console.ReadLine();

var rendererProvider = new SkiaRendererProvider();
FrameworkCoreProvider.RendererProvider = rendererProvider;

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

bool running = true;
Console.CancelKeyPress += (_, _) => running = false;
var renderTargetBitmap = new RenderTargetBitmap(200, 200, 96, 96, PixelFormats.Bgra32);
while (running)
{
    renderTargetBitmap.Render(visual);
    Console.ReadLine();
}