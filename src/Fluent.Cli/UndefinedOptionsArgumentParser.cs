using System.Text.RegularExpressions;

namespace Fluent.Cli;

public class UndefinedOptionsArgumentParser {
    public bool IsAnUndefinedOption(string possibleOption) {
        if (string.IsNullOrEmpty(possibleOption) || possibleOption.Length == 1) return false;
        return Regex.IsMatch(possibleOption, "^(--|-)(.*)$");
    }

    public void ThrowUndefinedOptionException(string optionArg) {
        var undefinedOption = Regex.Match(optionArg, "^(--|-)(.*)$");
        throw InvalidOptionArgumentException(undefinedOption.Groups[2].Value);
    }

    private static ArgumentException InvalidOptionArgumentException(string optionName) {
        return new ArgumentException($"PROGRAM: invalid option -- '{optionName}'\r\nTry 'PROGRAM --help' for more information.");
    }
}