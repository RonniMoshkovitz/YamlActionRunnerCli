using System.ComponentModel.DataAnnotations;
using YamlActionRunnerCli.ActionManagement;

namespace YamlActionRunnerCli.Utils.DataObjects.Instructions;

public record Step
{
    [Required]
    public ActionType? Action { get; set; }
    [Required]
    public Dictionary<string, object>? Parameters { get; set; }
    
    public IList<Step>? NestedSteps { get; set; }
}