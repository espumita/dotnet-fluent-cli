using System.Text.RegularExpressions;
using Fluent.Cli.Options;

namespace Fluent.Cli.Parsers;

public class UndefinedOptionsParser : IOptionsParser {
    public IList<ArgumentOption> TryToParse(string argument) {
        var undefinedOption = Regex.Match(argument, "^(--|-)(.*)$");
        throw InvalidOptionArgumentException(undefinedOption.Groups[2].Value);
    }

    public bool IsAnUndefinedOption(string possibleOption) {
        if (string.IsNullOrEmpty(possibleOption) || possibleOption.Length == 1) return false;
        return Regex.IsMatch(possibleOption, "^(--|-)(.*)$");
    }

    private static ArgumentException InvalidOptionArgumentException(string optionName) {
        return new ArgumentException($"PROGRAM: invalid option -- '{optionName}'\r\nTry 'PROGRAM --help' for more information.");
    }
}