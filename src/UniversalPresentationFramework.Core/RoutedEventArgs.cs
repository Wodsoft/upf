using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public class RoutedEventArgs : EventArgs
    {
        #region Construction

        public RoutedEventArgs() { }

        /// <summary>
        ///     Constructor for <see cref="RoutedEventArgs"/>
        /// </summary>
        /// <param name="routedEvent">The new value that the RoutedEvent Property is being set to </param>
        public RoutedEventArgs(RoutedEvent routedEvent) : this(routedEvent, null)
        {
        }

        /// <summary>
        ///     Constructor for <see cref="RoutedEventArgs"/>
        /// </summary>
        /// <param name="source">The new value that the SourceProperty is being set to </param>
        /// <param name="routedEvent">The new value that the RoutedEvent Property is being set to </param>
        public RoutedEventArgs(RoutedEvent routedEvent, object? source)
        {
            _routedEvent = routedEvent;
            _source = _originalSource = source;
        }

        #endregion Construction

        #region External API
        /// <summary>
        ///     Returns the <see cref="RoutedEvent"/> associated
        ///     with this <see cref="RoutedEventArgs"/>
        /// </summary>
        /// <remarks>
        ///     The <see cref="RoutedEvent"/> cannot be null
        ///     at any time
        /// </remarks>
        public RoutedEvent? RoutedEvent
        {
            get { return _routedEvent; }
            set
            {
                if (_userInitiated && _invokingHandler)
                    throw new InvalidOperationException("RoutedEvent can not change while routing.");

                _routedEvent = value;
            }
        }

        /// <summary>
        ///     Changes the RoutedEvent assocatied with these RoutedEventArgs
        /// </summary>
        /// <remarks>
        ///     Only used internally.  Added to support cracking generic MouseButtonDown/Up events
        ///     into MouseLeft/RightButtonDown/Up events.
        /// </remarks>
        /// <param name="newRoutedEvent">
        ///     The new RoutedEvent to associate with these RoutedEventArgs
        /// </param>
        internal void OverrideRoutedEvent(RoutedEvent newRoutedEvent)
        {
            _routedEvent = newRoutedEvent;
        }

        /// <summary>
        ///     Returns a boolean flag indicating if or not this
        ///     RoutedEvent has been handled this far in the route
        /// </summary>
        /// <remarks>
        ///     Initially starts with a false value before routing
        ///     has begun
        /// </remarks>
        public bool Handled { get => _handled; set => _handled = value; }

        /// <summary>
        ///     Returns Source object that raised the RoutedEvent
        /// </summary>
        public object? Source
        {
            get { return _source; }
            set
            {
                if (_invokingHandler && _userInitiated)
                    throw new InvalidOperationException("RoutedEvent can not change while routing.");

                if (_routedEvent == null)
                    throw new InvalidOperationException("RoutedEventArgs must have RoutedEvent.");


                object? source = value;
                if (_source == null && _originalSource == null)
                {
                    // Gets here when it is the first time that the source is set.
                    // This implies that this is also the original source of the event
                    _source = _originalSource = source;
                    OnSetSource(source);
                }
                else if (_source != source)
                {
                    // This is the actiaon taken at all other times when the
                    // source is being set to a different value from what it was
                    _source = source;
                    OnSetSource(source);
                }
            }
        }

        /// <summary>
        ///     Changes the Source assocatied with these RoutedEventArgs
        /// </summary>
        /// <remarks>
        ///     Only used internally.  Added to support cracking generic MouseButtonDown/Up events
        ///     into MouseLeft/RightButtonDown/Up events.
        /// </remarks>
        /// <param name="source">
        ///     The new object to associate as the source of these RoutedEventArgs
        /// </param>
        internal void OverrideSource(object source)
        {
            _source = source;
        }

        /// <summary>
        ///     Returns OriginalSource object that raised the RoutedEvent
        /// </summary>
        /// <remarks>
        ///     Always returns the OriginalSource object that raised the
        ///     RoutedEvent unlike <see cref="RoutedEventArgs.Source"/>
        ///     that may vary under specific scenarios <para/>
        ///     This property acquires its value once before the event
        ///     handlers are invoked and never changes then on
        /// </remarks>
        public object? OriginalSource
        {
            get { return _originalSource; }
        }

        /// <summary>
        ///     Invoked when the source of the event is set
        /// </summary>
        /// <remarks>
        ///     Changing the source of an event can often
        ///     require updating the data within the event.
        ///     For this reason, the OnSource=  method is
        ///     protected virtual and is meant to be
        ///     overridden by sub-classes of
        ///     <see cref="RoutedEventArgs"/> <para/>
        ///     Also see <see cref="RoutedEventArgs.Source"/>
        /// </remarks>
        /// <param name="source">
        ///     The new value that the SourceProperty is being set to
        /// </param>
        protected virtual void OnSetSource(object? source)
        {
        }

        /// <summary>
        ///     Invokes the generic handler with the
        ///     appropriate arguments
        /// </summary>
        /// <remarks>
        ///     Is meant to be overridden by sub-classes of
        ///     <see cref="RoutedEventArgs"/> to provide
        ///     more efficient invocation of their delegate
        /// </remarks>
        /// <param name="genericHandler">
        ///     Generic Handler to be invoked
        /// </param>
        /// <param name="genericTarget">
        ///     Target on whom the Handler will be invoked
        /// </param>
        protected virtual void InvokeEventHandler(Delegate genericHandler, object genericTarget)
        {
            if (genericHandler == null)
            {
                throw new ArgumentNullException("genericHandler");
            }

            if (genericTarget == null)
            {
                throw new ArgumentNullException("genericTarget");
            }

            _invokingHandler = true;
            try
            {
                if (genericHandler is RoutedEventHandler)
                {
                    ((RoutedEventHandler)genericHandler)(genericTarget, this);
                }
                else
                {
                    // Restricted Action - reflection permission required
                    genericHandler.DynamicInvoke(new object[] { genericTarget, this });
                }
            }
            finally
            {
                _invokingHandler = false;
            }
        }

        #endregion External API

        #region Operations

        // Calls the InvokeEventHandler protected
        // virtual method
        //
        // This method is needed because
        // delegates are invoked from
        // RoutedEventHandler which is not a
        // sub-class of RoutedEventArgs
        // and hence cannot invoke protected
        // method RoutedEventArgs.FireEventHandler
        internal void InvokeHandler(Delegate handler, object target)
        {
            _invokingHandler = true;

            try
            {
                InvokeEventHandler(handler, target);
            }
            finally
            {
                _invokingHandler = false;
            }
        }

        internal bool UserInitiated => _userInitiated;

        internal void MarkAsUserInitiated()
        {
            _userInitiated = true;
        }

        internal void ClearUserInitiated()
        {
            _userInitiated = false;
        }

        #endregion Operations



        #region Data

        private RoutedEvent? _routedEvent;
        private object? _source;
        private object? _originalSource;
        private bool _userInitiated, _invokingHandler, _handled;

        #endregion Data
    }
}
