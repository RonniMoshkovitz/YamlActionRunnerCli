using System.ComponentModel.DataAnnotations;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

public abstract class VariableAction : IAction
{ 
    [Required]
    public string? Name { get; set; }

    public abstract void Run(Scope scope);

    protected object? GetVariableValueFromScope(Scope scope)
    {
        if (!scope.Variables.TryGetValue(Name!, out var varByName))
            throw new InvalidOperationException($"No such var as : {Name}");
        return varByName;
    }

    protected void SetVariableValueFromScope(Scope scope, object? value)
    {
        scope.Variables[Name!] = value;
    }
}