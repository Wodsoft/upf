using System.Linq.Expressions;

namespace Wodsoft.UI
{
    public class DependencyObject
    {
        #region Values

        private Type? _type;
        private Dictionary<int, DependencyEffectiveValue> _valueStores = new Dictionary<int, DependencyEffectiveValue>();

        public void ClearValue(DependencyProperty dp)
        {
            if (dp == null)
                throw new ArgumentNullException(nameof(dp));
            ClearValueCore(dp);
        }
        public void ClearValue(DependencyPropertyKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            ClearValueCore(key.DependencyProperty);
        }
        protected virtual void ClearValueCore(DependencyProperty dp)
        {
            if (_valueStores.TryGetValue(dp.GlobalIndex, out var effectiveValue))
            {
                var newEffectiveValue = new DependencyEffectiveValue();
                UpdateEffectiveValue(dp, GetMetadata(dp), ref effectiveValue, ref newEffectiveValue);
                _valueStores.Remove(dp.GlobalIndex);
                //if (effectiveValue.Source == DependencyEffectiveSource.Expression)
                //    effectiveValue.Expression!.Detach();
                //var metadata = GetMetadata(dp);
                //if (metadata.PropertyChangedCallback != null && effectiveValue.Value != DependencyProperty.UnsetValue)
                //    metadata.PropertyChangedCallback(this, new DependencyPropertyChangedEventArgs(dp, metadata, effectiveValue.Value, metadata.DefaultValue, DependencyPropertyChangedEventFlags.LocalValueChanged));
            }
        }

        public object? GetValue(DependencyProperty dp)
        {
            if (dp == null)
                throw new ArgumentNullException(nameof(dp));
            return GetValueCore(dp);
        }
        protected virtual object? GetValueCore(DependencyProperty dp)
        {
            _valueStores.TryGetValue(dp.GlobalIndex, out var effectiveValue);
            return GetEffectiveValue(dp, ref effectiveValue, GetMetadata(dp));
        }
        private object? GetEffectiveValue(DependencyProperty dp, ref DependencyEffectiveValue effectiveValue, PropertyMetadata metadata)
        {
            var value = effectiveValue.Value;
            if (value == DependencyProperty.UnsetValue)
            {
                if (metadata.IsInherited)
                {
                    var parent = GetInheritanceParent();
                    if (parent != null)
                    {
                        value = parent.GetValueCore(dp);
                    }
                    else
                        value = metadata.DefaultValue;
                }
                else
                    value = metadata.DefaultValue;
            }
            return value;
        }
        public object? ReadLocalValue(DependencyProperty dp)
        {
            if (_valueStores.TryGetValue(dp.GlobalIndex, out var effectiveValue))
            {
                if (effectiveValue.Source == DependencyEffectiveSource.Expression)
                    //only one way to source will have local value
                    if (!effectiveValue.Expression!.CanUpdateSource || effectiveValue.Expression!.CanUpdateTarget)
                        return DependencyProperty.UnsetValue;
                    else
                        return effectiveValue.Value;
                else if (effectiveValue.Source == DependencyEffectiveSource.Local)
                    return effectiveValue.Value;
            }
            return DependencyProperty.UnsetValue;
        }
        public bool HasLocalValue(DependencyProperty dp)
        {
            if (_valueStores.TryGetValue(dp.GlobalIndex, out var effectiveValue))
            {
                //only one way to source will have local value
                if (effectiveValue.Source == DependencyEffectiveSource.Expression && (!effectiveValue.Expression!.CanUpdateSource || effectiveValue.Expression!.CanUpdateTarget))
                    return false;
                return effectiveValue.Source == DependencyEffectiveSource.Local && effectiveValue.Value != DependencyProperty.UnsetValue;
            }
            return false;
        }

        public Expression? GetExpression(DependencyProperty dp)
        {
            if (_valueStores.TryGetValue(dp.GlobalIndex, out var effectiveValue))
            {
                return effectiveValue.Expression;
            }
            return null;
        }

        public void SetValue(DependencyProperty dp, object? value)
        {
            if (dp == null)
                throw new ArgumentNullException(nameof(dp));
            SetValueCore(dp, value);
        }
        public void SetValue(DependencyPropertyKey key, object? value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            SetValueCore(key.DependencyProperty, value);
        }
        protected virtual void SetValueCore(DependencyProperty dp, object? value)
        {
            DependencyEffectiveValue oldValue, newValue;
            _valueStores.TryGetValue(dp.GlobalIndex, out oldValue);
            if (value is Expression expression)
            {
                newValue = new DependencyEffectiveValue(expression);
                UpdateEffectiveValue(dp, GetMetadata(dp), ref oldValue, ref newValue);
                return;
            }
            CoereceAndValidateValue(dp, ref value, true, out var metadata);

            if (oldValue.Source == DependencyEffectiveSource.Expression && oldValue.Expression!.IsAttached && oldValue.Expression!.CanUpdateSource)
            {
                newValue = new DependencyEffectiveValue(oldValue.Expression!);
                newValue.ModifyValue(value);
            }
            else
            {
                newValue = new DependencyEffectiveValue(value, DependencyEffectiveSource.Local);
            }
            UpdateEffectiveValue(dp, metadata, ref oldValue, ref newValue);
            //if (oldValue != value)
            //    PropertyChanged(new DependencyPropertyChangedEventArgs(dp, metadata, oldValue, value, DependencyPropertyChangedEventFlags.LocalValueChanged));
        }

        internal PropertyMetadata GetMetadata(DependencyProperty dp)
        {
            if (_type == null)
                _type = GetType();
            return dp.GetMetadata(_type);
        }

        internal bool CoereceAndValidateValue(DependencyProperty dp, ref object? value, bool throwException, out PropertyMetadata metadata)
        {
            metadata = GetMetadata(dp);
            if (metadata.CoerceValueCallback != null)
                value = metadata.CoerceValueCallback(this, value);
            if (dp.ValidateValueCallback != null && !dp.ValidateValueCallback(value))
                if (throwException)
                    throw new ArgumentException("Value validate failed.", nameof(value));
                else
                    return false;
            if (value != null && !dp.PropertyType.IsAssignableFrom(value.GetType()))
                if (throwException)
                    throw new ArgumentNullException($"Value can not assignable from \"{dp.PropertyType}\".");
                else
                    return false;
            return true;
        }

        /// <summary>
        /// Get inheritance parent dependency object.
        /// </summary>
        /// <returns></returns>
        protected virtual DependencyObject? GetInheritanceParent()
        {
            return null;
        }

        internal void PropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            OnPropertyChanged(e);
            DependencyPropertyChanged?.Invoke(this, e);
        }

        protected virtual void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Metadata?.PropertyChangedCallback != null)
                e.Metadata.PropertyChangedCallback(this, e);
        }

        public event DependencyPropertyChangedEventHandler? DependencyPropertyChanged;

        public void InvalidateProperty(DependencyProperty dp)
        {
            DependencyEffectiveValue oldValue, newValue;
            if (_valueStores.TryGetValue(dp.GlobalIndex, out oldValue))
            {
                if (oldValue.Source == DependencyEffectiveSource.Local || oldValue.Source == DependencyEffectiveSource.Expression)
                    newValue = oldValue;
                else
                    newValue = new DependencyEffectiveValue();
            }
            else
                newValue = new DependencyEffectiveValue();
            UpdateEffectiveValue(dp, GetMetadata(dp), ref oldValue, ref newValue);
        }

        private void UpdateEffectiveValue(DependencyProperty dp, PropertyMetadata metadata, ref DependencyEffectiveValue oldEffectiveValue, ref DependencyEffectiveValue newEffectiveValue)
        {
            //Update value from source if there is difference expression
            if (newEffectiveValue.Source == DependencyEffectiveSource.Expression)
            {
                if (newEffectiveValue.Expression != oldEffectiveValue.Expression)
                {
                    newEffectiveValue.Expression!.Attach(this, dp);
                    if (newEffectiveValue.Expression!.CanUpdateTarget)
                    {
                        newEffectiveValue.Expression!.UpdateTarget();
                    }
                }
            }

            //Evaluate framework value
            EvaluateBaseValue(dp, metadata, ref newEffectiveValue);

            //Nothing happen, return
            if (newEffectiveValue.Source == DependencyEffectiveSource.None && oldEffectiveValue.Source == DependencyEffectiveSource.None)
                return;

            //Save effective value
            if (newEffectiveValue.Source == DependencyEffectiveSource.None || newEffectiveValue.Source == DependencyEffectiveSource.Inherited)
            {
                _valueStores.Remove(dp.GlobalIndex);
            }
            else
            {
                _valueStores[dp.GlobalIndex] = newEffectiveValue;
            }

            if (CanBeInheritanceContext)
            {
                if (oldEffectiveValue.Source == DependencyEffectiveSource.Local)
                {
                    if (oldEffectiveValue.Value is DependencyObject oldValue && ShouldProvideInheritanceContext(oldValue, dp))
                        oldValue.RemoveInheritanceContext(this, dp);
                }
                if (newEffectiveValue.Source == DependencyEffectiveSource.Local)
                {
                    if (newEffectiveValue.Value is DependencyObject newValue && ShouldProvideInheritanceContext(newValue, dp))
                        newValue.AddInheritanceContext(this, dp);
                }
            }

            //Notify property changed
            //Don't notify when value is inherited and there is no old value
            if (!(newEffectiveValue.Source == DependencyEffectiveSource.Inherited && oldEffectiveValue.Source == DependencyEffectiveSource.None) &&
                oldEffectiveValue.Value != newEffectiveValue.Value)
            {
                var oldValue = GetEffectiveValue(dp, ref oldEffectiveValue, metadata);
                var newValue = GetEffectiveValue(dp, ref newEffectiveValue, metadata);
                if (oldValue != newValue)
                    PropertyChanged(new DependencyPropertyChangedEventArgs(dp, metadata, oldValue, newValue));
            }

            //Update value to source if there is same expression
            //Only a old expression that can update source may equal to new value expression
            if (oldEffectiveValue.Source == DependencyEffectiveSource.Expression)
            {
                if (oldEffectiveValue.Expression == newEffectiveValue.Expression)
                {
                    newEffectiveValue.Expression!.UpdateSource();
                }
                else
                {
                    oldEffectiveValue.Expression!.Detach();
                }
            }
            if (newEffectiveValue.Source == DependencyEffectiveSource.Expression)
            {
                if (oldEffectiveValue.Expression != newEffectiveValue.Expression && newEffectiveValue.Expression!.CanUpdateSource)
                {
                    //One way to source expression
                    //Set value to expression and return
                    newEffectiveValue.Expression!.UpdateSource();
                }
            }
        }

        protected virtual void EvaluateBaseValue(DependencyProperty dp, PropertyMetadata metadata, ref DependencyEffectiveValue effectiveValue)
        {

        }

        #endregion

        #region InheritanceContext

        public virtual DependencyObject? InheritanceContext => null;

        public virtual event EventHandler? InheritanceContextChanged;

        protected virtual void AddInheritanceContext(DependencyObject context, DependencyProperty property) { }

        protected virtual void RemoveInheritanceContext(DependencyObject context, DependencyProperty property) { }

        protected virtual bool ShouldProvideInheritanceContext(DependencyObject target, DependencyProperty property) => false;

        protected virtual bool CanBeInheritanceContext => false;

        #endregion
    }
}
