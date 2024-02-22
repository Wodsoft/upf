using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media.Animation;

namespace Wodsoft.UI.Providers
{
    public interface IClockProvider
    {
        void RegisterClock(DependencyObject d, Clock clock);
    }

    public delegate void ClockTickHandler(TimeSpan elapsedTime);
}
