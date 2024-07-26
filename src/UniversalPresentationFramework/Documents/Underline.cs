using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Documents
{
    public class Underline : Span
    {
        static Underline()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Underline), new FrameworkPropertyMetadata(typeof(Underline)));
        }

        public Underline()
        {

        }

        public Underline(Inline inline) : base(inline) { }
    }
}
