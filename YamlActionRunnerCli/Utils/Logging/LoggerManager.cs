using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace YamlActionRunnerCli.Utils.Logging;

public class LoggerManager
{ 
    private static readonly Lazy<LoggerManager> _instance = new(() => new LoggerManager());
    public static LoggerManager Instance => _instance.Value;
    public ILogger Logger { get;}
    public LoggingLevelSwitch LevelSwitch { get; set; }
    
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