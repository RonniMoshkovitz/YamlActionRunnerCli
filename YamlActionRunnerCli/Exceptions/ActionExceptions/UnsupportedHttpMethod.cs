using YamlActionRunnerCli.ActionManagement.Actions;

namespace YamlActionRunnerCli.Exceptions.ActionExceptions;

public class UnsupportedHttpMethod : ActionFailedException
{
    public UnsupportedHttpMethod(IAction action, string method) : base(action, $"Unsupported HTTP method: '{method}'")
    {
    }
}