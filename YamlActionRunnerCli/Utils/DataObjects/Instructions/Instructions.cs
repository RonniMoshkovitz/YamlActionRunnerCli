using System.ComponentModel.DataAnnotations;

namespace YamlActionRunnerCli.Utils.DataObjects.Instructions;

public record Instructions
{
    [Required]
    public IList<Step>? Steps { get; set; }
}