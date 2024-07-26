using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Documents
{
    public class TextTreeTextElementNode : TextTreeNode
    {
        private readonly TextElement _textElement;

        public TextTreeTextElementNode(TextElement textElement)
        {
            _textElement = textElement;
        }

        public TextElement TextElement => _textElement;

        protected override int InternalSymbolCount => _textElement.ContentCount;
    }
}
