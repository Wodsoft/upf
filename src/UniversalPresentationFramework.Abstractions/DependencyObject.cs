namespace Wodsoft.UI
{
    public class DependencyObject
    {
        private Type? _type;
        private SortedList<int, object?> _valueStores = new SortedList<int, object?>();

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
            if (_valueStores.TryGetValue(dp.GlobalIndex, out var storeValue))
            {
                _valueStores.Remove(dp.GlobalIndex);
                var metadata = GetMetadata(dp);
                if (metadata.PropertyChangedCallback != null && storeValue != DependencyProperty.UnsetValue)
                    metadata.PropertyChangedCallback(this, new DependencyPropertyChangedEventArgs(dp, metadata, storeValue, metadata.DefaultValue, DependencyPropertyChangedEventFlags.LocalValueChanged));
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
            if (!_valueStores.TryGetValue(dp.GlobalIndex, out var storeValue))
            {
                var metadata = GetMetadata(dp);
                if (metadata.IsInherited)
                {
                    var parent = GetInheritanceParent();
                    if (parent == null)
                        return metadata.DefaultValue;
                    return parent.GetValueCore(dp);
                }
                return metadata.DefaultValue;
            }
            if (storeValue is Expression expression)
                storeValue = expression.Value;
            if (storeValue == DependencyProperty.UnsetValue)
            {
                var metadata = GetMetadata(dp);
                return metadata.DefaultValue;
            }
            return storeValue;
        }
        public object? ReadLocalValue(DependencyProperty dp)
        {
            if (_valueStores.TryGetValue(dp.GlobalIndex, out var storeValue))
            {
                if (storeValue is Expression expression)
                    //only one way to source will have local value
                    if (!expression.CanUpdateSource || expression.CanUpdateTarget)
                        return null;
                    else
                        return expression.Value;
                return storeValue;
            }
            return null;
        }
        public bool HasLocalValue(DependencyProperty dp)
        {
            if (_valueStores.TryGetValue(dp.GlobalIndex, out var storeValue))
            {
                //only one way to source will have local value
                if (storeValue is Expression expression && (!expression.CanUpdateSource || expression.CanUpdateTarget))
                    return false;
                return true;
            }
            return false;
        }

        public Expression? GetExpression(DependencyProperty dp)
        {
            if (_valueStores.TryGetValue(dp.GlobalIndex, out var storeValue))
            {
                if (storeValue is Expression expression)
                    return expression;
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
            object? storeValue, oldValue;
            PropertyMetadata metadata;
            if (value is Expression expression)
            {
                //Always attach again
                expression.Attach(this, dp);
                if (expression.CanUpdateTarget)
                {
                    //Detach old expression
                    if (_valueStores.TryGetValue(dp.GlobalIndex, out storeValue))
                    {
                        if (storeValue is Expression oldExpression)
                        {
                            oldValue = oldExpression.Value;
                            oldExpression.Detach();
                        }
                        else
                        {
                            oldValue = storeValue;
                        }
                    }
                    else
                    {
                        metadata = GetMetadata(dp);
                        oldValue = metadata.DefaultValue;
                    }
                    //set old value to expression
                    expression.Value = oldValue;
                    expression.UpdateTarget();
                }
                else
                {
                    //One way to source expression
                    //Set value to expression and return
                    if (_valueStores.TryGetValue(dp.GlobalIndex, out storeValue))
                    {
                        if (storeValue is Expression oldExpression)
                        {
                            //Detach old expression
                            oldExpression.Detach();
                            //Do not use old expression value
                            metadata = GetMetadata(dp);
                            value = metadata.DefaultValue;
                        }
                        else
                        {
                            //Use old value
                            value = storeValue;
                        }
                    }
                    else
                    {
                        //Use default value
                        metadata = GetMetadata(dp);
                        value = metadata.DefaultValue;
                    }
                    expression.Value = value;
                    expression.UpdateSource();
                }
                _valueStores[dp.GlobalIndex] = expression;
                return;
            }
            CoereceAndValidateValue(dp, ref value, true, out metadata);
            if (_valueStores.TryGetValue(dp.GlobalIndex, out storeValue))
            {
                if (storeValue is Expression oldExpression && oldExpression.IsAttached && oldExpression.CanUpdateSource)
                {
                    //get expression old value
                    storeValue = oldExpression.Value;
                    //set expression current value
                    oldExpression.Value = value;
                    //Update source
                    oldExpression.UpdateSource();
                }
                else
                {
                    //Override express if can't set value to source
                    _valueStores[dp.GlobalIndex] = value;
                }
                if (storeValue == DependencyProperty.UnsetValue)
                    oldValue = metadata.DefaultValue;
                else
                    oldValue = storeValue;
            }
            else
            {
                oldValue = metadata.DefaultValue;
                _valueStores.Add(dp.GlobalIndex, value);
            }
            if (oldValue != value)
                PropertyChanged(new DependencyPropertyChangedEventArgs(dp, metadata, oldValue, value, DependencyPropertyChangedEventFlags.LocalValueChanged));
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
    }
}
