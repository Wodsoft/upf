using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Documents
{
    public class TextRange
    {
        private readonly TextPointer _start;
        private readonly TextPointer _end;

        public TextRange(TextPointer start, TextPointer end)
        {
            _start = start;
            _end = end;
        }

        public TextPointer Start => _start;

        public TextPointer End => _end;
    }
}
