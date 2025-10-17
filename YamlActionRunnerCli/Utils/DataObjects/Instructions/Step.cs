using System.ComponentModel.DataAnnotations;
using YamlActionRunnerCli.ActionManagement;

namespace YamlActionRunnerCli.Utils.DataObjects.Instructions;

public record Step
{
    public string? Name { get; set; }
    [Required]
    public ActionType? Action { get; set; }
    [Required]
    public Dictionary<string, object>? Parameters { get; set; }
}