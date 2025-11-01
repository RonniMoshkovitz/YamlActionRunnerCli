using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using YamlActionRunnerCli.Exceptions.ActionExceptions;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

public class ShellAction : IAction
{
    private const string SHELL_LOG_FORMAT = "[stdout] {Line}";
    private readonly string _shell = OperatingSystem.IsWindows() ? "cmd.exe" : "/bin/bash";
    private readonly string _shellArgsFormat = OperatingSystem.IsWindows() ? "/C {0}" : "-c \"{0}\"";
    
    [Required]
    public string? Command { get; set; }

    public void Run(Scope scope)
    {
        using var process = GetProcess(Command!);

        SetProcessRuntimeLogging(process, scope);
        
        scope.Logger!.Verbose("Starting to run shell command '{command}'", Command);
        RunProcess(process);

        ValidateProcessSuccess(process);
        scope.Logger.Verbose("Shell command finished successfully");
    }

    private void ValidateProcessSuccess(Process process)
    {
        if (process.ExitCode != 0)
            throw new ShellCommandFailedException(this, process.ExitCode);
    }
    
    private Process GetProcess(string command)
    {
        var processInfo = new ProcessStartInfo
        {
            FileName = _shell,
            Arguments = string.Format(_shellArgsFormat, command),
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        
        return new Process
        {
            StartInfo = processInfo,
            EnableRaisingEvents = true
        };
    }

    private static void SetProcessRuntimeLogging(Process process, Scope scope)
    {
        process.OutputDataReceived += (_, e) => LogRuntimeEvent(e, scope);
        process.ErrorDataReceived += (_, e) => LogRuntimeEvent(e, scope);
    }

    private static void LogRuntimeEvent(DataReceivedEventArgs eventArgs, Scope scope)
    {
        if (!string.IsNullOrEmpty(eventArgs.Data))
            scope.Logger!.Information(SHELL_LOG_FORMAT, eventArgs.Data);
    }

    private static void RunProcess(Process process)
    {
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        process.WaitForExit();
    }
}