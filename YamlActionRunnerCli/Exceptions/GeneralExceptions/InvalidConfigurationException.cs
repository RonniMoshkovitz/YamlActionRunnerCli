namespace YamlActionRunnerCli.Exceptions.GeneralExceptions;

/// <summary>
/// Exception thrown when action parameters fail validation.
/// </summary>
public class InvalidConfigurationException : RunnerBaseException
{
    /// <inheritdoc/>
    public override ExitCode ExitCode { get; } = ExitCode.InvalidConfiguration;

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidConfigurationException"/> class.
    /// </summary>
    /// <param name="configurationType">The type of object that failed validation.</param>
    /// <param name="reasons">The list of validation error messages.</param>
    public InvalidConfigurationException(Type configurationType, IEnumerable<string?> reasons) : base(
        $"Invalid configuration for {configurationType.Name} due to the following reasons: {string.Join("\n ", reasons)}")
    {
    }
}