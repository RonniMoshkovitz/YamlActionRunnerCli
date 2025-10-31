namespace YamlActionRunnerCli.Exceptions.GeneralExceptions;

public class InvalidYamlException : RunnerBaseException
{
    public override ExitCode ExitCode { get; } = ExitCode.InvalidConfiguration;

    public InvalidYamlException(string reason) : base($"Given YAML is invalid due to the following reason: '{reason}'")
    {
    }
}