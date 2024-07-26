using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Documents
{
    public class TextTreeRootNode : TextTreeNode
    {
        public TextTreeRootNode(IReadOnlyTextContainer textContainer)
        {
            TextContainer = textContainer;
        }

        public override int StartSymbolCount => 0;

        public override int EndSymbolCount => 0;
        
        public IReadOnlyTextContainer TextContainer { get; }
    }
}
