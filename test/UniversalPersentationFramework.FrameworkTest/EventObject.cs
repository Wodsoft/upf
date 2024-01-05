using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Test
{
    public class EventObject : FrameworkElement
    {
        public static readonly RoutedEvent PreClickEvent = EventManager.RegisterRoutedEvent("PreClick", RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(EventObject));
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(EventObject));
        public static readonly RoutedEvent DirectEvent = EventManager.RegisterRoutedEvent("Direct", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(EventObject));

        public event RoutedEventHandler PreClick { add { AddHandler(PreClickEvent, value); } remove { RemoveHandler(PreClickEvent, value); } }
        public event RoutedEventHandler Click { add { AddHandler(ClickEvent, value); } remove { RemoveHandler(ClickEvent, value); } }
        public event RoutedEventHandler Direct { add { AddHandler(DirectEvent, value); } remove { RemoveHandler(DirectEvent, value); } }

        private UIElement? _content;
        public UIElement? Content
        {
            get => _content;
            set
            {
                if (_content == value)
                    return;
                var oldValue = _content;
                if (oldValue != null)
                    RemoveVisualChild(oldValue);
                _content = value;
                if (value != null)
                    AddVisualChild(value);
            }
        }

        public void RaisePreClick()
        {
            RaiseEvent(new RoutedEventArgs(PreClickEvent));
        }

        public void RaiseClick()
        {
            RaiseEvent(new RoutedEventArgs(ClickEvent));
        }

        public void RaiseDirect()
        {
            RaiseEvent(new RoutedEventArgs(DirectEvent));
        }
    }
}
