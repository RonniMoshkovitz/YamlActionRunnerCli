using YamlActionRunnerCli.ActionManagement.Actions;
using YamlActionRunnerCli.Utils.ObjectManagement;

namespace YamlActionRunnerCli.Exceptions.ActionExceptions;

public abstract class ActionFailedException : Exception
{
    public ActionFailedException(IAction action, string message) : base(
        $"Action ({action.GetPropertiesDescription()}) Failed - {message}")
    {
    }
}