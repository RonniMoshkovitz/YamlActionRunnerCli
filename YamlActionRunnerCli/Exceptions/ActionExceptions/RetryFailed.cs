using YamlActionRunnerCli.ActionManagement.Actions;

namespace YamlActionRunnerCli.Exceptions.ActionExceptions;

/// <summary>
/// Exception thrown by <see cref="RetryAction"/> when a nested action fails all its retry attempts.
/// </summary>
public class RetryFailed : ActionFailedException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RetryFailed"/> class.
    /// </summary>
    /// <param name="action">The retry action instance.</param>
    /// <param name="times">The number of failed attempts.</param>
    /// <param name="innerException">The last exception caught from the last failed attempt.</param>
    public RetryFailed(IAction action, int times, Exception innerException) 
        : base(action, $"Retry failed: failed {times} times due to: {innerException.Message}")
    {
    }
}