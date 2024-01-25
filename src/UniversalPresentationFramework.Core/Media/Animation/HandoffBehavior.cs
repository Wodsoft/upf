using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    /// <summary>
    /// Used to specify how new animations will interact with any current
    /// animations already applied to a property.
    /// </summary>
    public enum HandoffBehavior
    {
        /// <summary>
        /// New animations will completely replace all current animations
        /// on a property. The current value at the time of replacement
        /// will be passed into the first new animation as the 
        /// defaultOriginValue parameter to allow for smooth handoff.
        /// </summary>
        SnapshotAndReplace,

        /// <summary>
        /// New animations will compose with the current animations. The new
        /// animations will be added after the current animations in the
        /// composition chain.
        /// </summary>
        Compose
    }
}
