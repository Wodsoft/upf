using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Controls;
using Wodsoft.UI.Data;

namespace Wodsoft.UI
{
    [XamlSetMarkupExtension("ReceiveMarkupExtension")]
    [XamlSetTypeConverter("ReceiveTypeConverter")]
    public class Setter : SetterBase, ISupportInitialize
    {
        private DependencyProperty? _property;
        private string? _targetName;
        private object? _value;

        #region Constructor

        public Setter()
        {

        }

        public Setter(DependencyProperty property, object? value)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            Initialize(property, value, null);
        }

        public Setter(DependencyProperty property, object? value, string targetName)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));
            Initialize(property, value, targetName);
        }

        private void Initialize(DependencyProperty property, object? value, string? targetName)
        {
            if (value == DependencyProperty.UnsetValue)
                throw new ArgumentException("Setter value can not be DependencyProperty.UnsetValue.");

            CheckValidProperty(property);

            // No null check for target since null is a valid value.

            _property = property;
            _value = value;
            _targetName = targetName;
        }

        private void CheckValidProperty(DependencyProperty property)
        {
            if (property.ReadOnly)
                throw new ArgumentException("Readonly property not allowed.");
            if (property == FrameworkElement.NameProperty)
                throw new InvalidOperationException("Can not set property \"FrameworkElement.Name\" in setter.");
        }

        #endregion

        #region Seal

        protected override void OnSeal()
        {
            if (_property == null)
            {
                throw new ArgumentException("Property can not be null.");
            }

            if (string.IsNullOrEmpty(_targetName))
            {
                // Setter on container is not allowed to affect the StyleProperty.
                if (_property == FrameworkElement.StyleProperty)
                {
                    throw new ArgumentException("Can not set property \"FrameworkElement.Style\" in setter.");
                }
            }

            // Value needs to be valid for the DP, or a deferred reference, or one of the supported
            // markup extensions.

            // The only markup extensions supported by styles is resources and bindings.
            if (_value is MarkupExtension)
            {
                if (!(_value is DynamicResourceExtension) && !(_value is Data.BindingBase))
                {
                    throw new ArgumentException($"Not support markup extension \"{_value.GetType()}\" value in setter.");
                }
            }
            //else if (!(value is DeferredReference))
            //{
            //    throw new ArgumentException(SR.Get(SRID.InvalidSetterValue, value, dp.OwnerType, dp.Name));
            //}
            else if (!_property.IsValidValue(_value))
            {

            }

            // Freeze the value for the setter
            if (_value is ISealable sealable)
                sealable.Seal();
        }

        #endregion

        #region Properties

        [Ambient]
        public DependencyProperty? Property
        {
            get { return _property; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                CheckValidProperty(value);
                CheckSealed();
                _property = value;
            }
        }

        [DependsOn("Property")]
        [DependsOn("TargetName")]
        [TypeConverter(typeof(SetterTriggerConditionValueConverter))]
        public object? Value
        {
            get
            {
                //// Inflate the deferred reference if the _value is one of those.
                //DeferredReference deferredReference = _value as DeferredReference;
                //if (deferredReference != null)
                //{
                //    _value = deferredReference.GetValue(BaseValueSourceInternal.Unknown);
                //}
                return _value;
            }
            set
            {
                if (value == DependencyProperty.UnsetValue)
                    throw new ArgumentException("Setter value can not be DependencyProperty.UnsetValue.");
                CheckSealed();

                // No Expression support
                if (value is Expression)
                    throw new ArgumentException("Setter value can not be Expression.");
                _value = value;
            }
        }

        [Ambient]
        public string? TargetName
        {
            get
            {
                return _targetName;
            }
            set
            {
                CheckSealed();
                _targetName = value;
            }
        }


        #endregion

        #region Xaml

        private ITypeDescriptorContext? _serviceProvider;
        private object? _unresolvedProperty, _unresolvedValue;
        private CultureInfo? _cultureInfoForTypeConverter;

        public static void ReceiveMarkupExtension(object targetObject, XamlSetMarkupExtensionEventArgs eventArgs)
        {
            if (targetObject == null)
                throw new ArgumentNullException("targetObject");
            if (eventArgs == null)
                throw new ArgumentNullException("eventArgs");

            Setter? setter = targetObject as Setter;

            if (setter == null || eventArgs.Member.Name != "Value")
                return;

            MarkupExtension me = eventArgs.MarkupExtension;

            if (me is StaticResourceExtension sr)
            {
                //setter.Value = sr.ProvideValueInternal(eventArgs.ServiceProvider, true /*allowDeferedReference*/);
                eventArgs.Handled = true;
            }
            else if (me is DynamicResourceExtension || me is BindingBase)
            {
                setter.Value = me;
                eventArgs.Handled = true;
            }
        }

        public static void ReceiveTypeConverter(object targetObject, XamlSetTypeConverterEventArgs eventArgs)
        {
            Setter? setter = targetObject as Setter;
            if (setter == null)
                throw new ArgumentNullException("targetObject");
            if (eventArgs == null)
                throw new ArgumentNullException("eventArgs");

            if (eventArgs.Member.Name == "Property")
            {
                setter._unresolvedProperty = eventArgs.Value;
                setter._serviceProvider = eventArgs.ServiceProvider;
                setter._cultureInfoForTypeConverter = eventArgs.CultureInfo;

                eventArgs.Handled = true;
            }
            else if (eventArgs.Member.Name == "Value")
            {
                setter._unresolvedValue = eventArgs.Value;
                setter._serviceProvider = eventArgs.ServiceProvider;
                setter._cultureInfoForTypeConverter = eventArgs.CultureInfo;

                eventArgs.Handled = true;
            }
        }

        #endregion

        #region ISupportInitialize

        void ISupportInitialize.BeginInit()
        {
        }

        void ISupportInitialize.EndInit()
        {
            // Resolve all properties here
            if (_unresolvedProperty != null)
            {
                try
                {
                    Property = DependencyPropertyHelper.ResolveProperty(_serviceProvider!,
                        _targetName, _unresolvedProperty);
                }
                finally
                {
                    _unresolvedProperty = null;
                }
            }
            if (_unresolvedValue != null)
            {
                try
                {
                    Value = SetterTriggerConditionValueConverter.ResolveValue(_serviceProvider,
                        Property, _cultureInfoForTypeConverter, _unresolvedValue);
                }
                finally
                {
                    _unresolvedValue = null;
                }
            }

            _serviceProvider = null;
            _cultureInfoForTypeConverter = null;
        }

        #endregion
    }
}
