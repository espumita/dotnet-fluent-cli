using Fluent.Cli.Configuration;

namespace Fluent.Cli;

public class CliArgumentsCommandsBuilder : CliArgumentsBuilder {
    private readonly CommandConfiguration _commandConfiguration;

    private CliArgumentsCommandsBuilder(string[] environmentArgs,
        IDictionary<string, OptionConfiguration> optionConfigurations,
        IDictionary<string, CommandConfiguration> commandConfigurations, CommandConfiguration commandConfiguration) : base(environmentArgs, commandConfigurations, optionConfigurations) {
        _commandConfiguration = commandConfiguration;
    }

    public static CliArgumentsCommandsBuilder With(string[] environmentArgs, IDictionary<string, OptionConfiguration> optionConfigurations, IDictionary<string, CommandConfiguration> commandConfigurations, CommandConfiguration commandConfiguration) {
        return new CliArgumentsCommandsBuilder(environmentArgs, optionConfigurations, commandConfigurations, commandConfiguration);
    }

}