using CommandLine;
using YamlActionRunnerCli.Cli.Commands;
using YamlActionRunnerCli.CommandsRunners;
using YamlActionRunnerCli.Exceptions;
using YamlActionRunnerCli.Utils.Logging;

namespace YamlActionRunnerCli;

public static class Program
{
    static int Main(string[] args)
    {
        return Parser.Default
            .ParseArguments<RunCommand>(args)
            .MapResult((RunCommand runCommand) => new RunRunner().Run(runCommand), error => (int)ExitCode.InvalidArguments);
    }
}