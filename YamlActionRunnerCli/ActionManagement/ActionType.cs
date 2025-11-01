namespace YamlActionRunnerCli.ActionManagement;

public enum ActionType
{
    Log,
    Delay,
    Assert,
    Http,
    SetVariable,
    PrintVariable,
    Retry,
    Parallel,
    Condition,
    Shell,
    Import
}