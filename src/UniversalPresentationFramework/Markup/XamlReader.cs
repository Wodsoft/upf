using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;
using System.Xml;

namespace Wodsoft.UI.Markup
{
    public static class XamlReader
    {
        private static readonly XamlSchemaContext _SchemaContext;
        static XamlReader()
        {
            _SchemaContext = new UpfXamlSchemaContext();
        }

        internal static XamlSchemaContext SchemaContext => _SchemaContext;

        public static object Parse(string xamlText)
        {
            if (xamlText == null)
                throw new ArgumentNullException(nameof(xamlText));
            StringReader stringReader = new StringReader(xamlText);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            return Load(xmlReader);
        }

        public static void Parse(string xamlText, object? rootObject)
        {
            if (xamlText == null)
                throw new ArgumentNullException(nameof(xamlText));
            StringReader stringReader = new StringReader(xamlText);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            Load(xmlReader, rootObject);
        }

        public static object Load(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            return Load(XmlReader.Create(stream));
        }

        public static object Load(XmlReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            System.Xaml.XamlXmlReaderSettings settings = new System.Xaml.XamlXmlReaderSettings();
            settings.IgnoreUidsOnPropertyElements = true;
            //settings.BaseUri = parserContext.BaseUri;
            settings.ProvideLineInfo = true;
            System.Xaml.XamlXmlReader xamlXmlReader = new System.Xaml.XamlXmlReader(reader, _SchemaContext, settings);

            object root = UpfXamlLoader.Load(xamlXmlReader, false);
            reader.Close();
            return root;
        }

        public static void Load(XmlReader reader, object? rootObject)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            System.Xaml.XamlXmlReaderSettings settings = new System.Xaml.XamlXmlReaderSettings();
            settings.IgnoreUidsOnPropertyElements = true;
            //settings.BaseUri = parserContext.BaseUri;
            settings.ProvideLineInfo = true;
            System.Xaml.XamlXmlReader xamlXmlReader = new System.Xaml.XamlXmlReader(reader, _SchemaContext, settings);

            UpfXamlLoader.Load(xamlXmlReader, false, rootObject);
            reader.Close();
        }

        public static void Load(System.Xaml.XamlReader reader, object rootObject)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            if (rootObject == null)
                throw new ArgumentNullException(nameof(rootObject));

            UpfXamlLoader.Load(reader, false, rootObject);
            reader.Close();
        }

        public static object Load(System.Xaml.XamlReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            object root = UpfXamlLoader.Load(reader, false);
            reader.Close();
            return root;
        }
    }
}
