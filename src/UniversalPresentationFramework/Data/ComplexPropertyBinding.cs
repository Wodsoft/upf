using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Data
{
    internal class ComplexPropertyBinding : PropertyBinding
    {
        private readonly PropertyBindingContext[] _bindingContexts;

        public ComplexPropertyBinding(object source, PropertyBindingContext[] bindingContexts)
        {
            _bindingContexts = bindingContexts;
        }

        public override bool CanSet => throw new NotImplementedException();

        public override bool CanGet => throw new NotImplementedException();

        public override object? GetValue()
        {
            throw new NotImplementedException();
        }

        public override void SetValue(object? value)
        {
            throw new NotImplementedException();
        }
    }
}
