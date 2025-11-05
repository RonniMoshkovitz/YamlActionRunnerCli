using YamlActionRunnerCli.ActionManagement.Actions;
using YamlActionRunnerCli.Exceptions.GeneralExceptions;
using YamlActionRunnerCli.Utils.DataObjects.Instructions;

namespace YamlActionRunnerCli.ActionManagement;

/// <summary>
/// A factory class for creating <see cref="IAction"/> instances from <see cref="Step"/> objects.
/// </summary>
public class ActionFactory
{
    private static readonly string _actionsKey = nameof(NestedAction.Actions).ToLower();

    private readonly ActionBuilder _actionBuilder = new();

    /// <summary>
    /// Static mapping of <see cref="ActionType"/> enum to the concrete action <see cref="Type"/>.
    /// </summary>
    private static readonly Dictionary<ActionType, Type> _actionTypes = new()
    {
        [ActionType.Log] = typeof(LogAction),
        [ActionType.Delay] = typeof(DelayAction),
        [ActionType.Assert] = typeof(AssertAction),
        [ActionType.Http] = typeof(HttpAction),
        [ActionType.SetVariable] = typeof(SetVariableAction),
        [ActionType.PrintVariable] = typeof(PrintVariableAction),
        [ActionType.Retry] = typeof(RetryAction),
        [ActionType.Parallel] = typeof(ParallelAction),
        [ActionType.Condition] = typeof(ConditionAction),
        [ActionType.Shell] = typeof(ShellAction),
        [ActionType.Import] = typeof(ImportAction)
    };

    /// <summary>
    /// Gets the concrete <see cref="IAction"/> for a given <see cref="Step"/>.
    /// </summary>
    /// <param name="step">The step object to create concrete action by.</param>
    /// <returns>A fully instantiated <see cref="IAction"/> with properties according to the given <see cref="Step"/>.</returns>
    public IAction GetAction(Step step)
    {
        var actionObjectType = GetActionObjectType(step.Action!.Value);

        if (actionObjectType.IsSubclassOf(typeof(NestedAction)) && step.NestedSteps is not null)
        {
            step.Parameters![_actionsKey] =
                (from nestedStep in step.NestedSteps as List<Step> select GetAction(nestedStep)).ToList();
        }

        return _actionBuilder.BuildAction(actionObjectType, step.Parameters!);
    }

    /// <summary>
    /// Find and returns the <see cref="IAction"/> object <see cref="Type"/> for a given <see cref="ActionType"/>.
    /// </summary>
    /// <param name="actionType">Enum describing <see cref="IAction"/> object <see cref="Type"/>.</param>
    /// <returns>The matching <see cref="IAction"/> <see cref="Type"/> for the given <see cref="ActionType"/>.</returns>
    /// <exception cref="InvalidConfigurationException">Thrown if <see cref="IAction"/> object <see cref="Type"/>
    /// not found for given <see cref="ActionType"/>.</exception>
    private static Type GetActionObjectType(ActionType actionType)
    {
        if (!_actionTypes.TryGetValue(actionType, out var actionObjectType))
            throw new InvalidConfigurationException(typeof(ActionType), [$"Unknown action type: {actionType}"]);

        return actionObjectType;
    }
}