using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Wodsoft.UI.Controls;
using Wodsoft.UI.Media.Imaging;

namespace Wodsoft.UI.Test
{
    public class ImageTest : RenderTest
    {
        [Fact]
        public void Suitable()
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("https://www.baidu.com/img/flexible/logo/pc/result.png");
            bitmap.EndInit();
            var image = new Image();
            image.Source = bitmap;
            //image.Width = bitmap.Width;
            //image.Height = bitmap.Height;
            image.Arrange(new Rect(0, 0, bitmap.Width, bitmap.Height));
            RenderToBitmap(image);
        }

        [Fact]
        public void UniformWidthLarge()
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("https://www.baidu.com/img/flexible/logo/pc/result.png");
            bitmap.EndInit();
            var image = new Image();
            image.Source = bitmap;
            image.Width = bitmap.Width + 20f;
            image.Height = bitmap.Height;
            image.Arrange(new Rect(0, 0, bitmap.Width + 20f, bitmap.Height));
            Assert.Equal(bitmap.Width, image.RenderSize.Width);
            Assert.Equal(bitmap.Height, image.RenderSize.Height);
            RenderToBitmap(image);
        }

        [Fact]
        public void UniformHeightLarge()
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("https://www.baidu.com/img/flexible/logo/pc/result.png");
            bitmap.EndInit();
            var image = new Image();
            image.Source = bitmap;
            image.Width = bitmap.Width;
            image.Height = bitmap.Height + 20f;
            image.Arrange(new Rect(0, 0, bitmap.Width, bitmap.Height + 20f));
            Assert.Equal(bitmap.Width, image.RenderSize.Width);
            Assert.Equal(bitmap.Height, image.RenderSize.Height);
            RenderToBitmap(image);
        }

        [Fact]
        public void UniformBothLarge()
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("https://www.baidu.com/img/flexible/logo/pc/result.png");
            bitmap.EndInit();
            var image = new Image();
            image.Source = bitmap;
            image.Width = bitmap.Width + 20f;
            image.Height = bitmap.Height + 20f;
            image.Arrange(new Rect(0, 0, bitmap.Width + 20f, bitmap.Height + 20f));
            Assert.Equal(bitmap.Width + 20f, image.RenderSize.Width);
            Assert.Equal((20f / bitmap.Width + 1) * bitmap.Height, image.RenderSize.Height);
            RenderToBitmap(image);
        }



        [Fact]
        public void UniformToFillWidthLarge()
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("https://www.baidu.com/img/flexible/logo/pc/result.png");
            bitmap.EndInit();
            var image = new Image();
            image.Stretch = Media.Stretch.UniformToFill;
            image.Source = bitmap;
            image.Width = bitmap.Width + 100f;
            image.Height = bitmap.Height;
            image.Arrange(new Rect(0, 0, bitmap.Width + 100f, bitmap.Height));
            Assert.Equal(bitmap.Width + 100f, image.RenderSize.Width);
            Assert.Equal(bitmap.Height, image.RenderSize.Height);
            RenderToBitmap(image);
        }

        [Fact]
        public void UniformToFillHeightLarge()
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("https://www.baidu.com/img/flexible/logo/pc/result.png");
            bitmap.EndInit();
            var image = new Image();
            image.Stretch = Media.Stretch.UniformToFill;
            image.Source = bitmap;
            image.Width = bitmap.Width;
            image.Height = bitmap.Height + 20f;
            image.Arrange(new Rect(0, 0, bitmap.Width, bitmap.Height + 20f));
            Assert.Equal(bitmap.Width, image.RenderSize.Width);
            Assert.Equal(bitmap.Height + 20f, image.RenderSize.Height);
            RenderToBitmap(image);
        }        
    }
}
