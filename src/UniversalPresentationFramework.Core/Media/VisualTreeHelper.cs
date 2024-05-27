using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public static class VisualTreeHelper
    {
        public static Visual GetChild(Visual visual, int childIndex)
        {
            return visual.GetVisualChild(childIndex);
        }

        public static int GetChildrenCount(Visual visual)
        {
            return visual.VisualChildrenCount;
        }

        public static Visual? GetParent(Visual visual)
        {
            return visual.VisualParent;
        }
    }
}
