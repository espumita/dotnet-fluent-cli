using System.Text.RegularExpressions;
using Fluent.Cli.Options;

namespace Fluent.Cli.Parsers;

public class ShortOptionsArgumentOptionsParser : IOptionsParser {
    private readonly IDictionary<string, Option> _optionsMap;

    public ShortOptionsArgumentOptionsParser(IDictionary<string, Option> optionsMap) {
        _optionsMap = optionsMap;
    }

    public IList<ArgumentOption> TryToParse(string argument) {
        return TryToMarkShortOptionsAsPresent(argument);
    }

    public bool IsAShortOption(string possibleOption) {
        if (string.IsNullOrEmpty(possibleOption) || possibleOption.Length == 1) return false;
        return Regex.IsMatch(possibleOption, "^(-)([a-zA-Z0-9])$");
    }

    public IList<ArgumentOption> TryToMarkShortOptionsAsPresent(string optionArg) {
        var optionArgWithoutPrefix = OptionWithoutPrefix(optionArg);
        if (!_optionsMap.ContainsKey(optionArgWithoutPrefix)) throw InvalidOptionArgumentException(optionArgWithoutPrefix);
        var option = _optionsMap[optionArgWithoutPrefix];
        var key = optionArgWithoutPrefix;
        return new List<ArgumentOption> {
            new ShortOption {
                key = key,
                NewOption = OptionPresent(option)

            }
        };
    }

    private static string OptionWithoutPrefix(string optionArg) {
        var match = Regex.Match(optionArg, "^(-)([a-zA-Z0-9])$");
        return match.Groups[2].Value;
    }

    private static Option OptionPresent(Option option) {
        return new Option(option.ShortName, option.Name, isPresent: true);
    }

    private static ArgumentException InvalidOptionArgumentException(string optionName) {
        return new ArgumentException($"PROGRAM: invalid option -- '{optionName}'\r\nTry 'PROGRAM --help' for more information.");
    }
}