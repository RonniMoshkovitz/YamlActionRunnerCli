using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

/// <summary>
/// Defines the interface for all executable actions.
/// </summary>
public interface IAction
{
    /// <summary>
    /// Executes the action's logic.
    /// </summary>
    /// <param name="scope">The shared execution scope.</param>
    void Run(Scope scope);
}