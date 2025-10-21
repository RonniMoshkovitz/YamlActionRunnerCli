using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace YamlActionRunnerCli.Utils.DataObjects.Run;

public class Variables
{
    private readonly Regex _variablesPattern;
    private readonly ConcurrentDictionary<string, object?> _variables = new();

    public object? this[string name]
    {
        get
        {
            if (!_variables.TryGetValue(name, out var value))
                throw new InvalidOperationException($"No such variable: {name}");
            return value;
        }
        set => _variables[name] = value;
    }

    public Variables(string variablesRegexPattern)
    {
        _variablesPattern = new(variablesRegexPattern, RegexOptions.Compiled);
    }
    
    public string PlaceVariablesInData(string data) => string.IsNullOrEmpty(data) ? data : ReplaceVariables(data);

    private string ReplaceVariables(string text) => _variablesPattern.Replace(text, ResolveVariable);

    private string ResolveVariable(Match match)
    {
        var key = match.Groups[1].Value;

        if (_variables.TryGetValue(key, out var value))
            return value!.ToString() ?? string.Empty;

        return match.Value;
    }
}