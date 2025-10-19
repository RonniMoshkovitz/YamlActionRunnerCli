using System.Diagnostics;

namespace YamlActionRunnerCli.Exceptions.ActionExceptions;

public class FailedHttpRequest(int statusCode, string reason)
    : ActionException("Request failed: {0} {1}", statusCode, reason)
{
}