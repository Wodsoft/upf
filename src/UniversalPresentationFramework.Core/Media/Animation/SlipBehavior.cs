using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    /// <summary>
    /// The SlipBehavior enumeration is used to indicate how a TimelineGroup will behave
    /// when one of its children slips.
    /// </summary>
    public enum SlipBehavior
    {
        /// <summary>
        /// Indicates that a TimelineGroup will not slip with the chidren, but will
        /// expand to fit all slipping children.
        /// NOTE: This is only effective when the TimelineGroup's duration is not explicitly
        /// specified.
        /// </summary>
        Grow,

        /// <summary>
        /// Indicates that a TimelineGroup will slip along with its first child that
        /// has CanSlip set to true.
        /// </summary>
        Slip,
    }
}
