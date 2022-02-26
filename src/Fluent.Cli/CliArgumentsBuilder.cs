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

    public CliArgumentsBuilder WithArgument(string argumentName) {
        if (string.IsNullOrEmpty(argumentName)) throw new ArgumentException("Argument name cannot be null or empty");
        if (string.IsNullOrEmpty(buildingOptionConfiguration)) throw new ArgumentException($"Argument '{argumentName}' could not be configured, you need to configure an Option first.");
        var option = optionConfigurations[buildingOptionConfiguration];
        if (!string.IsNullOrEmpty(option?.Argument?.ArgumentName)) throw new OptionWithMultipleArgumentsAreNotSupportedException($"Option -- '{buildingOptionConfiguration}' can only be configured with a single argument. If you need multiple arguments, consider use a command instead.");
        option.AddArgument(argumentName);
        return this;
    }

    public CliArguments Build() {
        var optionsDefinitions = InitializeOptionResultFrom(optionConfigurations);
        var optionsConfiguredWithName = OptionsConfiguredWithName(optionsDefinitions);

        var cliArgumentsParser = new CliArgumentsParser(
            new LongOptionsWithArgumentOptionsParser(optionsDefinitions, optionsConfiguredWithName),
            new ShortOptionsWithArgumentOptionsParser(optionsDefinitions),
            new LongOptionsArgumentOptionsParser(optionsDefinitions, optionsConfiguredWithName),
            new ShortOptionsArgumentOptionsParser(optionsDefinitions),
            new MultipleShortOptionsArgumentOptionsParser(optionsDefinitions),
            new UndefinedOptionsArgumentOptionsParser()
        );
        var parserResult = cliArgumentsParser.ParseFrom(environmentArgs);
        return BuildCliArgumentsFrom(optionsDefinitions, parserResult);
    }

    private CliArguments BuildCliArgumentsFrom(OptionsDefinitions optionsDefinitions, CliArgumentsParserResult parserResult) {
        var presentOptions = parserResult.presentOptions.Select(argumentOption => {
            var option = argumentOption.NewOption;
            return option;
        }).ToList();

        var options = optionsDefinitions.Options.Keys.Select(key => {
            var presentOption = presentOptions.FirstOrDefault(option => key.Equals(option.ShortName?.ToString()) || key.Equals(option.Name));
            if (presentOption != null) {
                return presentOption;
            }
            return OptionNotPresent(optionsDefinitions.Options[key]);
        }).ToList();

        return new CliArguments(options);
    }

    private Option OptionNotPresent(Option option) {
        return option;
       // return new Option(
       //     option.ShortName,
       //     option.Name,
       //     isPresent: false);//Argument
       // )
    }

    private static OptionsDefinitions InitializeOptionResultFrom(IDictionary<string, OptionConfiguration> optionConfigurations) {
        var options = optionConfigurations
            .ToDictionary(
                keyValuePair => keyValuePair.Key,
                keyValuePair => new Option(
                    keyValuePair.Value.PrimaryName,
                    keyValuePair.Value.SecondaryName,
                    isPresent: false,
                    keyValuePair.Value?.Argument?.ArgumentName)
            );
        return new OptionsDefinitions {
            Options = options
        };
    }

    private static Dictionary<string, Option> OptionsConfiguredWithName(OptionsDefinitions optionsDefinitions) {
        return optionsDefinitions.Options.Where(keyValuePair => !string.IsNullOrEmpty(keyValuePair.Value.Name))
            .ToDictionary(
                keyValuePair => keyValuePair.Value.Name,
                keyValuePair => keyValuePair.Value);
    }
}
