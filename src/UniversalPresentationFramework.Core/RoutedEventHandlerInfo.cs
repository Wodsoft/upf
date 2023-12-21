using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public record struct RoutedEventHandlerInfo
    {
        private readonly Delegate _handler;
        private readonly bool _handledEventsToo;

        public RoutedEventHandlerInfo(Delegate handler, bool handledEventsToo)
        {
            _handler = handler;
            _handledEventsToo = handledEventsToo;
        }

        public Delegate Handler => _handler;

        public bool HandledEventsToo => _handledEventsToo;
    }
}
