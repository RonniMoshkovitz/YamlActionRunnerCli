namespace YamlActionRunnerCli.InstructionsParsing.FileDataParsing;

/// <summary>
/// Generic interface for a file parser.
/// </summary>
/// <typeparam name="TData">The type of object the file will be parsed into.</typeparam>
public interface IFileDataParser<out TData>
{
    /// <summary>
    /// Parses a file from the specified path into <see cref="TData"/>.
    /// </summary>
    /// <param name="filePath">The path to the file.</param>
    /// <returns>An object of type <typeparamref name="TData"/>.</returns>
    TData ParseFile(string filePath);
}