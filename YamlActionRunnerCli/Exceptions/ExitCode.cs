namespace YamlActionRunnerCli.Exceptions;

/// <summary>
/// Defines the application's possible exit codes.
/// </summary>
public enum ExitCode
{
    Success,
    InvalidArguments,
    ActionFailed,
    InvalidConfiguration,
}