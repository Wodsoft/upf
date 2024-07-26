using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Documents
{
    public class Italic : Span
    {
        static Italic()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Italic), new FrameworkPropertyMetadata(typeof(Italic)));
        }

        public Italic()
        {

        }

        public Italic(Inline inline) : base(inline) { }
    }
}
