using YamlActionRunnerCli.ActionManagement.Actions;
using YamlActionRunnerCli.Utils;
using YamlActionRunnerCli.Utils.ObjectManagement;

namespace YamlActionRunnerCli.ActionManagement;

public class ActionBuilder
{
    public IAction BuildAction(Type actionObjectType, IDictionary<string, object> parameters)
    {
        if (parameters.ToObjectWithProperties(actionObjectType) is not IAction typedAction)
            throw new InvalidOperationException($"Failed to map parameters for {actionObjectType.Name}");
    
        return typedAction;
    }
}