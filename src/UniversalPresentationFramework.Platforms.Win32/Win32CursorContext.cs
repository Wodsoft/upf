using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;
using Wodsoft.UI.Input;

namespace Wodsoft.UI.Platforms.Win32
{
    public class Win32CursorContext : ICursorContext
    {
        private DestroyCursorSafeHandle _handle;

        internal Win32CursorContext(DestroyCursorSafeHandle handle)
        {
            _handle = handle;
        }

        internal DestroyCursorSafeHandle Handle => _handle;

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
