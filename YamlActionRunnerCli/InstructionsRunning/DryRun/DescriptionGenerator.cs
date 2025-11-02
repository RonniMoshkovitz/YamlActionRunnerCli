using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace YamlActionRunnerCli.InstructionsRunning.DryRun;

/// <summary>
/// Provides a helper method to serialize objects to a YAML string for dry runs.
/// </summary>
public static class DescriptionGenerator
{
    private static readonly ISerializer _serializer = new SerializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();

    /// <summary>
    /// Makes a Structure description for a given object (YAML-formatted string).
    /// </summary>
    /// <param name="instance">The object to be described.</param>
    /// <returns>A string representing the object</returns>
    public static string GetStructure(this object instance) => _serializer.Serialize(instance);
}