namespace YamlActionRunnerCli.Exceptions.ActionExceptions;

public abstract class ActionException(string messageFormat, params object[] data)
    : Exception(GetMessage(messageFormat, data))
{
    private static string GetMessage(string messageFormat, params object[] data)
    {
        return string.Format(messageFormat, data);
    }
}