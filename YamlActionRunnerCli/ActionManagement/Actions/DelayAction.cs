using System.ComponentModel.DataAnnotations;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

/// <summary>
/// Action to pause execution for a specified duration.
/// </summary>
public class DelayAction : IAction
{
    /// <summary>
    /// Duration to wait, in milliseconds.
    /// </summary>
    [Required, Range(0, int.MaxValue)]
    public int? Duration { get; set; }

    /// <inheritdoc/>
    public void Run(Scope scope)
    {
        scope.Logger!.Verbose("Starting delay for {duration}Ms", Duration);
        Thread.Sleep(Duration!.Value);
        scope.Logger.Verbose("Finished delay for {duration}Ms", Duration);
    }
}