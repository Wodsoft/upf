using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    internal class TriggerModifiedValue : IDependencyModifiedValue
    {
        public TriggerModifiedValue(object? value)
        {
            Value = value;
        }

        public object? Value { get; set; }

        public object? GetValue(ref DependencyEffectiveValue effectiveValue)
        {
            return Value;
        }
    }
}
