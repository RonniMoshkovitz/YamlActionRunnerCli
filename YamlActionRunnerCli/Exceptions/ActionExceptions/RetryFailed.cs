using YamlActionRunnerCli.ActionManagement.Actions;

namespace YamlActionRunnerCli.Exceptions.ActionExceptions;

public class RetryFailed : ActionFailedException
{
    public RetryFailed(IAction action, int times, Exception innerException) 
        : base(action, $"Retry failed: failed {times} times due to: {innerException.Message}")
    {
    }
}