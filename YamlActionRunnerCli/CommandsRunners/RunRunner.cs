using Serilog.Events;
using YamlActionRunnerCli.Cli.Commands;
using YamlActionRunnerCli.Exceptions;
using YamlActionRunnerCli.InstructionsParsing.YamlInstructionsParsing;
using YamlActionRunnerCli.InstructionsRunning;
using YamlActionRunnerCli.Utils.DataObjects.Run;
using YamlActionRunnerCli.Utils.Logging;

namespace YamlActionRunnerCli.CommandsRunners;

public class RunRunner : ICommandRunner<RunCommand>
{
    private YamlInstructionsParser _instructionsParser = new();
    private InstructionsRunner _instructionsRunner;

    public RunRunner(Scope? scope=null)
    {
        _instructionsRunner = new(scope ?? new Scope{Logger = LoggerManager.Instance.Logger});
    }

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