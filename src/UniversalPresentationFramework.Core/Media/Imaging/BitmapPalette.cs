using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Imaging
{
    public class BitmapPalette
    {
        public BitmapPalette(IList<Color> colors)
        {
            Colors = colors;
        }

        public IList<Color> Colors { get; }
    }
}
