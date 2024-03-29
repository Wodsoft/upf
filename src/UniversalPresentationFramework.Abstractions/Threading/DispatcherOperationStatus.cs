using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Threading
{
    /// <summary>
    ///     An enunmeration describing the status of a DispatcherOperation.
    /// </summary>
    ///
    public enum DispatcherOperationStatus
    {
        /// <summary>
        ///     The operation is still pending.
        /// </summary>
        Pending,

        /// <summary>
        ///     The operation has been aborted.
        /// </summary>
        Aborted,

        /// <summary>
        ///     The operation has been completed.
        /// </summary>
        Completed,

        /// <summary>
        ///     The operation has started executing, but has not completed yet.
        /// </summary>
        Executing
    }
}
