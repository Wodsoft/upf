using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public static class TextDecorations
    {
        static TextDecorations()
        {
            // Init Underline            
            TextDecoration td = new TextDecoration();
            td.Location = TextDecorationLocation.Underline;
            _Underline = new TextDecorationCollection();
            _Underline.Add(td);
            _Underline.Freeze();

            // Init strikethrough
            td = new TextDecoration();
            td.Location = TextDecorationLocation.Strikethrough;
            _Strikethrough = new TextDecorationCollection();
            _Strikethrough.Add(td);
            _Strikethrough.Freeze();

            // Init overline
            td = new TextDecoration();
            td.Location = TextDecorationLocation.OverLine;
            _OverLine = new TextDecorationCollection();
            _OverLine.Add(td);
            _OverLine.Freeze();

            // Init baseline
            td = new TextDecoration();
            td.Location = TextDecorationLocation.Baseline;
            _Baseline = new TextDecorationCollection();
            _Baseline.Add(td);
            _Baseline.Freeze();
        }

        //---------------------------------
        // Public properties
        //---------------------------------

        /// <summary>
        /// returns a frozen collection containing an underline
        /// </summary>
        public static TextDecorationCollection Underline
        {
            get
            {
                return _Underline;
            }
        }


        /// <summary>
        /// returns a frozen collection containing a strikethrough
        /// </summary>
        public static TextDecorationCollection Strikethrough
        {
            get
            {
                return _Strikethrough;
            }
        }

        /// <summary>
        /// returns a frozen collection containing an overline
        /// </summary>
        public static TextDecorationCollection OverLine
        {
            get
            {
                return _OverLine;
            }
        }

        /// <summary>
        /// returns a frozen collection containing a baseline
        /// </summary>
        public static TextDecorationCollection Baseline
        {
            get
            {
                return _Baseline;
            }
        }

        //--------------------------------
        // Private members
        //--------------------------------

        private static readonly TextDecorationCollection _Underline;
        private static readonly TextDecorationCollection _Strikethrough;
        private static readonly TextDecorationCollection _OverLine;
        private static readonly TextDecorationCollection _Baseline;
    }
}
