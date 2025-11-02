using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using YamlActionRunnerCli.Exceptions.ActionExceptions;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

/// <summary>
/// Action to evaluate a C# condition string. Fails the instructions run workflow if false.
/// </summary>
public class AssertAction: IAction
{ 
    /// <summary>
    /// C# condition to evaluate.
    /// </summary>
    [Required]
    public string? Condition { get; set; }
    
    /// <inheritdoc/>
    /// <exception cref="FailedAssertionException">Thrown if the condition is false.</exception>
    /// <exception cref="InvalidConditionException">Thrown if the C# condition is invalid (syntax).</exception>
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

    /// <summary>
    /// Evaluates whether the <see cref="Condition"/> is true or false
    /// </summary>
    /// <returns>True for true condition, False otherwise.</returns>
    private bool IsConditionTrue() => CSharpScript.EvaluateAsync<bool>(Condition, ScriptOptions.Default).Result;
}