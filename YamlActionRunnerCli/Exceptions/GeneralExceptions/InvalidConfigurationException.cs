namespace YamlActionRunnerCli.Exceptions.GeneralExceptions;

public class InvalidConfigurationException : RunnerBaseException
{
    public override ExitCode ExitCode { get; } = ExitCode.InvalidConfiguration;

    public InvalidConfigurationException(Type configurationType, IEnumerable<string?> reasons) : base(
        $"Invalid configuration for {configurationType.Name} due to the following reasons: {string.Join("\n ", reasons)}")
    {
    }
}