using YamlActionRunnerCli.Cli;

namespace YamlActionRunnerCli;

/// <summary>
/// The main entry point for the console application.
/// </summary>
public static class Program
{
    /// <summary>
    /// The main function of the application. Runs the CLI and returns the run's exit code.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    /// <returns>An exit code (0 for success, non-zero for errors).</returns>
    static int Main(string[] args)
    {
        return new RunnerCli().Start(args);
    }
}