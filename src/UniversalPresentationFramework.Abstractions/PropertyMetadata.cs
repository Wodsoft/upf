using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public class PropertyMetadata
    {
        public static readonly PropertyMetadata DefaultMetadata;
        private CoerceValueCallback? _coerceValueCallback;
        private object? _defaultValue;
        private PropertyChangedCallback? _propertyChangedCallback;
        private bool _isSealed;

        static PropertyMetadata()
        {
            DefaultMetadata = new PropertyMetadata();
            DefaultMetadata.Seal();
        }

        public PropertyMetadata() { }

        public PropertyMetadata(object? defaultValue)
        {
            _defaultValue = defaultValue;
        }

        public PropertyMetadata(PropertyChangedCallback? propertyChangedCallback)
        {
            _propertyChangedCallback = propertyChangedCallback;
        }

        public PropertyMetadata(object? defaultValue, PropertyChangedCallback? propertyChangedCallback)
        {
            _defaultValue = defaultValue;
            _propertyChangedCallback = propertyChangedCallback;
        }

        public PropertyMetadata(object? defaultValue, PropertyChangedCallback? propertyChangedCallback, CoerceValueCallback? coerceValueCallback)
        {
            _defaultValue = defaultValue;
            _propertyChangedCallback = propertyChangedCallback;
            _coerceValueCallback = coerceValueCallback;
        }

        public CoerceValueCallback? CoerceValueCallback { get => _coerceValueCallback; set { SealCheck(); _coerceValueCallback = value; } }

        public object? DefaultValue { get => _defaultValue; set { SealCheck(); _defaultValue = value; } }

        public object? GetDefaultValue(DependencyObject owner, DependencyProperty property)
        {
            if (_defaultValue is not DefaultValueFactory factory)
                return _defaultValue;
            if (owner.IsSealed)
                return factory.DefaultValue;
            var value = GetCachedDefaultValue(owner, property);
            if (value != DependencyProperty.UnsetValue)
                return value;
            value = factory.CreateDefaultValue(owner, property, property.Key);
            if (!property.IsValidType(value))
                throw new ArgumentException("Default value type mismatch to property.");
            if (value is Expression)
                throw new ArgumentException("Defaule value can't be Expression.");
            if (!property.IsValidValue(value))
                throw new ArgumentException("Default value invalid.");
            SetCachedDefaultValue(owner, property, value);
            return value;
        }

        public PropertyChangedCallback? PropertyChangedCallback { get => _propertyChangedCallback; set { SealCheck(); _propertyChangedCallback = value; } }

        public virtual GetReadOnlyValueCallback? GetReadOnlyValueCallback => null;

        public bool IsSealed { get => _isSealed; }

        private void SealCheck()
        {
            if (_isSealed)
                throw new InvalidOperationException("Can not change a sealed property metadata.");
        }

        public void Seal()
        {
            _isSealed = true;
        }

        public virtual bool IsInherited => false;

        #region DefaultValue

        private static readonly DependencyProperty _DefaultValueStore = DependencyProperty.RegisterAttached("__DefaultValueStore", typeof(Dictionary<int, object?>), typeof(PropertyMetadata));

        public object? GetCachedDefaultValue(DependencyObject owner, DependencyProperty property)
        {
            ref readonly var effectiveValue = ref owner.GetEffectiveValue(_DefaultValueStore);
            if (effectiveValue.Source != DependencyEffectiveSource.Local)
                return DependencyProperty.UnsetValue;
            var store = (Dictionary<int, object?>)effectiveValue.Value!;
            if (store.TryGetValue(property.GlobalIndex, out var value))
                return value;
            return DependencyProperty.UnsetValue;
        }

        public void ClearCachedDefaultValue(DependencyObject owner, DependencyProperty property)
        {
            ref readonly var effectiveValue = ref owner.GetEffectiveValue(_DefaultValueStore);
            if (effectiveValue.Source != DependencyEffectiveSource.Local)
                return;
            var store = (Dictionary<int, object?>)effectiveValue.Value!;
            store.Remove(property.GlobalIndex);
            if (store.Count == 0)
                owner.ClearValue(_DefaultValueStore);
        }

        private void SetCachedDefaultValue(DependencyObject owner, DependencyProperty property, object? value)
        {
            ref readonly var effectiveValue = ref owner.GetEffectiveValue(_DefaultValueStore);
            Dictionary<int, object?> store;
            if (effectiveValue.Source == DependencyEffectiveSource.Local)
                store = (Dictionary<int, object?>)effectiveValue.Value!;
            else
            {
                store = new Dictionary<int, object?>();
                owner.SetValue(_DefaultValueStore, store);
            }
            store[property.GlobalIndex] = value;
        }

        #endregion
    }
}
