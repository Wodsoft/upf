using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    /// <summary>
    /// DashStyles - The DashStyles class is static, and contains properties for well known
    /// dash styles.
    /// </summary> 
    public static class DashStyles
    {
        #region Public Static Properties

        /// <summary>
        /// Solid - A solid DashArray (no dashes).
        /// </summary>
        public static DashStyle Solid
        {
            get
            {
                if (_Solid == null)
                {
                    DashStyle solid = new DashStyle();
                    solid.Freeze();
                    _Solid = solid;
                }

                return _Solid;
            }
        }

        /// <summary>
        /// Dash - A DashArray which is 2 on, 2 off
        /// </summary>
        public static DashStyle Dash
        {
            get
            {
                if (_Dash == null)
                {
                    DashStyle style = new DashStyle(new float[] { 2, 2 }, 1);
                    style.Freeze();
                    _Dash = style;
                }

                return _Dash;
            }
        }

        /// <summary>
        /// Dot - A DashArray which is 0 on, 2 off
        /// </summary>
        public static DashStyle Dot
        {
            get
            {
                if (_Dot == null)
                {
                    DashStyle style = new DashStyle(new float[] { 0, 2 }, 0);
                    style.Freeze();
                    _Dot = style;
                }

                return _Dot;
            }
        }

        /// <summary>
        /// DashDot - A DashArray which is 2 on, 2 off, 0 on, 2 off
        /// </summary>
        public static DashStyle DashDot
        {
            get
            {
                if (_DashDot == null)
                {
                    DashStyle style = new DashStyle(new float[] { 2, 2, 0, 2 }, 1);
                    style.Freeze();
                    _DashDot = style;
                }

                return _DashDot;
            }
        }

        /// <summary>
        /// DashDot - A DashArray which is 2 on, 2 off, 0 on, 2 off, 0 on, 2 off
        /// </summary>
        public static DashStyle DashDotDot
        {
            get
            {
                if (_DashDotDot == null)
                {
                    DashStyle style = new DashStyle(new float[] { 2, 2, 0, 2, 0, 2 }, 1);
                    style.Freeze();
                    _DashDotDot = style;
                }

                return _DashDotDot;
            }
        }

        #endregion Public Static Properties

        #region Private Static Fields

        private static DashStyle? _Solid;
        private static DashStyle? _Dash;
        private static DashStyle? _Dot;
        private static DashStyle? _DashDot;
        private static DashStyle? _DashDotDot;

        #endregion Private Static Fields
    }
}
