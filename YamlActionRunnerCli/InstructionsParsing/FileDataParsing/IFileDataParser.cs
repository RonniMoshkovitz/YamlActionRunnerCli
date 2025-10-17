namespace YamlActionRunnerCli.InstructionsParsing.FileDataParsing;

public interface IFileDataParser<TData>
{
    TData ParseFile(string filePath);
}