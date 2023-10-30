using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Platforms.Win32
{
    public static class ApplicationExtensions
    {
        public static void UseWin32(this Application application)
        {
            if (application.IsRunning)
                throw new InvalidOperationException("Could not initialize services while application is running.");
            application.WindowProvider = new WindowProvider();
            application.LifecycleProvider = new LifecycleProvider();
        }
    }
}
