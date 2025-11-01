namespace YamlActionRunnerCli.Exceptions;

public abstract class RunnerBaseException : Exception
{
    public abstract ExitCode ExitCode { get; }

    public RunnerBaseException(string? message) : base(message)
    {
    }
}