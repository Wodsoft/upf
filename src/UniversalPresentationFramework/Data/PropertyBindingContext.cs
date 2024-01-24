using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Data
{
    internal abstract class PropertyBindingContext
    {
        public abstract PropertyBinding CreateBinding(object source);

        public abstract object? GetValue(object source);
    }
}
