using YamlActionRunnerCli.ActionManagement.Actions;

namespace YamlActionRunnerCli.Exceptions.ActionExceptions;

public class ShellCommandFailedException : ActionFailedException
{
    public ShellCommandFailedException(IAction action, int exitCode) 
        : base(action, $"Shell command failed: Command exited with non-zero code {exitCode}")
    {
    }
}