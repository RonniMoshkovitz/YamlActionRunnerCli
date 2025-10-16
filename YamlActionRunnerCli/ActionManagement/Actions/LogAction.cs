using System.ComponentModel.DataAnnotations;
using Serilog.Events;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

public class LogAction : IAction
{ 
    [Required]
    public string? Message { get; set; }
    public LogEventLevel Level { get; set; } = LogEventLevel.Information;

    public void Run(Scope scope)
    {
        scope.Logger.Write(Level, Message!);
    }
}