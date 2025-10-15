using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

public class ParallelAction : NestedAction
{
    public override void Run(Scope scope)
    {
        Parallel.ForEach(Actions!, action => action.Run(scope));
    }
}