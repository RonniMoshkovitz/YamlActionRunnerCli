using System.ComponentModel.DataAnnotations;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

public class PrintVariableAction : IAction
{
    private readonly LogAction _logAction = new();
    
    [Required]
    public string? Name { get; set; }
    
    public void Run(Scope scope)
    {
        _logAction.Message = (scope.Variables[Name!] ?? "").ToString();
        _logAction.Run(scope);
    }
}