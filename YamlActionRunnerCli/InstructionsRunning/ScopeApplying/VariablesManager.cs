using System.Text.RegularExpressions;

namespace YamlActionRunnerCli.InstructionsRunning.ScopeApplying;

public class VariablesManager(string variablesRegexPattern)
{
    private readonly Regex _variablesPattern = new(variablesRegexPattern, RegexOptions.Compiled);
    
    public string PlaceVariablesInData(string data, IDictionary<string, object> variables)
    {
        return string.IsNullOrEmpty(data) ? data : ReplaceVariables(data, variables);
    }

    private string ReplaceVariables(string text, IDictionary<string, object> variables)
    {
        return _variablesPattern.Replace(text, match => ResolveVariable(match, variables));
    }

    private static string ResolveVariable(Match match, IDictionary<string, object> variables)
    {
        var key = match.Groups[1].Value;

        if (variables.TryGetValue(key, out var val))
            return val.ToString() ?? string.Empty;
        
        return match.Value;
    }
}