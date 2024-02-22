using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniversalPresentationFramework.SourceGenerators
{
    public static class SymbolExtensions
    {
        public static string GetCSharpFullName(ITypeSymbol type)
        {
            return type.ToDisplayString(
                SymbolDisplayFormat.FullyQualifiedFormat
                );
        }
    }
}
