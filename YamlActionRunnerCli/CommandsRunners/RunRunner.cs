using Serilog.Events;
using YamlActionRunnerCli.Cli.Commands;
using YamlActionRunnerCli.Exceptions;
using YamlActionRunnerCli.InstructionsParsing.YamlInstructionsParsing;
using YamlActionRunnerCli.InstructionsRunning;
using YamlActionRunnerCli.Utils.DataObjects.Run;
using YamlActionRunnerCli.Utils.Logging;

namespace YamlActionRunnerCli.CommandsRunners;

/// <summary>
/// Implements the logic for the 'run' command.
/// </summary>
public class RunRunner : ICommandRunner<RunCommand>
{
    private YamlInstructionsParser _instructionsParser = new();
    private InstructionsRunner _instructionsRunner;

    /// <summary>
    /// Initializes a new instance of the <see cref="RunRunner"/> class.
    /// </summary>
    /// <param name="scope">An optional existing scope. If null, a new one is created.</param>
    public RunRunner(Scope? scope=null)
    {
        _instructionsRunner = new(scope ?? new Scope{Logger = LoggerManager.Instance.Logger});
    }

    /// <summary>
    /// Executes the 'run' command logic: run or dry-run actions from a given YAML file.
    /// </summary>
    /// <param name="command"><see cref="RunCommand"/> arguments.</param>
    /// <returns><inheritdoc/></returns>
    public int Run(RunCommand command)
    {
        if(command.Verbose)
            LoggerManager.Instance.LevelSwitch.MinimumLevel = LogEventLevel.Verbose;
        
        try
        {
            var instructions = _instructionsParser.ParseFile(command.FilePath);
            
            if (command.DryRun)
                _instructionsRunner.DryRun(instructions);
            else
                _instructionsRunner.Run(instructions);
            
            return (int)ExitCode.Success;
        }
        catch (RunnerBaseException runnerException)
        {
            LoggerManager.Instance.Logger.Error(runnerException.Message);
            return (int)runnerException.ExitCode;
        }
    }
}