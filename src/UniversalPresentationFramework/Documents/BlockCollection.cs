using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Documents
{
    public class BlockCollection : TextElementCollection<Block>
    {
        public BlockCollection(LogicalObject parent, TextTreeNode parentNode) : base(parent, parentNode)
        {
        }

        protected override Block ConvertToElement(object value)
        {
            if (value is Block block)
                return block;
            throw new NotSupportedException("Block only support block element.");
        }
    }
}
