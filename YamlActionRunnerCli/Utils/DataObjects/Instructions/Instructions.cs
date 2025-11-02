using System.ComponentModel.DataAnnotations;

namespace YamlActionRunnerCli.Utils.DataObjects.Instructions;

/// <summary>
/// Represents run instructions. Includes steps to be executed.
/// </summary>
public record Instructions
{
    /// <summary>
    /// List of top-level steps to execute.
    /// </summary>
    [Required]
    public IList<Step>? Steps { get; set; }
}