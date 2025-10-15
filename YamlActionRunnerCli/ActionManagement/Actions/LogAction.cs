using System.ComponentModel.DataAnnotations;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

public class LogAction : IAction
{ 
    [Required]
    public string? Message { get; set; }

    public void Run(Scope scope)
    {
        Console.WriteLine(Message);
        //scope.Logger.LogInformation(Message); --Nicer 
    }
}