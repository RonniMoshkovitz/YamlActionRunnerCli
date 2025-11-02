using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

/// <summary>
/// A nested action that executes all its child actions in parallel.
/// </summary>
public class ParallelAction : NestedAction
{
    /// <inheritdoc/>
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