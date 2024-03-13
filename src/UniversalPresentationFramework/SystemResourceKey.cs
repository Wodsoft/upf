using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    internal class SystemResourceKey : ResourceKey
    {
        private readonly SystemResourceKeyID _id;

        public SystemResourceKey(SystemResourceKeyID id)
        {
            _id = id;
        }

        public override Assembly Assembly => throw new NotSupportedException();

        public override bool HasResource => true;

        public override object? Resource
        {
            get
            {
                if (FrameworkProvider.ThemeProvider == null)
                    return null;
                return FrameworkProvider.ThemeProvider.GetResourceValue(_id);
            }
        }
    }
}
