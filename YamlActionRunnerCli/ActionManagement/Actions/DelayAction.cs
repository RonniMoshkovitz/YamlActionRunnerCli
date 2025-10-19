using System.ComponentModel.DataAnnotations;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

public class DelayAction : IAction
{
    [Required, Range(0, int.MaxValue)]
    public int? Duration { get; set; }

    public void Run(Scope scope)
    {
        Thread.Sleep(Duration!.Value);
    }
}