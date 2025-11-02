using YamlActionRunnerCli.ActionManagement.Actions;
using YamlActionRunnerCli.Exceptions.GeneralExceptions;
using YamlActionRunnerCli.Utils.ObjectManagement;

namespace YamlActionRunnerCli.ActionManagement;

/// <summary>
/// A class to build and populate an <see cref="IAction"/> instance from parameters.
/// </summary>
public class ActionBuilder
{
    /// <summary>
    /// Creates an instance of the specified action type and maps the parameters to its properties.
    /// </summary>
    /// <param name="actionObjectType">The <see cref="Type"/> of the action to create.</param>
    /// <param name="parameters">A dictionary of parameters (name of parameter to value).</param>
    /// <returns>A populated and validated <see cref="IAction"/> instance.</returns>
    /// <exception cref="InvalidConfigurationException">Thrown if mapping fails.</exception>
    public IAction BuildAction(Type actionObjectType, IDictionary<string, object> parameters)
    {
        if (parameters.ToObjectWithProperties(actionObjectType) is not IAction typedAction)
            throw new InvalidConfigurationException(actionObjectType, [$"Failed to map parameters for {actionObjectType.Name}"]);
    
        return typedAction;
    }
}