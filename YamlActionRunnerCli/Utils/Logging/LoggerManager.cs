using Serilog;
using Serilog.Core;

namespace YamlActionRunnerCli.Utils.Logging;

/// <summary>
/// Utility class that manages a Serilog logger instance for the application.
/// </summary>
public class LoggerManager
{ 
    private static readonly Lazy<LoggerManager> _instance = new(new LoggerManager());
    
    /// <summary>
    /// Gets the singleton instance of the LoggerManager.
    /// </summary>
    public static LoggerManager Instance => _instance.Value;
    
    /// <summary>
    /// Serilog logger instance.
    /// </summary>
    public ILogger Logger { get; }
    
    /// <summary>
    /// GSwitch to dynamically change the log level.
    /// </summary>
    public LoggingLevelSwitch LevelSwitch { get; }
    
    
    /// <summary>
    /// Private constructor to initialize the logger configuration.
    /// </summary>
    private LoggerManager()
    {
        LevelSwitch = new LoggingLevelSwitch();
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.ControlledBy(LevelSwitch)
            .WriteTo.Console(outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        Logger = Log.Logger;
    }
}