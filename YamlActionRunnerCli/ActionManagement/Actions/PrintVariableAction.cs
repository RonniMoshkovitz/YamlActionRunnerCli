using YamlActionRunnerCli.Utils.DataObjects.Run;

namespace YamlActionRunnerCli.ActionManagement.Actions;

public class PrintVariableAction : VariableAction
{
    public override void Run(Scope scope)
    {
        var logAction = new LogAction {Message = GetVariableValueFromScope(scope)?.ToString()};
        logAction.Run(scope);
    }
}