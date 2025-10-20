using System.ComponentModel.DataAnnotations;
using YamlActionRunnerCli.Exceptions.ActionExceptions;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

public class ConditionAction : NestedAction
{
    [Required] public string? Condition { get; set; }

    public override void Run(Scope scope)
    {
        if (IsConditionTrue(scope))
            Actions!.ToList().ForEach(action => action.Run(scope));
    }

    private bool IsConditionTrue(Scope scope)
    {
        try
        {
            new AssertAction { Condition = Condition }.Run(scope);
        }
        catch (FailedAssertionException)
        {
            return false;
        }

        return true;
    }
}