using YamlActionRunnerCli.ActionManagement.Actions;

namespace YamlActionRunnerCli.Exceptions.ActionExceptions;

public class InvalidConditionException : ActionFailedException
{
    public InvalidConditionException(IAction action, string condition)
        : base(action, $"Invalid condition '{condition}'")
    {
    }
}