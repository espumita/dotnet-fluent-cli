using System.Text.RegularExpressions;

namespace Fluent.Cli;

public class CliArgumentsParser {

    public static CliArguments ParseFrom(string[] environmentArgs, IDictionary<string, OptionConfiguration> optionConfigurations) {
        var optionsMap = InitializeOptionResultFrom(optionConfigurations);
        foreach (var arg in environmentArgs) {
            if (IsAnOption(arg)) TryToMarkOptionsAsPresent(arg, optionsMap);
        }
        return new CliArguments(
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
        if (optionsMap.ContainsKey(optionArgWithoutPrefix)) {
            optionsMap[optionArgWithoutPrefix] = new Option(optionArgWithoutPrefix, isPresent: true);
            return;
        }

        for (int index = 0; index < optionArgWithoutPrefix.Length; index++) {
            string possibleSimpleOptionChar = optionArgWithoutPrefix[index].ToString();
            if (!optionsMap.ContainsKey(possibleSimpleOptionChar)) throw InvalidOptionArgumentException(possibleSimpleOptionChar);
            optionsMap[possibleSimpleOptionChar] = new Option(possibleSimpleOptionChar, isPresent: true);
            if (index == optionArgWithoutPrefix.Length - 1) return;
        }

        throw InvalidOptionArgumentException(optionArgWithoutPrefix);
        
    }

    private static string OptionWithoutPrefix(string optionArg) {
        var match = Regex.Match(optionArg, "^(-|/|--)([a-zA-Z0-9]+)$");
        return match.Groups[2].Value;
    }

    private static ArgumentException InvalidOptionArgumentException(string optionShortName) {
        return new ArgumentException($"PROGRAM: invalid option -- '{optionShortName}'\r\nTry 'PROGRAM --help' for more information.");
    }
}