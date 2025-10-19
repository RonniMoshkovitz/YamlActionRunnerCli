namespace YamlActionRunnerCli.Exceptions.ActionExceptions;

public class UnsupportedHttpMethod(string method) : ActionException("Unsupported HTTP method: {0}", method)
{
}