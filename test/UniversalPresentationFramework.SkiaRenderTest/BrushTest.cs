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
                ImageSource = bitmapImage
            };
            grid.Children.Add(rect);
            grid.Arrange(new Rect(0, 0, 400, 400));
            RenderToBitmap(grid);
        }

        [Fact]
        public void ImageBrushTileTest()
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
                Viewport = new Rect(0, 0, 0.25f, 0.5f),
                AlignmentY = AlignmentY.Top,
                Stretch = Stretch.Uniform,
                Transform = new TranslateTransform(50, 50)
                //RelativeTransform = new TranslateTransform(0.25f, 0.25f)
            };
            grid.Children.Add(rect);
            var rect2 = new Rectangle();
            rect2.HorizontalAlignment = HorizontalAlignment.Left;
            rect2.VerticalAlignment = VerticalAlignment.Top;
            rect2.Width = 25;
            rect2.Height = 25;
            rect2.Fill = new SolidColorBrush(new Color(128, 0, 0, 0));
            grid.Children.Add(rect2);
            grid.Arrange(new Rect(0, 0, 400, 400));
            RenderToBitmap(grid);
        }
    }
}
