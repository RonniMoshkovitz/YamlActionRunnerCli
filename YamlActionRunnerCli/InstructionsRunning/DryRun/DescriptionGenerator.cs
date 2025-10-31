using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace YamlActionRunnerCli.InstructionsRunning.DryRun;

public static class DescriptionGenerator
{
    private static readonly ISerializer _serializer = new SerializerBuilder()
        .WithNamingConvention(new CamelCaseNamingConvention())
        .Build();

    public static string GetStructure(this object instance) => _serializer.Serialize(instance);
}