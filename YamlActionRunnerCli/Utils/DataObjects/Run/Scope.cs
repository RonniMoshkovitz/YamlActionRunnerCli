using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using Serilog;

namespace YamlActionRunnerCli.Utils.DataObjects.Run;

public record Scope
{
    public Variables Variables { get; set; } = new();
    [Required]
    public ILogger? Logger {get; set;}
}