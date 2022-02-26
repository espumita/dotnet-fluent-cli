namespace Fluent.Cli;

public class CliArgumentsParser {
    
    private readonly LongOptionsWithArgumentParser longOptionsWithArgumentParser;
    private readonly ShortOptionsWithArgumentParser shortOptionsWithArgumentParser;
    private readonly LongOptionsArgumentParser longOptionsArgumentParser;
    private readonly ShortOptionsArgumentParser shortOptionsArgumentParser;
    private readonly MultipleShortOptionsArgumentParser multipleShortOptionsArgumentParser;
    private readonly UndefinedOptionsArgumentParser undefinedOptionsArgumentParser;

    public CliArgumentsParser(LongOptionsWithArgumentParser longOptionsWithArgumentParser, ShortOptionsWithArgumentParser shortOptionsWithArgumentParser, LongOptionsArgumentParser longOptionsArgumentParser, ShortOptionsArgumentParser shortOptionsArgumentParser, MultipleShortOptionsArgumentParser multipleShortOptionsArgumentParser, UndefinedOptionsArgumentParser undefinedOptionsArgumentParser) {
        this.longOptionsWithArgumentParser = longOptionsWithArgumentParser;
        this.shortOptionsWithArgumentParser = shortOptionsWithArgumentParser;
        this.longOptionsArgumentParser = longOptionsArgumentParser;
        this.shortOptionsArgumentParser = shortOptionsArgumentParser;
        this.multipleShortOptionsArgumentParser = multipleShortOptionsArgumentParser;
        this.undefinedOptionsArgumentParser = undefinedOptionsArgumentParser;
    }

    public CliArguments ParseFrom(string[] environmentArgs, IDictionary<string, OptionConfiguration> optionConfigurations) {
        var optionsMap = InitializeOptionResultFrom(optionConfigurations);
        var optionsConfiguredWithName = OptionsConfiguredWithName(optionsMap);

        foreach (var arg in environmentArgs) {
            if (longOptionsWithArgumentParser.IsALongOptionWithArgument(arg)) longOptionsWithArgumentParser.TryToMarkLongOptionArgumentAsPresent(arg, optionsMap, optionsConfiguredWithName);
            else if (shortOptionsWithArgumentParser.IsAnOptionWithArgument(arg)) shortOptionsWithArgumentParser.TryToMarkShortOptionArgumentAsPresent(arg, optionsMap);
            else if (longOptionsArgumentParser.IsALongOption(arg)) longOptionsArgumentParser.TryToMarkLongOptionAsPresent(arg, optionsMap, optionsConfiguredWithName);
            else if (shortOptionsArgumentParser.IsAShortOption(arg)) shortOptionsArgumentParser.TryToMarkShortOptionsAsPresent(arg, optionsMap);
            else if (multipleShortOptionsArgumentParser.AreMultipleShortOptions(arg)) multipleShortOptionsArgumentParser.TryToMarkMultipleShortOptionsAsPresent(arg, optionsMap);
            else if (undefinedOptionsArgumentParser.IsAnUndefinedOption(arg)) undefinedOptionsArgumentParser.ThrowUndefinedOptionException(arg);
        }
        return new CliArguments(
            options: optionsMap.Values.ToList()
        );
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