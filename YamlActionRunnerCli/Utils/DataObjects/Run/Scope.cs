using Serilog;

namespace YamlActionRunnerCli.Utils.DataObjects.Run;

public record Scope
{
    public Dictionary<string, object?> Variables {get; set;}
    public ILogger Logger {get; set;}
}