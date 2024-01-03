using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Wodsoft.UI
{
    public abstract class Expression
    {
        private bool _isAttached;
        private readonly object _locker = new object();
        private DependencyObject? _object;
        private DependencyProperty? _property;

        public static readonly object NoValue = new object();

        /// <summary>
        /// Get a expression is attached or not.
        /// </summary>
        public virtual bool IsAttached => _isAttached;

        /// <summary>
        /// If expression can't update source, it will override expression while set value to property.
        /// </summary>
        public abstract bool CanUpdateSource { get; }

        /// <summary>
        /// If expression can't update target, means it a wrapper to get value while set value to property.
        /// </summary>
        public abstract bool CanUpdateTarget { get; }

        /// <summary>
        /// Current expression value.
        /// </summary>
        public object? Value { get; internal set; }

        public DependencyObject? AttachedObject => _object;

        public DependencyProperty? AttachedProperty => _property;

        protected abstract void SetSourceValue(object? value);

        protected abstract object? GetSourceValue();

        /// <summary>
        /// Update source.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public virtual void UpdateSource()
        {
            if (!IsAttached)
                throw new InvalidOperationException("Expression is not attaching.");
            if (!CanUpdateSource)
                throw new InvalidOperationException("Expression can't update source.");
            SetSourceValue(Value);
        }

        /// <summary>
        /// Update target. Maybe raise property changed event.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public virtual void UpdateTarget()
        {
            if (!IsAttached)
                throw new InvalidOperationException("Expression is not attaching.");
            if (!CanUpdateTarget)
                throw new InvalidOperationException("Expression can't update target.");
            var oldValue = Value;
            TryUpdateExpressionValue(GetSourceValue());
            var newValue = Value;
            if (oldValue != newValue)
                _object!.PropertyChanged(new DependencyPropertyChangedEventArgs(_property!, oldValue, newValue));
        }

        public virtual void Attach(DependencyObject d, DependencyProperty dp)
        {
            if (_isAttached)
                OnDetach();
            lock (_locker)
            {
                _object = d;
                _property = dp;
                OnAttach();
                _isAttached = true;
            }
        }

        protected abstract void OnAttach();

        public virtual void Detach()
        {
            if (!_isAttached)
                return;
            lock (_locker)
            {
                OnDetach();
                _isAttached = false;
                _object = null;
                _property = null;
                Value = DependencyProperty.UnsetValue;
            }
        }

        protected abstract void OnDetach();

        /// <summary>
        /// Update expression value.
        /// </summary>
        /// <param name="value"></param>
        protected virtual bool TryUpdateExpressionValue(object? value)
        {
            if (!_isAttached)
                throw new InvalidOperationException("Expression is not attaching.");
            if (_object!.CoereceAndValidateValue(_property!, ref value, false, out var metadata))
            {
                Value = value;
                return true;
            }
            else
            {
                Value = metadata.DefaultValue;
                return false;
            }
        }
    }
}
