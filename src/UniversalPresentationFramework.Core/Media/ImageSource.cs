using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public abstract class ImageSource : Freezable
    {
        public abstract float Width { get; }

        public abstract float Height { get; }

        public abstract IImageContext Context { get; }
    }
}
