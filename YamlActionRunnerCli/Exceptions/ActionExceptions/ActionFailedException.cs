using YamlActionRunnerCli.ActionManagement.Actions;

namespace YamlActionRunnerCli.Exceptions.ActionExceptions;

/// <summary>
/// Base abstract class for all exceptions thrown during the execution of an <see cref="IAction"/>.
/// </summary>
public abstract class ActionFailedException : RunnerBaseException
{
    /// <inheritdoc/>
    public override ExitCode ExitCode { get; } = ExitCode.ActionFailed;
    
    /// <summary>
    /// Action instance that failed.
    /// </summary>
    public IAction FailedAction { get;}

    /// <summary>
    /// Initializes a new instance of the <see cref="ActionFailedException"/> class.
    /// </summary>
    /// <param name="action">The action that failed.</param>
    /// <param name="message">The failure message.</param>
    public ActionFailedException(IAction action, string message) : base($"Action Failed - {message}")
    {
        FailedAction = action;
    }
}