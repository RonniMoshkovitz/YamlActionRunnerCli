using System.ComponentModel.DataAnnotations;
using Serilog.Events;
using YamlActionRunnerCli.Cli.Commands;
using YamlActionRunnerCli.CommandsRunners;
using YamlActionRunnerCli.Utils.DataObjects.Run;
using YamlActionRunnerCli.Utils.Logging;

namespace YamlActionRunnerCli.ActionManagement.Actions;

/// <summary>
/// Action to execute another YAML workflow file.
/// </summary>
public class ImportAction : IAction
{
    /// <summary>
    /// Path to the YAML file to import.
    /// </summary>
    [Required, FileExtensions(Extensions = ".yaml")]
    public string FilePath { get; set; }

    /// <inheritdoc/>
    public void Run(Scope scope)
    {
        scope.Logger!.Verbose("Importing steps from file '{file}'", FilePath);
        new RunRunner(scope).Run(new RunCommand
        {
            FilePath = FilePath, Verbose = LoggerManager.Instance.LevelSwitch.MinimumLevel == LogEventLevel.Verbose
        });
    }
}