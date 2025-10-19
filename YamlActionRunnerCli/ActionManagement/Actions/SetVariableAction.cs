using System.ComponentModel.DataAnnotations;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

public class SetVariableAction : IAction
{
    [Required]
    public string? Name { get; set; }
    [Required]
    public object? Value {get; set;}
    
    public void Run(Scope scope)
    {
        scope.Variables[Name!] = Value;
    }
}