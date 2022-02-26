namespace Fluent.Cli;

public class OptionsDefinitions {
    public IDictionary<string, OptionsDefinition> Definitions { get; set; }

    public bool IsOptionDefined(string optionName) {
        return optionName.Length == 1
            ? Definitions.ContainsKey(optionName)
            : Definitions.Values.Any(definition => optionName.Equals(definition.SecondaryName));
    }

    public bool IsArgumentOptionDefined(string optionName) {
        var optionsDefinition = optionName.Length == 1
                ? Definitions[optionName]
                : Definitions.Values.First(definition => optionName.Equals(definition.SecondaryName));
        return optionsDefinition.HasArgument;
    }
}