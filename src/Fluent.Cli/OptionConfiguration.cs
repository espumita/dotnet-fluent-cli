using System.Text.RegularExpressions;

namespace Fluent.Cli;

public class OptionConfiguration {
    public string PrimaryName { get; }
    public string SecondaryName { get; }

    public ArgumentConfiguration Argument { get; private set; }

    private OptionConfiguration(string primaryName, string secondaryName) {
        PrimaryName = primaryName;
        SecondaryName = secondaryName;
    }

    public static OptionConfiguration For(char primaryName) {
        Validate(primaryName);
        return new OptionConfiguration(primaryName: primaryName.ToString(), secondaryName: null);
    }

    public static OptionConfiguration For(char primaryName, string secondaryName) {
        Validate(primaryName);
        Validate(secondaryName);
        return new OptionConfiguration(primaryName.ToString(), secondaryName);
    }

    public static OptionConfiguration ForLong(string secondaryName) {
        Validate(secondaryName);
        return new OptionConfiguration(primaryName: null, secondaryName: secondaryName);
    }

    public void AddArgument(string argumentName) {
        Argument = new ArgumentConfiguration(argumentName);
    }

    private static void Validate(char primaryName) {
        if (!Regex.IsMatch(primaryName.ToString(), "^[a-zA-Z0-9]$"))
            throw new ArgumentException($"'{primaryName}' is not a valid short option, only alpha-numeric chars (a-zA-Z0-1) values can be configured");
    }

    private static void Validate(string secondaryName) {
        if (string.IsNullOrEmpty(secondaryName) || !Regex.IsMatch(secondaryName, "^[a-zA-Z0-9]+$|^[^/-][a-zA-Z0-9/-]+[^/-]$"))
            throw new ArgumentException($"'{secondaryName}' is not a valid option, only alpha-numeric values and words separated by hyphen minus '-' can be configured");
    }

    public bool IsArgumentConfigured() {
        return !string.IsNullOrEmpty(Argument?.ArgumentName);
    }
}