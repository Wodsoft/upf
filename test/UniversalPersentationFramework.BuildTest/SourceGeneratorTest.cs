using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UniversalPresentationFramework.SourceGenerators;

namespace UniversalPersentationFramework.BuildTest
{
    public class SourceGeneratorTest
    {
        [Fact]
        public async Task TestProject()
        {
            var instances = MSBuildLocator.QueryVisualStudioInstances().ToList();
            MSBuildLocator.RegisterInstance(instances.First());

            await Build(@"..\..\..\..\UniversalPresentationFramework.Test\UniversalPresentationFramework.Test.csproj");
        }

        [Fact]
        public async Task AeroProject()
        {
            var instances = MSBuildLocator.QueryVisualStudioInstances().ToList();
            MSBuildLocator.RegisterInstance(instances.First());

            await Build(@"..\..\..\..\..\src\UniversalPresentationFramework.Areo\UniversalPresentationFramework.Areo.csproj");
        }

        private async Task Build(string path)
        {
            var workspace = MSBuildWorkspace.Create();
            var project = await workspace.OpenProjectAsync(path);
            var compilation = await project.GetCompilationAsync();
            var generator = new CSharpXamlGenerator().AsSourceGenerator();
            GeneratorDriver driver = CSharpGeneratorDriver.Create(new[] { generator }, driverOptions: new GeneratorDriverOptions(default, trackIncrementalGeneratorSteps: true));

            driver = driver.RunGenerators(compilation!);
        }
    }
}
