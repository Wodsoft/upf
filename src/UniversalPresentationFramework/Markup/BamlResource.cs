using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;

namespace Wodsoft.UI.Markup
{
    public class BamlResource : Sealable
    {
        private readonly List<BamlResourceNode> _nodes;
        private readonly IReadOnlyList<BamlResourceNode> _readOnlyNodes;

        public BamlResource(string path)
        {
            _nodes = new List<BamlResourceNode>();
            _readOnlyNodes = new ReadOnlyCollection<BamlResourceNode>(_nodes);
            Path = path;
        }

        public XamlSchemaContext SchemaContext => XamlReader.SchemaContext;

        public IReadOnlyList<BamlResourceNode> Nodes => _readOnlyNodes;

        public string Path { get; }

        public void WriteStartObject(Type type, int lineNumber, int linePosition)
        {
            CheckSealed();
            var xamlType = SchemaContext.GetXamlType(type);
            _nodes.Add(new BamlResourceNode(XamlNodeType.StartObject, null, xamlType, null, null, lineNumber, linePosition));
        }

        public void WriteGetObject(int lineNumber, int linePosition)
        {
            CheckSealed();
            _nodes.Add(new BamlResourceNode(XamlNodeType.GetObject, null, null, null, null, lineNumber, linePosition));
        }

        public void WriteEndObject(int lineNumber, int linePosition)
        {
            CheckSealed();
            _nodes.Add(new BamlResourceNode(XamlNodeType.EndObject, null, null, null, null, lineNumber, linePosition));
        }

        public void WriteStartMember(Type type, string member, int lineNumber, int linePosition)
        {
            CheckSealed();
            var xamlType = SchemaContext.GetXamlType(type);
            var xamlMember = xamlType.GetMember(member);
            _nodes.Add(new BamlResourceNode(XamlNodeType.StartMember, null, null, xamlMember, null, lineNumber, linePosition));
        }

        public void WriteStartMember(XamlMember member, int lineNumber, int linePosition)
        {
            CheckSealed();
            _nodes.Add(new BamlResourceNode(XamlNodeType.StartMember, null, null, member, null, lineNumber, linePosition));
        }

        public void WriteStartAttachableMember(Type type, string member, int lineNumber, int linePosition)
        {
            CheckSealed();
            var xamlType = SchemaContext.GetXamlType(type);
            var xamlMember = xamlType.GetAttachableMember(member);
            _nodes.Add(new BamlResourceNode(XamlNodeType.StartMember, null, null, xamlMember, null, lineNumber, linePosition));
        }

        public void WriteEndMember(int lineNumber, int linePosition)
        {
            CheckSealed();
            _nodes.Add(new BamlResourceNode(XamlNodeType.EndMember, null, null, null, null, lineNumber, linePosition));
        }

        public void WriteValue(object? value, int lineNumber, int linePosition)
        {
            CheckSealed();
            _nodes.Add(new BamlResourceNode(XamlNodeType.Value, value, null, null, null, lineNumber, linePosition));
        }

        public void WriteNamespace(NamespaceDeclaration ns, int lineNumber, int linePosition)
        {
            CheckSealed();
            _nodes.Add(new BamlResourceNode(XamlNodeType.NamespaceDeclaration, null, null, null, ns, lineNumber, linePosition));
        }

        public System.Xaml.XamlReader GetReader() => new BamlResourceReader(this);

        internal static Func<string, object>? GetLoadResourcesFunction(Assembly assembly)
        {
            var resourcesType = assembly.GetType(assembly.GetName().Name + ".BamlResources");
            if (resourcesType == null)
                return null;
            var method = resourcesType.GetMethod("LoadResources", BindingFlags.Public | BindingFlags.Static, [typeof(string)]);
            if (method == null)
                return null;
            return (Func<string, object>)Delegate.CreateDelegate(typeof(Func<string, object>), method);
        }

        internal static Action<string, object>? GetLoadComponentFunction(Assembly assembly)
        {
            var resourcesType = assembly.GetType(assembly.GetName().Name + ".BamlResources");
            if (resourcesType == null)
                return null;
            var method = resourcesType.GetMethod("LoadResources", BindingFlags.Public | BindingFlags.Static, [typeof(string), typeof(object)]);
            if (method == null)
                return null;
            return (Action<string, object>)Delegate.CreateDelegate(typeof(Action<string, object>), method);
        }
    }
}
