using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;
using System.Xaml.Markup;
using Wodsoft.UI.Controls;

namespace Wodsoft.UI.Data
{
    [MarkupExtensionReturnType(typeof(object))]
    public abstract class BindingBase : MarkupExtension
    {
        #region Properties

        private object? _fallbackValue = DependencyProperty.UnsetValue;
        public object? FallbackValue { get => _fallbackValue; set { CheckSealed(); _fallbackValue = value; } }

        private string? _stringFormat;
        public string? StringFormat { get => _stringFormat; set { CheckSealed(); _stringFormat = value; } }

        private object? _targetNullValue = DependencyProperty.UnsetValue;
        public object? TargetNullValue { get => _targetNullValue; set { CheckSealed(); _targetNullValue = value; } }

        private string _bindingGroupName = string.Empty;
        public string BindingGroupName
        {
            get => _bindingGroupName; set
            {
                CheckSealed();
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _bindingGroupName = value;
            }
        }

        private int _delay;
        public int Delay { get => _delay; set { CheckSealed(); _delay = value; } }

        #endregion

        #region Seal
        private bool _isSealed;

        protected void CheckSealed()
        {
            if (_isSealed)
                throw new InvalidOperationException("Can not change values after sealed.");
        }

        #endregion

        #region Methods

        public sealed override object? ProvideValue(IServiceProvider serviceProvider)
        {
            IProvideValueTarget? provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (provideValueTarget == null)
                return null;
            if (!(provideValueTarget.TargetObject is FrameworkElement fe))
                throw new InvalidOperationException("Binding can only set to a FrameworkElement.");
            if (!(provideValueTarget.TargetProperty is DependencyProperty dp))
                throw new InvalidOperationException("Binding can only set to dependency property.");
            return CreateBindingExpression(fe, dp);
        }

        internal BindingExpressionBase CreateBindingExpression(FrameworkElement targetObject, DependencyProperty targetProperty)
        {
            _isSealed = true;
            return CreateBindingExpressionCore(targetObject, targetProperty);
        }

        protected abstract BindingExpressionBase CreateBindingExpressionCore(FrameworkElement targetObject, DependencyProperty targetProperty);

        public abstract bool IsEqual(BindingBase bindingBase);

        #endregion

        
    }
}
