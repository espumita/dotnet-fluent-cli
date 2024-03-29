﻿using Fluent.Cli.Configuration;
using Fluent.Cli.Containers;

namespace Fluent.Cli;

public class CliArgumentsBuilder {
    private readonly string[] environmentArgs;
    private readonly IDictionary<string, OptionConfiguration> optionConfigurations;
    private readonly IDictionary<string, CommandConfiguration> commandConfigurations;
    private readonly ProgramDescriptionsConfiguration programDescriptionsConfiguration;
    protected CliArgumentsBuilder(string[] environmentArgs) {
        this.environmentArgs = environmentArgs;
        optionConfigurations = new Dictionary<string, OptionConfiguration>();
        commandConfigurations = new Dictionary<string, CommandConfiguration>();
        programDescriptionsConfiguration = new ProgramDescriptionsConfiguration();
    }

    protected CliArgumentsBuilder(string[] environmentArgs, IDictionary<string, CommandConfiguration> commandConfigurations, IDictionary<string, OptionConfiguration> optionConfigurations, ProgramDescriptionsConfiguration programDescriptionsConfiguration) {
        this.environmentArgs = environmentArgs;
        this.commandConfigurations = commandConfigurations;
        this.optionConfigurations = optionConfigurations;
        this.programDescriptionsConfiguration = programDescriptionsConfiguration;
    }

    public static CliArgumentsBuilder With(string[] args) {
        if (args == null) throw new ArgumentException("args cannot be null");
        return new CliArgumentsBuilder(args);
    }

    public CliArgumentsBuilder WithProgramHeaderDescription(string description) {
        programDescriptionsConfiguration.AddHeaderDescription(description);
        return this;
    }

    public CliArgumentsBuilder WithProgramFooterDescription(string description) {
        programDescriptionsConfiguration.AddFooterDescription(description);
        return this;
    }

    public CliArgumentsCommandsBuilder Command(string name) {
        var commandConfiguration = CommandConfiguration.For(name);
        commandConfigurations[name] = commandConfiguration;
        return CliArgumentsCommandsBuilderFromBaseBuilder(commandConfiguration);
    }

    public CliArgumentsOptionsBuilder Option(char shortName) {
        var optionConfiguration = OptionConfiguration.For(shortName);
        optionConfigurations[shortName.ToString()] = optionConfiguration;
        return CliArgumentsOptionsBuilderFromBaseBuilder(optionConfiguration);
    }

    public CliArgumentsOptionsBuilder Option(char shortName, string longName) {
        if (string.IsNullOrEmpty(longName)) throw new ArgumentException("Option long name cannot be null, use other method instead");
        var optionConfiguration = OptionConfiguration.For(shortName, longName);
        optionConfigurations[shortName.ToString()] = optionConfiguration;
        return CliArgumentsOptionsBuilderFromBaseBuilder(optionConfiguration);
    }

    public CliArgumentsOptionsBuilder LongOption(string longName) {
        if (string.IsNullOrEmpty(longName)) throw new ArgumentException("Option long name cannot be null or empty, use other method instead");
        var optionConfiguration = OptionConfiguration.ForLong(longName);
        optionConfigurations[longName] = optionConfiguration;
        return CliArgumentsOptionsBuilderFromBaseBuilder(optionConfiguration);
    }

    public CliArguments Build() {
        return new ParserExecutionContainer(environmentArgs, commandConfigurations, optionConfigurations, programDescriptionsConfiguration)
            .Run();
    }

    private CliArgumentsCommandsBuilder CliArgumentsCommandsBuilderFromBaseBuilder(CommandConfiguration commandConfiguration) {
        return CliArgumentsCommandsBuilder.With(environmentArgs, optionConfigurations, commandConfigurations, commandConfiguration, programDescriptionsConfiguration);
    }

    private CliArgumentsOptionsBuilder CliArgumentsOptionsBuilderFromBaseBuilder(OptionConfiguration optionConfiguration) {
        return CliArgumentsOptionsBuilder.With(environmentArgs, optionConfigurations, commandConfigurations, optionConfiguration, programDescriptionsConfiguration);
    }
}