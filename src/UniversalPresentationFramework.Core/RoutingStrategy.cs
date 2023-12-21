using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    /// <summary>
    ///     Routing Strategy can be either of 
    ///     Tunnel or Bubble
    /// </summary>
    public enum RoutingStrategy
    {
        /// <summary>
        ///     Tunnel 
        /// </summary>
        /// <remarks>
        ///     Route the event starting at the root of 
        ///     the visual tree and ending with the source
        /// </remarks>
        Tunnel,

        /// <summary>
        ///     Bubble 
        /// </summary>
        /// <remarks>
        ///     Route the event starting at the source 
        ///     and ending with the root of the visual tree
        /// </remarks>
        Bubble,

        /// <summary>
        ///     Direct 
        /// </summary>
        /// <remarks>
        ///     Raise the event at the source only.
        /// </remarks>
        Direct
    }
}
