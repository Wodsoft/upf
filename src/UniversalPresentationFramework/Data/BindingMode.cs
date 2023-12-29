using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Data
{
    /// <summary> This enum describes how the data flows through a given Binding
    /// </summary>
    public enum BindingMode
    {
        /// <summary> Data flow is obtained from target property default </summary>
        Default,
        /// <summary> Data flows from source to target and vice-versa </summary>
        TwoWay,
        /// <summary> Data flows from source to target, source changes cause data flow </summary>
        OneWay,
        /// <summary> Data flows from source to target once, source changes are ignored </summary>
        OneTime,
        /// <summary> Data flows from target to source, target changes cause data flow </summary>
        OneWayToSource
    }
}
