using YamlActionRunnerCli.Exceptions.GeneralExceptions;
using YamlActionRunnerCli.InstructionsParsing.FileDataParsing;
using YamlActionRunnerCli.Utils.DataObjects.Instructions;
using YamlActionRunnerCli.Utils.ObjectManagement;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace YamlActionRunnerCli.InstructionsParsing.YamlInstructionsParsing;

/// <summary>
/// Parser for YAML files describing <see cref="Step"/>s workflow into an <see cref="Instructions"/> object.
/// </summary>
public class YamlInstructionsParser : YamlDataParser<Instructions>
{
    private static readonly string _stepsKey = nameof(Instructions.Steps).ToLower();
    private static readonly string _parametersKey = nameof(Step.Parameters).ToLower();

    /// <summary>
    /// Parses the given YAML file into an <see cref="Instructions"/> object.
    /// </summary>
    /// <param name="filePath">The path to the YAML file.</param>
    /// <returns>The parsed <see cref="Instructions"/> object described by the given file.</returns>
    /// <exception cref="InvalidYamlException">Thrown if the YAML is invalid or missing <see cref="Step"/>s.</exception>
    public override Instructions ParseFile(string filePath)
    {
        var root = GetRootNode(filePath);

        if (root is null || !TryParseSteps(root, out var steps))
            throw new InvalidYamlException("YAML content couldn't be parsed, no 'steps' found.");

        return new Instructions { Steps = steps };
    }
    
    /// <summary>
    /// Tries to parse a Node into a <see cref="Step"/>s sequence.
    /// </summary>
    /// <param name="node">Yaml node that might represent <see cref="Step"/>s.</param>
    /// <param name="steps">List of steps from given node (empty if none are found).</param>
    /// <returns>True if node contains steps, False otherwise</returns>
    private bool TryParseSteps(YamlNode node, out IList<Step> steps)
    {
        steps = [];

        if (!TryGetStepsNode(node, out var stepsNode))
            return false;

        steps = stepsNode!.Children.Select(NodeToStep).ToList();
        return steps.Count > 0;
    }

    /// <summary>
    /// Tries to find and return the 'steps' YAML sequence node.
    /// </summary>
    /// <param name="node">node to look for 'steps' node inside.</param>
    /// <param name="stepsSequenceNode">node that describes <see cref="Step"/>s, found inside the given node.</param>
    /// <returns>True if node contains 'step' node, False otherwise</returns>
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

    /// <summary>
    /// Converts a <see cref="YamlNode"/> into a <see cref="Step"/> object.
    /// </summary>
    /// <param name="stepNode">Node to convert into <see cref="Step"/>.</param>
    /// <returns>Step described by the given node.</returns>
    private Step NodeToStep(YamlNode stepNode)
    {
        var step = (Step)StepNodeToStepDictionary(stepNode).ToObjectWithProperties(typeof(Step));

        if (TryParseSteps(stepNode[new YamlScalarNode(_parametersKey)], out var nestedSteps))
            step.NestedSteps = nestedSteps;

        return step;
    }

    /// <summary>
    /// Converts a YAML 'step' node into a dictionary describing the step.
    /// </summary>
    /// <param name="stepNode">Node that described a <see cref="Step"/> object.</param>
    /// <returns>Dictionary describing the same <see cref="Step"/> object as the given node.</returns>
    /// <exception cref="InvalidYamlException">Thrown if the 'step' node doesn't describe s <see cref="Step"/>.</exception>
    private IDictionary<string, object> StepNodeToStepDictionary(YamlNode stepNode)
    {
        if (_deserializer.Deserialize(_serializer.Serialize(stepNode)) is not IDictionary<object, object> stepProperties)
            throw new InvalidYamlException("Step structure is invalid");

        return (Dictionary<string, object>)FixMappingDeserialization(stepProperties)!;
    }
}