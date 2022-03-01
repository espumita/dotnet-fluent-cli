using Fluent.Cli.Exceptions;
using Fluent.Cli.Options;
using Fluent.Cli.Parsers;

namespace Fluent.Cli; 

public class CliArgumentsBuilder {
    private readonly string[] environmentArgs;
    private readonly IDictionary<string, OptionConfiguration> optionConfigurations;
    private string buildingOptionConfiguration;

    private CliArgumentsBuilder(string[] environmentArgs) {
        this.environmentArgs = environmentArgs;
        optionConfigurations = new Dictionary<string, OptionConfiguration>();
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

    public CliArguments Build() {
        //Configure
        var optionsDefinitions = OptionDefinitionsFrom(optionConfigurations);
        var enableArgumentProcess = true; //enabled by default
        var enableOptionsProcess = true; //enabled by default

        //Preprocess (If configured)
        var argumentsPreprocessor = new ArgumentsPreprocessor(enableArgumentProcess, enableOptionsProcess);
        var argumentsPreprocessResult = argumentsPreprocessor.Preprocess(environmentArgs);

        //Process (If configured)
        var optionsArgumentsParser = new OptionsArgumentsParser(
            new LongOptionsWithArgumentParser(optionsDefinitions),
            new ShortOptionsWithArgumentParser(optionsDefinitions),
            new LongOptionsParser(optionsDefinitions),
            new ShortOptionsParser(optionsDefinitions),
            new MultipleShortOptionsParser(optionsDefinitions),
            new UndefinedOptionsParser()
        );
        var argumentsParser = new ArgumentsParser();

        var optionsArgumentsParserResult = optionsArgumentsParser.ParseFrom(argumentsPreprocessResult.PossibleOptions);
        var argumentsParserResult = argumentsParser.ParseFrom(argumentsPreprocessResult.PossibleArguments);

        return CliArgumentsFrom(argumentsPreprocessResult.Program, optionsArgumentsParserResult, argumentsParserResult);
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

    private CliArguments CliArgumentsFrom(string program, OptionsArgumentsParserResult optionsParserResult, ArgumentsParserResult argumentsParserResult) {
        var options = AllOptionsNotPresentByDefaultFrom(optionConfigurations);
        MarkOptionsAsPresentBasedOn(optionsParserResult, options);
        var arguments = ArgumentsFrom(argumentsParserResult);
        return new CliArguments(
            program: program,
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
            argumentName: option._Argument?.Name,
            argumentValue: argumentValue
        );
    }
}