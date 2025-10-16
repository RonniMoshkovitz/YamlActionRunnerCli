using System.ComponentModel.DataAnnotations;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

public class SetVariableAction : VariableAction
{
    [Required]
    public object? Value {get; set;}
    
    public override void Run(Scope scope)
    {
        SetVariableValueFromScope(scope, Value);
    }
}