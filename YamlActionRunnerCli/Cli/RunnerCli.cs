using CommandLine;
using YamlActionRunnerCli.Cli.Commands;
using YamlActionRunnerCli.CommandsRunners;
using YamlActionRunnerCli.Exceptions;

namespace YamlActionRunnerCli.Cli;

public class RunnerCli
{
    private readonly Parser _parser = Parser.Default;

    public int Start(string[] commandArguments)
    {
        return _parser
            .ParseArguments<RunCommand>(commandArguments)
            .MapResult((RunCommand runCommand) => new RunRunner().Run(runCommand),
                error => (int)ExitCode.InvalidArguments);
    }
}