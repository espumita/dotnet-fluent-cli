using System.Text.RegularExpressions;
using Fluent.Cli.Definitions;
using Fluent.Cli.Options;

namespace Fluent.Cli.Parsers;

public class LongOptionsParser : IOptionsParser {
    
    private readonly OptionsDefinitions _optionsDefinitions;

    public LongOptionsParser(OptionsDefinitions _optionsDefinitions) {
        this._optionsDefinitions = _optionsDefinitions;
    }

    public IList<ArgumentOption> TryToParse(string argument) {
        return TryToMarkLongOptionAsPresent(argument);
    }

    public bool IsALongOption(string possibleLongOption) {
        if (string.IsNullOrEmpty(possibleLongOption) || possibleLongOption.Length == 1) return false;
        return Regex.IsMatch(possibleLongOption, "^(--)([a-zA-Z0-9/-]+)$");
    }

    public IList<ArgumentOption> TryToMarkLongOptionAsPresent(string argument) {
        var longOptionArgWithoutPrefix = LongOptionWithoutPrefix(argument);
        if (!_optionsDefinitions.IsOptionDefined(longOptionArgWithoutPrefix)) throw InvalidOptionArgumentException(longOptionArgWithoutPrefix); ;
        return new List<ArgumentOption> {
            new LongOption {
                OptionNamePresent = longOptionArgWithoutPrefix
            }
        };
    }

    private static string LongOptionWithoutPrefix(string optionArg) {
        var match = Regex.Match(optionArg, "^(--)([a-zA-Z0-9/-]+)$");
        return match.Groups[2].Value;
    }

    private static ArgumentException InvalidOptionArgumentException(string optionName) {
        return new ArgumentException($"PROGRAM: invalid option -- '{optionName}'\r\nTry 'PROGRAM --help' for more information.");
    }

}