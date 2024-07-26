using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Documents
{
    public enum ElementEdge : byte
    {
        None = 0,
        /// <summary>
        ///   Located before the beginning of a DependencyObject
        /// </summary>
        BeforeStart = 1,
        /// <summary>
        ///   Located after the beginning of a DependencyObject
        /// </summary>
        AfterStart = 2,
        /// <summary>
        ///   Located before the end of a DependencyObject
        /// </summary>
        BeforeEnd = 3,
        /// <summary>
        ///   Located after the end of a DependencyObject
        /// </summary>
        AfterEnd = 4
    }
}
