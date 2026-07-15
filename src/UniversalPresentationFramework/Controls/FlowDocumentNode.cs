using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Documents;

namespace Wodsoft.UI.Controls
{
    public partial class FlowDocument
    {
        private class FlowDocumentNode : TextTreeNode
        {
            public override int StartSymbolCount => 0;

            public override int EndSymbolCount => 0;                        
        }
    }
}
