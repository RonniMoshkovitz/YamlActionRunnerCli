using System.ComponentModel.DataAnnotations;
using System.Text;
using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

public class HttpAction: IAction
{
    private const string MEDIA_TYPE = "application/json";
    private const string USER_AGENT = "YamlRunner/1.0";
    private readonly Dictionary<string, Func<HttpClient, Task<HttpResponseMessage>>> _requestMethods;
    
    [Required]
    public string? Method {get; set;}
    [Required]
    public string? Url {get; set;}
    public string Body {get; set;} = string.Empty;
    public string UserAgent { get; set; } = USER_AGENT;

    public HttpAction()
    {
        _requestMethods = new()
        {
            ["GET"] = Get,
            ["POST"] = Post
        };
    }

    public void Run(Scope scope) => ExecuteAsync(scope).GetAwaiter().GetResult();

    private async Task ExecuteAsync(Scope scope)
    {
        using var client = new HttpClient();
        
        client.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);

        var response = GetRequestMethod()(client).GetAwaiter().GetResult();
        EnsureSuccess(response);
        
        var logAction = new LogAction {Message = await response.Content.ReadAsStringAsync()};
        logAction.Run(scope);
    }

    private static void EnsureSuccess(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Request failed ({(int)response.StatusCode} {response.ReasonPhrase})");
        }
    }

    private Func<HttpClient, Task<HttpResponseMessage>> GetRequestMethod()
    {
        var methodUpper = Method!.Trim().ToUpperInvariant();
        if (!_requestMethods.TryGetValue(methodUpper, out var requestMethod))
            throw new InvalidOperationException($"Unsupported HTTP method: {Method}");
        return requestMethod;
    }

    private async Task<HttpResponseMessage> Get(HttpClient client)
    {
        return await client.GetAsync(Url);
    }
    
    private async Task<HttpResponseMessage> Post(HttpClient client)
    {
        var content = new StringContent(Body, Encoding.UTF8, MEDIA_TYPE);
        return await client.PostAsync(Url, content);
    }
}