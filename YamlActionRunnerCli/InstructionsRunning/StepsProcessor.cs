using YamlActionRunnerCli.ActionManagement;
using YamlActionRunnerCli.ActionManagement.Actions;
using YamlActionRunnerCli.Utils.DataObjects.Instructions;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.InstructionsRunning;

public class StepsProcessor(Scope scope)
{
    private ActionFactory _actionFactory = new();
    private Scope _scope = scope;

    public IEnumerable<IAction> ProcessSteps(IEnumerable<Step> steps)
    {
        foreach (var step in steps)
        {
            ApplyScopeToStep(step);
            yield return _actionFactory.GetAction(step);
        }
    }

    private void ApplyScopeToStep(Step step)
    {
        step.NestedSteps?.ToList().ForEach(ApplyScopeToStep);

        foreach (var (key, value) in step.Parameters!)
        {
            if (value is string stringValue)
            {
                step.Parameters[key] = _scope.Variables.PlaceVariablesInData(stringValue);
            }
        }
    }
}