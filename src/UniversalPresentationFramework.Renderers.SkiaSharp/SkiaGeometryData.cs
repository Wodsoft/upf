using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Renderers
{
    public class SkiaGeometryData : PathGeometryData
    {
        private readonly SKPath _path;

        public SkiaGeometryData(SKPath path)
        {
            _path = path;
        }

        public SKPath Path => _path;

        public override string ToPathString()
        {
            return _path.ToSvgPathData();
        }
    }
}
