namespace Fluent.Cli;

public class ClisArgumentsParser {

    public static ClisArguments ParseFrom(string[] environmentArgs, IDictionary<string, OptionConfiguration> optionConfigurations) {
        var options = InitializeOptionResultFrom(optionConfigurations);
        foreach (var arg in environmentArgs) {
            if (IsAnOption(arg)) MarkOptionsAsPresent(arg, options);
        }
        return new ClisArguments(options);
    }

    private static List<Option> InitializeOptionResultFrom(IDictionary<string, OptionConfiguration> optionConfigurations) {
        return optionConfigurations.Keys
            .Select(configuration => new Option(configuration, false))
            .ToList();
    }

    private static bool IsAnOption(string possibleOption) {
        var possibleOptionStartingChar = possibleOption[0];
        return possibleOptionStartingChar.Equals('-');
        // || possibleOptionStartingChar.Equals("/");
    }

    private static void MarkOptionsAsPresent(string arg, List<Option> options) {
        foreach (var argCharacter in arg) {
            var option = options.FirstOrDefault(option => char.Parse(option.ShortName).Equals(argCharacter));
            if (option != null) {
                option = new Option(option.ShortName, isPresent: true);
            }
        }
    }

    private static bool IsPresent(string[] environmentArgs, string shortName) {
        return environmentArgs.ToList().Any(arg =>
            arg.Equals(shortName)
            || arg.Equals($"-{shortName}")
            || arg.Equals($"--{shortName}")
        );
    }
}