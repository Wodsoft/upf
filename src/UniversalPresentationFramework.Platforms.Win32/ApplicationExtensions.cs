using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Providers;
using Wodsoft.UI.Renderers;

namespace Wodsoft.UI.Platforms.Win32
{
    public static class ApplicationExtensions
    {
        public static void UseWin32(this Application application)
        {
            if (application.IsRunning)
                throw new InvalidOperationException("Could not initialize services while application is running.");
            var platform = new Win32Platform();
            application.WindowProvider = platform.WindowProvider;
            application.LifecycleProvider = platform.LifecycleProvider;
            application.RendererProvider = platform.RendererProvider;
            application.ThemeProvider = platform.ThemeProvider;
            application.ParameterProvider = platform.ThemeProvider;
            application.ClockProvider = new FrameworkClockProvider();
            application.ResourceProvider = new EmbeddedResourceProvider();
        }
    }
}
