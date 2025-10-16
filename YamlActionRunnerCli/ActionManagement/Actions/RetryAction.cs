using System.ComponentModel.DataAnnotations;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

public class RetryAction : NestedAction
{
    [Required]
    public int? Times { get; set; }
    
    public override void Run(Scope scope)
    {
        foreach (var action in Actions)
        {
            for (int i = 0; i < Times; i++)
            {
                try
                {
                    action.Run(scope);
                    break;
                }
                catch (Exception)
                {
                    if (i == Times - 1) throw;
                }
            }
        }
    }
}