using System.Text.RegularExpressions;

namespace Fluent.Cli;

public class MultipleShortOptionsArgumentParser {
    public bool AreMultipleShortOptions(string possibleOption) {
        if (string.IsNullOrEmpty(possibleOption) || possibleOption.Length == 1) return false;
        return Regex.IsMatch(possibleOption, "^(-)([a-zA-Z0-9]+)$");
    }

    public void TryToMarkMultipleShortOptionsAsPresent(string optionArg, IDictionary<string, Option> optionsMap) {
        var multipleOptionsArgWithoutPrefix = MultipleOptionsWithoutPrefix(optionArg);
        MultipleSimpleOptionsArePresent(optionsMap, multipleOptionsArgWithoutPrefix);
    }

    private static string MultipleOptionsWithoutPrefix(string optionArg) {
        var match = Regex.Match(optionArg, "^(-)([a-zA-Z0-9]+)$");
        return match.Groups[2].Value;
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

    private static ArgumentException InvalidOptionArgumentException(string optionName) {
        return new ArgumentException($"PROGRAM: invalid option -- '{optionName}'\r\nTry 'PROGRAM --help' for more information.");
    }

    private static Option OptionPresent(Option option) {
        return new Option(option.ShortName, option.Name, isPresent: true);
    }
}