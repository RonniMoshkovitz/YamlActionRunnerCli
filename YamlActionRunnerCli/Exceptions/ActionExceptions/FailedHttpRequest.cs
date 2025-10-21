using System.Diagnostics;
using YamlActionRunnerCli.ActionManagement.Actions;

namespace YamlActionRunnerCli.Exceptions.ActionExceptions;

public class FailedHttpRequest : ActionFailedException
{
    public FailedHttpRequest(IAction action, string reason, int? statusCode=null) 
        : base(action, $"HTTP Request failed: {reason} {statusCode.ToString()}")
    {
    }
}