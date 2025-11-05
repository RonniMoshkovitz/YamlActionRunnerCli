namespace YamlActionRunnerCli.Exceptions.GeneralExceptions;

/// <summary>
/// Exception thrown when the YAML file is malformed, unreadable, or missing required structure.
/// </summary>
public class InvalidYamlException : RunnerBaseException
{
    /// <inheritdoc/>
    public override ExitCode ExitCode { get; } = ExitCode.InvalidConfiguration;

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidYamlException"/> class.
    /// </summary>
    /// <param name="reason">The underlying reason for yaml parsing failure.</param>
    public InvalidYamlException(string reason) : base($"Given YAML is invalid due to the following reason: '{reason}'")
    {
    }
}