using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Xaml;

namespace UniversalPresentationFramework.SourceGenerators
{
    [Generator(LanguageNames.CSharp)]
    public class CSharpXamlGenerator : XamlGenerator
    {
        protected override FileGenerator CreateFileGenerator(Compilation compilation, string relativePath)
        {
            return new CSharpFileGenerator(compilation, relativePath);
        }

        protected override string GenerateBamlResources(Compilation compilation, List<(string Path, string Name)> resources)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"namespace {compilation.Assembly.Name}");
            sb.AppendLine("{");
            sb.AppendLine("    public static partial class BamlResources");
            sb.AppendLine("    {");
            sb.AppendLine("        public static void LoadResources(string path, object rootObject)");
            sb.AppendLine("        {");
            sb.AppendLine("            global::Wodsoft.UI.Markup.BamlResource resource;");
            sb.AppendLine("            switch (path.ToLower())");
            sb.AppendLine("            {");
            foreach (var resource in resources)
            {
                sb.AppendLine($"                case @\"{resource.Path.ToLower()}\":");
                sb.AppendLine($"                    resource = Get{resource.Name}();");
                sb.AppendLine("                    break;");
            }
            sb.AppendLine("                default:");
            sb.AppendLine("                    resource = null;");
            sb.AppendLine("                    break;");
            sb.AppendLine("            }");
            sb.AppendLine("            if (resource != null)");
            sb.AppendLine("                global::Wodsoft.UI.Markup.XamlReader.Load(resource.GetReader(), rootObject);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public static object LoadResources(string path)");
            sb.AppendLine("        {");
            sb.AppendLine("            global::Wodsoft.UI.Markup.BamlResource resource;");
            sb.AppendLine("            switch (path.ToLower())");
            sb.AppendLine("            {");
            foreach (var resource in resources)
            {
                sb.AppendLine($"                case @\"{resource.Path.ToLower()}\":");
                sb.AppendLine($"                    resource = Get{resource.Name}();");
                sb.AppendLine("                    break;");
            }
            sb.AppendLine("                default:");
            sb.AppendLine("                    resource = null;");
            sb.AppendLine("                    break;");
            sb.AppendLine("            }");
            sb.AppendLine("            if (resource == null)");
            sb.AppendLine("                return null;");
            sb.AppendLine("            return global::Wodsoft.UI.Markup.XamlReader.Load(resource.GetReader());");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
