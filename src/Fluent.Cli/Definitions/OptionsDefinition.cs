namespace Fluent.Cli.Definitions;

public class OptionsDefinition {
    public string SecondaryName { get; set; }
    public bool HasArgument { get; set; }

    public OptionsDefinition(string secondaryName, bool hasArgument) {
        SecondaryName = secondaryName;
        HasArgument = hasArgument;
    }
}