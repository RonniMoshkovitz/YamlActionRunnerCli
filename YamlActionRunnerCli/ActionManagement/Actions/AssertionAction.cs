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
            scope.Logger!.Verbose("Evaluating condition '{Condition}'", Condition);
            if (!IsConditionTrue())
                throw new FailedAssertionException(this, Condition!);
            scope.Logger.Verbose("Condition '{Condition}' is true", Condition);
        }
        catch (CompilationErrorException)
        {
            throw new InvalidConditionException(this, Condition!);
        }    
    }

    private bool IsConditionTrue() => CSharpScript.EvaluateAsync<bool>(Condition, ScriptOptions.Default).Result;
}