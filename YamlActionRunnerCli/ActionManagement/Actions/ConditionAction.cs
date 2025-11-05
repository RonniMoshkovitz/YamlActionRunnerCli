using System.ComponentModel.DataAnnotations;
using YamlActionRunnerCli.Exceptions.ActionExceptions;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

/// <summary>
/// A nested action that executes its child steps only if a given condition is true.
/// </summary>
public class ConditionAction : NestedAction
{
    /// <summary>
    /// C# condition to evaluate.
    /// </summary>
    [Required]
    public string? Condition { get; set; }
    
    /// <inheritdoc/>
    public override void Run(Scope scope)
    {
        if (IsConditionTrue(scope))
        {
            scope.Logger!.Verbose("Running {count} actions: {@actions}", Actions!.Count,
                Actions!.Select(action => action.GetType().Name));
            Actions!.ToList().ForEach(action => action.Run(scope));
        }
    }

    /// <summary>
    /// Checks if the <see cref="Condition"/> is true or false.
    /// </summary>
    /// <param name="scope">The execution scope.</param>
    /// <returns>True if the condition is true, false otherwise.</returns>
    private bool IsConditionTrue(Scope scope)
    {
        try
        {
            new AssertAction { Condition = Condition }.Run(scope);
        }
        catch (FailedAssertionException)
        {
            scope.Logger!.Verbose("Condition {Condition}' is false", Condition);
            return false;
        }

        return true;
    }
}