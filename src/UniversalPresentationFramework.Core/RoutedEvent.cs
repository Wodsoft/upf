using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    [TypeConverter("Wodsoft.UI.Markup.RoutedEventConverter, UniversalPresentationFramework")]
    public class RoutedEvent
    {
        private readonly string _name;
        private readonly RoutingStrategy _routingStrategy;
        private readonly Type _handlerType;
        private readonly Type _ownerType;
        private readonly int _globalIndex;
        private readonly Dictionary<Type, List<RoutedEventHandlerInfo>> _listeners = new Dictionary<Type, List<RoutedEventHandlerInfo>>();

        #region Construction

        internal RoutedEvent(
            string name,
            RoutingStrategy routingStrategy,
            Type handlerType,
            Type ownerType,
            int index)
        {
            _name = name;
            _routingStrategy = routingStrategy;
            _handlerType = handlerType;
            _ownerType = ownerType;

            _globalIndex = index;
        }

        /// <summary>
        ///    Index in GlobalEventManager 
        /// </summary>
        internal int GlobalIndex
        {
            get { return _globalIndex; }
        }

        #endregion Construction

        #region External API
        /// <summary>
        ///     Associate another owner type with this event.
        /// </summary>
        /// <remarks>
        ///     The owner type is used when resolving an event by name.
        /// </remarks>
        /// <param name="ownerType">Additional owner type</param>
        /// <returns>This event.</returns>
        public RoutedEvent AddOwner(Type ownerType)
        {
            EventManager.AddOwner(this, ownerType);
            return this;
        }

        /// <summary>
        ///     Returns the Name of the RoutedEvent
        /// </summary>
        /// <remarks>
        ///     RoutedEvent Name is unique within the 
        ///     OwnerType (super class types not considered 
        ///     when talking about uniqueness)
        /// </remarks>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        ///     Returns the <see cref="RoutingStrategy"/> 
        ///     of the RoutedEvent
        /// </summary>
        public RoutingStrategy RoutingStrategy
        {
            get { return _routingStrategy; }
        }

        /// <summary>
        ///     Returns Type of Handler for the RoutedEvent
        /// </summary>
        /// <remarks>
        ///     HandlerType is a type of delegate
        /// </remarks>
        public Type HandlerType
        {
            get { return _handlerType; }
        }

        // Check to see if the given delegate is a legal handler for this type.
        //  It either needs to be a type that the registering class knows how to
        //  handle, or a RoutedEventHandler which we can handle without the help
        //  of the registering class.
        internal bool IsLegalHandler(Delegate handler)
        {
            Type handlerType = handler.GetType();

            return ((handlerType == HandlerType) ||
                     (handlerType == typeof(RoutedEventHandler)));
        }

        /// <summary>
        ///     Returns Type of Owner for the RoutedEvent
        /// </summary>
        /// <remarks>
        ///     OwnerType is any object type
        /// </remarks>
        public Type OwnerType
        {
            get { return _ownerType; }
        }

        /// <summary>
        ///    String representation
        /// </summary>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", _ownerType.Name, _name);
        }

        public override int GetHashCode()
        {
            return _globalIndex;
        }

        public void RegisterClassHandler(
            Type classType,
            Delegate handler,
            bool handledEventsToo)
        {
            if (!_listeners.TryGetValue(classType, out var list))
            {
                list = new List<RoutedEventHandlerInfo>();
                //get base type listeners
                foreach (var kv in _listeners)
                    if (classType.IsSubclassOf(kv.Key))
                        list.AddRange(kv.Value);
                _listeners.Add(classType, list);
            }
            var info = new RoutedEventHandlerInfo(handler, handledEventsToo);
            if (!list.Contains(info))
            {
                list.Add(info);
                //add listener to subclasses
                foreach (var kv in _listeners)
                    if (kv.Key.IsSubclassOf(classType) && !kv.Value.Contains(info))
                        kv.Value.Add(info);
            }
        }

        internal List<RoutedEventHandlerInfo> GetClassHandlers(Type classType)
        {
            if (_listeners.TryGetValue(classType, out var list))
                return list;
            //get base type listeners
            list = new List<RoutedEventHandlerInfo>();
            foreach (var kv in _listeners)
                if (classType.IsSubclassOf(kv.Key))
                    list.AddRange(kv.Value);
            _listeners.Add(classType, list);
            return list;
        }               

        #endregion External API


    }
}
