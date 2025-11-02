using System.ComponentModel.DataAnnotations;
using Serilog.Events;
using YamlActionRunnerCli.Cli.Commands;
using YamlActionRunnerCli.CommandsRunners;
using YamlActionRunnerCli.Utils.DataObjects.Run;
using YamlActionRunnerCli.Utils.Logging;

namespace YamlActionRunnerCli.ActionManagement.Actions;

public class ImportAction : IAction
{
    [Required, FileExtensions(Extensions = ".yaml")]
    public string FilePath { get; set; }

    public void Run(Scope scope)
    {
        scope.Logger!.Verbose("Importing steps from file '{file}'", FilePath);
        new RunRunner(scope).Run(new RunCommand
        {
            FilePath = FilePath, Verbose = LoggerManager.Instance.LevelSwitch.MinimumLevel == LogEventLevel.Verbose
        });
    }
}