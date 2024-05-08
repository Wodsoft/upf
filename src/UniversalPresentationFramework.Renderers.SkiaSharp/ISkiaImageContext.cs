using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media.Imaging;

namespace Wodsoft.UI.Renderers
{
    public interface ISkiaImageContext
    {
        public SKImage Image { get; }

        Rotation Rotation { get; }
    }
}
