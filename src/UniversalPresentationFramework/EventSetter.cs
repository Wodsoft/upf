using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Controls;
using Wodsoft.UI.Markup;

namespace Wodsoft.UI
{
    public class EventSetter : SetterBase
    {
        private RoutedEvent? _event;
        private Delegate? _handler;
        private bool _handledEventsToo;

        /// <summary>
        ///     EventSetter construction
        /// </summary>
        public EventSetter()
        {
        }

        /// <summary>
        ///     EventSetter construction
        /// </summary>
        public EventSetter(RoutedEvent routedEvent, Delegate handler)
        {
            if (routedEvent == null)
            {
                throw new ArgumentNullException("routedEvent");
            }
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }

            _event = routedEvent;
            _handler = handler;
        }


        public RoutedEvent? Event
        {
            get { return _event; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                CheckSealed();
                _event = value;
            }
        }

        [TypeConverter(typeof(EventSetterHandlerConverter))]
        public Delegate? Handler
        {
            get { return _handler; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                CheckSealed();
                _handler = value;
            }
        }

        public bool HandledEventsToo
        {
            get { return _handledEventsToo; }
            set
            {
                CheckSealed();
                _handledEventsToo = value;
            }
        }

        protected override void OnSeal()
        {
            if (_event == null)
                throw new ArgumentException("Event can't be null.");
            if (_handler == null)
                throw new ArgumentException("Handler can't be null.");
            if (_handler.GetType() != _event.HandlerType)
                throw new ArgumentException("Event handler type invalid.");
        }
    }
}
