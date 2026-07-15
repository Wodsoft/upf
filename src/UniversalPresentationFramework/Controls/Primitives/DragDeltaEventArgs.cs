using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Controls.Primitives
{
    /// <summary>
    /// This DragDeltaEventArgs class contains additional information about the
    /// DragDeltaEvent event.
    /// </summary>
    /// <seealso cref="Thumb.DragDeltaEvent" />
    /// <seealso cref="RoutedEventArgs" />
    public class DragDeltaEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// This is an instance constructor for the DragDeltaEventArgs class.  It
        /// is constructed with a reference to the event being raised.
        /// </summary>
        /// <returns>Nothing.</returns>
        public DragDeltaEventArgs(float horizontalChange, float verticalChange) : base()
        {
            _horizontalChange = horizontalChange;
            _verticalChange = verticalChange;
            RoutedEvent = Thumb.DragDeltaEvent;
        }

        /// <value>
        /// Read-only access to the horizontal change.
        /// </value>
        public float HorizontalChange
        {
            get { return _horizontalChange; }
        }

        /// <value>
        /// Read-only access to the vertical change.
        /// </value>
        public float VerticalChange
        {
            get { return _verticalChange; }
        }

        /// <summary>
        /// This method is used to perform the proper type casting in order to
        /// call the type-safe DragDeltaEventHandler delegate for the DragDeltaEvent event.
        /// </summary>
        /// <param name="genericHandler">The handler to invoke.</param>
        /// <param name="genericTarget">The current object along the event's route.</param>
        /// <returns>Nothing.</returns>
        /// <seealso cref="Thumb.DragDeltaEvent" />
        /// <seealso cref="DragDeltaEventHandler" />
        protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
        {
            DragDeltaEventHandler handler = (DragDeltaEventHandler)genericHandler;

            handler(genericTarget, this);
        }

        private float _horizontalChange;
        private float _verticalChange;
    }

    /// <summary>
    ///     This delegate must used by handlers of the DragDeltaEvent event.
    /// </summary>
    /// <param name="sender">The current element along the event's route.</param>
    /// <param name="e">The event arguments containing additional information about the event.</param>
    /// <returns>Nothing.</returns>
    public delegate void DragDeltaEventHandler(object sender, DragDeltaEventArgs e);
}
