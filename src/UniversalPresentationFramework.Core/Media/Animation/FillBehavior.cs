using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public enum FillBehavior
    {
        /// <summary>
        /// Indicates that a Timeline will hold its progress between the period of
        /// time between the end of its active period and the end of its parents active and
        /// hold periods. 
        /// </summary>
        HoldEnd,

#if IMPLEMENTED // Uncomment when implemented

        /// <summary>
        /// Indicates that a Timeline will hold its initial active progress during the
        /// period of time between when its parent has become active and it 
        /// becomes active. The Timeline will stop after the completion of
        /// its own active period.
        /// </summary>
        HoldBegin,

        /// <summary>
        /// Indicates that a Timeline will hold its progress both before and after
        /// its active period as long as its parent is in its active or hold periods.
        /// </summary>
        HoldBeginAndEnd

#endif

        /// <summary>
        /// Indicates that a Timeline will stop if it's outside its active
        /// period while its parent is inside its active period.
        /// </summary>
        Stop,
    }
}
