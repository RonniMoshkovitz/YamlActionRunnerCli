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
        scope.Logger.Verbose("Trying to run {count} actions: {@actions}", Actions!.Count, Actions!.Select(a => a.GetType().Name));

        Actions!.ToList().ForEach(action => RunActionWithRetries(action, scope));
    }

    private void RunActionWithRetries(IAction action, Scope scope)
    {
        ActionFailedException? failReasonException = null;
        
        for (int i = 1; i <= Times; i++)
        {
            if (TryRunAction(action, scope, out failReasonException))
            {
                scope.Logger.Verbose("{actions} complete on attempt {attempt}", action.GetType().Name, i);
                return;
            }
            scope.Logger.Verbose("{actions} failed on attempt {attempt}", action.GetType().Name, i);
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