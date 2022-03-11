using Fluent.Cli.Configuration;

namespace Fluent.Cli;

public class CliArgumentsOptionsBuilder : CliArgumentsBuilder {
    private readonly OptionConfiguration _optionConfiguration;

    private CliArgumentsOptionsBuilder(string[] environmentArgs,
        IDictionary<string, OptionConfiguration> optionConfigurations,
        IDictionary<string, CommandConfiguration> commandConfigurations, OptionConfiguration optionConfiguration, ProgramDescriptionsConfiguration programDescriptionsConfiguration) : base(environmentArgs, commandConfigurations, optionConfigurations, programDescriptionsConfiguration) {
        _optionConfiguration = optionConfiguration;
    }

    public static CliArgumentsOptionsBuilder With(string[] environmentArgs, IDictionary<string, OptionConfiguration> optionConfigurations, IDictionary<string, CommandConfiguration> commandConfigurations, OptionConfiguration optionConfiguration, ProgramDescriptionsConfiguration programDescriptionsConfiguration) {
        return new CliArgumentsOptionsBuilder(environmentArgs, optionConfigurations, commandConfigurations, optionConfiguration, programDescriptionsConfiguration);
    }

    public CliArgumentsOptionsBuilder WithDescription(string description) {
        //TODO
        //if (string.IsNullOrEmpty(description)) throw new ArgumentException("Description cannot be null or empty");
        _optionConfiguration.AddDescription(description);
        return this;
    }

    public CliArgumentsBuilder WithArgument(string argumentName) {
        if (string.IsNullOrEmpty(argumentName)) throw new ArgumentException("Argument name cannot be null or empty");
        _optionConfiguration.AddArgument(argumentName);
        return this;
    }
}