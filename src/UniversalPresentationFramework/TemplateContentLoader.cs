using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;
using Wodsoft.UI.Controls;

namespace Wodsoft.UI
{
    public class TemplateContentLoader : XamlDeferringLoader
    {
        public override object Load(XamlReader xamlReader, IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }
            else if (xamlReader == null)
            {
                throw new ArgumentNullException("xamlReader");
            }

            IXamlObjectWriterFactory factory = RequireService<IXamlObjectWriterFactory>(serviceProvider);
            return new TemplateContent(xamlReader, factory, serviceProvider);
        }

        private static T RequireService<T>(IServiceProvider provider) where T : class
        {
            T? result = provider.GetService(typeof(T)) as T;
            if (result == null)
                throw new InvalidOperationException("Template content deffering loader has no context.");
            return result;
        }

        public override XamlReader Save(object value, IServiceProvider serviceProvider)
        {
            throw new NotSupportedException("Template content deffering loader not support save.");
        }
    }
}
