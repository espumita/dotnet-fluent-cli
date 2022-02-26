using System.Text.RegularExpressions;
using Fluent.Cli.Options;

namespace Fluent.Cli.Parsers;

public class LongOptionsWithArgumentParser : IOptionsParser {
    private readonly OptionsDefinitions _optionsDefinitions;

    public LongOptionsWithArgumentParser(OptionsDefinitions optionsDefinitions) {
        _optionsDefinitions = optionsDefinitions;
    }

    public IList<ArgumentOption> TryToParse(string argument) {
        return TryToMarkLongOptionArgumentAsPresent(argument);
    }

    public bool IsALongOptionWithArgument(string possibleOptionWithArgument) {
        if (string.IsNullOrEmpty(possibleOptionWithArgument) || possibleOptionWithArgument.Length < 3) return false;
        return Regex.IsMatch(possibleOptionWithArgument, "^(--)([a-zA-Z0-9/-]+)(=)(.*)");
    }

    public IList<ArgumentOption> TryToMarkLongOptionArgumentAsPresent(string argument) {
        var optionWithArgumentWithoutPrefix = LongOptionWithArgument(argument);
        if (!_optionsDefinitions.IsOptionDefined(optionWithArgumentWithoutPrefix.option)) throw InvalidOptionArgumentException(optionWithArgumentWithoutPrefix.option);
        if (!_optionsDefinitions.IsArgumentOptionDefined(optionWithArgumentWithoutPrefix.option)) throw ArgumentNotConfiguredArgumentException(optionWithArgumentWithoutPrefix.option);
        return new List<ArgumentOption> {
            new LongOptionWithArgument {
                OptionNamePresent = optionWithArgumentWithoutPrefix.option,
                ArgumentValue = optionWithArgumentWithoutPrefix.argumentValue
            }
        };
    }

    private (string option, string argumentValue) LongOptionWithArgument(string optionArg) {
        var match = Regex.Match(optionArg, "^(--)([a-zA-Z0-9/-]+)(=)(.*)");
        return (match.Groups[2].Value, match.Groups[4].Value);
    }

    private static ArgumentException InvalidOptionArgumentException(string optionName) {
        return new ArgumentException($"PROGRAM: invalid option -- '{optionName}'\r\nTry 'PROGRAM --help' for more information.");
    }
    private static ArgumentException ArgumentNotConfiguredArgumentException(string optionName) {
        return new ArgumentException($"PROGRAM: option -- '{optionName}' cannot be used with arguments.\r\nTry 'PROGRAM --help' for more information.");
    }

}