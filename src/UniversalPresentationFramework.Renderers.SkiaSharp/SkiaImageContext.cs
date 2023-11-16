using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;
using Wodsoft.UI.Media.Imaging;

namespace Wodsoft.UI.Renderers
{
    public class SkiaImageContext : IImageContext
    {
        private readonly SKImage _image;

        public SkiaImageContext(SKImage image, Rotation rotation)
        {
            _image = image ?? throw new ArgumentNullException(nameof(image));
            Rotation = rotation;
        }

        public SKImage Image => _image;

        public int Width => _image.Width;

        public int Height => _image.Height;

        public Rotation Rotation { get; }
    }
}
