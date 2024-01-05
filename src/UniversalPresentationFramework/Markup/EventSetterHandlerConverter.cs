using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using System.Xaml;
using Wodsoft.UI.Controls;

namespace Wodsoft.UI.Markup
{
    public sealed class EventSetterHandlerConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? typeDescriptorContext, Type sourceType)
        {
            // We can only convert from a string and that too only if we have all the contextual information
            // Note: Sometimes even the serializer calls CanConvertFrom in order
            // to determine if it is a valid converter to use for serialization.
            if (sourceType == typeof(string))
                return true;
            return false;
        }

        public override bool CanConvertTo(ITypeDescriptorContext? typeDescriptorContext, Type? destinationType)
        {
            return false;
        }

        /// <summary>
        ///     Convert a string like "Button.Click" into the corresponding RoutedEvent
        /// </summary>
        public override object? ConvertFrom(ITypeDescriptorContext? typeDescriptorContext,
                                           CultureInfo? cultureInfo,
                                           object? source)
        {
            if (typeDescriptorContext == null)
                throw new ArgumentNullException("typeDescriptorContext");
            if (source == null)
                throw new ArgumentNullException("source");
            IRootObjectProvider? rootProvider = (IRootObjectProvider?)typeDescriptorContext.GetService(typeof(IRootObjectProvider));
            if (rootProvider != null && source is string)
            {
                IProvideValueTarget? ipvt = (IProvideValueTarget?)typeDescriptorContext.GetService(typeof(IProvideValueTarget));
                if (ipvt != null)
                {
                    EventSetter? setter = ipvt.TargetObject as EventSetter;
                    if (setter != null && source is string handlerName)
                    {
                        handlerName = handlerName.Trim();
                        return Delegate.CreateDelegate(setter.Event!.HandlerType, rootProvider.RootObject, handlerName);
                    }
                }
            }
            throw GetConvertFromException(source);
        }

        /// <summary>
        ///     Convert a RoutedEventID into a XAML string like "Button.Click"
        /// </summary>
        public override object? ConvertTo(ITypeDescriptorContext? typeDescriptorContext,
                                         CultureInfo? cultureInfo,
                                         object? value,
                                         Type destinationType)
        {
            throw GetConvertToException(value, destinationType);
        }
    }
}
