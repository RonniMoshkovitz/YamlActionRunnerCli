using YamlActionRunnerCli.Cli.Commands;

namespace YamlActionRunnerCli.CommandsRunners;

public interface ICommandRunner<in TCommand> where TCommand : ICommand
{
    int Run(TCommand command);
}