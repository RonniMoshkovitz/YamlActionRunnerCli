using Serilog;
using YamlActionRunnerCli.Cli.Commands;
using YamlActionRunnerCli.Utils.DataObjects.Run;
using YamlActionRunnerCli.Utils.Logging;

namespace YamlActionRunnerCli.CommandsRunners;

public abstract class BaseCommandRunner<TCommand> : ICommandRunner<TCommand> where TCommand : ICommand
{
    public abstract int Run(TCommand command);
}