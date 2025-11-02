using CommandLine;
using YamlActionRunnerCli.Cli.Commands;
using YamlActionRunnerCli.CommandsRunners;
using YamlActionRunnerCli.Exceptions;

namespace YamlActionRunnerCli.Cli;


/// <summary>
/// CLI to manage all supported commands in the application.
/// </summary>
public class RunnerCli
{
    private readonly Parser _parser = Parser.Default;
    
    /// <summary>
    /// Parses command-line arguments and executes the appropriate command runner.
    /// </summary>
    /// <param name="commandArguments">Command-line arguments.</param>
    /// <returns>An exit code (0 for success, non-zero for errors).</returns>
    public int Start(string[] commandArguments)
    {
        return _parser
            .ParseArguments<RunCommand>(commandArguments)
            .MapResult((RunCommand runCommand) => new RunRunner().Run(runCommand),
                error => (int)ExitCode.InvalidArguments);
    }
}