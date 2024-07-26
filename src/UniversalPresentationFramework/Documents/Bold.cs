using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Documents
{
    public class Bold : Span
    {
        static Bold()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Bold), new FrameworkPropertyMetadata(typeof(Bold)));
        }

        public Bold()
        {

        }

        public Bold(Inline inline) : base(inline) { }
    }
}
