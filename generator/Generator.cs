using System;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;

namespace generator 
{
    [Generator]
    public class Generator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var settings = new CSharpGeneratorSettings
            {
                ClassStyle = CSharpClassStyle.Poco,
                ArrayType = "System.Collections.Generic.List",
                GenerateDataAnnotations = true,
                NullHandling = NullHandling.Swagger,
                RequiredPropertiesMustBeDefined = true,
                DateTimeType = "System.DateTimeOffset",
                
                Namespace = "GeneratedSchemas",
            };

            foreach (var file in context.AdditionalFiles)
            {
                var name = Path.GetFileName(file.Path);
                var schema = JsonSchema4.FromJsonAsync(file.GetText().ToString()).Result;
                var generator = new CSharpGenerator(schema, settings);
                context.AddSource($"{name}.cs", generator.GenerateFile(""));
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
        }
    }
}