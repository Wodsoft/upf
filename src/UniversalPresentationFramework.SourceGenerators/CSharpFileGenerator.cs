using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xaml;

namespace UniversalPresentationFramework.SourceGenerators
{
    public class CSharpFileGenerator : FileGenerator
    {
        private readonly Compilation _compilation;
        private readonly string _relativePath, _resourceName;
        private readonly StringBuilder _resourceBuilder;
        private AnalyzerXamlType _rootType;
        private bool _hasRoot;

        public CSharpFileGenerator(Compilation compilation, string relativePath)
        {
            _compilation = compilation;
            _relativePath = relativePath;
            _resourceBuilder = new StringBuilder();
            _resourceName = GetRelativePathName(_relativePath) + "Resource";
            WriteResourceStart();
        }

        public override string ResourceName => _resourceName;

        public override string GenerateClassFile(INamedTypeSymbol classType, List<ConnectItem> connectItems)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("namespace ");
            sb.AppendLine(classType.ContainingNamespace.ToString());
            sb.AppendLine("{");
            sb.AppendLine($"    {SyntaxFacts.GetText(classType.DeclaredAccessibility)} partial class {classType.Name} : {classType.BaseType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}, global::System.Xaml.Markup.IComponentConnector");
            sb.AppendLine("    {");
            for (int i = 0; i < connectItems.Count; i++)
            {
                var item = connectItems[i];
                if (item.Type != null)
                {
                    sb.AppendLine($"        private {((AnalyzerXamlType)item.Type).Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)} {item.Name};");
                }
            }
            sb.AppendLine();
            sb.AppendLine("        private bool _contentLoaded;");
            sb.AppendLine();
            sb.AppendLine("        public void InitializeComponent()");
            sb.AppendLine("        {");
            sb.AppendLine("            if (_contentLoaded) return;");
            sb.AppendLine("            _contentLoaded = true;");
            sb.AppendLine($"            global::System.Uri resourceLocater = new global::System.Uri(\"/{_compilation.Assembly.Name};component/{_relativePath}\", global::System.UriKind.Relative);");
            sb.AppendLine($"            global::Wodsoft.UI.Application.LoadComponent(this, resourceLocater, global::{_compilation.Assembly.Name}.BamlResources.Get{_resourceName});");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public void Connect(int connectionId, object target)");
            sb.AppendLine("        {");
            sb.AppendLine("            switch(connectionId)");
            sb.AppendLine("            {");
            for (int i = 0; i < connectItems.Count; i++)
            {
                var item = connectItems[i];
                sb.AppendLine($"                case {i + 1}:");
                if (item.Member != null)
                    sb.AppendLine($"                    (({((AnalyzerXamlType)item.Member.DeclaringType).Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)})target).{item.Member.Name} += this.{item.Name};");
                else if (item.Type != null)
                    sb.AppendLine($"                    {item.Name} = ({((AnalyzerXamlType)item.Type).Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)})target;");
                sb.AppendLine($"                    return;");
            }
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public bool IsComponentInitialized => _contentLoaded;");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }

        public override string GenerateResourceFile()
        {
            WriteResourceEnd();
            var source = _resourceBuilder.ToString();
            var index = source.IndexOf("##ROOTTYPE##");
            source = source.Substring(0, index) + _rootType.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) + source.Substring(index + 12);
            return source;
        }

        private void WriteResourceStart()
        {
            _resourceBuilder.AppendLine($"namespace {_compilation.Assembly.Name}");
            _resourceBuilder.AppendLine("{");
            _resourceBuilder.AppendLine("    public static partial class BamlResources");
            _resourceBuilder.AppendLine("    {");
            _resourceBuilder.AppendLine($"        private static global::Wodsoft.UI.Markup.BamlResource _{_resourceName};");
            _resourceBuilder.AppendLine($"        internal static global::Wodsoft.UI.Markup.BamlResource Get{_resourceName}()");
            _resourceBuilder.AppendLine("        {");
            _resourceBuilder.AppendLine($"            if (_{_resourceName} == null)");
            _resourceBuilder.AppendLine("            {");
            _resourceBuilder.AppendLine($"                _{_resourceName} = new global::Wodsoft.UI.Markup.BamlResource(@\"{_relativePath}\");");
        }

        private void WriteResourceEnd()
        {
            _resourceBuilder.AppendLine($"                _{_resourceName}.Seal();");
            _resourceBuilder.AppendLine("            }");
            _resourceBuilder.AppendLine($"            return _{_resourceName};");
            _resourceBuilder.AppendLine("        }");
            _resourceBuilder.AppendLine("    }");
            _resourceBuilder.AppendLine("}");
        }

        public override void ResourceStartObject(XamlType type, int lineNumber, int linePosition)
        {
            if (type is AnalyzerXamlType analyzerXamlType)
            {
                if (_hasRoot)
                    _resourceBuilder.AppendLine($"                _{_resourceName}.WriteStartObject(typeof({analyzerXamlType.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}), {lineNumber}, {linePosition});");
                else
                {
                    _hasRoot = true;
                    _rootType = analyzerXamlType;
                    _resourceBuilder.AppendLine($"                _{_resourceName}.WriteStartObject(typeof(##ROOTTYPE##), {lineNumber}, {linePosition});");
                }

            }
            else
                throw new NotSupportedException($"Unknown XamlType \"{type}\".");
        }

        public override void ResourceGetObject(int lineNumber, int linePosition)
        {
            _resourceBuilder.AppendLine($"                _{_resourceName}.WriteGetObject({lineNumber}, {linePosition});");
        }

        public override void ResourceEndObject(int lineNumber, int linePosition)
        {
            _resourceBuilder.AppendLine($"                _{_resourceName}.WriteEndObject({lineNumber}, {linePosition});");
        }

        public override void ResourceStartMember(XamlMember member, int lineNumber, int linePosition)
        {
            if (member is AnalyzerXamlMember analyzerXamlMember)
            {
                if (analyzerXamlMember.IsAttachable)
                    _resourceBuilder.Append($"                _{_resourceName}.WriteStartAttachableMember(");
                else
                    _resourceBuilder.Append($"                _{_resourceName}.WriteStartMember(");
                _resourceBuilder.AppendLine($"typeof({((AnalyzerXamlType)analyzerXamlMember.DeclaringType).Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}), \"{member.Name}\", {lineNumber}, {linePosition});");
            }
            else if (member is XamlDirective directive)
                _resourceBuilder.AppendLine($"                _{_resourceName}.WriteStartMember(global::System.Xaml.XamlLanguage.{GetXamlDirective(directive)}, {lineNumber}, {linePosition});");
            else
                throw new NotSupportedException($"Unknown XamlMember \"{member}\".");
        }

        private string GetXamlDirective(XamlDirective directive)
        {
            if (directive == XamlLanguage.Items)
                return "Items";
            else if (directive == XamlLanguage.Key)
                return "Key";
            else if (directive == XamlLanguage.ConnectionId)
                return "ConnectionId";
            else if (directive == XamlLanguage.Name)
                return "Name";
            else if (directive == XamlLanguage.Members)
                return "Members";
            else if (directive == XamlLanguage.TypeArguments)
                return "TypeArguments";
            else if (directive == XamlLanguage.Arguments)
                return "Arguments";
            else if (directive == XamlLanguage.PositionalParameters)
                return "PositionalParameters";
            else if (directive == XamlLanguage.Class)
                return "Class";
            else if (directive == XamlLanguage.FactoryMethod)
                return "FactoryMethod";
            else if (directive == XamlLanguage.Lang)
                return "Lang";
            else if (directive == XamlLanguage.Subclass)
                return "Subclass";
            else if (directive == XamlLanguage.SynchronousMode)
                return "SynchronousMode";
            else if (directive == XamlLanguage.Shared)
                return "Shared";
            else if (directive == XamlLanguage.Space)
                return "Space";
            else if (directive == XamlLanguage.Uid)
                return "Uid";
            else
                throw new NotSupportedException($"Not support xaml directive \"{directive}\".");
        }

        public override void ResourceEndMember(int lineNumber, int linePosition)
        {
            _resourceBuilder.AppendLine($"                _{_resourceName}.WriteEndMember({lineNumber}, {linePosition});");
        }

        public override void ResourceValue(object value, int lineNumber, int linePosition)
        {
            _resourceBuilder.Append($"                _{_resourceName}.WriteValue(");
            if (value == null)
                _resourceBuilder.Append("null");
            else if (value is ITypeSymbol typeSymbol)
            {
                _resourceBuilder.Append($"typeof({typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)})");
            }
            else
            {
                var valueType = value.GetType();
                switch (Type.GetTypeCode(valueType))
                {
                    case TypeCode.Boolean:
                        _resourceBuilder.Append((bool)value ? "true" : "false");
                        break;
                    case TypeCode.Byte:
                        _resourceBuilder.Append($"(byte){value}");
                        break;
                    case TypeCode.SByte:
                        _resourceBuilder.Append($"(sbyte){value}");
                        break;
                    case TypeCode.Int16:
                        _resourceBuilder.Append($"(short){value}");
                        break;
                    case TypeCode.UInt16:
                        _resourceBuilder.Append($"(ushort){value}");
                        break;
                    case TypeCode.Int32:
                        _resourceBuilder.Append($"{value}");
                        break;
                    case TypeCode.UInt32:
                        _resourceBuilder.Append($"{value}u");
                        break;
                    case TypeCode.Int64:
                        _resourceBuilder.Append($"{value}l");
                        break;
                    case TypeCode.UInt64:
                        _resourceBuilder.Append($"{value}ul");
                        break;
                    case TypeCode.Char:
                        _resourceBuilder.Append($"'{value}'");
                        break;
                    case TypeCode.Single:
                        _resourceBuilder.Append($"{value}f");
                        break;
                    case TypeCode.Double:
                        _resourceBuilder.Append($"{value}d");
                        break;
                    case TypeCode.Decimal:
                        _resourceBuilder.Append($"{value}m");
                        break;
                    case TypeCode.DateTime:
                        _resourceBuilder.Append($"DateTime.Parse(\"{value}\")");
                        break;
                    default:
                        _resourceBuilder.Append($"\"{value}\"");
                        break;
                }
            }
            _resourceBuilder.AppendLine($", {lineNumber}, {linePosition});");
        }

        public override void ResourceClassType(AnalyzerXamlType type)
        {
            _rootType = type;
        }
    }
}
