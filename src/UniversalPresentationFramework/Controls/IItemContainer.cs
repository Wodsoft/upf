using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls
{
    public interface IItemContainer
    {
        void PrepareContainer(ItemsControl parent, object? item);

        void ClearContainer(object? item);
    }
}
