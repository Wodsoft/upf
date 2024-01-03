using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public class LogicalParentChangedEventArgs : EventArgs
    {
        public LogicalParentChangedEventArgs(LogicalObject? oldParent, LogicalObject? newParent)
        {
            OldParent = oldParent;
            NewParent = newParent;
        }
        public LogicalObject? OldParent { get; }

        public LogicalObject? NewParent { get; }
    }
}
