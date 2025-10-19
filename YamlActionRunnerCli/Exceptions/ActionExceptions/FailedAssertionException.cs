namespace YamlActionRunnerCli.Exceptions.ActionExceptions;

using System;

public class FailedAssertionException(string condition) : ActionException("Assertion failed: '{0}' is false", condition)
{
    
}