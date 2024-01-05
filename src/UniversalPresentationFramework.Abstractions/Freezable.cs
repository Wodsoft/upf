using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public abstract class Freezable : DependencyObject
    {
        private bool _isFrozen;

        public bool CanFreeze => !_isFrozen;

        public bool IsFrozen => _isFrozen;

        public void Freeze()
        {
            if (!CanFreeze)
                throw new InvalidOperationException("Object can not freeze.");
            _isFrozen = true;
        }

        protected override void SetValueCore(DependencyProperty dp, object? value)
        {
            if (IsFrozen)
                throw new InvalidOperationException("Object is frozen.");
            base.SetValueCore(dp, value);
        }

        #region InheritanceContext

        private List<(DependencyObject, DependencyProperty)>? _inheritanceContext;

        public override DependencyObject? InheritanceContext
        {
            get
            {
                if (_inheritanceContext == null || _inheritanceContext.Count != 1)
                    return null;
                return _inheritanceContext[0].Item1;
            }
        }

        public override event EventHandler? InheritanceContextChanged;

        protected override void AddInheritanceContext(DependencyObject context, DependencyProperty property)
        {
            if (_inheritanceContext == null)
                _inheritanceContext = new List<(DependencyObject, DependencyProperty)>();
            _inheritanceContext.Add((context, property));
            InheritanceContextChanged?.Invoke(this, EventArgs.Empty);
        }

        protected override void RemoveInheritanceContext(DependencyObject context, DependencyProperty property)
        {
            if (_inheritanceContext == null)
                return;
            _inheritanceContext.Remove((context, property));
            InheritanceContextChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
