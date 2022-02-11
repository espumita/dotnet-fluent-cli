using System.Text.RegularExpressions;

namespace Fluent.Cli;

public class CliArgumentsParser {

    public static CliArguments ParseFrom(string[] environmentArgs, IDictionary<string, OptionConfiguration> optionConfigurations) {
        var optionsMap = InitializeOptionResultFrom(optionConfigurations);
        var optionsConfiguredWithName = OptionsConfiguredWithName(optionsMap);

        foreach (var arg in environmentArgs) {
            if (IsAnOption(arg)) TryToMarkOptionsAsPresent(arg, optionsMap, optionsConfiguredWithName);
        }
        return new CliArguments(
            options: optionsMap.Values.ToList()
        );
    }

    private static IDictionary<string, Option> InitializeOptionResultFrom(IDictionary<string, OptionConfiguration> optionConfigurations) {
        return optionConfigurations
            .ToDictionary(
                keyValuePair => keyValuePair.Key,
                keyValuePair => new Option(keyValuePair.Value.PrimaryName, keyValuePair.Value.SecondaryName, isPresent: false));
    }

    private static Dictionary<string, Option> OptionsConfiguredWithName(IDictionary<string, Option> optionsMap) {
        return optionsMap.Where(keyValuePair => !string.IsNullOrEmpty(keyValuePair.Value.Name))
            .ToDictionary(
                keyValuePair => keyValuePair.Value.Name,
                keyValuePair => keyValuePair.Value);
    }

    private static bool IsAnOption(string possibleOption) {
        if (string.IsNullOrEmpty(possibleOption) || possibleOption.Length == 1) return false;
        return Regex.IsMatch(possibleOption, "^((--|/)|(-))([a-zA-Z0-9/-]+)$");
    }

    private static void TryToMarkOptionsAsPresent(string optionArg, IDictionary<string, Option> optionsMap, IDictionary<string, Option> optionsConfiguredWithName) {
        var optionArgWithoutPrefix = OptionWithoutPrefix(optionArg);
        if (ShortNameIsPresent(optionArgWithoutPrefix, optionsMap)) return;
        if (NameIsPresent(optionArgWithoutPrefix, optionsMap, optionsConfiguredWithName)) return;
        if (MultipleSimpleOptionsArePresent(optionsMap, optionArgWithoutPrefix)) return;
        throw InvalidOptionArgumentException(optionArgWithoutPrefix);
    }

    private static string OptionWithoutPrefix(string optionArg) {
        var match = Regex.Match(optionArg, "^((--|/)|(-))([a-zA-Z0-9/-]+)$");
        return match.Groups[4].Value;
    }

    private static bool ShortNameIsPresent(string optionArgWithoutPrefix, IDictionary<string, Option> optionsMap) {
        if (!optionsMap.ContainsKey(optionArgWithoutPrefix)) return false;
        var option = optionsMap[optionArgWithoutPrefix];
        optionsMap[option.ShortName] = OptionPresent(option);
        return true;

    }

    private static Option OptionPresent(Option option) {
        return new Option(option.ShortName, option.Name, isPresent: true);
    }

    private static bool NameIsPresent(string optionArgWithoutPrefix, IDictionary<string, Option> optionsMap, IDictionary<string, Option> optionsConfiguredWithName) {
        if (!optionsConfiguredWithName.ContainsKey(optionArgWithoutPrefix)) return false;
        var option = optionsConfiguredWithName[optionArgWithoutPrefix];
        optionsMap[option.ShortName] = OptionPresent(option);
        return true;
    }

    private static bool MultipleSimpleOptionsArePresent(IDictionary<string, Option> optionsMap, string optionArgWithoutPrefix) {
        for (int index = 0; index < optionArgWithoutPrefix.Length; index++) {
            string possibleSimpleOptionChar = optionArgWithoutPrefix[index].ToString();
            if (!optionsMap.ContainsKey(possibleSimpleOptionChar)) throw InvalidOptionArgumentException(possibleSimpleOptionChar);
            var option = optionsMap[possibleSimpleOptionChar];
            optionsMap[option.ShortName] = OptionPresent(option);
            if (index == optionArgWithoutPrefix.Length - 1) return true;
        }
        return false;
    }

    private static ArgumentException InvalidOptionArgumentException(string optionShortName) {
        return new ArgumentException($"PROGRAM: invalid option -- '{optionShortName}'\r\nTry 'PROGRAM --help' for more information.");
    }
}