namespace Fluent.Cli.Configuration;

public class ArgumentConfiguration {
    public string ArgumentName { get; }

    public ArgumentConfiguration(string argumentName) {
        ArgumentName = argumentName;
    }
}