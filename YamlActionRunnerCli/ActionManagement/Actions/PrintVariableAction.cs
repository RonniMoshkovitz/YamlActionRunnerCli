using System.ComponentModel.DataAnnotations;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

/// <summary>
/// Action to log the value of a variable from the scope.
/// </summary>
public class PrintVariableAction : IAction
{
    /// <summary>
    /// Name of the variable to print.
    /// </summary>
    [Required]
    public string? Name { get; set; }
    
    /// <inheritdoc/>
    public void Run(Scope scope)
    {
        scope.Logger!.Verbose("Logging variable named '{name}'", Name);
        scope.Logger.Information("{S} = {ScopeVariable}", Name, scope.Variables[Name!]);
    }
}