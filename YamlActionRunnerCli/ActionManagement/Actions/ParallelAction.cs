using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

public class ParallelAction : NestedAction
{
    public override void Run(Scope scope)
    {
        scope.Logger!.Verbose("Starting to run {count} actions in parallel: {@actions}", Actions!.Count,
            Actions!.Select(action => action.GetType().Name));
        try
        {
            Parallel.ForEach(Actions!, action => action.Run(scope));
        }
        catch (AggregateException aggregateException)
        {
            throw aggregateException.InnerException!;
        }
        
    }
}