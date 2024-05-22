using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public class ExpressionValueChangedEventArgs : EventArgs
    {
        public ExpressionValueChangedEventArgs(object? value)
        {
            Value = value;
        }

        public object? Value { get; }
    }
}
