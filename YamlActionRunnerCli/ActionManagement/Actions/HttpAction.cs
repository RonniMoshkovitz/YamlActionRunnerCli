using System.ComponentModel.DataAnnotations;
using System.Text;
using YamlActionRunnerCli.ActionManagement.Actions.Utils;
using YamlActionRunnerCli.Exceptions.ActionExceptions;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

/// <summary>
/// Action to perform an HTTP request (Get or Post).
/// </summary>
public class HttpAction : IAction
{
    private const string MEDIA_TYPE = "application/json";
    private static readonly Encoding _encoding = Encoding.UTF8;

    private readonly Dictionary<HttpMethodType, Func<HttpRequestMessage>> _requestsGetters;

    /// <summary>
    /// HTTP method (Get or Post).
    /// </summary>
    [Required]
    public HttpMethodType? Method { get; set; }
    
    /// <summary>
    /// Target URL.
    /// </summary>
    [Required, Url] 
    public string? Url { get; set; }
    
    /// <summary>
    /// Request body.
    /// </summary>
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpAction"/> class.
    /// </summary>
    public HttpAction()
    {
        _requestsGetters = new()
        {
            [HttpMethodType.Get] = GetGetRequest,
            [HttpMethodType.Post] = GetPostRequest
        };
    }

    /// <inheritdoc/>
    /// <exception cref="FailedHttpRequest">Thrown if the request fails or returns a non-success status.</exception>
    public void Run(Scope scope)
    {
        var client = new HttpClient();
        var request = _requestsGetters[Method!.Value]();
        
        scope.Logger!.Verbose("Sending {method:G} HTTP Request to {url})", Method, request.RequestUri);
        var response = SendRequestAndGetResponse(request, client);
        
        EnsureSuccess(response);
        scope.Logger.Verbose("Received HTTP Response: {response}", response.Content.ReadAsStringAsync().Result);
    }

    /// <summary>
    /// Sends the request and handles network exceptions.
    /// </summary>
    /// <param name="request">HTTP request to be sent.</param>
    /// <param name="client">The client to send the request.</param>
    /// <returns>Result of the HTTP request (response).</returns>
    /// <exception cref="FailedHttpRequest">Thrown if the request fails.</exception>
    private HttpResponseMessage SendRequestAndGetResponse(HttpRequestMessage request, HttpClient client)
    {
        try
        {
            return client.SendAsync(request).GetAwaiter().GetResult();
        }
        catch (HttpRequestException httpRequestException)
        {
            throw new FailedHttpRequest(this, httpRequestException.Message);
        }
    }

    /// <summary>
    /// Checks if the returned status code is successful. 
    /// </summary>
    /// <param name="response">HTTP response.</param>
    /// <exception cref="FailedHttpRequest">Thrown if the response has a non-success status.</exception>
    private void EnsureSuccess(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
            throw new FailedHttpRequest(this, response.ReasonPhrase ?? "", (int)response.StatusCode);
    }

    /// <summary>
    /// Builds an <see cref="HttpRequestMessage"/> for a GET request.
    /// </summary>
    /// <returns>GET HTTP request</returns>
    private HttpRequestMessage GetGetRequest()
    {
        return new HttpRequestMessage(new HttpMethod(Method!.Value.ToString()), Url);
    }

    /// <summary>
    /// Builds an <see cref="HttpRequestMessage"/> for a POST request.
    /// </summary>
    /// <returns>POST HTTP request</returns>
    private HttpRequestMessage GetPostRequest()
    {
        var request = new HttpRequestMessage(new HttpMethod(Method!.Value.ToString()), Url);
        request.Content = new StringContent(Body, _encoding, MEDIA_TYPE);

        return request;
    }
}