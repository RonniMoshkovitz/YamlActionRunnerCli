using System.ComponentModel.DataAnnotations;
using YamlActionRunnerCli.ActionManagement;
using YamlDotNet.Serialization;

namespace YamlActionRunnerCli.Utils.DataObjects.Instructions;

/// <summary>
/// Represents a single step (an action and its parameters) in the instructions. Also supports nested steps for relevant actions to execute.
/// </summary>
public record Step
{
    /// <summary>
    /// Type of action this step represents.
    /// </summary>
    [Required]
    public ActionType? Action { get; set; }
    
    /// <summary>
    /// Parameters for this action.
    /// </summary>
    [Required]
    public Dictionary<string, object>? Parameters { get; set; }
    
    /// <summary>
    /// List of nested steps.
    /// </summary>
    [YamlIgnore]
    public IList<Step>? NestedSteps { get; set; }
}