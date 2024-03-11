using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls.Primitives
{
    /// <summary>
    /// This enum is used by the ItemContainerGenerator and its client to specify
    /// the direction in which the generator produces UI.
    /// </summary>
    public enum GeneratorDirection
    {
        /// <summary> generate forward through the item collection </summary>
        Forward,

        /// <summary> generate backward through the item collection </summary>
        Backward
    }
}
