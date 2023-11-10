using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls
{
    public class ScrollBar
    {
        internal static bool IsValidOrientation(object? o)
        {
            if (o is Orientation value)
                return value == Orientation.Horizontal
                    || value == Orientation.Vertical;
            return false;
        }
    }
}
