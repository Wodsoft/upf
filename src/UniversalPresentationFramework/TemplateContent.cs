using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;
using System.Xaml.Markup;

namespace Wodsoft.UI
{
    [XamlDeferLoad(typeof(TemplateContentLoader), typeof(FrameworkElement))]
    public class TemplateContent
    {
        private XamlReader? _xamlReader;
        private readonly IXamlObjectWriterFactory _factory;
        private readonly IServiceProvider _serviceProvider;
        private XamlNodeList? _xamlNodeList;

        internal TemplateContent(XamlReader xamlReader, IXamlObjectWriterFactory factory, IServiceProvider serviceProvider)
        {
            _xamlReader = xamlReader;
            _factory = factory;
            _serviceProvider = serviceProvider;
        }

        internal void Parse()
        {
            if (_xamlReader == null)
                throw new InvalidOperationException("Xaml already parsed.");

            _xamlNodeList = new XamlNodeList(_xamlReader.SchemaContext);
            System.Xaml.XamlWriter writer = _xamlNodeList.Writer;

            // Prepare to provide source info if needed
            IXamlLineInfoConsumer? lineInfoConsumer = null;
            IXamlLineInfo? lineInfo = _xamlReader as IXamlLineInfo;
            if (lineInfo != null)
                lineInfoConsumer = writer as IXamlLineInfoConsumer;

            while (_xamlReader.Read())
            {
                writer.WriteNode(_xamlReader);
            }
            writer.Close();
            _xamlReader = null;
        }

        public FrameworkElement Create(out INameScope nameScope)
        {
            if (_xamlNodeList == null)
                throw new InvalidOperationException("Xaml content not parse yet.");
            //var names = new NameScope();
            //nameScope = names;
            var settings = _factory.GetParentSettings();
            //settings.AfterPropertiesHandler = (_, e) =>
            //{
            //    if (e.Instance is FrameworkElement element && element.Name != null)
            //        names.RegisterName(element.Name, element);
            //};
            var reader = _xamlNodeList.GetReader();
            var writer = _factory.GetXamlObjectWriter(settings);

            // Prepare to provide source info if needed
            IXamlLineInfoConsumer? lineInfoConsumer = null;
            IXamlLineInfo? lineInfo = null;
            lineInfo = reader as IXamlLineInfo;
            if (lineInfo != null)
            {
                lineInfoConsumer = writer as IXamlLineInfoConsumer;
            }

            while (reader.Read())
            {
                if (lineInfoConsumer != null)
                    lineInfoConsumer.SetLineInfo(lineInfo!.LineNumber, lineInfo.LinePosition);

                // We need to call the ObjectWriter first because x:Name & RNPA needs to be registered
                // before we call InvalidateProperties.
                writer.WriteNode(reader);

                //switch (reader.NodeType)
                //{
                //    case XamlNodeType.EndMember:
                        
                //        break;
                //    //case System.Xaml.XamlNodeType.EndObject:
                //    //    if (writer.Result is )
                //    //    break;
                //}
            }
            var element = (FrameworkElement)writer.Result;
            NameScope.SetNameScope(element, writer.RootNameScope);
            nameScope = writer.RootNameScope;
            return element;
        }
    }
}
