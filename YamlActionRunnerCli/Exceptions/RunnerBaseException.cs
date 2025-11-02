namespace YamlActionRunnerCli.Exceptions;

/// <summary>
/// Base abstract class for all custom exceptions in the application.
/// </summary>
public abstract class RunnerBaseException : Exception
{
    /// <summary>
    /// Corresponding exit code for this exception.
    /// </summary>
    public abstract ExitCode ExitCode { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RunnerBaseException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public RunnerBaseException(string? message) : base(message)
    {
    }
}