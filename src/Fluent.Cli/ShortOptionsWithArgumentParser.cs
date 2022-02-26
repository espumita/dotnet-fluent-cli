using System.Text.RegularExpressions;

namespace Fluent.Cli;

public class ShortOptionsWithArgumentParser {
    public bool IsAnOptionWithArgument(string possibleOptionWithArgument) {
        if (string.IsNullOrEmpty(possibleOptionWithArgument) || possibleOptionWithArgument.Length < 3) return false;
        var isAMultipleShortOption = Regex.IsMatch(possibleOptionWithArgument, "^(-)([a-zA-Z0-9]+)(=)(.*)");
        if (!isAMultipleShortOption) return false;
        var isASingleShortOption = Regex.IsMatch(possibleOptionWithArgument, "^(-)([a-zA-Z0-9])(=)(.*)");
        if (isASingleShortOption) return true;
        var value = Regex.Match(possibleOptionWithArgument, "^(-)([a-zA-Z0-9]+)(=)(.*)").Groups[2].Value;
        throw InvalidOptionArgumentException(value);
    }

    public void TryToMarkShortOptionArgumentAsPresent(string optionArg, IDictionary<string, Option> optionsMap) {
        var optionWithArgumentWithoutPrefix = OptionWithArgument(optionArg);
        if (!optionsMap.ContainsKey(optionWithArgumentWithoutPrefix.option)) throw InvalidOptionArgumentException(optionWithArgumentWithoutPrefix.option);
        var option = optionsMap[optionWithArgumentWithoutPrefix.option];
        optionsMap[optionWithArgumentWithoutPrefix.option] = OptionPresentWithArgument(option, optionWithArgumentWithoutPrefix.argumentValue);
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