using System.Reflection;
using YamlActionRunnerCli.InstructionsParsing.FileDataParsing;
using YamlActionRunnerCli.Utils.DataObjects.Instructions;
using YamlActionRunnerCli.Utils.ObjectManagement;
using YamlDotNet.RepresentationModel;

namespace YamlActionRunnerCli.InstructionsParsing.YamlInstructionsParsing;

public class YamlInstructionsParser : YamlDataParser<Instructions>
{
    private static readonly string _stepsKey = nameof(Instructions.Steps).ToLower();
    private static readonly string _parametersKey = nameof(Step.Parameters).ToLower();

    public override Instructions ParseFile(string filePath)
    {
        var root = GetRootNode(filePath);

        if (root is null || !TryParseSteps(root, out var steps))
            throw new InvalidDataException("YAML content couldn't be parsed, no 'steps' found.");
        
        return new Instructions{Steps = steps};
    }

    private static YamlMappingNode? GetRootNode(string filePath)
    {
        var yamlStream = new YamlStream();
        yamlStream.Load(new StringReader(File.ReadAllText(filePath)));
        
        return (YamlMappingNode?)yamlStream.Documents.FirstOrDefault()?.RootNode;
    }


    private bool TryParseSteps(YamlNode node, out IList<Step> steps)
    {
        steps = new List<Step>();
        
        if (!TryGetStepsNode(node, out var stepsNode))
            return false;
        
        steps = stepsNode!.Children.Select(NodeToStep).ToList();

        return steps.Count > 0;
    }

    private bool TryGetStepsNode(YamlNode node, out YamlSequenceNode? stepsSequenceNode)
    {
        stepsSequenceNode = null;

        if (node is not YamlMappingNode mapping ||
            !mapping.Children.TryGetValue(new YamlScalarNode(_stepsKey), out var stepsNode) ||
            stepsNode is not YamlSequenceNode stepsSequence)
            return false;

        stepsSequenceNode = stepsSequence;
        return true;
    }

    private Step NodeToStep(YamlNode stepNode)
    {
        var step = _deserializer.Deserialize<Step>(_serializer.Serialize(stepNode));
        step.ValidateMembers();

        if (TryParseSteps(stepNode[new YamlScalarNode(_parametersKey)], out var nestedSteps))
            step.NestedSteps = nestedSteps;

        return step;
    }
}