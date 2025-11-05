using YamlActionRunnerCli.ActionManagement.Actions;

namespace YamlActionRunnerCli.Exceptions.ActionExceptions;

/// <summary>
/// Exception thrown by <see cref="AssertAction"/> when its condition evaluates to false.
/// </summary>
public class FailedAssertionException : ActionFailedException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FailedAssertionException"/> class.
    /// </summary>
    /// <param name="action">The assert action instance.</param>
    /// <param name="condition">The condition string that evaluated to false.</param>
    public FailedAssertionException(IAction action, string condition) 
        : base(action, $"Assertion failed: '{condition}' is false")
    {
    }
}