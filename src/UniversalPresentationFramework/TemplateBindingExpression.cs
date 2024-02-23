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
        private FrameworkElement? _rootElement;

        public TemplateBindingExpression(TemplateBindingExtension extension)
        {
            _extension = extension;
        }

        public override bool CanUpdateSource => false;

        public override bool CanUpdateTarget => true;

        protected override object? GetSourceValue()
        {
            if (_rootElement == null)
                return Expression.NoValue;
            if (_rootElement.TemplatedParent == null)
                return null;
            var value = _rootElement.TemplatedParent.GetValue(_extension.Property!);
            if (_extension.Converter != null)
                value = _extension.Converter.Convert(value, AttachedProperty!.PropertyType, _extension.ConverterParameter, null);
            return value;
        }

        protected override void OnAttach()
        {
            var logicalObject = LogicalTreeHelper.FindMentor(AttachedObject!);
            if (logicalObject != null && logicalObject.LogicalRoot is FrameworkElement fe)
            {
                _rootElement = fe;
                _rootElement.TemplatedParentChanged += TemplatedParentChanged;
            }
        }

        private void TemplatedParentChanged(object? sender, EventArgs e)
        {
            UpdateTarget();
        }

        protected override void OnDetach()
        {
            if (_rootElement != null)
            {
                _rootElement.TemplatedParentChanged -= TemplatedParentChanged;
            }
        }

        protected override void SetSourceValue(object? value)
        {

        }
    }
}
