using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ComponentModel
{
    public class CurrentChangingEventArgs : EventArgs
    {
        //------------------------------------------------------
        //
        //  Constructors
        //
        //------------------------------------------------------

        /// <summary>
        /// Construct a cancelable CurrentChangingEventArgs that is used
        /// to notify listeners when CurrentItem is about to change.
        /// </summary>
        public CurrentChangingEventArgs()
        {
            Initialize(true);
        }

        /// <summary>
        /// Construct a CurrentChangingEventArgs that is used to notify listeners when CurrentItem is about to change.
        /// </summary>
        /// <param name="isCancelable">if false, setting Cancel to true will cause an InvalidOperationException to be thrown.</param>
        public CurrentChangingEventArgs(bool isCancelable)
        {
            Initialize(isCancelable);
        }

        private void Initialize(bool isCancelable)
        {
            _isCancelable = isCancelable;
        }

        //------------------------------------------------------
        //
        //  Public Properties
        //
        //------------------------------------------------------

        /// <summary>
        /// If this event can be canceled.  When this is False, setting Cancel to True will cause an InvalidOperationException to be thrown.
        /// </summary>
        public bool IsCancelable
        {
            get { return _isCancelable; }
        }

        /// <summary>
        /// When set to true, this event will be canceled.
        /// </summary>
        /// <remarks>
        /// If IsCancelable is False, setting this value to True will cause an InvalidOperationException to be thrown.
        /// </remarks>
        public bool Cancel
        {
            get { return _cancel; }
            set
            {
                if (IsCancelable)
                {
                    _cancel = value;
                }
                else if (value)
                {
                    throw new InvalidOperationException("Current changing can not cancel.");
                }
            }
        }

        //------------------------------------------------------
        //
        //  Private Fields
        //
        //------------------------------------------------------

        private bool _cancel = false;
        private bool _isCancelable;
    }

    /// <summary>
    ///     The delegate to use for handlers that receive the CurrentChanging event.
    /// </summary>
    public delegate void CurrentChangingEventHandler(object sender, CurrentChangingEventArgs e);
}
