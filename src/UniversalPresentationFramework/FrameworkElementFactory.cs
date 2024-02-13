using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;
using System.Xaml.Markup;
using Wodsoft.UI.Controls;
using Wodsoft.UI.Data;
using Wodsoft.UI.Markup;

namespace Wodsoft.UI
{
    public class FrameworkElementFactory : Sealable
    {
        private readonly Type _type;
        private string? _name;
        private Dictionary<DependencyProperty, object?>? _values;
        private Dictionary<DependencyProperty, BindingBase>? _bindings;
        private Dictionary<(RoutedEvent RoutedEvent, Delegate Handler), bool>? _events;
        private FrameworkElementFactory? _parent, _firstChild, _lastChild, _nextSibling;

        public FrameworkElementFactory(Type type)
        {
            _type = type;
        }

        public Type Type
        {
            get => _type;
            //set
            //{
            //    if (!typeof(FrameworkElement).IsAssignableFrom(_type))
            //        throw new ArgumentException("Type must be FrameworkElement derived.");
            //    _type = value;
            //}
        }

        public string? Name
        {
            get { return _name; }
            set
            {
                CheckSealed();
                _name = value;
            }
        }

        public event FrameworkElementFacrotyInstanceCreated? InstanceCreated;

        public FrameworkElement Create(out INameScope nameScope)
        {
            nameScope = new NameScope();
            var fe = CreateCore(nameScope);
            NameScope.SetNameScope(fe, nameScope);
            return fe;
        }

        protected virtual FrameworkElement CreateCore(INameScope nameScope)
        {
            var instance = CreateInstance();
            if (_name != null)
                nameScope.RegisterName(_name, instance);
            if (_values != null)
                foreach (var item in _values)
                    instance.SetValue(item.Key, item.Value);
            if (_bindings != null)
                foreach (var item in _bindings)
                    instance.SetBinding(item.Key, item.Value);
            if (_events != null)
                foreach (var item in _events)
                    instance.AddHandler(item.Key.RoutedEvent, item.Key.Handler, item.Value);
            if (_firstChild != null)
            {
                if (instance is not IAddChild addChild)
                    throw new InvalidOperationException("Parent can't add child.");
                var child = _firstChild;
                while (child != null)
                {
                    var childInstance = child.CreateCore(nameScope);
                    addChild.AddChild(child);
                    child = child._nextSibling;
                }
            }
            InstanceCreated?.Invoke(this, instance);
            return instance;
        }

        public void AppendChild(FrameworkElementFactory child)
        {
            CheckSealed();
            if (child._parent != null)
                throw new ArgumentException("Factory already has a parent.");
            if (_lastChild == null)
            {
                _firstChild = _lastChild = child;
            }
            else
            {
                _lastChild._nextSibling = child;
                _lastChild = child;
            }
            child._parent = this;
        }

        public void SetValue(DependencyProperty dp, object? value)
        {
            CheckSealed();
            if (_values == null)
                _values = new Dictionary<DependencyProperty, object?>();
            _values.Add(dp, value);
        }

        public void SetBinding(DependencyProperty dp, BindingBase binding)
        {
            CheckSealed();
            if (_bindings == null)
                _bindings = new Dictionary<DependencyProperty, BindingBase>();
            _bindings.Add(dp, binding);
        }

        public void AddHandler(RoutedEvent routedEvent, Delegate handler)
        {
            AddHandler(routedEvent, handler, false);
        }

        public void AddHandler(RoutedEvent routedEvent, Delegate handler, bool handledEventsToo)
        {
            CheckSealed();
            if (_events == null)
                _events = new Dictionary<(RoutedEvent, Delegate), bool>();
            _events[(routedEvent, handler)] = handledEventsToo;
        }

        public void RemoveHandler(RoutedEvent routedEvent, Delegate handler)
        {
            CheckSealed();
            if (_events == null)
                return;
            _events.Remove((routedEvent, handler));
        }

        protected virtual FrameworkElement CreateInstance()
        {
            return (FrameworkElement)Activator.CreateInstance(_type)!;
        }
    }

    public class FrameworkElementFactory<T> : FrameworkElementFactory
    {
        public FrameworkElementFactory() : base(typeof(T))
        {

        }
    }

    public delegate void FrameworkElementFacrotyInstanceCreated(FrameworkElementFactory factory, FrameworkElement fe);
}
