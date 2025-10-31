using YamlActionRunnerCli.ActionManagement.Actions;
using YamlActionRunnerCli.Utils.ObjectManagement;

namespace YamlActionRunnerCli.Exceptions.ActionExceptions;

public abstract class ActionFailedException : RunnerBaseException
{
    public override ExitCode ExitCode { get; } = ExitCode.ActionFailed;
    public IAction FailedAction { get;}

    public ActionFailedException(IAction action, string message) : base($"Action Failed - {message}")
    {
        FailedAction = action;
    }
}