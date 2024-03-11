using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Controls;

namespace Wodsoft.UI.Data
{
    public abstract class BindingExpressionBase : Expression
    {
        private readonly WeakReference<FrameworkElement> _target;

        protected BindingExpressionBase(FrameworkElement targetObject, DependencyProperty targetProperty)
        {
            _target = new WeakReference<FrameworkElement>(targetObject);
            TargetProperty = targetProperty;
        }

        public FrameworkElement? Target
        {
            get
            {
                if (_target.TryGetTarget(out var target))
                    return target;
                return null;
            }
        }

        public DependencyProperty TargetProperty { get; }

        public abstract bool HasError { get; }

        public abstract bool HasValidationError { get; }

        public abstract ValidationError? ValidationError { get; }

        public abstract ReadOnlyCollection<ValidationError>? ValidationErrors { get; }

        public abstract BindingBase ParentBindingBase { get; }

        public override void Attach(DependencyObject d, DependencyProperty dp)
        {
            if (d != Target)
                throw new InvalidOperationException("Attaching dependency object must same with target.");
            if (dp != TargetProperty)
                throw new InvalidOperationException("Attaching dependency property must same with target property.");
            base.Attach(d, dp);
        }

        private bool _hasBindingRetry;
        protected sealed override void OnAttach()
        {
            if (OnAttachCore())
            {
                if (_hasBindingRetry)
                {
                    var list = (List<BindingExpressionBase>?)Target!.GetValue(BindingRetryProperty);
                    if (list != null)
                    {
                        list.Remove(this);
                        if (list.Count == 0)
                            Target.ClearValue(_BindingRetryPropertyKey);
                    }
                    _hasBindingRetry = false;
                }
            }
            else
            {
                if (!_hasBindingRetry)
                {
                    var list = (List<BindingExpressionBase>?)Target!.GetValue(BindingRetryProperty);
                    if (list == null)
                        list = new List<BindingExpressionBase>();
                    list.Add(this);
                    Target!.SetValue(_BindingRetryPropertyKey, list);
                    _hasBindingRetry = true;
                }
            }
        }

        protected abstract bool OnAttachCore();

        internal void RetryAttach()
        {
            OnAttach();
            if (CanUpdateTarget)
                UpdateTarget();
            else
                UpdateSource();
        }

        private static readonly DependencyPropertyKey _BindingRetryPropertyKey = DependencyProperty.RegisterAttachedReadOnly("BindingRetry", typeof(List<BindingExpressionBase>), typeof(BindingExpressionBase), new PropertyMetadata());
        public static readonly DependencyProperty BindingRetryProperty = _BindingRetryPropertyKey.DependencyProperty;


        internal static readonly DependencyProperty NoTargetProperty = DependencyProperty.RegisterAttached("NoTarget", typeof(object), typeof(BindingExpressionBase), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));
    }
}
