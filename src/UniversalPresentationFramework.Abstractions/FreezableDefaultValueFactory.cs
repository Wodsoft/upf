using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Input;

namespace Wodsoft.UI
{
    public class FreezableDefaultValueFactory : DefaultValueFactory
    {
        private readonly Freezable _defaultValuePrototype;

        public FreezableDefaultValueFactory(Freezable defaultValue)
        {
            _defaultValuePrototype = defaultValue.GetAsFrozen();
        }

        public override object? DefaultValue => _defaultValuePrototype;

        public override object CreateDefaultValue(DependencyObject owner, DependencyProperty property, DependencyPropertyKey? key)
        {
            Freezable result = _defaultValuePrototype;
            Freezable? ownerFreezable = owner as Freezable;

            // If the owner is frozen, just return the frozen prototype.
            if (ownerFreezable != null && ownerFreezable.IsFrozen)
                return result;

            result = _defaultValuePrototype.Clone();

            // Wire up a FreezableDefaultPromoter to observe the default value we
            // just created and automatically promote it to local if it is modified.
            FreezableDefaultPromoter promoter = new FreezableDefaultPromoter(owner, property, key, result);

            return result;
        }

        private class FreezableDefaultPromoter
        {
            private readonly DependencyObject _owner;
            private readonly DependencyProperty _property;
            private readonly DependencyPropertyKey? _key;
            private readonly Freezable _defauleValue;

            public FreezableDefaultPromoter(DependencyObject owner, DependencyProperty property, DependencyPropertyKey? key, Freezable defaultValue)
            {
                // We hang on to the property and owner so we can write the default
                // value back to the local store if it changes.  See also
                // OnDefaultValueChanged.
                _owner = owner;
                _property = property;
                _key = key;
                _defauleValue = defaultValue;
                defaultValue.Changed += OnDefaultValueChanged;
            }

            private void OnDefaultValueChanged(object? sender, EventArgs e)
            {
                PropertyMetadata metadata = _property.GetMetadata(_owner.GetType());

                // Remove this value from the DefaultValue cache so we stop
                // handing it out as the default value now that it has changed.
                metadata.ClearCachedDefaultValue(_owner, _property);

                // Since Changed is raised when the user freezes the default
                // value, we need to check before removing our handler.
                // (If the value is frozen, it will remove it's own handlers.)
                _defauleValue.Changed -= OnDefaultValueChanged;

                ref readonly var effectiveValue = ref _owner.GetEffectiveValue(_property);
                if (effectiveValue.Source != DependencyEffectiveSource.Local)
                    if (_key == null)
                        _owner.SetValue(_property, _defauleValue);
                    else
                        _owner.SetValue(_key, _defauleValue);
            }
        }
    }
}
