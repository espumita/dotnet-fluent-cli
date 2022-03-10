using Fluent.Cli.Configuration;

namespace Fluent.Cli;

public class CliArgumentsOptionsBuilder : CliArgumentsBuilder {
    private readonly OptionConfiguration _optionConfiguration;

    private CliArgumentsOptionsBuilder(string[] environmentArgs,
        IDictionary<string, OptionConfiguration> optionConfigurations,
        IDictionary<string, CommandConfiguration> commandConfigurations, OptionConfiguration optionConfiguration) : base(environmentArgs, commandConfigurations, optionConfigurations) {
        _optionConfiguration = optionConfiguration;
    }

    public static CliArgumentsOptionsBuilder With(string[] environmentArgs, IDictionary<string, OptionConfiguration> optionConfigurations, IDictionary<string, CommandConfiguration> commandConfigurations, OptionConfiguration optionConfiguration) {
        return new CliArgumentsOptionsBuilder(environmentArgs, optionConfigurations, commandConfigurations, optionConfiguration);
    }

    public CliArgumentsBuilder WithArgument(string argumentName) {
        if (string.IsNullOrEmpty(argumentName)) throw new ArgumentException("Argument name cannot be null or empty");
        _optionConfiguration.AddArgument(argumentName);
        return this;
    }
}