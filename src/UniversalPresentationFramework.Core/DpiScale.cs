using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public struct DpiScale
    {
        /// <summary>
        /// Initializes a new instance of the DpiScale structure.
        /// </summary>
        public DpiScale(float dpiScaleX, float dpiScaleY)
        {
            _dpiScaleX = dpiScaleX;
            _dpiScaleY = dpiScaleY;
        }

        /// <summary>
        /// Gets the DPI scale on the X axis.When DPI is 96, <see cref="DpiScaleX"/> is 1. 
        /// </summary>
        /// <remarks>
        /// On Windows Desktop, this value is the same as <see cref="DpiScaleY"/>
        /// </remarks>
        public float DpiScaleX
        {
            get { return _dpiScaleX; }
        }

        /// <summary>
        /// Gets the DPI scale on the Y axis. When DPI is 96, <see cref="DpiScaleY"/> is 1. 
        /// </summary>
        /// <remarks>
        /// On Windows Desktop, this value is the same as <see cref="DpiScaleX"/>
        /// </remarks>
        public float DpiScaleY
        {
            get { return _dpiScaleY; }
        }

        /// <summary>
        /// Get or sets the PixelsPerDip at which the text should be rendered.
        /// </summary>
        public float PixelsPerDip
        {
            get { return _dpiScaleY; }
        }

        /// <summary>
        /// Gets the PPI along X axis.
        /// </summary>
        /// <remarks>
        /// On Windows Desktop, this value is the same as <see cref="PixelsPerInchY"/>
        /// </remarks>
        public float PixelsPerInchX
        {
            get { return DpiUtil.DefaultPixelsPerInch * _dpiScaleX; }
        }

        /// <summary>
        /// Gets the PPI along Y axis.
        /// </summary>
        /// <remarks>
        /// On Windows Desktop, this value is the same as <see cref="PixelsPerInchX"/>
        /// </remarks>
        public float PixelsPerInchY
        {
            get { return DpiUtil.DefaultPixelsPerInch * _dpiScaleY; }
        }

        internal bool Equals(DpiScale other) => _dpiScaleX == other._dpiScaleX && _dpiScaleY == other._dpiScaleY;

        public static readonly DpiScale Default = new DpiScale(1f, 1f);

        private readonly float _dpiScaleX;
        private readonly float _dpiScaleY;
    }
}
