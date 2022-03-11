using Fluent.Cli.Configuration;

namespace Fluent.Cli;

public class CliArgumentsCommandsBuilder : CliArgumentsBuilder {
    private readonly CommandConfiguration _commandConfiguration;

    private CliArgumentsCommandsBuilder(string[] environmentArgs,
        IDictionary<string, OptionConfiguration> optionConfigurations,
        IDictionary<string, CommandConfiguration> commandConfigurations, CommandConfiguration commandConfiguration, ProgramDescriptionsConfiguration programDescriptionsConfiguration) : base(environmentArgs, commandConfigurations, optionConfigurations, programDescriptionsConfiguration) {
        _commandConfiguration = commandConfiguration;
    }

    public static CliArgumentsCommandsBuilder With(string[] environmentArgs, IDictionary<string, OptionConfiguration> optionConfigurations, IDictionary<string, CommandConfiguration> commandConfigurations, CommandConfiguration commandConfiguration, ProgramDescriptionsConfiguration programDescriptionsConfiguration) {
        return new CliArgumentsCommandsBuilder(environmentArgs, optionConfigurations, commandConfigurations, commandConfiguration, programDescriptionsConfiguration);
    }

}