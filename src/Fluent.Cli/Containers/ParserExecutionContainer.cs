using System.Reflection;
using Fluent.Cli.Configuration;
using Fluent.Cli.ConsolePrinters;
using Fluent.Cli.Definitions;
using Fluent.Cli.Options;
using Fluent.Cli.Parsers;
using Fluent.Cli.Preprocess;

namespace Fluent.Cli.Containers;

public class ParserExecutionContainer {
    private readonly string[] environmentArgs;
    private readonly IDictionary<string, OptionConfiguration> optionConfigurations;
    private readonly IDictionary<string, CommandConfiguration> commandConfigurations;

    public ParserExecutionContainer(string[] environmentArgs, IDictionary<string, OptionConfiguration> optionConfigurations, IDictionary<string, CommandConfiguration> commandConfigurations) {
        this.environmentArgs = environmentArgs;
        this.optionConfigurations = optionConfigurations;
        this.commandConfigurations = commandConfigurations;
    }

    public CliArguments Run() {
        //Configure
        var programName = ProgramNameFromAssembly();
        var programVersion = ProgramVersionFromAssembly();
        var optionsDefinitions = OptionDefinitionsFrom(optionConfigurations);
        var commandDefinitions = CommandDefinitionsFrom(commandConfigurations);
        var enableCommandProcess = true; //enabled by default
        var enableOptionsProcess = true; //enabled by default
        var enableArgumentProcess = true; //enabled by default

        //Preprocess (If configured)
        var argumentsPreprocessor = new ArgumentsPreprocessor(enableCommandProcess, enableOptionsProcess, enableArgumentProcess);
        var argumentsPreprocessResult = argumentsPreprocessor.Preprocess(environmentArgs, commandDefinitions);

        if (VersionOptionIsPresent(argumentsPreprocessResult.PossibleOptions)) new VersionOptionConsolePrinter().PrintVersionAndStopProcess(programName, programVersion);
        if (HelpOptionIsPresent(argumentsPreprocessResult.PossibleOptions)) new HelpOptionConsolePrinter().PrintHelpAndStopProcess(programName, optionConfigurations, commandDefinitions);

        //Parse process (If configured)
        var commandArgumentsParser = new CommandArgumentsParser(commandDefinitions);
        var optionsArgumentsParser = new OptionsArgumentsParser(
            new LongOptionsWithArgumentParser(optionsDefinitions),
            new ShortOptionsWithArgumentParser(optionsDefinitions),
            new LongOptionsParser(optionsDefinitions),
            new ShortOptionsParser(optionsDefinitions),
            new MultipleShortOptionsParser(optionsDefinitions),
            new UndefinedOptionsParser()
        );
        var argumentsParser = new ArgumentsParser();

        var commandsArgumentsParserResult = commandArgumentsParser.ParseFrom(argumentsPreprocessResult.PossibleCommand);
        var optionsArgumentsParserResult = optionsArgumentsParser.ParseFrom(argumentsPreprocessResult.PossibleOptions);
        var argumentsParserResult = argumentsParser.ParseFrom(argumentsPreprocessResult.PossibleArguments);

        return CliArgumentsFrom(programName, programVersion, commandsArgumentsParserResult, optionsArgumentsParserResult,
            argumentsParserResult);
    }

    private string ProgramNameFromAssembly() {
        var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
        var assemblyTitles = assembly.GetCustomAttributes<AssemblyTitleAttribute>().ToList();
        var assemblyTitle = assemblyTitles.FirstOrDefault();
        return assemblyTitle?.Title ?? assembly.GetName()?.Name ?? string.Empty;
    }

    private string ProgramVersionFromAssembly() {
        var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
        var assemblyTitles = assembly.GetCustomAttributes<AssemblyInformationalVersionAttribute>().ToList();
        var assemblyTitle = assemblyTitles.FirstOrDefault();
        return assemblyTitle?.InformationalVersion ?? assembly.GetName()?.Version?.ToString() ?? string.Empty;
    }

    private static OptionsDefinitions OptionDefinitionsFrom(IDictionary<string, OptionConfiguration> optionConfigurations) {
        var definitions = optionConfigurations
            .ToDictionary(
                keyValuePair => keyValuePair.Key,
                keyValuePair => new OptionsDefinition(
                    secondaryName: keyValuePair.Value.SecondaryName,
                    hasArgument: keyValuePair.Value.IsArgumentConfigured()
                )
            );
        return new OptionsDefinitions {
            Definitions = definitions
        };
    }

    private static CommandsDefinitions CommandDefinitionsFrom(IDictionary<string, CommandConfiguration> commandConfigurations) {
        var definitions = commandConfigurations
            .ToDictionary(
                keyValuePair => keyValuePair.Key,
                keyValuePair => new CommandDefinition()
            );
        return new CommandsDefinitions {
            Definitions = definitions
        };
    }

    private static bool VersionOptionIsPresent(IList<string> possibleOptions) {
        return possibleOptions.Contains("-v") || possibleOptions.Contains("--version");
    }

    private static bool HelpOptionIsPresent(IList<string> possibleOptions) {
        return possibleOptions.Contains("--help");
    }

    private CliArguments CliArgumentsFrom(string program, string version, CommandsArgumentsParserResult commandsArgumentsParserResult, OptionsArgumentsParserResult optionsParserResult, ArgumentsParserResult argumentsParserResult) {
        var command = CommandFrom(commandsArgumentsParserResult);
        var options = AllOptionsNotPresentByDefaultFrom(optionConfigurations);
        MarkOptionsAsPresentBasedOn(optionsParserResult, options);
        var arguments = ArgumentsFrom(argumentsParserResult);
        return new CliArguments(
            program: program,
            version: version,
            selectedCommand: command,
            options: options.Values.ToList(),
            arguments: arguments
        );
    }
    
    private List<Argument> ArgumentsFrom(ArgumentsParserResult argumentsParserResult) {
        return argumentsParserResult.arguments.Select(
            (argument, index) => new Argument($"${index}", argument)
        ).ToList();
    }

    private IDictionary<string, Option> AllOptionsNotPresentByDefaultFrom(IDictionary<string, OptionConfiguration> optionConfigurations) {
        return optionConfigurations
            .ToDictionary(
                keyValuePair => keyValuePair.Key,
                keyValuePair => OptionNotPresent(keyValuePair.Value)
            );
    }

    private Option OptionNotPresent(OptionConfiguration optionConfiguration) {
        return new Option(
            string.IsNullOrEmpty(optionConfiguration.PrimaryName) ? null : char.Parse(optionConfiguration.PrimaryName),
            optionConfiguration.SecondaryName,
            isPresent: false,
            argumentName: optionConfiguration.Argument?.ArgumentName
        );
    }

    private static Command? CommandFrom(CommandsArgumentsParserResult commandsArgumentsParserResult) {
        return commandsArgumentsParserResult.Command?.Name != null
            ? new Command(commandsArgumentsParserResult.Command.Name)
            : null;
    }

    private void MarkOptionsAsPresentBasedOn(OptionsArgumentsParserResult parserResult, IDictionary<string, Option> options) {
        parserResult.presentOptions.ForEach(presentOption => {
            var optionNamePresent = OptionNamePresent(presentOption.OptionNamePresent, options);
            var option = options[optionNamePresent];
            if (presentOption is ArgumentOptionWithArgument presentOptionWithArgument) {
                options[optionNamePresent] = OptionPresent(option, presentOptionWithArgument.ArgumentValue);
            } else {
                options[optionNamePresent] = OptionPresent(option);
            }
        });
    }

    private static string OptionNamePresent(string OptionName, IDictionary<string, Option> options) {
        return OptionName.Length == 1
            ? OptionName
            : options.First(keyValuePair => OptionName.Equals(keyValuePair.Value.Name)).Key;
    }

    private Option OptionPresent(Option option, string argumentValue = null) {
        return new Option(
            option.ShortName,
            option.Name,
            isPresent: true,
            argumentName: option.ArgumentPresent?.Name,
            argumentValue: argumentValue
        );
    }
}