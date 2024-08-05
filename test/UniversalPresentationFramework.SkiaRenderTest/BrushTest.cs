using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Controls;
using Wodsoft.UI.Media;
using Wodsoft.UI.Media.Imaging;
using Wodsoft.UI.Shapes;

namespace Wodsoft.UI.Test
{
    public class BrushTest : RenderTest
    {
        [Fact]
        public void LinearGradientBrushTest()
        {
            var grid = new Grid();
            grid.Background = Brushes.White;
            var rect = new Rectangle();
            rect.Margin = new Thickness(25);
            rect.Fill = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1),
                GradientStops =
                {
                    new GradientStop(Colors.Blue, 0f),
                    new GradientStop(Colors.Red, 1f)
                }
            };
            grid.Children.Add(rect);
            grid.Arrange(new Rect(0, 0, 400, 200));
            RenderToBitmap(grid);
        }

        [Fact]
        public void RadialGradientBrushTest()
        {
            var grid = new Grid();
            grid.Background = Brushes.White;
            var rect = new Rectangle();
            rect.Margin = new Thickness(25);
            rect.Fill = new RadialGradientBrush
            {
                GradientStops =
                {
                    new GradientStop(Colors.Blue, 0f),
                    new GradientStop(Colors.Red, 1f)
                }
            };
            grid.Children.Add(rect);
            grid.Arrange(new Rect(0, 0, 400, 200));
            RenderToBitmap(grid);
        }

        [Fact]
        public void ImageBrushTest()
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = File.OpenRead("tri.png");
            bitmapImage.EndInit();
            var grid = new Grid();
            grid.Background = Brushes.White;
            var rect = new Rectangle();
            rect.Margin = new Thickness(25);
            rect.StrokeThickness = 10;
            rect.Stroke = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0));
            rect.Fill = new ImageBrush
            {
                ImageSource = bitmapImage,
                TileMode = TileMode.Tile,
                Viewport = new Rect(0, 0, 0.5f, 0.5f)
            };
            grid.Children.Add(rect);
            grid.Arrange(new Rect(0, 0, 400, 400));
            RenderToBitmap(grid);
        }
    }
}
