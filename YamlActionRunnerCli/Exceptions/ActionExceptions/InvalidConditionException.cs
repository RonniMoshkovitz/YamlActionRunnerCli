namespace YamlActionRunnerCli.Exceptions.ActionExceptions;

public class InvalidConditionException(string condition) : ActionException("Invalid condition '{0}'",  condition)
{
    
}
