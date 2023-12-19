using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Data
{
    /// <summary> This enum describes when updates (target-to-source data flow)
    /// happen in a given Binding.
    /// </summary>
    public enum UpdateSourceTrigger
    {
        /// <summary> Obtain trigger from target property default </summary>
        Default,
        /// <summary> Update whenever the target property changes </summary>
        PropertyChanged,
        /// <summary> Update only when target element loses focus, or when Binding deactivates </summary>
        LostFocus,
        /// <summary> Update only by explicit call to BindingExpression.UpdateSource() </summary>
        Explicit
    }
}
