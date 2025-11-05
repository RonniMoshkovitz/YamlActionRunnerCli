using CommandLine;

namespace YamlActionRunnerCli.Cli.Commands;

/// <summary>
/// A base class for CLI commands, providing common options.
/// </summary>
public class BaseCommand : ICommand
{
    /// <summary>
    /// Gets or sets a value indicating whether to show detailed execution logs.
    /// </summary>
    [Option("verbose", Default = false, HelpText = "Show detailed execution logs.")]
    public bool Verbose { get; set; }
}