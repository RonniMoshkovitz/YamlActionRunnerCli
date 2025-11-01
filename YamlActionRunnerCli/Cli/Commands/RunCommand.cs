using CommandLine;

namespace YamlActionRunnerCli.Cli.Commands;

[Verb("run", HelpText = "Execute YAML-defined steps.")]
public class RunCommand : BaseCommand
{
    [Option('f', "file", Required = true, HelpText = "Path to the YAML file.")]
    public string FilePath { get; set; } = string.Empty;

    [Option("dry-run", Default = false, HelpText = "Print parsed steps but do not execute them.")]
    public bool DryRun { get; set; }
}
