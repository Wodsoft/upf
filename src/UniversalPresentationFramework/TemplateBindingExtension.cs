using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Controls;
using Wodsoft.UI.Data;

namespace Wodsoft.UI
{
    [TypeConverter(typeof(TemplateBindingExtensionConverter))]
    [MarkupExtensionReturnType(typeof(object))]
    public class TemplateBindingExtension : MarkupExtension
    {
        public TemplateBindingExtension() { }

        public TemplateBindingExtension(DependencyProperty property)
        {
            _property = property;
        }

        #region Properties

        private DependencyProperty? _property;
        private IValueConverter? _converter;
        private object? _parameter;

        [ConstructorArgument("property")]
        public DependencyProperty? Property
        {
            get { return _property; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                _property = value;
            }
        }

        public IValueConverter? Converter
        {
            get { return _converter; }
            set
            {
                _converter = value;
            }
        }

        public object? ConverterParameter
        {
            get { return _parameter; }
            set { _parameter = value; }
        }

        #endregion

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Property == null)
                throw new InvalidOperationException("Property can't be null.");
            return new TemplateBindingExpression(this);
        }
    }
}
