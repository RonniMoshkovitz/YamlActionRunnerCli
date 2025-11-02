using YamlActionRunnerCli.Cli;

namespace YamlActionRunnerCli;

public static class Program
{
    static int Main(string[] args)
    {
        return new RunnerCli().Start(args);
    }
}