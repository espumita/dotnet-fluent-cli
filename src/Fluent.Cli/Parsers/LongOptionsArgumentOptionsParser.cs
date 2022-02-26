using System.Text.RegularExpressions;
using Fluent.Cli.Options;

namespace Fluent.Cli.Parsers;

public class LongOptionsArgumentOptionsParser : IOptionsParser {
    
    private readonly IDictionary<string, Option> _optionsMap;
    private readonly Dictionary<string, Option> _optionsConfiguredWithName;

    public LongOptionsArgumentOptionsParser(IDictionary<string, Option> optionsMap, Dictionary<string, Option> optionsConfiguredWithName) {
        _optionsMap = optionsMap;
        _optionsConfiguredWithName = optionsConfiguredWithName;
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
        if (!_optionsConfiguredWithName.ContainsKey(longOptionArgWithoutPrefix)) throw InvalidOptionArgumentException(longOptionArgWithoutPrefix); ;
        var option = _optionsConfiguredWithName[longOptionArgWithoutPrefix];
        var key = option.ShortName != null ? option.ShortName.ToString() : option.Name; //?
        return new List<ArgumentOption> {
            new LongOption {
                key = key,
                NewOption = OptionPresent(option)
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

    private static Option OptionPresent(Option option) {
        return new Option(option.ShortName, option.Name, isPresent: true);
    }
}