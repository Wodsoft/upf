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

        internal static IInputProvider? InputProvider;

        internal static IRendererProvider GetRendererProvider()
        {
            if (RendererProvider == null)
                throw new InvalidOperationException("Framework not initialized.");
            return RendererProvider;
        }

        internal static IClockProvider GetClockProvider()
        {
            if (ClockProvider == null)
                throw new InvalidOperationException("Framework not initialized.");
            return ClockProvider;
        }

        internal static IInputProvider GetInputProvider()
        {
            if (InputProvider == null)
                throw new InvalidOperationException("Framework not initialized.");
            return InputProvider;
        }
    }
}
