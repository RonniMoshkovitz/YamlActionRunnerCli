using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using YamlActionRunnerCli.Exceptions.ActionExceptions;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

/// <summary>
/// Action to execute a command in the operating system's native shell.
/// </summary>
public class ShellAction : IAction
{
    private const string SHELL_LOG_FORMAT = "[stdout] {Line}";
    private const string SHELL_ERROR_FORMAT = "[stdout] {Line}";
    private readonly string _shell = OperatingSystem.IsWindows() ? "cmd.exe" : "/bin/bash";
    private readonly string _shellArgsFormat = OperatingSystem.IsWindows() ? "/C {0}" : "-c \"{0}\"";
    
    /// <summary>
    /// Shell command to execute.
    /// </summary>
    [Required]
    public string? Command { get; set; }

    /// <inheritdoc/>
    /// <exception cref="ShellCommandFailedException">Thrown if the process exits with a non-zero code.</exception>
    public void Run(Scope scope)
    {
        using var process = GetProcess(Command!);

        SetProcessRuntimeLogging(process, scope);
        
        scope.Logger!.Verbose("Starting to run shell command '{command}'", Command);
        RunProcess(process);

        ValidateProcessSuccess(process);
        scope.Logger.Verbose("Shell command finished successfully");
    }

    /// <summary>
    /// Checks the process exit code and throws if it's non-zero.
    /// </summary>
    /// <param name="process">Process to validate its success status.</param>
    /// <exception cref="ShellCommandFailedException">Thrown if the process exits with a non-zero code.</exception>
    private void ValidateProcessSuccess(Process process)
    {
        if (process.ExitCode != 0)
            throw new ShellCommandFailedException(this, process.ExitCode);
    }
    
    /// <summary>
    /// Creates and configures the <see cref="Process"/> object.
    /// </summary>
    /// <param name="command">Shell command to run.</param>
    /// <returns>Process that once started will run given command.</returns>
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

    /// <summary>
    /// Hooks up the process's stdout/stderr streams to the logger.
    /// </summary>
    /// <param name="process">Process that will run <see cref="Command"/></param>
    /// <param name="scope">The shared execution scope.</param>
    private static void SetProcessRuntimeLogging(Process process, Scope scope)
    {
        process.OutputDataReceived += (_, e) => LogRuntimeEvent(e, scope, SHELL_LOG_FORMAT);
        process.ErrorDataReceived += (_, e) => LogRuntimeEvent(e, scope, SHELL_ERROR_FORMAT);
    }

    /// <summary>
    /// Logs a line from the process output.
    /// </summary>
    /// <param name="eventArgs">Arguments of event from a process' run.</param>
    /// <param name="scope">The shared execution scope.</param>
    /// <param name="format">Logging format.</param>
    private static void LogRuntimeEvent(DataReceivedEventArgs eventArgs, Scope scope, string format)
    {
        if (!string.IsNullOrEmpty(eventArgs.Data))
            scope.Logger!.Information(format, eventArgs.Data);
    }

    /// <summary>
    /// Starts the process and waits for it to exit.
    /// </summary>
    /// <param name="process">Process to run.</param>
    private static void RunProcess(Process process)
    {
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        process.WaitForExit();
    }
}