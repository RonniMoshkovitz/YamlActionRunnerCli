using YamlActionRunnerCli.Exceptions.GeneralExceptions;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace YamlActionRunnerCli.InstructionsParsing.FileDataParsing;

/// <summary>
/// Abstract base class for YAML file parsers.
/// </summary>
/// <typeparam name="TData">The type of object the YAML will be deserialized into.</typeparam>
public abstract class YamlDataParser<TData> : IFileDataParser<TData>
{
    private static readonly INamingConvention _namingConvention = CamelCaseNamingConvention.Instance;

    protected readonly IDeserializer _deserializer;
    protected readonly ISerializer _serializer;
    
    /// <summary>
    /// Initializes the serializer and deserializer with wanted naming conventions.
    /// </summary>
    /// <param name="namingConvention">Wanted naming convention. Defaults to CamelCase.</param>
    protected YamlDataParser(INamingConvention? namingConvention=null)
    {
        var naming = namingConvention ?? _namingConvention;
        _deserializer = new DeserializerBuilder().WithNamingConvention(naming).IgnoreUnmatchedProperties().Build();
        _serializer = new SerializerBuilder().WithNamingConvention(naming).Build();
    }
    
    /// <inheritdoc/>>
    public abstract TData ParseFile(string filePath);
    
    /// <summary>
    /// Loads the YAML file and returns the root mapping node.
    /// </summary>
    /// <param name="filePath">The path to the YAML file.</param>
    /// <returns>root <see cref="YamlMappingNode"/></returns>
    /// <exception cref="InvalidYamlException">Thrown if the YAML is semantically invalid or not found</exception>
    protected static YamlMappingNode? GetRootNode(string filePath)
    {
        var yamlStream = new YamlStream();
        try
        {
            yamlStream.Load(new StringReader(File.ReadAllText(filePath)));
        }
        catch (Exception exception) when (exception is SemanticErrorException or YamlException or IOException)
        {
            throw new InvalidYamlException(exception.Message);
        }

        return (YamlMappingNode?)yamlStream.Documents.FirstOrDefault()?.RootNode;
    }
    
    /// <summary>
    /// Recursively converts object <see cref="object"/> keys to <see cref="string"/> keys (fixing classic YAML deserialisation)..
    /// </summary>
    /// <param name="deserialized">Object that might be a dictionary.</param>
    /// <returns></returns>
    protected object? FixMappingDeserialization(object? deserialized)
    {
        if (deserialized is not IDictionary<object, object> mapping)
            return deserialized;

        return mapping.ToDictionary(
            entry => entry.Key.ToString()!,
            entry => FixMappingDeserialization(entry.Value));
    }
}