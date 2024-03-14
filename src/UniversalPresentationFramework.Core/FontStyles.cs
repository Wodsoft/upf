using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    /// <summary>
    /// FontStyles contains predefined font style structures for common font styles.
    /// </summary>
    public static class FontStyles
    {
        /// <summary>
        /// Predefined font style : Normal.
        /// </summary>
        public static FontStyle Normal { get { return new FontStyle(0); } }

        /// <summary>
        /// Predefined font style : Oblique.
        /// </summary>
        public static FontStyle Oblique { get { return new FontStyle(1); } }

        /// <summary>
        /// Predefined font style : Italic.
        /// </summary>
        public static FontStyle Italic { get { return new FontStyle(2); } }

        internal static bool FontStyleStringToKnownStyle(string s, ref FontStyle fontStyle)
        {
            if (s.Equals("Normal", StringComparison.OrdinalIgnoreCase))
            {
                fontStyle = Normal;
                return true;
            }
            if (s.Equals("Italic", StringComparison.OrdinalIgnoreCase))
            {
                fontStyle = Italic;
                return true;
            }
            if (s.Equals("Oblique", StringComparison.OrdinalIgnoreCase))
            {
                fontStyle = Oblique;
                return true;
            }
            return false;
        }
    }
}
