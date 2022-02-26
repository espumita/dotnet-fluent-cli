using System.Text.RegularExpressions;
using Fluent.Cli.Options;

namespace Fluent.Cli.Parsers;

public class LongOptionsWithArgumentOptionsParser : IOptionsParser {
    private readonly IDictionary<string, Option> optionsMap;
    private readonly Dictionary<string, Option> optionsConfiguredWithName;

    public LongOptionsWithArgumentOptionsParser(IDictionary<string, Option> optionsMap, Dictionary<string, Option> optionsConfiguredWithName) {
        this.optionsMap = optionsMap;
        this.optionsConfiguredWithName = optionsConfiguredWithName;
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
        if (!optionsConfiguredWithName.ContainsKey(optionWithArgumentWithoutPrefix.option)) throw InvalidOptionArgumentException(optionWithArgumentWithoutPrefix.option);
        var option = optionsConfiguredWithName[optionWithArgumentWithoutPrefix.option];
        var key = option.ShortName != null ? option.ShortName.ToString() : option.Name; //?
        return new List<ArgumentOption> {
            new LongOptionWithArgument {
                key = key,
                NewOption = OptionPresentWithArgument(option, optionWithArgumentWithoutPrefix.argumentValue)
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

    private static Option OptionPresentWithArgument(Option option, string argumentValue) {
        return new Option(option.ShortName, option.Name, isPresent: true, option._Argument.Name, argumentValue);
    }
}