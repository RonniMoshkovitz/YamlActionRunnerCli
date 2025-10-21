using System.ComponentModel.DataAnnotations;
using YamlActionRunnerCli.Exceptions.ActionExceptions;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

public class ConditionAction : NestedAction
{
    [Required]
    public string? Condition { get; set; }

    public override void Run(Scope scope)
    {
        if (IsConditionTrue(scope))
        {
            scope.Logger!.Verbose("Running {count} actions: {@actions}", Actions!.Count,
                Actions!.Select(action => action.GetType().Name));
            Actions!.ToList().ForEach(action => action.Run(scope));
        }
    }

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