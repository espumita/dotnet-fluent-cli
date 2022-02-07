namespace Fluent.Cli;

public class OptionConfiguration {
    public string ShortName { get; }

    public OptionConfiguration(string shortName) {
        ShortName = shortName;
    }

}