using System.Text.RegularExpressions;

namespace Fluent.Cli;

public class LongOptionsArgumentParser {
    public bool IsALongOption(string possibleLongOption) {
        if (string.IsNullOrEmpty(possibleLongOption) || possibleLongOption.Length == 1) return false;
        return Regex.IsMatch(possibleLongOption, "^(--)([a-zA-Z0-9/-]+)$");
    }

    public void TryToMarkLongOptionAsPresent(string optionArg, IDictionary<string, Option> optionsMap, Dictionary<string, Option> optionsConfiguredWithName) {
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

    private static ArgumentException InvalidOptionArgumentException(string optionName) {
        return new ArgumentException($"PROGRAM: invalid option -- '{optionName}'\r\nTry 'PROGRAM --help' for more information.");
    }

    private static Option OptionPresent(Option option) {
        return new Option(option.ShortName, option.Name, isPresent: true);
    }


}