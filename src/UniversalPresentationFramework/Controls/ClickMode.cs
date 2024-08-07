﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls
{
    /// <summary>
    /// ClickMode specifies when the Click event should fire
    /// </summary>
    public enum ClickMode
    {
        /// <summary>
        /// Used to specify that the Click event will fire on the
        /// normal down->up semantics of Button interaction.
        /// Escaping mechanisms work, too. Capture is taken by the
        /// Button while it is down and released after the
        /// Click is fired.
        /// </summary>
        Release,

        /// <summary>
        /// Used to specify that the Click event should fire on the
        /// down of the Button.  Basically, Click will fire as
        /// soon as the IsPressed property on Button becomes true.
        /// Even if the mouse is held down on the Button, capture
        /// is not taken.
        /// </summary>
        Press,

        /// <summary>
        /// Used to specify that the Click event should fire when the
        /// mouse hovers over a Button.
        /// </summary>
        Hover
    }
}
