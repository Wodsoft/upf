using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls
{
    public class ColumnDefinitionCollection : DefinitionCollection<ColumnDefinition>
    {
        internal ColumnDefinitionCollection(Grid grid) : base(grid)
        {
        }
    }
}
