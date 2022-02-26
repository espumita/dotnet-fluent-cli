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
        var optionsMap = InitializeOptionResultFrom(optionConfigurations);
        var optionsConfiguredWithName = OptionsConfiguredWithName(optionsMap);

        var cliArgumentsParser = new CliArgumentsParser(
            optionsMap,
            new LongOptionsWithArgumentOptionsParser(optionsMap, optionsConfiguredWithName),
            new ShortOptionsWithArgumentOptionsParser(optionsMap),
            new LongOptionsArgumentOptionsParser(optionsMap, optionsConfiguredWithName),
            new ShortOptionsArgumentOptionsParser(optionsMap),
            new MultipleShortOptionsArgumentOptionsParser(optionsMap),
            new UndefinedOptionsArgumentOptionsParser()
        );
        return cliArgumentsParser.ParseFrom(environmentArgs);
    }

    private static IDictionary<string, Option> InitializeOptionResultFrom(IDictionary<string, OptionConfiguration> optionConfigurations) {
        return optionConfigurations
            .ToDictionary(
                keyValuePair => keyValuePair.Key,
                keyValuePair => new Option(keyValuePair.Value.PrimaryName, keyValuePair.Value.SecondaryName, isPresent: false, keyValuePair.Value?.Argument?.ArgumentName));
    }

    private static Dictionary<string, Option> OptionsConfiguredWithName(IDictionary<string, Option> optionsMap) {
        return optionsMap.Where(keyValuePair => !string.IsNullOrEmpty(keyValuePair.Value.Name))
            .ToDictionary(
                keyValuePair => keyValuePair.Value.Name,
                keyValuePair => keyValuePair.Value);
    }
}