using System.Text.RegularExpressions;

namespace Fluent.Cli;

public class ShortOptionsArgumentParser {
    public bool IsAShortOption(string possibleOption) {
        if (string.IsNullOrEmpty(possibleOption) || possibleOption.Length == 1) return false;
        return Regex.IsMatch(possibleOption, "^(-)([a-zA-Z0-9])$");
    }

    public void TryToMarkShortOptionsAsPresent(string optionArg, IDictionary<string, Option> optionsMap) {
        var optionArgWithoutPrefix = OptionWithoutPrefix(optionArg);
        if (ShortNameIsPresent(optionArgWithoutPrefix, optionsMap)) return;
        throw InvalidOptionArgumentException(optionArgWithoutPrefix);
    }

    private static string OptionWithoutPrefix(string optionArg) {
        var match = Regex.Match(optionArg, "^(-)([a-zA-Z0-9])$");
        return match.Groups[2].Value;
    }

    private static bool ShortNameIsPresent(string optionArgWithoutPrefix, IDictionary<string, Option> optionsMap) {
        if (!optionsMap.ContainsKey(optionArgWithoutPrefix)) return false;
        var option = optionsMap[optionArgWithoutPrefix];
        optionsMap[optionArgWithoutPrefix] = OptionPresent(option);
        return true;
    }

    private static Option OptionPresent(Option option) {
        return new Option(option.ShortName, option.Name, isPresent: true);
    }

    private static ArgumentException InvalidOptionArgumentException(string optionName) {
        return new ArgumentException($"PROGRAM: invalid option -- '{optionName}'\r\nTry 'PROGRAM --help' for more information.");
    }
}