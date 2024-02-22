using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public class TemplateBindingExpression : Expression
    {
        private readonly TemplateBindingExtension _extension;
        private FrameworkElement? _templatedParent;

        public TemplateBindingExpression(TemplateBindingExtension extension)
        {
            _extension = extension;
        }

        public override bool CanUpdateSource => false;

        public override bool CanUpdateTarget => true;

        protected override object? GetSourceValue()
        {
            if (_templatedParent == null)
                return Expression.NoValue;
            var value = _templatedParent.GetValue(_extension.Property!);
            if (_extension.Converter != null)
                value = _extension.Converter.Convert(value, AttachedProperty!.PropertyType, _extension.ConverterParameter, null);
            return value;
        }

        protected override void OnAttach()
        {
            var logicalObject = LogicalTreeHelper.FindMentor(AttachedObject!);
            if (logicalObject != null && logicalObject.LogicalRoot is FrameworkElement fe)
            {
                _templatedParent = fe.TemplatedParent;
            }
        }

        protected override void OnDetach()
        {

        }

        protected override void SetSourceValue(object? value)
        {

        }
    }
}
