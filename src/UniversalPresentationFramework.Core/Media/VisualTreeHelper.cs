using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public static class VisualTreeHelper
    {
        public static Visual GetChild(this Visual visual, int childIndex)
        {
            return visual.GetVisualChild(childIndex);
        }

        public static int GetChildrenCount(this Visual visual)
        {
            return visual.VisualChildrenCount;
        }

        public static Visual? GetParent(this Visual visual)
        {
            return visual.VisualParent;
        }
    }
}
