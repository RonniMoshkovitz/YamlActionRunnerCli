using System.ComponentModel.DataAnnotations;
using YamlActionRunnerCli.Exceptions.ActionExceptions;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

/// <summary>
/// A nested action that retries its child actions upon failure.
/// </summary>
public class RetryAction : NestedAction
{
    /// <summary>
    /// Total number of attempts.
    /// </summary>
    [Required, Range(1, int.MaxValue)]
    public int? Times { get; set; }

    /// <inheritdoc/>
    public override void Run(Scope scope)
    {
        scope.Logger!.Verbose("Trying to run {count} actions: {@actions}", Actions!.Count,
            Actions!.Select(action => action.GetType().Name));

        Actions!.ToList().ForEach(action => RunActionWithRetries(action, scope));
    }

    /// <summary>
    /// Tries to run a single action up to <see cref="Times"/>.
    /// </summary>
    /// <param name="action">Action to run</param>
    /// <param name="scope">The shared execution scope.</param>
    /// <exception cref="RetryFailed">Thrown if the action fails all attempts.</exception>
    private void RunActionWithRetries(IAction action, Scope scope)
    {
        ActionFailedException? failReasonException = null;

        for (int i = 1; i <= Times; i++)
        {
            if (TryRunAction(action, scope, out failReasonException))
            {
                scope.Logger!.Verbose("{actions} complete on attempt {attempt}", action.GetType().Name, i);
                return;
            }

            scope.Logger!.Verbose("{actions} failed on attempt {attempt}", action.GetType().Name, i);
        }

        if (failReasonException is not null)
            throw new RetryFailed(this, Times!.Value, failReasonException);
    }

    /// <summary>
    /// Attempts to run an action once, catching any <see cref="ActionFailedException"/>.
    /// </summary>
    /// <param name="action">Action to run</param>
    /// <param name="scope">The shared execution scope.</param>
    /// <param name="failReasonException">The exception thrown while trying to run action, or null.</param>
    /// <returns>True if successful, False if an exception was caught.</returns>
    private static bool TryRunAction(IAction action, Scope scope, out ActionFailedException? failReasonException)
    {
        failReasonException = null;

        try
        {
            action.Run(scope);
        }
        catch (ActionFailedException actionFailedException)
        {
            failReasonException = actionFailedException;
            return false;
        }

        return true;
    }
}