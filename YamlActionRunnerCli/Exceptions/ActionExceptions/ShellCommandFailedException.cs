using YamlActionRunnerCli.ActionManagement.Actions;

namespace YamlActionRunnerCli.Exceptions.ActionExceptions;

/// <summary>
/// Exception thrown by <see cref="ShellAction"/> when the external process exits with a non-zero code.
/// </summary>
public class ShellCommandFailedException : ActionFailedException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ShellCommandFailedException"/> class.
    /// </summary>
    /// <param name="action">The shell action instance.</param>
    /// <param name="exitCode">The non-zero exit code from the process.</param>
    public ShellCommandFailedException(IAction action, int exitCode) 
        : base(action, $"Shell command failed: Command exited with non-zero code {exitCode}")
    {
    }
}