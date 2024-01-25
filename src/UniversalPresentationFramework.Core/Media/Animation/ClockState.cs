using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    /// <summary>
    /// This enumeration represents the different states that a clock can be in
    /// at any given time.
    /// </summary>
    public enum ClockState
    {
        /// <summary>
        /// The clock is currently active meaning that the current time of the
        /// clock changes relative to its parent time. 
        /// </summary>
        Active,

        /// <summary>
        /// The clock is currenty in a fill state meaning that its current time
        /// and progress do not change relative to the parent's time, but the 
        /// clock is not stopped.
        /// </summary>
        Filling,

        /// <summary>
        /// The clock is currently stopped which means that its current time and
        /// current progress property values are undefined. 
        /// </summary>
        Stopped
    }
}
