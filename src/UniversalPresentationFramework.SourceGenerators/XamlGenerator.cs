using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text;
using System.Xaml;
using System.Xaml.Schema;

namespace UniversalPresentationFramework.SourceGenerators
{
    public abstract class XamlGenerator : IIncrementalGenerator
    {
        private const string _SourceItemGroupMetadata = "build_metadata.AdditionalFiles.SourceItemGroup";
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var xamlFiles = context.AdditionalTextsProvider.Where(t => t.Path.EndsWith(".xaml")).Combine(context.AnalyzerConfigOptionsProvider)
                .Where(((AdditionalText Text, AnalyzerConfigOptionsProvider Options) input) =>
                {
                    input.Options.GetOptions(input.Text).TryGetValue(_SourceItemGroupMetadata, out var value);
                    return value == "XAML";
                })
                //.Select(((AdditionalText Text, AnalyzerConfigOptionsProvider Options) input, CancellationToken cts) => input.Text)
                .Combine(context.CompilationProvider)
                .Collect();
            context.RegisterSourceOutput(xamlFiles, (c, values) => CompileXaml(c, values));
            //context.RegisterSourceOutput(xamlFiles, (c, values) => CompileXaml(c, values.Left.Item1, values.Left.Item2, values.Right, schemaContext));
        }

        public void CompileXaml(SourceProductionContext sourceContext, ImmutableArray<((AdditionalText, AnalyzerConfigOptionsProvider), Compilation)> values)
        {
            AnalyzerXamlSchemaContext schemaContext = new AnalyzerXamlSchemaContext();
            List<(string Path, string Name)> resources = new List<(string Path, string Name)>();
            foreach (var value in values)
            {
                CompileXaml(sourceContext, value.Item1.Item1, value.Item1.Item2, value.Item2, schemaContext, out var relativePath, out var resourceName);
                if (resourceName != null)
                    resources.Add((relativePath, resourceName));
            }
            if (resources.Count != 0)
            {
                sourceContext.AddSource("BamlResources.g", GenerateBamlResources(values[0].Item2, resources));
            }
        }

        public void CompileXaml(SourceProductionContext sourceContext, AdditionalText additionalText, AnalyzerConfigOptionsProvider optionsProvider, Compilation compilation, AnalyzerXamlSchemaContext schemaContext, out string relativePath, out string resourceName)
        {
            optionsProvider.GlobalOptions.TryGetValue("build_property.projectdir", out var projectDir);

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            additionalText.GetText().Write(writer);
            writer.Flush();
            stream.Position = 0;

            relativePath = additionalText.Path.Substring(projectDir.Length).Replace('\\', '/');

            schemaContext.SetCompilation(compilation);

            INamedTypeSymbol classType = null;
            XamlXmlReaderSettings settings = new XamlXmlReaderSettings();
            settings.IgnoreUidsOnPropertyElements = true;
            settings.ProvideLineInfo = true;
            XamlXmlReader xamlXmlReader = new System.Xaml.XamlXmlReader(stream, schemaContext, settings);

            Stack<XamlType> typeStacks = new Stack<XamlType>();
            Stack<XamlMember> memberStacks = new Stack<XamlMember>();
            List<ConnectItem> connectItems = new List<ConnectItem>();

            var fileGenerator = CreateFileGenerator(compilation, relativePath);

            resourceName = null;
            Dictionary<string, string> namespaces = new Dictionary<string, string>();
            while (xamlXmlReader.Read())
            {
                switch (xamlXmlReader.NodeType)
                {
                    case XamlNodeType.StartMember:
                        if (xamlXmlReader.Member == XamlLanguage.Class)
                        {
                            //fileGenerator.ResourceStartMember(xamlXmlReader.Member, xamlXmlReader.LineNumber, xamlXmlReader.LinePosition);
                            xamlXmlReader.Read();
                            if (xamlXmlReader.NodeType != XamlNodeType.Value)
                            {
                                return;
                            }
                            var className = xamlXmlReader.Value as string;
                            classType = compilation.Assembly.GetTypeByMetadataName(className);
                            //fileGenerator.ResourceValue(classType, xamlXmlReader.LineNumber, xamlXmlReader.LinePosition);
                            if (classType == null)
                            {
                                return;
                            }
                            if (classType.IsGenericType)
                            {
                                return;
                            }
                            xamlXmlReader.Read();
                            //fileGenerator.ResourceEndMember(xamlXmlReader.LineNumber, xamlXmlReader.LinePosition);
                            fileGenerator.ResourceClassType(schemaContext.GetXamlType(classType));
                            continue;
                        }
                        else if (xamlXmlReader.Member == XamlLanguage.Name)
                        {
                            fileGenerator.ResourceStartMember(xamlXmlReader.Member, xamlXmlReader.LineNumber, xamlXmlReader.LinePosition);
                            xamlXmlReader.Read();
                            if (xamlXmlReader.NodeType != XamlNodeType.Value)
                            {
                                return;
                            }
                            var name = xamlXmlReader.Value as string;
                            connectItems.Add(new ConnectItem
                            {
                                Name = name,
                                Type = typeStacks.Peek()
                            });
                            fileGenerator.ResourceValue(name, xamlXmlReader.LineNumber, xamlXmlReader.LinePosition);
                            xamlXmlReader.Read();
                            fileGenerator.ResourceEndMember(xamlXmlReader.LineNumber, xamlXmlReader.LinePosition);
                            fileGenerator.ResourceStartMember(XamlLanguage.ConnectionId, xamlXmlReader.LineNumber, xamlXmlReader.LinePosition);
                            fileGenerator.ResourceValue(connectItems.Count, xamlXmlReader.LineNumber, xamlXmlReader.LinePosition);
                            fileGenerator.ResourceEndMember(xamlXmlReader.LineNumber, xamlXmlReader.LinePosition);
                            continue;
                        }
                        else if (xamlXmlReader.Member == XamlLanguage.Base)
                        {
                            xamlXmlReader.Read();
                            if (xamlXmlReader.NodeType != XamlNodeType.Value)
                            {
                                return;
                            }
                            xamlXmlReader.Read();
                            continue;
                        }
                        else if (xamlXmlReader.Member.IsEvent)
                        {
                            var member = xamlXmlReader.Member;
                            fileGenerator.ResourceStartMember(member, xamlXmlReader.LineNumber, xamlXmlReader.LinePosition);
                            xamlXmlReader.Read();
                            if (xamlXmlReader.NodeType != XamlNodeType.Value)
                            {
                                return;
                            }
                            var delegateName = xamlXmlReader.Value as string;
                            connectItems.Add(new ConnectItem
                            {
                                Name = delegateName,
                                Member = member
                            });
                            fileGenerator.ResourceValue(connectItems.Count, xamlXmlReader.LineNumber, xamlXmlReader.LinePosition);
                            xamlXmlReader.Read();
                            fileGenerator.ResourceEndMember(xamlXmlReader.LineNumber, xamlXmlReader.LinePosition);
                            continue;
                        }
                        else
                        {
                            memberStacks.Push(xamlXmlReader.Member);
                            fileGenerator.ResourceStartMember(xamlXmlReader.Member, xamlXmlReader.LineNumber, xamlXmlReader.LinePosition);
                        }
                        continue;
                    case XamlNodeType.EndMember:
                        {
                            memberStacks.Pop();
                            fileGenerator.ResourceEndMember(xamlXmlReader.LineNumber, xamlXmlReader.LinePosition);
                            continue;
                        }
                    case XamlNodeType.StartObject:
                        if (XamlLanguage.Type == xamlXmlReader.Type)
                        {
                            var lineNumber = xamlXmlReader.LineNumber;
                            var linePosition = xamlXmlReader.LinePosition;
                            xamlXmlReader.Read();
                            xamlXmlReader.Read();
                            var typeName = xamlXmlReader.Value as string;
                            if (string.IsNullOrEmpty(typeName))
                            {
                                return;
                            }
                            var types = typeName.Split(':');
                            AnalyzerXamlType xamlType;
                            if (types.Length == 1)
                                xamlType = schemaContext.GetXamlType(new XamlTypeName(namespaces[string.Empty], types[0])) as AnalyzerXamlType;
                            else
                                xamlType = schemaContext.GetXamlType(new XamlTypeName(namespaces[types[0]], types[1])) as AnalyzerXamlType;
                            if (xamlType == null)
                            {
                                return;
                            }
                            xamlXmlReader.Read();
                            xamlXmlReader.Read();
                            fileGenerator.ResourceValue(xamlType.Type, lineNumber, linePosition);
                            continue;
                        }
                        typeStacks.Push(xamlXmlReader.Type);
                        fileGenerator.ResourceStartObject(xamlXmlReader.Type, xamlXmlReader.LineNumber, xamlXmlReader.LinePosition);
                        continue;
                    case XamlNodeType.GetObject:
                        typeStacks.Push(null);
                        fileGenerator.ResourceGetObject(xamlXmlReader.LineNumber, xamlXmlReader.LinePosition);
                        continue;
                    case XamlNodeType.EndObject:
                        typeStacks.Pop();
                        fileGenerator.ResourceEndObject(xamlXmlReader.LineNumber, xamlXmlReader.LinePosition);
                        continue;
                    case XamlNodeType.Value:
                        fileGenerator.ResourceValue(xamlXmlReader.Value, xamlXmlReader.LineNumber, xamlXmlReader.LinePosition);
                        continue;
                    case XamlNodeType.NamespaceDeclaration:
                        namespaces.Add(xamlXmlReader.Namespace.Prefix, xamlXmlReader.Namespace.Namespace);
                        fileGenerator.ResourceNamespace(xamlXmlReader.Namespace, xamlXmlReader.LineNumber, xamlXmlReader.LinePosition);
                        continue;
                }
            }

            if (classType != null)
                sourceContext.AddSource(relativePath.Substring(0, relativePath.Length - 4) + "g", fileGenerator.GenerateClassFile(classType, connectItems));
            sourceContext.AddSource(relativePath.Substring(0, relativePath.Length - 4) + "xaml.g", fileGenerator.GenerateResourceFile());
            resourceName = fileGenerator.ResourceName;
        }

        protected abstract FileGenerator CreateFileGenerator(Compilation compilation, string relativePath);

        protected abstract string GenerateBamlResources(Compilation compilation, List<(string Path, string Name)> resources);
    }
}
