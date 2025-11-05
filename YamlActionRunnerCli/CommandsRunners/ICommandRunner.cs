using YamlActionRunnerCli.Cli.Commands;

namespace YamlActionRunnerCli.CommandsRunners;

/// <summary>
/// Generic interface for a command runner.
/// </summary>
/// <typeparam name="TCommand">The command type it runs.</typeparam>
public interface ICommandRunner<in TCommand> where TCommand : ICommand
{
    /// <summary>
    /// Runs the command logic.
    /// </summary>
    /// <param name="command">The command arguments object.</param>
    /// <returns>A run's exit code.</returns>
    int Run(TCommand command);
}