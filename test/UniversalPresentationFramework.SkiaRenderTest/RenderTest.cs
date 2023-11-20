using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Wodsoft.UI.Renderers;
using UPF = Wodsoft.UI;
using WPF = System.Windows;

namespace Wodsoft.UI.Test
{
    public class RenderTest
    {
        public RenderTest()
        {
            FrameworkProvider.RendererProvider = new SkiaRendererProvider();
        }

        protected void RenderToBitmap(UPF.UIElement element)
        {
            var renderTargetBitmap = new UPF.Media.Imaging.RenderTargetBitmap((int)element.RenderSize.Width, (int)element.RenderSize.Height, 96, 96, UPF.Media.PixelFormats.Bgra32);
            renderTargetBitmap.Render(element);

            using Bitmap bitmap = new Bitmap((int)element.RenderSize.Width, (int)element.RenderSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var data = bitmap.LockBits(new Rectangle(new System.Drawing.Point(), bitmap.Size), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            renderTargetBitmap.CopyPixels(UPF.Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
            bitmap.UnlockBits(data);
            bitmap.Save("upf.png", System.Drawing.Imaging.ImageFormat.Png);
            //using SkiaRendererSoftwareContext context = new SkiaRendererSoftwareContext();
            //context.Render(element);
            //var image = context.GetImage();
            //Bitmap bitmap = new Bitmap(image.Width, image.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            //var data = bitmap.LockBits(new Rectangle(new System.Drawing.Point(), bitmap.Size), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            //using (var pixmap = new SKPixmap(new SKImageInfo(data.Width, data.Height), data.Scan0, data.Stride))
            //{
            //    image.ReadPixels(pixmap, 0, 0);
            //}
            //bitmap.UnlockBits(data);
            //bitmap.Save("upf.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        protected void RenderToBitmap(WPF.UIElement element)
        {
            var renderTargetBitmap = new WPF.Media.Imaging.RenderTargetBitmap((int)element.RenderSize.Width, (int)element.RenderSize.Height, 96,96, WPF.Media.PixelFormats.Default);
            renderTargetBitmap.Render(element);

            using Bitmap bitmap = new Bitmap((int)element.RenderSize.Width, (int)element.RenderSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var data = bitmap.LockBits(new Rectangle(new System.Drawing.Point(), bitmap.Size), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            renderTargetBitmap.CopyPixels(WPF.Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
            bitmap.UnlockBits(data);
            bitmap.Save("wpf.png", System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}
