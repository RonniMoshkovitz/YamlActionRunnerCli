using CommandLine;

namespace YamlActionRunnerCli.Cli.Commands;

/// <summary>
/// Defines the 'run' command (verb) and its associated options for the CLI.
/// </summary>
[Verb("run", HelpText = "Execute YAML-defined steps.")]
public class RunCommand : BaseCommand
{
    /// <summary>
    /// Path to the YAML file to execute.
    /// </summary>
    [Option('f', "file", Required = true, HelpText = "Path to the YAML file.")]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// Indicating whether to print parsed steps without executing them.
    /// </summary>
    [Option("dry-run", Default = false, HelpText = "Print parsed steps but do not execute them.")]
    public bool DryRun { get; set; }
}