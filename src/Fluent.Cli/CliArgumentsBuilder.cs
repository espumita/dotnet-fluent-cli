using System.Reflection;
using Fluent.Cli.Exceptions;
using Fluent.Cli.Options;
using Fluent.Cli.Parsers;
using Fluent.Cli.Preprocess;

namespace Fluent.Cli; 

public class CliArgumentsBuilder {
    private readonly string[] environmentArgs;
    private readonly IDictionary<string, OptionConfiguration> optionConfigurations;
    private readonly IDictionary<string, CommandConfiguration> commandConfigurations;
    private string buildingOptionConfiguration;

    private CliArgumentsBuilder(string[] environmentArgs) {
        this.environmentArgs = environmentArgs;
        optionConfigurations = new Dictionary<string, OptionConfiguration>();
        commandConfigurations = new Dictionary<string, CommandConfiguration>();
    }

    public static CliArgumentsBuilder With(string[] args) {
        if (args == null) throw new ArgumentException("args cannot be null");
        return new CliArgumentsBuilder(args);
    }

    public CliArgumentsBuilder Option(char shortName) {
        var optionConfiguration = OptionConfiguration.For(shortName);
        optionConfigurations[shortName.ToString()] = optionConfiguration;
        buildingOptionConfiguration = shortName.ToString();
        return this;
    }

    public CliArgumentsBuilder Option(char shortName, string longName) {
        if (string.IsNullOrEmpty(longName)) throw new ArgumentException("Option long name cannot be null, use other method instead");
        var optionConfiguration = OptionConfiguration.For(shortName, longName);
        optionConfigurations[shortName.ToString()] = optionConfiguration;
        buildingOptionConfiguration = shortName.ToString();
        return this;
    }

    public CliArgumentsBuilder LongOption(string longName) {
        if (string.IsNullOrEmpty(longName)) throw new ArgumentException("Option long name cannot be null or empty, use other method instead");
        var optionConfiguration = OptionConfiguration.ForLong(longName);
        optionConfigurations[longName] = optionConfiguration;
        buildingOptionConfiguration = longName;
        return this;
    }

    public CliArgumentsBuilder WithOptionArgument(string argumentName) {
        if (string.IsNullOrEmpty(argumentName)) throw new ArgumentException("Argument name cannot be null or empty");
        if (string.IsNullOrEmpty(buildingOptionConfiguration)) throw new ArgumentException($"Argument '{argumentName}' could not be configured, you need to configure an Option first.");
        var option = optionConfigurations[buildingOptionConfiguration];
        if (option.IsArgumentConfigured()) throw new OptionWithMultipleArgumentsAreNotSupportedException($"Option -- '{buildingOptionConfiguration}' can only be configured with a single argument. If you need multiple arguments, consider use a command instead.");
        option.AddArgument(argumentName);
        return this;
    }

    public CliArgumentsBuilder Command(string name) {
        var commandConfiguration = CommandConfiguration.For(name);
        commandConfigurations[name] = commandConfiguration;
        return this;
    }

    public CliArguments Build() {
        //Configure
        var programName = ProgramNameFromAssembly();
        var programVersion = ProgramVersionFromAssembly();
        var commandDefinitions = CommandDefinitionsFrom(commandConfigurations);
        var optionsDefinitions = OptionDefinitionsFrom(optionConfigurations);
        var enableCommandProcess = true; //enabled by default
        var enableOptionsProcess = true; //enabled by default
        var enableArgumentProcess = true; //enabled by default
        

        //Preprocess (If configured)
        var argumentsPreprocessor = new ArgumentsPreprocessor(enableCommandProcess, enableOptionsProcess, enableArgumentProcess);
        var argumentsPreprocessResult = argumentsPreprocessor.Preprocess(environmentArgs, commandDefinitions);

        if (VersionOptionIsPresent(argumentsPreprocessResult.PossibleOptions)) PrintVersionAndStopProcess(programName, programVersion);
        if (HelpOptionIsPresent(argumentsPreprocessResult.PossibleOptions)) PrintHelpAndStopProcess(programName, optionConfigurations, commandDefinitions);

        //Process (If configured)
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

        return CliArgumentsFrom(programName, programVersion, commandsArgumentsParserResult, optionsArgumentsParserResult, argumentsParserResult);
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

    private void PrintVersionAndStopProcess(string programName, string programVersion) {
        Console.Write($"{programName} version {programVersion}");
        Environment.Exit(0);
    }

    private void PrintHelpAndStopProcess(string programName, IDictionary<string, OptionConfiguration> optionsConfigurations, CommandsDefinitions commandsDefinitions) {
        Console.Write(Environment.NewLine);
        var options = optionsConfigurations.Any() ? "[OPTIONS] " : string.Empty;
        var commands = commandsDefinitions.Definitions.Any() ? "[COMMAND] " : string.Empty;
        var arguments = true ? "[ARGUMENTS]" : string.Empty; // enabled by default
        var usageLie = $"Usage: {programName} {options}{commands}{arguments}";
        Console.WriteLine(usageLie);
        Console.Write(Environment.NewLine);
        Console.WriteLine("_____________________________________________________"); //program description
        if (optionsConfigurations.Any()) {
            Console.Write(Environment.NewLine);
            Console.WriteLine("Options:");
            foreach (var optionConfiguration in optionsConfigurations.Values) {
                var shortOptionName = ShortOptionName(optionConfiguration.PrimaryName);
                var comma = Comma(optionConfiguration.PrimaryName, optionConfiguration.SecondaryName);
                var longOptionName = LongOptionName(optionConfiguration.SecondaryName);
                var optionArgument = OptionArgument(optionConfiguration.Argument);
                var optionLine = $"  {shortOptionName}{comma} {longOptionName} {optionArgument}";
                if (optionLine.Length <= 27) {
                    var optionLineWithFirstColumnWithPadding = optionLine.PadRight(27, ' ');
                    var optionLineWithSecondColumnWithPadding = optionLineWithFirstColumnWithPadding.PadRight(80, '_'); //option description
                    Console.WriteLine(optionLineWithSecondColumnWithPadding);
                } else {
                    Console.WriteLine(optionLine);
                }

            }
        }
        if (commandsDefinitions.Definitions.Any()) {
            Console.Write(Environment.NewLine);
            Console.WriteLine("Commands:");
            foreach (var commandDefinition in commandsDefinitions.Definitions) {
                var command = commandDefinition.Key;
                var commandLine = $"  {command}";
                var commandLineWithFirstColumnWithPadding = commandLine.PadRight(14, ' ');
                var commandLineWithSecondColumnWithPadding = commandLineWithFirstColumnWithPadding.PadRight(80, '_'); //selectedCommand description
                Console.WriteLine(commandLineWithSecondColumnWithPadding);
            }
        }
        Console.Write(Environment.NewLine);
        Environment.Exit(0);
    }

    private string Comma(string shortOptionName, string longOptionName) {
        return !string.IsNullOrEmpty(shortOptionName) && !string.IsNullOrEmpty(longOptionName)
            ? ","
            : " ";
    }

    private string ShortOptionName(string shortOptionName) {
        return !string.IsNullOrEmpty(shortOptionName)
            ? $"-{shortOptionName}"
            : "  ";
    }

    private string LongOptionName(string longOptionName) {
        return !string.IsNullOrEmpty(longOptionName)
            ? $"--{longOptionName}"
            : string.Empty;
    }

    private string OptionArgument(ArgumentConfiguration optionArgument) {
        return !string.IsNullOrEmpty(optionArgument?.ArgumentName)
            ? optionArgument?.ArgumentName
            : string.Empty;
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