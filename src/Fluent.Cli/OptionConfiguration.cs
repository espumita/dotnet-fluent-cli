using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Fluent.Cli;

public class OptionConfiguration {
    public string ShortName { get; }
    public string Name { get; }

    private OptionConfiguration(string shortName, [Optional] string name) {
        ShortName = shortName;
        Name = name;
    }

    public static OptionConfiguration For(string shortName) {
        Validate(shortName);
        return new OptionConfiguration(shortName);
    }

    public static OptionConfiguration For(string shortName, string name) {
        Validate(shortName);
        Validate(name);
        return new OptionConfiguration(shortName, name);
    }

    private static void Validate(string shortName) {
        if (string.IsNullOrEmpty(shortName) || !Regex.IsMatch(shortName, "^[a-zA-Z0-9]+$|^[^/-][a-zA-Z0-9/-]+[^/-]$"))
            throw new ArgumentException($"{shortName} is not a valid option, only alpha-numeric values and words separated by hyphen minus can be configured");
    }
}