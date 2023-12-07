using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls
{
    public class RowDefinitionCollection : DefinitionCollection<RowDefinition>
    {
        internal RowDefinitionCollection(Grid grid) : base(grid)
        {
        }
    }
}
