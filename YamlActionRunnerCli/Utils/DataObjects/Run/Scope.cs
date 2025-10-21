using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using Serilog;

namespace YamlActionRunnerCli.Utils.DataObjects.Run;

public record Scope
{
    [Required]
    public Variables? Variables { get; set; }
    [Required]
    public ILogger? Logger {get; set;}
}