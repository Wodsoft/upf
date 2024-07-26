using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Documents
{
    public class TextEditor
    {
        private readonly ITextContainer _textContainer;
        private readonly FrameworkElement _owner;

        public TextEditor(ITextContainer textContainer, FrameworkElement owner)
        {
            _textContainer = textContainer;
            _owner = owner;
        }
    }
}
