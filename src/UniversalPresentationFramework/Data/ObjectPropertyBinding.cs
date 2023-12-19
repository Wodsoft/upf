using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Data
{
    internal class ObjectPropertyBinding : PropertyBinding
    {
        private readonly object _value;

        public ObjectPropertyBinding(object value)
        {
            _value = value;
        }

        public override bool CanSet => false;

        public override bool CanGet => true;

        public override object? GetValue() => _value;

        public override void SetValue(object? value)
        {
            throw new NotSupportedException();
        }
    }
}
