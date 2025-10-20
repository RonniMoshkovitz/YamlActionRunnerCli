using YamlActionRunnerCli.ActionManagement.Actions;
using YamlActionRunnerCli.Utils.DataObjects.Instructions;

namespace YamlActionRunnerCli.ActionManagement;

public class ActionFactory
{
    private static readonly string _stepsKey = nameof(Instructions.Steps).ToLower();
    private static readonly string _actionsKey = nameof(NestedAction.Actions).ToLower();
    
    private readonly ActionBuilder _actionBuilder = new();

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
        [ActionType.Condition] = typeof(ConditionAction),
        [ActionType.Shell] = typeof(ShellAction)
    };

    public IAction GetAction(Step step)
    {
        var actionObjectType = GetActionObjectType(step.Action!.Value);
        if (actionObjectType.IsSubclassOf(typeof(NestedAction)) &&
            step.Parameters!.TryGetValue(_stepsKey, out var nestedSteps))
        {
            step.Parameters[_actionsKey] = (from nestedStep in nestedSteps as List<Step> select GetAction(nestedStep)).ToList();
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