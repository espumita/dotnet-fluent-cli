using System.Text.RegularExpressions;

namespace Fluent.Cli;

public class CommandConfiguration {
    private readonly string _name;

    private CommandConfiguration(string name) {
        _name = name;
    }

    public static CommandConfiguration For(string name) {
        Validate(name);
        return new CommandConfiguration(name);
    }

    private static void Validate(string name) {
        if (!Regex.IsMatch(name, "^[a-zA-Z0-9]+$"))
            throw new ArgumentException($"'{name}' is not a valid command, only alpha-numeric chars (a-zA-Z0-1) values can be configured");
    }
}