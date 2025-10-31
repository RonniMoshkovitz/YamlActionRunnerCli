using YamlActionRunnerCli.Exceptions.GeneralExceptions;
using YamlActionRunnerCli.InstructionsParsing.FileDataParsing;
using YamlActionRunnerCli.Utils.DataObjects.Instructions;
using YamlActionRunnerCli.Utils.ObjectManagement;
using YamlDotNet.Core;
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
            throw new InvalidYamlException("YAML content couldn't be parsed, no 'steps' found.");

        return new Instructions { Steps = steps };
    }

    private static YamlMappingNode? GetRootNode(string filePath)
    {
        var yamlStream = new YamlStream();
        try
        {
            yamlStream.Load(new StringReader(File.ReadAllText(filePath)));
        }
        catch (Exception exception) when (exception is SemanticErrorException or YamlException)
        {
            throw new InvalidYamlException(exception.Message);
        }

        return (YamlMappingNode?)yamlStream.Documents.FirstOrDefault()?.RootNode;
    }


    private bool TryParseSteps(YamlNode node, out IList<Step> steps)
    {
        steps = [];

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
        var step = (Step)StepNodeToStepDictionary(stepNode).ToObjectWithProperties(typeof(Step));

        if (TryParseSteps(stepNode[new YamlScalarNode(_parametersKey)], out var nestedSteps))
            step.NestedSteps = nestedSteps;

        return step;
    }

    private IDictionary<string, object> StepNodeToStepDictionary(YamlNode stepNode)
    {
        if (_deserializer.Deserialize(_serializer.Serialize(stepNode)) is not IDictionary<object, object>
            stepProperties)
            throw new InvalidYamlException("Step structure is invalid");

        return (Dictionary<string, object>)FixMappingDeserialization(stepProperties)!;
    }

    private object? FixMappingDeserialization(object? deserialized)
    {
        if (deserialized is not IDictionary<object, object> mapping)
            return deserialized;

        return mapping.ToDictionary(
            entry => entry.Key.ToString()!,
            entry => FixMappingDeserialization(entry.Value));
    }
}