using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Data
{
    /// <summary> This enum describes the type of RelativeSource
    /// </summary>
    public enum RelativeSourceMode
    {
        /// <summary>use the DataContext from the previous scope
        /// </summary>
        PreviousData,

        /// <summary>use the target element's styled parent
        /// </summary>
        TemplatedParent,

        /// <summary>use the target element itself
        /// </summary>
        Self,

        /// <summary>use the target element's ancestor of a specified Type
        /// </summary>
        FindAncestor
    }
}
