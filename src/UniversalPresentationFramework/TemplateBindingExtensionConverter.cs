using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Controls;

namespace Wodsoft.UI
{
    public class TemplateBindingExtensionConverter : TypeConverter
    {
        /// <summary>
        /// Returns true if converting to an InstanceDescriptor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Converts to an InstanceDescriptor
        /// </summary>
        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                if (value is TemplateBindingExtension templateBinding)
                {
                    return new InstanceDescriptor(typeof(TemplateBindingExtension).GetConstructor(new Type[] { typeof(DependencyProperty) }),
                        new object?[] { templateBinding.Property });
                }
                throw new ArgumentException("Value must be TemplateBindingExtension.");
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
