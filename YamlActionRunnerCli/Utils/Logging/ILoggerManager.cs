using Serilog;
using Serilog.Core;

namespace YamlActionRunnerCli.Utils.Logging;

/// <summary>
/// Interface for managing Serilog logger instances.
/// </summary>
public interface ILoggerManager
{ 
    /// <summary>
    /// Serilog logger instance.
    /// </summary>
    public ILogger Logger { get;}
    
    /// <summary>
    /// GSwitch to dynamically change the log level.
    /// </summary>
    public LoggingLevelSwitch LevelSwitch { get; }
}