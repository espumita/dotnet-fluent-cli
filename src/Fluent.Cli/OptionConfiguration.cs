using System.Text.RegularExpressions;

namespace Fluent.Cli;

public class OptionConfiguration {
    public string ShortName { get; }

    private OptionConfiguration(string shortName) {
        ShortName = shortName;
    }

    public static OptionConfiguration For(string shortName) {
        if (string.IsNullOrEmpty(shortName) || !Regex.IsMatch(shortName, "^[a-zA-Z0-9]+$"))
            throw new ArgumentException($"{shortName} is not a valid option, only alpha-numeric values can be configured");
        return new OptionConfiguration(shortName);
    }
}