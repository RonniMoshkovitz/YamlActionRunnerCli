using YamlActionRunnerCli.ActionManagement.Actions;
using YamlActionRunnerCli.Utils.DataObjects.Instructions;

namespace YamlActionRunnerCli.ActionManagement;

public class ActionFactory
{
    private readonly ActionBuilder _actionBuilder = new ActionBuilder();

    private static readonly Dictionary<ActionType, Type> _actionTypes = new()
    {
        [ActionType.Log] = typeof(LogAction),
        [ActionType.Delay] = typeof(DelayAction),
        [ActionType.Assert] = typeof(AssertAction),
        [ActionType.Http] = typeof(HttpAction),
        [ActionType.SetVariable] = typeof(SetVariableAction),
        [ActionType.PrintVariable] = typeof(PrintVariableAction),
        [ActionType.Retry] =  typeof(RetryAction),
        [ActionType.Parallel] = typeof(ParallelAction),
    };

    public IAction Create(Step step)
    {
        var actionObjectType = GetActionObjectType(step.ActionType!.Value);
        if (actionObjectType.IsSubclassOf(typeof(NestedAction)) &&
            step.Parameters!.TryGetValue("steps", out var nestedSteps))
        {
            step.Parameters["Actions"] = (from nestedStep in nestedSteps as List<Step> select Create(nestedStep)).ToList();
        }

        return _actionBuilder.BuildAction(actionObjectType, step.Parameters!);
    }

    private static Type GetActionObjectType(ActionType actionType)
    {
        if (!_actionTypes.TryGetValue(actionType, out var actionObjectType))
            throw new InvalidOperationException($"Unknown action type: {actionType}");

        return actionObjectType;
    }
}