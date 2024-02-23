using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xaml;

namespace UniversalPresentationFramework.SourceGenerators
{
    public abstract class FileGenerator
    {
        public abstract string GenerateClassFile(INamedTypeSymbol classType, List<ConnectItem> connectItems);

        public abstract string GenerateResourceFile();

        public abstract void ResourceStartObject(XamlType type, int lineNumber, int linePosition);

        public abstract void ResourceGetObject(int lineNumber, int linePosition);

        public abstract void ResourceEndObject(int lineNumber, int linePosition);

        public abstract void ResourceStartMember(XamlMember member, int lineNumber, int linePosition);

        public abstract void ResourceEndMember(int lineNumber, int linePosition);

        public abstract void ResourceValue(object value, int lineNumber, int linePosition);

        public abstract void ResourceNamespace(NamespaceDeclaration ns, int lineNumber, int linePosition);

        public abstract void ResourceClassType(AnalyzerXamlType type);

        protected string GetRelativePathName(string relativePath)
        {
            if (relativePath.EndsWith(".xaml"))
                relativePath = relativePath.Substring(0, relativePath.Length - 5);
            return relativePath.Replace("_", "__").Replace('.', '_').Replace('/', '_').Replace('-', '_');
        }

        public abstract string ResourceName { get; }
    }
}
