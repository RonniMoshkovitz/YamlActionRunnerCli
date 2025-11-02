using YamlActionRunnerCli.InstructionsRunning.DryRun;
using YamlActionRunnerCli.InstructionsRunning.Run;
using YamlActionRunnerCli.Utils.DataObjects.Instructions;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.InstructionsRunning;

/// <summary>
/// Orchestrates the execution of instructions (run or dry-run).
/// </summary>
public class InstructionsRunner
{
    private readonly Scope _scope;
    private readonly StepsProcessor _stepsProcessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="InstructionsRunner"/> class.
    /// </summary>
    /// <param name="scope">The execution scope.</param>
    public InstructionsRunner(Scope scope)
    {
        _scope = scope;
        _stepsProcessor = new(scope);
    }

    /// <summary>
    /// Executes the instruction's <see cref="Step"/>s in order.
    /// </summary>
    /// <param name="instructions">The parsed instructions to run.</param>
    public void Run(Instructions instructions)
    {
        foreach (var action in _stepsProcessor.ProcessSteps(instructions.Steps!))
        {
            _scope.Logger!.Verbose("Starting {type} step", action.GetType().Name);
            action.Run(_scope);
        }
    }

    /// <summary>
    /// Performs a dry run, logging the structure of the <see cref="Step"/>s without executing them.
    /// </summary>
    /// <param name="instructions">The parsed instructions to display.</param>
    public void DryRun(Instructions instructions) =>
        _scope.Logger!.Information("\n{instruction}", instructions.GetStructure());
}