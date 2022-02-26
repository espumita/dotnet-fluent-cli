using System.Text.RegularExpressions;
using Fluent.Cli.Options;

namespace Fluent.Cli.Parsers;

public class ShortOptionsParser : IOptionsParser {
    private readonly OptionsDefinitions _optionsDefinitions;

    public ShortOptionsParser(OptionsDefinitions _optionsDefinitions) {
        this._optionsDefinitions = _optionsDefinitions;
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
        if (!_optionsDefinitions.IsOptionDefined(optionArgWithoutPrefix)) throw InvalidOptionArgumentException(optionArgWithoutPrefix);
        return new List<ArgumentOption> {
            new ShortOption {
                OptionNamePresent = optionArgWithoutPrefix,
            }
        };
    }

    private static string OptionWithoutPrefix(string optionArg) {
        var match = Regex.Match(optionArg, "^(-)([a-zA-Z0-9])$");
        return match.Groups[2].Value;
    }

    private static ArgumentException InvalidOptionArgumentException(string optionName) {
        return new ArgumentException($"PROGRAM: invalid option -- '{optionName}'\r\nTry 'PROGRAM --help' for more information.");
    }
}