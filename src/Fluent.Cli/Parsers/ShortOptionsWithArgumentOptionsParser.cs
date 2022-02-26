using System.Text.RegularExpressions;
using Fluent.Cli.Options;

namespace Fluent.Cli.Parsers;

public class ShortOptionsWithArgumentOptionsParser : IOptionsParser {
    private readonly IDictionary<string, Option> _optionsMap;

    public ShortOptionsWithArgumentOptionsParser(IDictionary<string, Option> optionsMap) {
        _optionsMap = optionsMap;
    }

    public IList<ArgumentOption> TryToParse(string argument) {
        return TryToMarkShortOptionArgumentAsPresent(argument);
    }

    public bool IsAnOptionWithArgument(string possibleOptionWithArgument) {
        if (string.IsNullOrEmpty(possibleOptionWithArgument) || possibleOptionWithArgument.Length < 3) return false;
        var isAMultipleShortOption = Regex.IsMatch(possibleOptionWithArgument, "^(-)([a-zA-Z0-9]+)(=)(.*)");
        if (!isAMultipleShortOption) return false;
        var isASingleShortOption = Regex.IsMatch(possibleOptionWithArgument, "^(-)([a-zA-Z0-9])(=)(.*)");
        if (isASingleShortOption) return true;
        var value = Regex.Match(possibleOptionWithArgument, "^(-)([a-zA-Z0-9]+)(=)(.*)").Groups[2].Value;
        throw InvalidOptionArgumentException(value);
    }

    public IList<ArgumentOption> TryToMarkShortOptionArgumentAsPresent(string argument) {
        var optionWithArgumentWithoutPrefix = OptionWithArgument(argument);
        if (!_optionsMap.ContainsKey(optionWithArgumentWithoutPrefix.option)) throw InvalidOptionArgumentException(optionWithArgumentWithoutPrefix.option);
        var option = _optionsMap[optionWithArgumentWithoutPrefix.option];
        var key = optionWithArgumentWithoutPrefix.option;
        return new List<ArgumentOption> {
            new ShortOptionWithArgument {
                key = key,
                NewOption = OptionPresentWithArgument(option, optionWithArgumentWithoutPrefix.argumentValue)
            }
        };
    }

    private static (string option, string argumentValue) OptionWithArgument(string optionArg) {
        var match = Regex.Match(optionArg, "^(-)([a-zA-Z0-9])(=)(.*)");
        return (match.Groups[2].Value, match.Groups[4].Value);
    }

    private static ArgumentException InvalidOptionArgumentException(string optionName) {
        return new ArgumentException($"PROGRAM: invalid option -- '{optionName}'\r\nTry 'PROGRAM --help' for more information.");
    }

    private static Option OptionPresentWithArgument(Option option, string argumentValue) {
        return new Option(option.ShortName, option.Name, isPresent: true, option._Argument.Name, argumentValue);
    }
}