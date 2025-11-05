using System.ComponentModel.DataAnnotations;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

/// <summary>
/// Abstract base class for actions that contain other actions
/// </summary>
public abstract class NestedAction : IAction
{
    /// <summary>
    /// Nested <see cref="IAction"/> to be used as inside the current action.
    /// </summary>
    [Required]
    public List<IAction>? Actions { get; set; }
    
    /// <inheritdoc/>
    public abstract void Run(Scope scope);
}