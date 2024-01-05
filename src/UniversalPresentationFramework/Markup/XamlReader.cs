using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;
using System.Xml;

namespace Wodsoft.UI.Markup
{
    public class XamlReader
    {
        private static readonly XamlSchemaContext _SchemaContext;
        static XamlReader()
        {
            _SchemaContext = new UpfXamlSchemaContext();            
        }

        internal static XamlSchemaContext SchemaContext => _SchemaContext;

        public static object Parse(string xamlText)
        {
            StringReader stringReader = new StringReader(xamlText);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            return Load(xmlReader);
        }

        public static void Parse(string xamlText, object? rootObject)
        {
            StringReader stringReader = new StringReader(xamlText);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            Load(xmlReader, rootObject);
        }

        public static object Load(XmlReader reader)
        {
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
            System.Xaml.XamlXmlReaderSettings settings = new System.Xaml.XamlXmlReaderSettings();
            settings.IgnoreUidsOnPropertyElements = true;
            //settings.BaseUri = parserContext.BaseUri;
            settings.ProvideLineInfo = true;            
            System.Xaml.XamlXmlReader xamlXmlReader = new System.Xaml.XamlXmlReader(reader, _SchemaContext, settings);

            UpfXamlLoader.Load(xamlXmlReader, false, rootObject);
            reader.Close();
        }
    }
}
