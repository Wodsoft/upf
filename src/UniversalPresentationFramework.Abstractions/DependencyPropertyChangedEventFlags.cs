using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    [Flags]
    public enum DependencyPropertyChangedEventFlags
    {
        None = 0,
        LocalValueChanged = 1
    }
}
