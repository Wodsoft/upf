using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    /// <summary>
    /// Specifies the behavior of the <see cref="ClockController.Seek"/>
    /// method by defining the meaning of that method's offset parameter.
    /// </summary>
    public enum TimeSeekOrigin
    {
        /// <summary>
        /// The offset parameter specifies the new position of the timeline as a
        /// distance from time t=0
        /// </summary>
        BeginTime,

        /// <summary>
        /// The offset parameter specifies the new position of the timeline as a
        /// distance from the end of the simple duration. If the duration is not
        /// defined, this causes the method call to have no effect.
        /// </summary>
        Duration
    }
}
