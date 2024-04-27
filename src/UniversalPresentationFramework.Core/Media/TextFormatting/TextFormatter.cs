using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.TextFormatting
{
    public abstract class TextFormatter
    {
        public abstract TextLine FormatLine(TextSource textSource, int firstCharIndex, float paragraphWidth, TextParagraphProperties paragraphProperties);
    }
}
