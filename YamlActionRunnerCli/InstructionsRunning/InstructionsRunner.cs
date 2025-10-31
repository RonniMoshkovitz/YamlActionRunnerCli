using YamlActionRunnerCli.InstructionsRunning.DryRun;
using YamlActionRunnerCli.InstructionsRunning.Run;
using YamlActionRunnerCli.Utils.DataObjects.Instructions;
using YamlActionRunnerCli.Utils.DataObjects.Run;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace YamlActionRunnerCli.InstructionsRunning;

public class InstructionsRunner
{
    private readonly Scope _scope;
    private readonly StepsProcessor _stepsProcessor;
    
    public InstructionsRunner(Scope scope)
    {
        _scope = scope;
        _stepsProcessor = new(scope);
    }
    
    public void Run(Instructions instructions)
    {
        foreach (var action in _stepsProcessor.ProcessSteps(instructions.Steps!))
        {
            _scope.Logger!.Verbose("Starting {type} step", action.GetType().Name);
            action.Run(_scope);
        }
    }

    public void DryRun(Instructions instructions)
    {
        _scope.Logger!.Information("\n{instruction}", instructions.GetStructure());
    }
}