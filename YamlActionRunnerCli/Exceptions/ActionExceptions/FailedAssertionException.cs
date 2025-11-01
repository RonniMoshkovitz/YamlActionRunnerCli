using YamlActionRunnerCli.ActionManagement.Actions;

namespace YamlActionRunnerCli.Exceptions.ActionExceptions;

using System;

public class FailedAssertionException : ActionFailedException
{
    public FailedAssertionException(IAction action, string condition) 
        : base(action, $"Assertion failed: '{condition}' is false")
    {
    }
}