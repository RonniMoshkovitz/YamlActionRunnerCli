using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using YamlActionRunnerCli.Exceptions.ActionExceptions;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

public class AssertAction: IAction
{ 
    [Required]
    public string? Condition { get; set; }

    public void Run(Scope scope)
    {
        try
        {
            var result = CSharpScript.EvaluateAsync<bool>(Condition, ScriptOptions.Default).Result;
            if (!result)
                throw new FailedAssertionException(Condition!);
        }
        catch (CompilationErrorException)
        {
            throw new InvalidConditionException(Condition!);
        }    
    }
}