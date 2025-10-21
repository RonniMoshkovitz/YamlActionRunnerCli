using System.ComponentModel.DataAnnotations;
using System.Text;
using YamlActionRunnerCli.ActionManagement.Actions.Utils;
using YamlActionRunnerCli.Exceptions.ActionExceptions;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

public class HttpAction : IAction
{
    private const string MEDIA_TYPE = "application/json";
    private static readonly Encoding _encoding = Encoding.UTF8;

    private readonly Dictionary<HttpMethodType, Func<HttpRequestMessage>> _requestsGetters;

    [Required]
    public HttpMethodType? Method { get; set; }
    [Required, Url] 
    public string? Url { get; set; }
    public string Body { get; set; } = string.Empty;

    public HttpAction()
    {
        _requestsGetters = new()
        {
            [HttpMethodType.Get] = GetGetRequest,
            [HttpMethodType.Post] = GetPostRequest
        };
    }

    public void Run(Scope scope)
    {
        var client = new HttpClient();
        var request = _requestsGetters[Method!.Value]();
        
        scope.Logger.Verbose("Sending {method:G} HTTP Request to {url})", Method, request.RequestUri);
        var response = SendRequestAndGetResponse(request, client);
        
        EnsureSuccess(response);
        scope.Logger.Verbose("Received HTTP Response: {response}", response.Content.ReadAsStringAsync().Result);
    }

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

    private void EnsureSuccess(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
            throw new FailedHttpRequest(this, response.ReasonPhrase ?? "", (int)response.StatusCode);
    }

    private HttpRequestMessage GetGetRequest()
    {
        return new HttpRequestMessage(new HttpMethod(Method!.Value.ToString()), Url);
    }

    private HttpRequestMessage GetPostRequest()
    {
        var request = new HttpRequestMessage(new HttpMethod(Method!.Value.ToString()), Url);
        request.Content = new StringContent(Body, _encoding, MEDIA_TYPE);

        return request;
    }
}