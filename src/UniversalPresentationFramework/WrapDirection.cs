using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public enum WrapDirection
    {
        /// <summary>
        /// Content does not flow around the object.
        /// </summary>
        None = 0,

        /// <summary>
        /// Content flows around the left side of the object only; no content is displayed to the right.
        /// </summary>
        Left = 1,

        /// <summary>
        /// Content flows around the right side of the object only; no content is displayed to the left.
        /// </summary>
        Right = 2,

        /// <summary>
        /// Content flows around both sides of the object.
        /// </summary>
        Both = 3,
    }
}
