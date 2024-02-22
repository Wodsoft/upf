using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Providers;

namespace Wodsoft.UI
{
    internal static class FrameworkCoreProvider
    {
        internal static IRendererProvider? RendererProvider;

        internal static IClockProvider? ClockProvider;
    }
}
