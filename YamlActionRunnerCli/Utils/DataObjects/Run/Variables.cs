using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using YamlActionRunnerCli.Exceptions.GeneralExceptions;

namespace YamlActionRunnerCli.Utils.DataObjects.Run;

/// <summary>
/// Manages a thread-safe collection of variables for an instruction's run.It also handles placeholder substitution in strings.
/// </summary>
public class Variables
{
    private const string VARIABLES_DEFAULT_REGEX = @"\$\{([^}]+)\}";
    private readonly Regex _variablesPattern;
    private readonly ConcurrentDictionary<string, object?> _variables = new();
    
    /// <summary>
    /// Indexer to reach variable's value by its name.
    /// </summary>
    /// <param name="name">The name of the variable.</param>
    /// <returns>The stored value.</returns>
    /// <exception cref="InvalidConfigurationException">Thrown if getting a variable that does not exist.</exception>
    public object? this[string name]
    {
        get
        {
            if (!_variables.TryGetValue(name, out var value))
                throw new InvalidConfigurationException(typeof(Variables), [$"No such variable: {name}"]);
            return value;
        }
        set => _variables[name] = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Variables"/> class.
    /// </summary>
    /// <param name="variablesRegexPattern">An optional custom regex pattern for variable placeholder.</param>
    public Variables(string? variablesRegexPattern=null)
    {
        _variablesPattern = new(variablesRegexPattern ?? VARIABLES_DEFAULT_REGEX, RegexOptions.Compiled);
    }
    
    /// <summary>
    /// Replaces variable placeholders in a string with their stored values.
    /// </summary>
    /// <param name="data">The input string containing placeholders.</param>
    /// <returns>A new string with variables substituted.</returns>
    public string PlaceVariablesInData(string data) => string.IsNullOrEmpty(data) ? data : ReplaceVariables(data);

    /// <summary>
    /// Performs the regex-based replacement.
    /// </summary>
    private string ReplaceVariables(string text) => _variablesPattern.Replace(text, ResolveVariable);
    
    /// <summary>
    /// Resolves a regex match to a variable's value.
    /// </summary>
    private string ResolveVariable(Match match)
    {
        var key = match.Groups[1].Value;

        if (_variables.TryGetValue(key, out var value))
            return value!.ToString() ?? string.Empty;

        // If variable not found, return the original placeholder
        return match.Value;
    }
}