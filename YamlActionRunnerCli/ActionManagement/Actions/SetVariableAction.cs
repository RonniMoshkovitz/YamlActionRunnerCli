using System.ComponentModel.DataAnnotations;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

/// <summary>
/// Action to set or update a variable in the execution scope.
/// </summary>
public class SetVariableAction : IAction
{
    /// <summary>
    /// Name of the variable.
    /// </summary>
    [Required]
    public string? Name { get; set; }
    
    /// <summary>
    /// Value to assign to the variable.
    /// </summary>
    [Required]
    public object? Value {get; set;}
    
    /// <inheritdoc/>
    public void Run(Scope scope)
    {
        scope.Logger!.Verbose("Setting value for variable '{name}' = {value}", Name, Value);
        scope.Variables[Name!] = Value;
    }
}