using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public class LogicalRootChangedEventArgs : EventArgs
    {
        public LogicalRootChangedEventArgs(LogicalObject oldRoot, LogicalObject newRoot)
        {
            OldRoot = oldRoot;
            NewRoot = newRoot;
        }

        public LogicalObject OldRoot { get; }

        public LogicalObject NewRoot { get; }
    }
}
