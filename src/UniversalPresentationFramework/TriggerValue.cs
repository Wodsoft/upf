using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    internal class TriggerValue
    {
        public TriggerValue(object? value)
        {
            Value = value;
        }

        public object? Value { get; }

        public bool IsEnabled { get; set; }
    }
}
