using System.Diagnostics;
using YamlActionRunnerCli.ActionManagement.Actions;

namespace YamlActionRunnerCli.Exceptions.ActionExceptions;

public class FailedHttpRequest : ActionFailedException
{
    public FailedHttpRequest(IAction action, int statusCode, string reason) 
        : base(action, $"Request failed: {statusCode} {reason}")
    {
    }
}