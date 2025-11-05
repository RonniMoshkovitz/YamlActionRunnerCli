using YamlActionRunnerCli.ActionManagement.Actions;

namespace YamlActionRunnerCli.Exceptions.ActionExceptions;

/// <summary>
/// Exception thrown by <see cref="HttpAction"/> when the request fails or returns a non-success status.
/// </summary>
public class FailedHttpRequest : ActionFailedException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FailedHttpRequest"/> class.
    /// </summary>
    /// <param name="action">The HTTP action instance.</param>
    /// <param name="reason">The reason for failure.</param>
    /// <param name="statusCode">The HTTP status code, if available.</param>
    public FailedHttpRequest(IAction action, string reason, int? statusCode=null) 
        : base(action, $"HTTP Request failed: {reason} {statusCode.ToString()}")
    {
    }
}