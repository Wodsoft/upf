using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public class VisualTreeHelper
    {
        public static Visual GetVisualChild(Visual visual, int childIndex)
        {
            return visual.GetVisualChild(childIndex);
        }

        public static int GetChildrenCount(Visual visual)
        {
            return visual.VisualChildrenCount;
        }
    }
}
