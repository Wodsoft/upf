using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Input
{
    public class InputEventArgs : RoutedEventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the InputEventArgs class.
        /// </summary>
        /// <param name="inputDevice">
        ///     The input device to associate with this event.
        /// </param>
        /// <param name="timestamp">
        ///     The time when the input occurred. 
        /// </param>
        public InputEventArgs(InputDevice inputDevice, int timestamp)
        {
            /* inputDevice parameter being null is valid*/
            /* timestamp parameter is valuetype, need not be checked */
            _inputDevice = inputDevice;
            _timestamp = timestamp;
        }

        /// <summary>
        ///     Read-only access to the input device that initiated this
        ///     event.
        /// </summary>
        public InputDevice Device
        {
            get { return _inputDevice; }
            internal set { _inputDevice = value; }
        }

        /// <summary>
        ///     Read-only access to the input timestamp.
        /// </summary>
        public int Timestamp
        {
            get { return _timestamp; }
        }

        /// <summary>
        ///     The mechanism used to call the type-specific handler on the
        ///     target.
        /// </summary>
        /// <param name="genericHandler">
        ///     The generic handler to call in a type-specific way.
        /// </param>
        /// <param name="genericTarget">
        ///     The target to call the handler on.
        /// </param>
        protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
        {
            InputEventHandler handler = (InputEventHandler)genericHandler;
            handler(genericTarget, this);
        }

        private InputDevice _inputDevice;
        private int _timestamp;
    }
}
