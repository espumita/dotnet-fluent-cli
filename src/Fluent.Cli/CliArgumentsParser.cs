using System.Text.RegularExpressions;

namespace Fluent.Cli;

public class CliArgumentsParser {

    public static CliArguments ParseFrom(string[] environmentArgs, IDictionary<string, OptionConfiguration> optionConfigurations) {
        var optionsMap = InitializeOptionResultFrom(optionConfigurations);
        var optionsConfiguredWithName = OptionsConfiguredWithName(optionsMap);

        foreach (var arg in environmentArgs) {
            if (IsALongOptionWithArgument(arg)) TryToMarkLongOptionArgumentAsPresent(arg, optionsMap, optionsConfiguredWithName);
            else if (IsAnOptionWithArgument(arg)) TryToMarkShortOptionArgumentAsPresent(arg, optionsMap);
            else if (IsALongOption(arg)) TryToMarkLongOptionAsPresent(arg, optionsMap, optionsConfiguredWithName);
            else if (IsAShortOption(arg)) TryToMarkShortOptionsAsPresent(arg, optionsMap);
            else if (AreMultipleShortOptions(arg)) TryToMarkMultipleShortOptionsAsPresent(arg, optionsMap);
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

    private static bool IsALongOptionWithArgument(string possibleOptionWithArgument) {
        if (string.IsNullOrEmpty(possibleOptionWithArgument) || possibleOptionWithArgument.Length < 3) return false;
        return Regex.IsMatch(possibleOptionWithArgument, "^(--)([a-zA-Z0-9/-]+)(=)(.*)");
    }

    private static bool IsAnOptionWithArgument(string possibleOptionWithArgument) {
        if (string.IsNullOrEmpty(possibleOptionWithArgument) || possibleOptionWithArgument.Length < 3) return false;
        return Regex.IsMatch(possibleOptionWithArgument, "^(-)([a-zA-Z0-9])(=)(.*)");
    }

    private static void TryToMarkShortOptionArgumentAsPresent(string optionArg, IDictionary<string, Option> optionsMap) {
        var optionWithArgumentWithoutPrefix = OptionWitArgument(optionArg);
        if (!optionsMap.ContainsKey(optionWithArgumentWithoutPrefix.option)) return;
        var option = optionsMap[optionWithArgumentWithoutPrefix.option];
        optionsMap[optionWithArgumentWithoutPrefix.option] = OptionPresentWithArgument(option, optionWithArgumentWithoutPrefix.argumentValue);
    }

    private static void TryToMarkLongOptionArgumentAsPresent(string optionArg, IDictionary<string, Option> optionsMap, Dictionary<string, Option> optionsConfiguredWithName) {
        var optionWithArgumentWithoutPrefix = LongOptionWitArgument(optionArg);
        if (!optionsConfiguredWithName.ContainsKey(optionWithArgumentWithoutPrefix.option)) return;
        var option = optionsConfiguredWithName[optionWithArgumentWithoutPrefix.option];
        var key = option.ShortName != null ? option.ShortName.ToString() : option.Name;
        optionsMap[key] = OptionPresentWithArgument(option, optionWithArgumentWithoutPrefix.argumentValue);
    }

    private static Option OptionPresentWithArgument(Option option, string argumentValue) {
        return new Option(option.ShortName, option.Name, isPresent: true, option._Argument.Name, argumentValue);
    }

    private static (string option, string argumentValue) OptionWitArgument(string optionArg) {
        var match = Regex.Match(optionArg, "^(-)([a-zA-Z0-9])(=)(.*)");
        return (match.Groups[2].Value, match.Groups[4].Value);
    }

    private static (string option, string argumentValue) LongOptionWitArgument(string optionArg) {
        var match = Regex.Match(optionArg, "^(--)([a-zA-Z0-9/-]+)(=)(.*)");
        return (match.Groups[2].Value, match.Groups[4].Value);
    }

    private static bool IsALongOption(string possibleLongOption) {
        if (string.IsNullOrEmpty(possibleLongOption) || possibleLongOption.Length == 1) return false;
        return Regex.IsMatch(possibleLongOption, "^(--)([a-zA-Z0-9/-]+)$");
    }

    private static void TryToMarkLongOptionAsPresent(string optionArg, IDictionary<string, Option> optionsMap, Dictionary<string, Option> optionsConfiguredWithName) {
        var longOptionArgWithoutPrefix = LongOptionWithoutPrefix(optionArg);
        if (NameIsPresent(longOptionArgWithoutPrefix, optionsMap, optionsConfiguredWithName)) return;
        throw InvalidOptionArgumentException(longOptionArgWithoutPrefix);
    }

    private static string LongOptionWithoutPrefix(string optionArg) {
        var match = Regex.Match(optionArg, "^(--)([a-zA-Z0-9/-]+)$");
        return match.Groups[2].Value;
    }

    private static bool NameIsPresent(string optionArgWithoutPrefix, IDictionary<string, Option> optionsMap, IDictionary<string, Option> optionsConfiguredWithName) {
        if (!optionsConfiguredWithName.ContainsKey(optionArgWithoutPrefix)) return false;
        var option = optionsConfiguredWithName[optionArgWithoutPrefix];
        var key = option.ShortName != null ? option.ShortName.ToString() : option.Name;
        optionsMap[key] = OptionPresent(option);
        return true;
    }

    private static Option OptionPresent(Option option) {
        return new Option(option.ShortName, option.Name, isPresent: true);
    }

    private static ArgumentException InvalidOptionArgumentException(string optionShortName) {
        return new ArgumentException($"PROGRAM: invalid option -- '{optionShortName}'\r\nTry 'PROGRAM --help' for more information.");
    }

    private static bool IsAShortOption(string possibleOption) {
        if (string.IsNullOrEmpty(possibleOption) || possibleOption.Length == 1) return false;
        return Regex.IsMatch(possibleOption, "^(-)([a-zA-Z0-9])$");
    }

    private static bool AreMultipleShortOptions(string possibleOption) {
        if (string.IsNullOrEmpty(possibleOption) || possibleOption.Length == 1) return false;
        return Regex.IsMatch(possibleOption, "^(-)([a-zA-Z0-9]+)$");
    }

    private static void TryToMarkShortOptionsAsPresent(string optionArg, IDictionary<string, Option> optionsMap) {
        var optionArgWithoutPrefix = OptionWithoutPrefix(optionArg);
        if (ShortNameIsPresent(optionArgWithoutPrefix, optionsMap)) return;
        throw InvalidOptionArgumentException(optionArgWithoutPrefix);
    }

    private static void TryToMarkMultipleShortOptionsAsPresent(string optionArg, IDictionary<string, Option> optionsMap) {
        var multipleOptionsArgWithoutPrefix = MultipleOptionsWithoutPrefix(optionArg);
        MultipleSimpleOptionsArePresent(optionsMap, multipleOptionsArgWithoutPrefix);
    }

    private static string OptionWithoutPrefix(string optionArg) {
        var match = Regex.Match(optionArg, "^(-)([a-zA-Z0-9])$");
        return match.Groups[2].Value;
    }

    private static string MultipleOptionsWithoutPrefix(string optionArg) {
        var match = Regex.Match(optionArg, "^(-)([a-zA-Z0-9]+)$");
        return match.Groups[2].Value;
    }

    private static bool ShortNameIsPresent(string optionArgWithoutPrefix, IDictionary<string, Option> optionsMap) {
        if (!optionsMap.ContainsKey(optionArgWithoutPrefix)) return false;
        var option = optionsMap[optionArgWithoutPrefix];
        optionsMap[optionArgWithoutPrefix] = OptionPresent(option);
        return true;

    }

    private static void MultipleSimpleOptionsArePresent(IDictionary<string, Option> optionsMap, string optionArgWithoutPrefix) {
        for (int index = 0; index < optionArgWithoutPrefix.Length; index++) {
            string possibleSimpleOptionChar = optionArgWithoutPrefix[index].ToString();
            if (!optionsMap.ContainsKey(possibleSimpleOptionChar)) throw InvalidOptionArgumentException(possibleSimpleOptionChar);
            var option = optionsMap[possibleSimpleOptionChar];
            optionsMap[possibleSimpleOptionChar] = OptionPresent(option);
            if (index == optionArgWithoutPrefix.Length - 1) return;
        }
    }

}