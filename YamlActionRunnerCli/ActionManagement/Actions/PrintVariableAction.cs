using System.ComponentModel.DataAnnotations;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

public class PrintVariableAction : IAction
{
    [Required]
    public string? Name { get; set; }
    
    public void Run(Scope scope)
    {
        scope.Logger!.Verbose("Logging variable named '{name}'", Name);
        scope.Logger.Information("{S} = {ScopeVariable}", Name, scope.Variables![Name!]);
    }
}