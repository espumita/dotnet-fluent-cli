using System.Text.RegularExpressions;

namespace Fluent.Cli;

public class ClisArgumentsParser {

    public static ClisArguments ParseFrom(string[] environmentArgs, IDictionary<string, OptionConfiguration> optionConfigurations) {
        var optionsMap = InitializeOptionResultFrom(optionConfigurations);
        foreach (var arg in environmentArgs) {
            if (IsAnOption(arg)) TryToMarkOptionsAsPresent(arg, optionsMap);
        }
        return new ClisArguments(
            options: optionsMap.Values.ToList()
        );
    }

    private static IDictionary<string, Option> InitializeOptionResultFrom(IDictionary<string, OptionConfiguration> optionConfigurations) {
        return optionConfigurations
            .ToDictionary(
                keyValuePair => keyValuePair.Key,
                keyValuePair => new Option(keyValuePair.Value.ShortName, isPresent: false));
    }

    private static bool IsAnOption(string possibleOption) {
        if (string.IsNullOrEmpty(possibleOption) || possibleOption.Length == 1) return false;
        return Regex.IsMatch(possibleOption, "^(-|/|--)[a-zA-Z0-9]+$");
    }

    private static void TryToMarkOptionsAsPresent(string optionArg, IDictionary<string, Option> optionsMap) {
        var optionArgWithoutPrefix = OptionWithoutPrefix(optionArg);
        if (!optionsMap.ContainsKey(optionArgWithoutPrefix)) throw new ArgumentException($"PROGRAM: invalid option -- '{optionArgWithoutPrefix}'\r\nTry 'PROGRAM --help' for more information.");
        optionsMap[optionArgWithoutPrefix] = new Option(optionArgWithoutPrefix, isPresent: true);
    }

    private static string OptionWithoutPrefix(string optionArg) {
        var match = Regex.Match(optionArg, "^(-|/|--)([a-zA-Z0-9]+)$");
        return match.Groups[2].Value;
    }
}