namespace Fluent.Cli;

public class OptionsDefinitions {
    public IDictionary<string, OptionsDefinition> Definitions { get; set; }

    public bool IsOptionDefined(string optionName) {
        return optionName.Length == 1
            ? Definitions.ContainsKey(optionName)
            : Definitions.Values.Any(x => optionName.Equals(x.SecondaryName));
    }

    //TODO IsArgumentDefined
}