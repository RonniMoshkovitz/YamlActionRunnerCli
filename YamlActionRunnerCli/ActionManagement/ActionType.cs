namespace YamlActionRunnerCli.ActionManagement;

/// <summary>
/// Defines the available action types that can be specified in the YAML file.
/// </summary>
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