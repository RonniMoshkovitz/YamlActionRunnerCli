using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace YamlActionRunnerCli.InstructionsParsing.FileDataParsing;

public abstract class YamlDataParser<TData> : IFileDataParser<TData>
{
    private static readonly INamingConvention _namingConvention = new CamelCaseNamingConvention();

    protected readonly IDeserializer _deserializer;
    protected readonly ISerializer _serializer;

    protected YamlDataParser(INamingConvention? namingConvention=null)
    {
        var naming = namingConvention ?? _namingConvention;
        _deserializer = new DeserializerBuilder().WithNamingConvention(naming).IgnoreUnmatchedProperties().Build();
        _serializer = new SerializerBuilder().WithNamingConvention(naming).Build();
    }

    public abstract TData ParseFile(string filePath);
}