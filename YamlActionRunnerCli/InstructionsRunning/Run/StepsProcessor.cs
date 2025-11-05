using YamlActionRunnerCli.ActionManagement;
using YamlActionRunnerCli.ActionManagement.Actions;
using YamlActionRunnerCli.Utils.DataObjects.Instructions;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.InstructionsRunning.Run;

/// <summary>
/// Processes raw <see cref="Step"/> objects into executable <see cref="IAction"/> objects. This includes variable substitution and action instantiation.
/// </summary>
public class StepsProcessor
{
    private ActionFactory _actionFactory = new();
    private Scope _scope;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="StepsProcessor"/> class.
    /// </summary>
    /// <param name="scope">The execution scope</param>
    public StepsProcessor(Scope scope)
    {
        _scope = scope;
    }
    
    /// <summary>
    /// Lazily processes a sequence of steps.
    /// </summary>
    /// <param name="steps">The raw steps to process into <see cref="IAction"/>s.</param>
    /// <returns>An <see cref="Enumerable"/> of fully instantiated and ready-to-run <see cref="IAction"/> objects.</returns>
    public IEnumerable<IAction> ProcessSteps(IEnumerable<Step> steps)
    {
        foreach (var step in steps)
        {
            ApplyScopeToStep(step);
            yield return _actionFactory.GetAction(step);
        }
    }

    /// <summary>
    /// Recursively applies variable substitution to all string parameters in a step.
    /// </summary>
    /// <param name="step">The step to modify.</param>
    private void ApplyScopeToStep(Step step)
    {
        step.NestedSteps?.ToList().ForEach(ApplyScopeToStep);

        foreach (var (key, value) in step.Parameters!)
        {
            if (value is string stringValue)
            {
                step.Parameters[key] = _scope.Variables!.PlaceVariablesInData(stringValue);
            }
        }
    }
}