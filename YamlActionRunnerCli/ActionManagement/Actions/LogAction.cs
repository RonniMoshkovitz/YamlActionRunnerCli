using System.ComponentModel.DataAnnotations;
using Serilog.Events;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

/// <summary>
/// Action to write a message to the logger.
/// </summary>
public class LogAction : IAction
{ 
    /// <summary>
    /// Message to log.
    /// </summary>
    [Required]
    public string? Message { get; set; }
    
    /// <summary>
    /// Log level to log the <see cref="Message"/>.
    /// </summary>
    public LogEventLevel Level { get; set; } = LogEventLevel.Information;

    /// <inheritdoc/>
    public void Run(Scope scope)
    {
        scope.Logger!.Verbose("Logging message '{message}' with '{level:G}' logging level ", Message, Level);
        scope.Logger.Write(Level, Message!);
    }
}