using System.Reflection;
using Fluent.Cli.Configuration;
using Fluent.Cli.ConsolePrinters;
using Fluent.Cli.Definitions;
using Fluent.Cli.Parsers;
using Fluent.Cli.Preprocess;

namespace Fluent.Cli.Containers;

public class ParserExecutionContainer {
    private readonly string[] environmentArgs;
    private readonly IDictionary<string, CommandConfiguration> commandConfigurations;
    private readonly IDictionary<string, OptionConfiguration> optionConfigurations;

    public ParserExecutionContainer(string[] environmentArgs, IDictionary<string, CommandConfiguration> commandConfigurations, IDictionary<string, OptionConfiguration> optionConfigurations) {
        this.environmentArgs = environmentArgs;
        this.optionConfigurations = optionConfigurations;
        this.commandConfigurations = commandConfigurations;
    }

    public CliArguments Run() {
        //Configure
        var programName = ProgramNameFromAssembly();
        var programVersion = ProgramVersionFromAssembly();
        var commandDefinitions = CommandDefinitionsFrom(commandConfigurations);
        var optionsDefinitions = OptionDefinitionsFrom(optionConfigurations);
        var enableCommandProcess = true; //enabled by default
        var enableOptionsProcess = true; //enabled by default
        var enableArgumentProcess = true; //enabled by default

        //Preprocess
        var argumentsPreprocessResult = ArgumentsPreprocessor(enableCommandProcess, enableOptionsProcess, enableArgumentProcess)
            .Preprocess(environmentArgs, commandDefinitions);
        
        CheckIfDefaultOptionsArePresent(argumentsPreprocessResult, programName, programVersion);

        //Parse process
        var commandsArgumentsParserResult = CommandArgumentsParser(commandDefinitions)
            .ParseFrom(argumentsPreprocessResult.PossibleCommand);
        var optionsArgumentsParserResult = OptionsArgumentsParser(optionsDefinitions)
            .ParseFrom(argumentsPreprocessResult.PossibleOptions);
        var argumentsParserResult = ArgumentsParser()
            .ParseFrom(argumentsPreprocessResult.PossibleArguments);
        
        return new ParserExecutionsResultsMerger(programName, programVersion, commandsArgumentsParserResult, optionsArgumentsParserResult, optionConfigurations, argumentsParserResult)
            .MergeAsCliArguments();
    }

    private static ArgumentsPreprocessor ArgumentsPreprocessor(bool enableCommandProcess, bool enableOptionsProcess, bool enableArgumentProcess) {
        return new ArgumentsPreprocessor(enableCommandProcess, enableOptionsProcess, enableArgumentProcess);
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

    private void CheckIfDefaultOptionsArePresent(ArgumentsPreprocessResult argumentsPreprocessResult, string programName, string programVersion) {
        if (VersionOptionIsPresent(argumentsPreprocessResult.PossibleOptions))
            new VersionOptionConsolePrinter(programName, programVersion).PrintVersionAndStopProcess();
        if (HelpOptionIsPresent(argumentsPreprocessResult.PossibleOptions))
            new HelpOptionConsolePrinter().PrintHelpAndStopProcess(programName, optionConfigurations, commandConfigurations);
    }

    private static bool VersionOptionIsPresent(IList<string> possibleOptions) {
        return possibleOptions.Contains("-v") || possibleOptions.Contains("--version");
    }

    private static bool HelpOptionIsPresent(IList<string> possibleOptions) {
        return possibleOptions.Contains("--help");
    }

    private static CommandArgumentsParser CommandArgumentsParser(CommandsDefinitions commandDefinitions) {
        return new CommandArgumentsParser(commandDefinitions);
    }

    private static OptionsArgumentsParser OptionsArgumentsParser(OptionsDefinitions optionsDefinitions) {
        return new OptionsArgumentsParser(
            new LongOptionsWithArgumentParser(optionsDefinitions),
            new ShortOptionsWithArgumentParser(optionsDefinitions),
            new LongOptionsParser(optionsDefinitions),
            new ShortOptionsParser(optionsDefinitions),
            new MultipleShortOptionsParser(optionsDefinitions),
            new UndefinedOptionsParser()
        );
    }

    private static ArgumentsParser ArgumentsParser() {
        return new ArgumentsParser();
    }

}