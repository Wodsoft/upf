using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Controls;
using Wodsoft.UI.Markup;

namespace Wodsoft.UI
{
    [DictionaryKeyProperty("TargetType")]
    [ContentProperty("Setters")]
    public class Style : Sealable, INameScope, IHaveResources, IQueryAmbient
    {
        private int _index;
        private static volatile int _StyleCount;

        #region Constructor

        /// <summary>
        ///     Style construction
        /// </summary>
        public Style()
        {
            GetUniqueGlobalIndex();
        }

        /// <summary>
        ///     Style construction
        /// </summary>
        /// <param name="targetType">Type in which Style will be applied</param>
        public Style(Type targetType)
        {
            TargetType = targetType;

            GetUniqueGlobalIndex();
        }

        /// <summary>
        ///     Style construction
        /// </summary>
        /// <param name="targetType">Type in which Style will be applied</param>
        /// <param name="basedOn">Style to base this Style on</param>
        public Style(Type targetType, Style basedOn)
        {
            TargetType = targetType;
            BasedOn = basedOn;

            GetUniqueGlobalIndex();
        }

        private void GetUniqueGlobalIndex()
        {
            _index = ++_StyleCount;
        }

        #endregion

        #region INameScope

        private NameScope _nameScope = new NameScope();

        /// <summary>
        /// Registers the name - Context combination
        /// </summary>
        /// <param name="name">Name to register</param>
        /// <param name="scopedElement">Element where name is defined</param>
        public void RegisterName(string name, object scopedElement)
        {
            _nameScope.RegisterName(name, scopedElement);
        }

        /// <summary>
        /// Unregisters the name - element combination
        /// </summary>
        /// <param name="name">Name of the element</param>
        public void UnregisterName(string name)
        {
            _nameScope.UnregisterName(name);
        }

        /// <summary>
        /// Find the element given name
        /// </summary>
        /// <param name="name">Name of the element</param>
        object? INameScope.FindName(string name)
        {
            return _nameScope.FindName(name);
        }

        #endregion IIdScope

        #region Properties

        private Type? _targetType;
        [Ambient]
        public Type? TargetType
        {
            get
            {
                return _targetType;
            }

            set
            {
                CheckSealed();

                if (value == null)
                    throw new ArgumentNullException("value");

                if (!typeof(FrameworkElement).IsAssignableFrom(value))
                    throw new ArgumentException("Target type must derive from FrameworkElement.");

                _targetType = value;
            }
        }

        private Style? _basedOn;
        [Ambient]
        public Style? BasedOn
        {
            get
            {
                return _basedOn;
            }
            set
            {
                CheckSealed();

                if (value == this)
                    throw new ArgumentException("Based on style can not be self.");
                _basedOn = value;
            }
        }

        private ResourceDictionary? _resources;
        [Ambient]
        public ResourceDictionary Resources
        {
            get
            {
                if (_resources == null)
                {
                    _resources = new ResourceDictionary();

                    // If the style has been sealed prior to this the newly
                    // created ResourceDictionary also needs to be sealed
                    //if (_isSealed)
                    //{
                    //    _resources.IsReadOnly = true;
                    //}
                }
                return _resources;
            }
            set
            {
                CheckSealed();
                _resources = value;
            }
        }

        private SetterBaseCollection? _setters;
        public SetterBaseCollection Setters
        {
            get
            {
                if (_setters == null)
                {
                    _setters = new SetterBaseCollection();

                    // If the style has been sealed prior to this the newly
                    // created SetterBaseCollection also needs to be sealed
                    if (IsSealed)
                        _setters.Seal();
                }
                return _setters;
            }
        }

        #endregion

        #region Seal

        protected override void OnSeal()
        {
            // Most parameter checking is done as "upstream" as possible, but some
            //  can't be checked until Style is sealed.
            if (_targetType == null)
                throw new InvalidOperationException("TargetType can not be null.");

            if (_basedOn != null)
            {
                if (_basedOn.TargetType?.IsAssignableFrom(_targetType) != true)
                {
                    throw new InvalidOperationException("Target type of based on style must be base type of target type of this style.");
                }
            }

            //// Seal setters
            if (_setters != null)
            {
                _setters.Seal();
            }

            //// Seal triggers
            //if (_visualTriggers != null)
            //{
            //    _visualTriggers.Seal();
            //}

            // Will throw InvalidOperationException if we find a loop of
            //  BasedOn references.  (A.BasedOn = B, B.BasedOn = C, C.BasedOn = A)
            CheckForCircularBasedOnReferences();

            // Seal BasedOn Style chain
            if (_basedOn != null)
            {
                _basedOn.Seal();
            }

            // Seal the ResourceDictionary
            //if (_resources != null)
            //{
            //    _resources.IsReadOnly = true;
            //}

            //
            // Build shared tables
            //

            // Process all Setters set on the selfStyle. This stores all the property
            // setters on the current styles into PropertyValues list, so it can be used
            // by ProcessSelfStyle in the next step. The EventSetters for the current
            // and all the basedOn styles are merged into the EventHandlersStore on the
            // current style.
            ProcessSetters(this);

            // Add an entry in the EventDependents list for
            // the TargetType's EventHandlersStore. Notice
            // that the childIndex is 0.
            //StyleHelper.AddEventDependent(0, this.EventHandlersStore, ref EventDependents);

            // Process all PropertyValues (all are "Self") in the Style
            // chain (base added first)
            //ProcessSelfStyles(this);

            // Process all TriggerBase PropertyValues ("Self" triggers
            // and child triggers) in the Style chain last (highest priority)
            //ProcessVisualTriggers(this);

            // Sort the ResourceDependents, to help avoid duplicate invalidations
            //StyleHelper.SortResourceDependents(ref ResourceDependents);

            // Remove thread affinity so it can be accessed across threads
            //DetachFromDispatcher();
        }

        private void CheckForCircularBasedOnReferences()
        {
            Stack basedOnHierarchy = new Stack(10);  // 10 because that's the default value (see MSDN) and the perf team wants us to specify something.
            Style? latestBasedOn = this;

            while (latestBasedOn != null)
            {
                if (basedOnHierarchy.Contains(latestBasedOn))
                {
                    // Uh-oh.  We've seen this Style before.  This means
                    //  the BasedOn hierarchy contains a loop.
                    throw new InvalidOperationException("Style based on has loop.");

                    // Debugging note: If we stop here, the basedOnHierarchy
                    //  object is still alive and we can browse through it to
                    //  see what we've explored.  (This does not apply if
                    //  somebody catches this exception and re-throws.)
                }

                // Haven't seen it, push on stack and go to next level.
                basedOnHierarchy.Push(latestBasedOn);
                latestBasedOn = latestBasedOn.BasedOn;
            }

            return;
        }

        // Iterates through the setters collection and adds the EventSetter information into
        // an EventHandlersStore for easy and fast retrieval during event routing. Also adds
        // an entry in the EventDependents list for EventhandlersStore holding the TargetType's
        // events.
        private void ProcessSetters(Style? style)
        {
            // Walk down to bottom of based-on chain
            if (style == null)
                return;

            style.Setters.Seal(); // Does not mark individual setters as sealed, that's up to the loop below.

            // On-demand create the PropertyValues list, so that we can specify the right size.

            //if (PropertyValues.Count == 0)
            //{
            //    PropertyValues = new FrugalStructList<System.Windows.PropertyValue>(style.Setters.Count);
            //}

            // Add EventSetters to local EventHandlersStore
            for (int i = 0; i < style.Setters.Count; i++)
            {
                SetterBase setterBase = style.Setters[i];
                Debug.Assert(setterBase != null, "Setter collection must contain non-null instances of SetterBase");

                // Setters are folded into the PropertyValues table only for the current style. The
                // processing of BasedOn Style properties will occur in subsequent call to ProcessSelfStyle
                if (setterBase is Setter setter)
                {
                    // Style Setters are not allowed to have a child target name - since there are no child nodes in a Style.
                    if (setter.TargetName != null)
                        throw new InvalidOperationException("Setter on style not allowed to have target.");

                    if (style == this)
                    {
                        if (!_propertySetters.ContainsKey(setter.Property!))
                            _propertySetters[setter.Property!] = setter.Value;
                        //if (setter.Value is DynamicResourceExtension dynamicResource)
                        //{
                        //    //UpdatePropertyValueList(setter.Property, PropertyValueType.Resource, dynamicResource.ResourceKey);
                        //}
                        //else
                        //{
                        //    if (!_propertySetters.ContainsKey(setter.Property!))
                        //        _propertySetters.Add(setter.Property!, setter.Value);
                        //    //UpdatePropertyValueList(setter.Property, PropertyValueType.Set, setter.ValueInternal);
                        //}
                    }
                }
                else
                {
                    //// Add this to the _eventHandlersStore

                    //EventSetter eventSetter = (EventSetter)setterBase;
                    //if (_eventHandlersStore == null)
                    //{
                    //    _eventHandlersStore = new EventHandlersStore();
                    //}
                    //_eventHandlersStore.AddRoutedEventHandler(eventSetter.Event, eventSetter.Handler, eventSetter.HandledEventsToo);

                    //SetModified(HasEventSetter);

                    //// If this event setter watches the loaded/unloaded events, set the optimization
                    //// flag.

                    //if (eventSetter.Event == FrameworkElement.LoadedEvent || eventSetter.Event == FrameworkElement.UnloadedEvent)
                    //{
                    //    _hasLoadedChangeHandler = true;
                    //}
                }
            }

            // Process EventSetters on based on style so they get merged
            // into the EventHandlersStore for the current style.
            ProcessSetters(style._basedOn);
        }

        #endregion

        public override int GetHashCode()
        {
            return _index;
        }

        bool IQueryAmbient.IsAmbientPropertyAvailable(string propertyName)
        {
            // We want to make sure that StaticResource resolution checks the .Resources
            // Ie.  The Ambient search should look at Resources if it is set.
            // Even if it wasn't set from XAML (eg. the Ctor (or derived Ctor) added stuff)
            switch (propertyName)
            {
                case "Resources":
                    if (_resources == null)
                    {
                        return false;
                    }
                    break;
                case "BasedOn":
                    if (_basedOn == null)
                    {
                        return false;
                    }
                    break;
            }
            return true;
        }

        #region Style

        private Dictionary<DependencyProperty, object?> _propertySetters = new Dictionary<DependencyProperty, object?>();

        internal bool TryApplyProperty(DependencyProperty dp, ref DependencyEffectiveValue effectiveValue)
        {
            if (_propertySetters.TryGetValue(dp, out var value))
            {
                effectiveValue = new DependencyEffectiveValue(value, DependencyEffectiveSource.Internal);
                return true;
            }
            return false;
        }

        internal static void ApplyStyle(FrameworkElement element, Style? oldStyle, Style? newStyle)
        {
            if (oldStyle == null && newStyle == null)
                return;
            IEnumerable<DependencyProperty> properties;
            if (oldStyle == null)
                properties = newStyle!._propertySetters.Keys;
            else if (newStyle == null)
                properties = oldStyle._propertySetters.Keys;
            else
                properties = oldStyle._propertySetters.Keys.Concat(newStyle._propertySetters.Keys).Distinct();
            foreach (var property in properties)
                element.InvalidateProperty(property);
        }

        #endregion
    }
}
