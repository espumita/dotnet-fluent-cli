using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Fluent.Cli;

public class OptionConfiguration {
    public string PrimaryName { get; }
    public string SecondaryName { get; }

    private OptionConfiguration(string primaryName, [Optional] string secondaryName) {
        PrimaryName = primaryName;
        SecondaryName = secondaryName;
    }

    public static OptionConfiguration For(string primaryName) {
        Validate(primaryName);
        return new OptionConfiguration(primaryName);
    }

    public static OptionConfiguration For(string primaryName, string secondaryName) {
        Validate(primaryName);
        Validate(secondaryName);
        return new OptionConfiguration(primaryName, secondaryName);
    }

    private static void Validate(string shortName) {
        if (string.IsNullOrEmpty(shortName) || !Regex.IsMatch(shortName, "^[a-zA-Z0-9]+$|^[^/-][a-zA-Z0-9/-]+[^/-]$"))
            throw new ArgumentException($"{shortName} is not a valid option, only alpha-numeric values and words separated by hyphen minus can be configured");
    }
}