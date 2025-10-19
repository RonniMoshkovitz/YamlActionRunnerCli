using System.Collections.Concurrent;
using Serilog;

namespace YamlActionRunnerCli.Utils.DataObjects.Run;

public record Scope
{
    public Variables Variables { get; set; }
    public ILogger Logger {get; set;}
}