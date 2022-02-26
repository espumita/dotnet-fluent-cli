using System.Text.RegularExpressions;
using Fluent.Cli.Options;

namespace Fluent.Cli.Parsers;

public class MultipleShortOptionsParser : IOptionsParser {
    private readonly OptionsDefinitions _optionsDefinitions;

    public MultipleShortOptionsParser(OptionsDefinitions _optionsDefinitions) {
        this._optionsDefinitions = _optionsDefinitions;
    }

    public IList<ArgumentOption> TryToParse(string argument) {
        return TryToMarkMultipleShortOptionsAsPresent(argument);
    }

    public bool AreMultipleShortOptions(string possibleOption) {
        if (string.IsNullOrEmpty(possibleOption) || possibleOption.Length == 1) return false;
        return Regex.IsMatch(possibleOption, "^(-)([a-zA-Z0-9]+)$");
    }

    public IList<ArgumentOption> TryToMarkMultipleShortOptionsAsPresent(string argument) {
        var multipleOptionsArgWithoutPrefix = MultipleOptionsWithoutPrefix(argument);
        var options = new List<ArgumentOption>();
        for (int index = 0; index < multipleOptionsArgWithoutPrefix.Length; index++) {
            string possibleSimpleOptionChar = multipleOptionsArgWithoutPrefix[index].ToString();
            if (!_optionsDefinitions.IsOptionDefined(possibleSimpleOptionChar)) throw InvalidOptionArgumentException(possibleSimpleOptionChar);
            if (!options.Any(x => x.OptionNamePresent.Equals(possibleSimpleOptionChar))) {
                var shortOption = new ShortOption {
                    OptionNamePresent = possibleSimpleOptionChar
                };
                options.Add(shortOption);
            }

            if (index == multipleOptionsArgWithoutPrefix.Length - 1) break;
        }

        return options;
    }

    private static string MultipleOptionsWithoutPrefix(string optionArg) {
        var match = Regex.Match(optionArg, "^(-)([a-zA-Z0-9]+)$");
        return match.Groups[2].Value;
    }

    private static ArgumentException InvalidOptionArgumentException(string optionName) {
        return new ArgumentException($"PROGRAM: invalid option -- '{optionName}'\r\nTry 'PROGRAM --help' for more information.");
    }

    private static Option OptionPresent(Option option) {
        return new Option(option.ShortName, option.Name, isPresent: true);
    }
}