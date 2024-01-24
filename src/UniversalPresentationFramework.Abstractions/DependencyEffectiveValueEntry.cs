using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public ref struct DependencyEffectiveValueEntry
    {
        private readonly DependencyProperty _property;
        private readonly ref DependencyEffectiveValue _value;

        public DependencyEffectiveValueEntry(DependencyProperty property, ref DependencyEffectiveValue effectiveValue) : this()
        {
            _property = property;
            _value = ref effectiveValue;
        }

        public DependencyProperty Property => _property;

        public ref readonly DependencyEffectiveValue Value => ref _value;
    }
}
