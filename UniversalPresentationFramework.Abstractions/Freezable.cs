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
    }
}
