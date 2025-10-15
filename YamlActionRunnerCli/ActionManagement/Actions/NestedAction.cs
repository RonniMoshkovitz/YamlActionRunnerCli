using System.ComponentModel.DataAnnotations;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

public abstract class NestedAction : IAction
{
    [Required]
    public List<IAction>? Actions { get; set; }
    
    public abstract void Run(Scope scope);
}