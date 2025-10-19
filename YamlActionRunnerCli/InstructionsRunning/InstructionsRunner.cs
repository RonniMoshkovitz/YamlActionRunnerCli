using YamlActionRunnerCli.ActionManagement;
using YamlActionRunnerCli.Utils.DataObjects.Instructions;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.InstructionsRunning;

public class InstructionsRunner(Scope scope)
{
    private Scope _scope = scope;
    private StepsProcessor _stepsProcessor = new(scope);


    //handle error code?
    public void Run(Instructions instructions)
    {
        foreach (var step in _stepsProcessor.ProcessSteps(instructions.Steps))
        {
            step.Run(_scope);
        }
    }
}