using System.ComponentModel.DataAnnotations;
using Serilog;

namespace YamlActionRunnerCli.Utils.DataObjects.Run;

/// <summary>
/// Represents the shared execution state for an instruction's run. This is passed to every action.
/// </summary>
public record Scope
{
    /// <summary>
    /// Variables set in current run.
    /// </summary>
    public Variables Variables { get; set; } = new();
    
    /// <summary>
    /// Logger instance for this run.
    /// </summary>
    [Required]
    public ILogger? Logger {get; set;}
}