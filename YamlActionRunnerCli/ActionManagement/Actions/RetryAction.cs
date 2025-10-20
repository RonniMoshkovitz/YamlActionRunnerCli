using System.ComponentModel.DataAnnotations;
using YamlActionRunnerCli.Exceptions.ActionExceptions;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

public class RetryAction : NestedAction
{
    [Required, Range(1, int.MaxValue)]
    public int? Times { get; set; }

    public override void Run(Scope scope)
    {
        Actions!.ToList().ForEach(action => RunActionWithRetries(action, scope));
    }

    private void RunActionWithRetries(IAction action, Scope scope)
    {
        ActionFailedException? failReasonException = null;
        
        for (int i = 0; i < Times; i++)
        {
            if (TryRunAction(action, scope, out failReasonException)) return;
        }

        if (failReasonException is not null)
            throw new RetryFailed(this, Times!.Value, failReasonException);
    }
    
    private static bool TryRunAction(IAction action, Scope scope, out ActionFailedException? failReasonException)
    {
        failReasonException = null;
        
        try
        {
            action.Run(scope);
        }
        catch (ActionFailedException a)
        {
            failReasonException = a;
            return false;
        }

        return true;
    }
}