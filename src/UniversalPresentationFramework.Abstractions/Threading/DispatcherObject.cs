using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Threading
{
    public abstract class DispatcherObject
    {
        public abstract Dispatcher Dispatcher { get; }

        public virtual bool CheckAccess()
        {
            var dispatcher = Dispatcher;
            if (dispatcher != null)
                return dispatcher.CheckAccess();
            return true;
        }

        public virtual void VerifyAccess()
        {
            Dispatcher?.VerifyAccess();
        }
    }
}
