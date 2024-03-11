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
        private readonly Dictionary<string, Type> _nameTypes;

        internal TemplateContent(XamlReader xamlReader, IXamlObjectWriterFactory factory, IServiceProvider serviceProvider)
        {
            _xamlReader = xamlReader;
            _factory = factory;
            _serviceProvider = serviceProvider;
            _nameTypes = new Dictionary<string, Type>();
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

            XamlMember nameMember = _xamlReader.SchemaContext.GetXamlType(typeof(FrameworkElement)).GetMember("Name")!;

            Stack<XamlType?> objectStack = new Stack<XamlType?>();
            bool isNameMember = false;
            while (_xamlReader.Read())
            {
                switch (_xamlReader.NodeType)
                {
                    case XamlNodeType.StartObject:
                        if (RootType == null)
                            RootType = _xamlReader.Type.UnderlyingType;
                        if (_xamlReader.Type.UnderlyingType == typeof(StaticResourceExtension))
                        {
                            var obj = LoadTimeBindUnshareableStaticResource(_xamlReader);
                            writer.WriteValue(obj);
                            continue;
                        }
                        else
                            objectStack.Push(_xamlReader.Type);
                        break;
                    case XamlNodeType.GetObject:
                        objectStack.Push(null);
                        break;
                    case XamlNodeType.EndObject:
                        objectStack.Pop();
                        break;
                    case XamlNodeType.StartMember:
                        if (_xamlReader.Member == nameMember)
                            isNameMember = true;
                        break;
                    case XamlNodeType.EndMember:
                        isNameMember = false;
                        break;
                    case XamlNodeType.Value:
                        if (isNameMember && _xamlReader.Value is string nameValue)
                        {
                            var type = objectStack.Peek();
                            _nameTypes[nameValue] = type!.UnderlyingType;
                        }
                        break;
                }
                writer.WriteNode(_xamlReader);
            }
            writer.Close();
            _xamlReader = null;
        }

        public Type? RootType { get; private set; }

        private StaticResourceExtension LoadTimeBindUnshareableStaticResource(XamlReader xamlReader)
        {
            var settings = _factory.GetParentSettings();
            XamlObjectWriter writer = _factory.GetXamlObjectWriter(settings);
            settings.SkipProvideValueOnRoot = true;

            int elementDepth = 0;
            do
            {
                writer.WriteNode(xamlReader);
                switch (xamlReader.NodeType)
                {
                    case XamlNodeType.StartObject:
                    case XamlNodeType.GetObject:
                        elementDepth++;
                        break;
                    case XamlNodeType.EndObject:
                        elementDepth--;
                        break;
                }
            }
            while (elementDepth > 0 && xamlReader.Read());

            StaticResourceExtension resource = (StaticResourceExtension)writer.Result;

            object? value = resource.TryFindValue(_serviceProvider);
            return new Markup.DeferredStaticResource(resource.ResourceKey!, value);
        }

        public FrameworkElement Create(out INameScope nameScope)
        {
            if (_xamlNodeList == null)
                throw new InvalidOperationException("Xaml content not parse yet.");
            //var names = new NameScope();
            //nameScope = names;
            var settings = _factory.GetParentSettings();
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

        internal Type? GetTypeForName(string name)
        {
            _nameTypes.TryGetValue(name, out var type);
            return type;
        }

        internal FrameworkTemplate? OwnerTemplate;
    }
}
