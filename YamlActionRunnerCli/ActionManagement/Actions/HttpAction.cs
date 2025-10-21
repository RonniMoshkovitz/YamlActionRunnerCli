using System.ComponentModel.DataAnnotations;
using System.Text;
using YamlActionRunnerCli.Exceptions.ActionExceptions;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

public class HttpAction : IAction
{
    private const string MEDIA_TYPE = "application/json";
    private static readonly Encoding _encoding = Encoding.UTF8;

    private readonly Dictionary<string, Func<HttpClient, Task<HttpResponseMessage>>> _requestMethods;

    [Required]
    public string? Method { get; set; }
    [Required, Url]
    public string? Url { get; set; }
    public string Body { get; set; } = string.Empty;

    public HttpAction()
    {
        _requestMethods = new()
        {
            [nameof(Get).ToUpper()] = Get,
            [nameof(Post).ToUpper()] = Post
        };
    }

    public void Run(Scope scope)
    {
        using var client = new HttpClient();
        var method = GetRequestMethod();
        HttpResponseMessage response;
        
        try
        {
            response = GetRequestMethod()(client).GetAwaiter().GetResult();
        }
        catch (HttpRequestException httpRequestException)
        {
            throw new FailedHttpRequest(this, httpRequestException.Message);
        }

        scope.Logger.Verbose("Sent {method} HTTP Request to {url})",
            response.RequestMessage?.Method.Method, response.RequestMessage?.RequestUri);

        EnsureSuccess(response);
        scope.Logger.Verbose("Got HTTP Response: {response}", response.Content.ReadAsStringAsync().Result);
    }

    private void EnsureSuccess(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
            throw new FailedHttpRequest(this, response.ReasonPhrase ?? "", (int)response.StatusCode);
    }

    private Func<HttpClient, Task<HttpResponseMessage>> GetRequestMethod()
    {
        var methodUpper = Method!.Trim().ToUpper();
        if (!_requestMethods.TryGetValue(methodUpper, out var requestMethod))
            throw new UnsupportedHttpMethod(this, Method);

        return requestMethod;
    }

    private async Task<HttpResponseMessage> Get(HttpClient client)
    {
        return await client.GetAsync(Url);
    }

    private async Task<HttpResponseMessage> Post(HttpClient client)
    {
        var content = new StringContent(Body, _encoding, MEDIA_TYPE);
        return await client.PostAsync(Url, content);
    }
}