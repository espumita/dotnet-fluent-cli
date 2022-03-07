using Fluent.Cli.Exceptions;

namespace Fluent.Cli;

public class CliArgumentsOptionsBuilder : CliArgumentsBuilder {
    private CliArgumentsOptionsBuilder(string[] environmentArgs, IDictionary<string, OptionConfiguration> optionConfigurations, IDictionary<string, CommandConfiguration> commandConfigurations, string buildingOptionConfiguration) : base(environmentArgs, optionConfigurations, commandConfigurations, buildingOptionConfiguration) { }

    public static CliArgumentsOptionsBuilder With(string[] environmentArgs, IDictionary<string, OptionConfiguration> optionConfigurations, IDictionary<string, CommandConfiguration> commandConfigurations, string buildingOptionConfiguration) {
        return new CliArgumentsOptionsBuilder(environmentArgs, optionConfigurations, commandConfigurations, buildingOptionConfiguration );
    }

    public CliArgumentsBuilder WithOptionArgument(string argumentName) {
        if (string.IsNullOrEmpty(argumentName)) throw new ArgumentException("Argument name cannot be null or empty");
        if (string.IsNullOrEmpty(buildingOptionConfiguration)) throw new ArgumentException($"Argument '{argumentName}' could not be configured, you need to configure an Option first.");
        var option = optionConfigurations[buildingOptionConfiguration];
        if (option.IsArgumentConfigured()) throw new OptionWithMultipleArgumentsAreNotSupportedException($"Option -- '{buildingOptionConfiguration}' can only be configured with a single argument. If you need multiple arguments, consider use a command instead.");
        option.AddArgument(argumentName);
        return this;
    }
}