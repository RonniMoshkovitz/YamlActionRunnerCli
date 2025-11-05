using YamlActionRunnerCli.ActionManagement.Actions;

namespace YamlActionRunnerCli.Exceptions.ActionExceptions;

/// <summary>
/// Exception thrown by <see cref="AssertAction"/> or <see cref="ConditionAction"/> when the C# condition string is invalid.
/// </summary>
public class InvalidConditionException : ActionFailedException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidConditionException"/> class.
    /// </summary>
    /// <param name="action">The action instance.</param>
    /// <param name="condition">The invalid C# condition string.</param>
    public InvalidConditionException(IAction action, string condition)
        : base(action, $"Invalid condition '{condition}'")
    {
    }
}