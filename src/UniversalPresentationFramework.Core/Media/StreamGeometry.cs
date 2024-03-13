using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public sealed class StreamGeometry : Geometry
    {
        private StreamGeometryContext? _context;

        public StreamGeometryContext Open()
        {
            WritePreamble();
            if (_context == null)
            {
                _context = FrameworkCoreProvider.GetRendererProvider().CreateGeometryContext();
            }
            return _context;
        }

        public override PathGeometryData GetPathGeometryData()
        {
            if (_context == null)
            {
                _context = FrameworkCoreProvider.GetRendererProvider().CreateGeometryContext();
            }
            return _context.GetGeometryData();
        }

        public override bool IsEmpty()
        {
            return _context == null;
        }

        #region Clone

        protected override Freezable CreateInstanceCore()
        {
            var geometry = new StreamGeometry();
            if (IsFrozen)
                geometry._context = _context;
            else if (_context != null)
                geometry._context = _context.Clone();
            return geometry;
        }

        #endregion
    }
}
