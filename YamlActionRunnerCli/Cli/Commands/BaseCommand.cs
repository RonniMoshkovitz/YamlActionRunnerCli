using CommandLine;

namespace YamlActionRunnerCli.Cli.Commands;

public class BaseCommand : ICommand
{
    [Option("verbose", Default = false, HelpText = "Show detailed execution logs.")]
    public bool Verbose { get; set; }
}